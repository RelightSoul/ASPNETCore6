//  Основные операции с данными в Entity Framework Core

//  Пусть у нас проекте определен класс User, который будет представлять данные:

//public class User
//{
//    public int Id { get; set; }
//    public string Name { get; set; } = ""; // имя пользователя
//    public int Age { get; set; } // возраст пользователя
//}

//  Для взаимодействия с базой данных MS SQL Server в качестве контекста данных определим следующий класс ApplicationContext:

//using Microsoft.EntityFrameworkCore;
//public class ApplicationContext : DbContext
//{
//    public DbSet<User> Users { get; set; } = null!;
//    public ApplicationContext(DbContextOptions<ApplicationContext> options)
//        : base(options)
//    {
//        Database.EnsureCreated();   // создаем базу данных при первом обращении
//    }
//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<User>().HasData(
//                new User { Id = 1, Name = "Tom", Age = 37 },
//                new User { Id = 2, Name = "Bob", Age = 41 },
//                new User { Id = 3, Name = "Sam", Age = 24 }
//        );
//    }
//}

//  Далее в файле Program.cs определим основной код приложения, который будет обрабатывать запросы и подключаться к базе данных:

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();
string connection = "Server=(localdb)\\mssqllocaldb;Database=applicationdb;Trusted_Connection=True;";
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/users", async (ApplicationContext db) => await db.Users.ToListAsync());

app.MapGet("/api/users/{id:int}", async (int id, ApplicationContext db) =>
{
    // получаем пользователя по id
    User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

    // если не найден, отправляем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });

    // если пользователь найден, отправляем его
    return Results.Json(user);
});

app.MapDelete("/api/users/{id:int}", async (int id, ApplicationContext db) =>
{
    // получаем пользователя по id
    User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

    // если не найден, отправляем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });

    // если пользователь найден, удаляем его
    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.Json(user);
});

app.MapPost("/api/users", async (User user, ApplicationContext db) =>
{
    // добавляем пользователя в массив
    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
    return user;
});

app.MapPut("/api/users", async (User userData, ApplicationContext db) =>
{
    // получаем пользователя по id
    var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userData.Id);

    // если не найден, отправляем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });

    // если пользователь найден, изменяем его данные и отправляем обратно клиенту
    user.Age = userData.Age;
    user.Name = userData.Name;
    await db.SaveChangesAsync();
    return Results.Json(user);
});

app.Run();

//  Вначале добавляем класс ApplicationContext в сервисы приложения:
//var builder = WebApplication.CreateBuilder();
//string connection = "Server=(localdb)\\mssqllocaldb;Database=applicationdb;Trusted_Connection=True;";
//builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

//  Далее после создания объекта WebApplication подключаем функциональность статических файлов:
//app.UseDefaultFiles();
//app.UseStaticFiles();

//  Затем с помощью методов MapGet/MapPost/MapPut/MapDelete определяется набор конечных точек, которые будут обрабатывать
//  разные типы запросов.

//  Первая конечная точка обрабатывает запрос типа GET по маршруту "api/users":
//  app.MapGet("/api/users", async (ApplicationContext db) => await db.Users.ToListAsync());

//  Поскольку выше в коде контекст данных ApplicationContext был добавлен в качестве сервиса, то мы можем его получить через
//  параметр обработчика конечной точки и через полученный контекст данных получить из БД список объектов User и отправить их клиенту.

//  Когда клиент обращается к приложению для получения одного объекта по id в запрос типа GET по адресу "api/users/{id}", то
//  срабатывает другая конечная точка:

//app.MapGet("/api/users/{id:int}", async (int id, ApplicationContext db) =>
//{
//    // получаем пользователя по id
//    User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

//    // если не найден, отправляем статусный код и сообщение об ошибке
//    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });

//    // если пользователь найден, отправляем его
//    return Results.Json(user);
//});

//  Здесь через параметр id получаем из пути запроса идентификатор объекта User и по этому идентификатору ищем нужный объект в базе
//  данных, используя контекст данных ApplicationContext. Если объект по Id не был найден, то возвращаем с помощью метода
//  Results.NotFound() статусный код 404 с некоторым сообщением в формате JSON. Если объект найден, то с помощью метода Results.Json()
//  отправляет найденный объект клиенту.

//  При получении запроса типа DELETE по маршруту "/api/users/{id}" срабатывает другая конечная точка:

//app.MapDelete("/api/users/{id:int}", async (int id, ApplicationContext db) =>
//{
//    // получаем пользователя по id
//    User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

//    // если не найден, отправляем статусный код и сообщение об ошибке
//    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });

//    // если пользователь найден, удаляем его
//    db.Users.Remove(user);
//    await db.SaveChangesAsync();
//    return Results.Json(user);
//});

//  Здесь если объект по Id не найден в базе данных, то отправляем статусный код 404. Если же объект найден, то с помощью
//  вызова db.Users.Remove(user) указываем, что данный объект надо удалить из БД. А с помощью последующего вызова db.SaveChangesAsync()
//  сохраняем изменения в базу данных (то есть удаляем объект). И в конце посылаем удаленный объект клиенту.

//  При получении запроса с методом POST по адресу "/api/users" срабатывает следующая конечная точка:

//app.MapPost("/api/users", async (User user, ApplicationContext db) =>
//{
//    // добавляем пользователя в массив
//    await db.Users.AddAsync(user);
//    await db.SaveChangesAsync();
//    return user;
//});

//Здесь мы ожидаем, что в запросе типа POST клиент будет передавать на сервер данные, которые соответствуют определению типа User.
//И поэтому инфраструктура ASP.NET Core сможет автоматически создать из них объект User. И этот объект мы сможем получить в качестве
//параметра в обработчике конечной точки вместе с сервисом ApplicationContext.

//После получения объекта User с помощью метода db.Users.AddAsync(user) указываем, что данный объект надо добавить в БД. А с помощью
//последующего вызова db.SaveChangesAsync() сохраняем изменения в базу данных (то есть добавляем объект). После добавления отправляем
//объект User обратно клиенту.

//Если приложению приходит PUT-запрос по адресу "/api/users", то запрос обрабатывает последняя конечная точка:

//app.MapPut("/api/users", async (User userData, ApplicationContext db) =>
//{
//    // получаем пользователя по id
//    var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userData.Id);

//    // если не найден, отправляем статусный код и сообщение об ошибке
//    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });

//    // если пользователь найден, изменяем его данные и отправляем обратно клиенту
//    user.Age = userData.Age;
//    user.Name = userData.Name;
//    await db.SaveChangesAsync();
//    return Results.Json(user);
//});

//  Здесь аналогичным образом получаем отправленные клиентом данные в виде объекта User и сервис ApplicationContext. Затем пытаемся
//  найти подобный объект в базе данных. Если объект не найден, отправляем статусный код 404. Если объект найден, то изменяем его данные,
//  с помощью вызова db.SaveChangesAsync() сохраняем изменения в базу данных и отправляем измененный объект обратно клиенту

//  Теперь добавим код клиента. Для этого создадим в проекте новую папку wwwroot, в которую добавим новый файл index.html

//  Определим в файле index.html следующим код для взаимодействия с веб-приложением ASP.NET Core:

//<!DOCTYPE html>
//<html>
//<head>
//    <meta charset="utf-8" />
//    <meta name="viewport" content="width=device-width" />
//    <title>Список пользователей</title>
//    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" />
//</head>
//<body>
//    <h2>Список пользователей</h2>
//    <form name="userForm">
//        <input type="hidden" name="id" value="0" />
//        <div class="mb-3">
//            <label class="form-label" for="name">Имя:</label>
//            <input class="form-control" name="name" />
//        </div>
//        <div class="mb-3">
//            <label for="age" class="form-label">Возраст:</label>
//            <input class="form-control" name="age" />
//        </div>
//        <div class="mb-3">
//            <button type="submit" class="btn btn-sm btn-primary">Сохранить</button>
//            <a id="reset" class="btn btn-sm btn-primary">Сбросить</a>
//        </div>
//    </form>
//    <table class="table table-condensed table-striped table-bordered">
//        <thead><tr><th>Имя</th><th>возраст</th><th></th></tr></thead>
//        <tbody>
//        </tbody>
//    </table>

//    <script>
//    // Получение всех пользователей
//        async function getUsers() {
//            // отправляет запрос и получаем ответ
//            const response = await fetch("/api/users", {
//                method: "GET",
//                headers: { "Accept": "application/json" }
//            });
//            // если запрос прошел нормально
//            if (response.ok === true) {
//                // получаем данные
//                const users = await response.json();
//                const rows = document.querySelector("tbody");
//                // добавляем полученные элементы в таблицу
//                users.forEach(user => rows.append(row(user)));
//            }
//        }
//        // Получение одного пользователя
//        async function getUser(id) {
//            const response = await fetch("/api/users/" + id, {
//                method: "GET",
//                headers: { "Accept": "application/json" }
//            });
//            if (response.ok === true) {
//                const user = await response.json();
//                const form = document.forms["userForm"];
//                form.elements["id"].value = user.id;
//                form.elements["name"].value = user.name;
//                form.elements["age"].value = user.age;
//            }
//            else {
//                // если произошла ошибка, получаем сообщение об ошибке
//                const error = await response.json();
//                console.log(error.message); // и выводим его на консоль
//            }
//        }
//        // Добавление пользователя
//        async function createUser(userName, userAge) {

//            const response = await fetch("api/users", {
//                method: "POST",
//                headers: { "Accept": "application/json", "Content-Type": "application/json" },
//                body: JSON.stringify({
//                    name: userName,
//                    age: parseInt(userAge, 10)
//                })
//            });
//            if (response.ok === true) {
//                const user = await response.json();
//                reset();
//                document.querySelector("tbody").append(row(user));
//            }
//            else {
//                const error = await response.json();
//                console.log(error.message);
//            }
//        }
//        // Изменение пользователя
//        async function editUser(userId, userName, userAge) {
//            const response = await fetch("api/users", {
//                method: "PUT",
//                headers: { "Accept": "application/json", "Content-Type": "application/json" },
//                body: JSON.stringify({
//                    id: userId,
//                    name: userName,
//                    age: parseInt(userAge, 10)
//                })
//            });
//            if (response.ok === true) {
//                const user = await response.json();
//                reset();
//                document.querySelector("tr[data-rowid='" + user.id + "']").replaceWith(row(user));
//            }
//            else {
//                const error = await response.json();
//                console.log(error.message);
//            }
//        }
//        // Удаление пользователя
//        async function deleteUser(id) {
//            const response = await fetch("/api/users/" + id, {
//                method: "DELETE",
//                headers: { "Accept": "application/json" }
//            });
//            if (response.ok === true) {
//                const user = await response.json();
//                document.querySelector("tr[data-rowid='" + user.id + "']").remove();
//            }
//            else {
//                const error = await response.json();
//                console.log(error.message);
//            }
//        }

//        // сброс данных формы после отправки
//        function reset() {
//            const form = document.forms["userForm"];
//            form.reset();
//            form.elements["id"].value = 0;
//        }
//        // создание строки для таблицы
//        function row(user) {

//            const tr = document.createElement("tr");
//            tr.setAttribute("data-rowid", user.id);

//            const nameTd = document.createElement("td");
//            nameTd.append(user.name);
//            tr.append(nameTd);

//            const ageTd = document.createElement("td");
//            ageTd.append(user.age);
//            tr.append(ageTd);

//            const linksTd = document.createElement("td");

//            const editLink = document.createElement("a");
//            editLink.setAttribute("style", "cursor:pointer;padding:15px;");
//            editLink.append("Изменить");
//            editLink.addEventListener("click", e => {

//                e.preventDefault();
//                getUser(user.id);
//            });
//            linksTd.append(editLink);

//            const removeLink = document.createElement("a");
//            removeLink.setAttribute("style", "cursor:pointer;padding:15px;");
//            removeLink.append("Удалить");
//            removeLink.addEventListener("click", e => {

//                e.preventDefault();
//                deleteUser(user.id);
//            });

//            linksTd.append(removeLink);
//            tr.appendChild(linksTd);

//            return tr;
//        }
//        // сброс значений формы
//        document.getElementById("reset").addEventListener("click", e => {

//            e.preventDefault();
//            reset();
//        })

//        // отправка формы
//        document.forms["userForm"].addEventListener("submit", e => {
//            e.preventDefault();
//            const form = document.forms["userForm"];
//            const id = form.elements["id"].value;
//            const name = form.elements["name"].value;
//            const age = form.elements["age"].value;
//            if (id == 0)
//                createUser(name, age);
//            else
//                editUser(id, name, age);
//        });

//        // загрузка пользователей
//        getUsers();
//    </script>
//</body>
//</html>

//  Основная логика здесь заключена в коде javascript. При загрузке страницы в браузере получаем все объекты из БД с помощью функции
//  getUsers():

//async function getUsers()
//{
//    // отправляет запрос и получаем ответ
//    const response = await fetch("/api/users", {
//    method: "GET",
//        headers: { "Accept": "application/json" }
//    });
//// если запрос прошел нормально
//if (response.ok === true)
//{
//    // получаем данные
//    const users = await response.json();
//    let rows = document.querySelector("tbody");
//    users.forEach(user => {
//        // добавляем полученные элементы в таблицу
//        rows.append(row(user));
//    });
//}
//}

//  Для добавления строк в таблицу используется функция row(), которая возвращает строку. В этой строке будут определены ссылки для
//  изменения и удаления пользователя.

//  Ссылка для изменения пользователя с помощью функции getUser() получает с сервера выделенного пользователя:

//async function getUser(id)
//{
//    const response = await fetch("/api/users/" + id, {
//    method: "GET",
//        headers: { "Accept": "application/json" }
//    });
//if (response.ok === true)
//{
//    const user = await response.json();
//    const form = document.forms["userForm"];
//    form.elements["id"].value = user.id;
//    form.elements["name"].value = user.name;
//    form.elements["age"].value = user.age;
//}
//else
//{
//    // если произошла ошибка, получаем сообщение об ошибке
//    const error = await response.json();
//    console.log(error.message); // и выводим его на консоль
//}
//}

//  И выделенный пользователь добавляется в форму над таблицей. Эта же форма применяется и для добавления объекта. С помощью скрытого
//  поля, которое хранит id пользователя, мы можем узнать, какое действие выполняется - добавление или редактирование. Если id равен 0,
//  то выполняется функция createUser, которая отправляет данные в POST-запросе:

//async function createUser(userName, userAge)
//{

//    const response = await fetch("api/users", {
//    method: "POST",
//        headers: { "Accept": "application/json", "Content-Type": "application/json" },
//        body: JSON.stringify({
//name: userName,
//            age: parseInt(userAge, 10)
//        })
//    });
//if (response.ok === true)
//{
//    const user = await response.json();
//    reset();
//    document.querySelector("tbody").append(row(user));
//}
//else
//{
//    const error = await response.json();
//    console.log(error.message);
//}
//}

//  Если же ранее пользователь был загружен на форму, и в скрытом поле сохранился его id, то выполняется функция editUser, которая
//  отправляет PUT-запрос:

//async function editUser(userId, userName, userAge)
//{
//    const response = await fetch("api/users", {
//    method: "PUT",
//        headers: { "Accept": "application/json", "Content-Type": "application/json" },
//        body: JSON.stringify({
//id: userId,
//            name: userName,
//            age: parseInt(userAge, 10)
//        })
//    });
//if (response.ok === true)
//{
//    const user = await response.json();
//    reset();
//    document.querySelector("tr[data-rowid='" + user.id + "']").replaceWith(row(user));
//}
//else
//{
//    const error = await response.json();
//    console.log(error.message);
//}

//  И функция deleteUser() посылает приложению ASP.NET Core запрос типа DELETE на удаление пользователя, и при успешном удалении на
//  сервере удаляет пользователя по id из таблицы пользователей.

//  Теперь запустим проект, и по умолчанию приложение отправит браузеру веб-страницу index.html, которая загрузит список объектов:

//  После этого мы сможем выполнять все базовые операции с пользователями - получение, добавление, изменение, удаление