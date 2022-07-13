//  Web API
//  Пример приложения Web API

//  Web API представляет способ построения приложения в стиле REST (Representation State Transfer или "передача состояния представления").
//  REST - архитектура предполагает применение следующих методов или типов запросов HTTP для взаимодействия с сервером:
//      GET(получение данных)
//      POST(добавление данных)
//      PUT(изменение данных)
//      DELETE(удаление данных)

//  Для реализации подобной архитектуру фреймворк ASP.NET Core предоставляет ряд встроенных методов, которые как и метод Map()
//  реализованы как методы расширения для типа Microsoft.AspNetCore.Routing.IEndpointRouteBuilder (а соответственно и для типа
//  WebApplication). Эти методы также встраивают в конвейер обработки запроса конечные точки, которые обрабатывают определенные
//  типы запросов:
//      MapGet(запрос GET)
//      MapPost(запрос POST)
//      MapPut(запрос PUT)
//      MapDelete(запрос DELETE)

//  Рассмотрим, как мы можем реализовать с помощью этих методов простейший API.

#region Создание сервера
//  Вначале определим веб-приложение на ASP.NET Core, которое и будет собственно представлять Web API:

// начальные данные
List<Person> users = new List<Person>
{
    new() { Id = Guid.NewGuid().ToString(), Name = "Tom" , Age = 37},
    new() { Id = Guid.NewGuid().ToString(), Name = "Bob" , Age = 41},
    new() { Id = Guid.NewGuid().ToString(), Name = "Sam" , Age = 24}
};

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/users", () => users);

app.MapGet("/api/users/{id}", (string id) => 
{
    // получаем пользователя по id
    Person? user = users.FirstOrDefault(u => u.Id == id);
    // если не найден, отправляем статусный код и сообщение об ошибке
    if (user == null)
    {
        return Results.NotFound(new { message = "Пользователь не найден"});
    }

    // если пользователь найден, отправляем его
    return Results.Json(user);
});

app.MapDelete("/api/users/{id}", (string id) => 
{
    // получаем пользователя по id
    Person? user = users.FirstOrDefault(u => u.Id == id);

    // если не найден, отправляем статусный код и сообщение об ошибке
    if (user == null)
    {
        return Results.NotFound(new { message ="Пользователь не найден"});
    }

    // если пользователь найден, удаляем его
    users.Remove(user);
    return Results.Json(user);
});

app.MapPost("/api/users", (Person user) =>
{
    // устанавливаем id для нового пользователя
    user.Id = Guid.NewGuid().ToString();
    // добавляем пользователя в список
    users.Add(user);
    return user;
});

app.MapPut("/api/users", (Person userData) =>
{
    // получаем пользователя по id
    var user = users.FirstOrDefault(u => u.Id == userData.Id);
    // если не найден, отправляем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });
    // если пользователь найден, изменяем его данные и отправляем обратно клиенту
    user.Age = userData.Age;
    user.Name = userData.Name;
    return Results.Json(user);
});

app.Run();

public class Person
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }
}

//  Разберем в общих чертах этот код. Вначале создается список объектов Person - те данные, с которыми будет работать пользователь:

//var users = new List<Person>
//{
//    new() { Id = Guid.NewGuid().ToString(), Name = "Tom", Age = 37 },
//    new() { Id = Guid.NewGuid().ToString(), Name = "Bob", Age = 41 },
//    new() { Id = Guid.NewGuid().ToString(), Name = "Sam", Age = 24 }
//};

//  Стоит обратить внимание, что каждый объект Person имеет свойство Id, которое в качестве значения получает Guid - уникальный
//  идентификатор, например "2e752824-1657-4c7f-844b-6ec2e168e99c".

//  Для упрошения данные определены в виде обычного списка объектов, но в реальной ситуации обычно подобные данные извлекаются из
//  какой-нибудь базы данных.

//  Далее после создания объекта WebApplication подключаем функциональность статических файлов:
//      app.UseDefaultFiles();
//      app.UseStaticFiles();

//  Затем с помощью методов MapGet/MapPost/MapPut/MapDelete определяется набор конечных точек, которые будут обрабатывать разные типы
//  запросов.

//  Вначале добавляется конечная точка, которая обрабатывает запрос типа GET по маршруту "api/users":
//      app.MapGet("/api/users", () => users);

//  Запрос GET предполагает получение объектов, и в данном случае отправляем выше определенный список объектов Person.

//  Когда клиент обращается к приложению для получения одного объекта по id в запрос типа GET по адресу "api/users/{id}", то срабатывает
//  другая конечная точка:

//app.MapGet("/api/users/{id}", (string id) =>
//{
//    // получаем пользователя по id
//    Person? user = users.FirstOrDefault(u => u.Id == id);
//    // если не найден, отправляем статусный код и сообщение об ошибке
//    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });

//    // если пользователь найден, отправляем его
//    return Results.Json(user);
//});

//Здесь через параметр id получаем из пути запроса идентификатор объекта Person и по этому идентификатору ищем нужный объект в списке
//users. Если объект по Id не был найден, то возвращаем с помощью метода Results.NotFound() статусный код 404 с некоторым сообщением в
//формате JSON. Если объект найден, то с помощью метода Results.Json() отправляет найденный объект клиенту.

//  При получении запроса типа DELETE по маршруту "/api/users/{id}" срабатывает другая конечная точка:

//app.MapDelete("/api/users/{id}", (string id) =>
//{
//    // получаем пользователя по id
//    Person? user = users.FirstOrDefault(u => u.Id == id);

//    // если не найден, отправляем статусный код и сообщение об ошибке
//    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });

//    // если пользователь найден, удаляем его
//    users.Remove(user);
//    return Results.Json(user);
//});

//  Здесь действует аналогичная логика - если объект по Id не найден, отправляет статусный код 404. Если же объект найден,
//  то удаляем его из списка и посылаем клиенту.

//  При получении запроса с методом POST по адресу "/api/users" срабатывает следующая конечная точка:

//app.MapPost("/api/users", (Person user) => {

//    // устанавливаем id для нового пользователя
//    user.Id = Guid.NewGuid().ToString();
//    // добавляем пользователя в список
//    users.Add(user);
//    return user;
//});

//  Запрос типа POST предполагает передачу приложению отправляемых данных. Причем мы ожидаем, что клиент отправит данные, которые
//  соответствуют определению типа Person. И поэтому инфраструктура ASP.NET Core сможет автоматически собрать из них объект Person.
//  И этот объект мы сможем получить в качестве параметра в обработчике конечной точки.

//  После получения данных устанавливаем у нового объекта свойство Id, добавляем его в список users и отправляем обратно клиенту.

//  Если приложению приходит PUT-запрос по адресу "/api/users", то аналогичным образом получаем отправленные клиентом данные в виде
//  объекта Person и пытаемся найти подобный объект в списке users. Если объект не найден, отправляем статусный код 404. Если объект
//  найден, то изменяем его данные и отправляем обратно клиенту:

//app.MapPut("/api/users", (Person userData) => {

//    // получаем пользователя по id
//    var user = users.FirstOrDefault(u => u.Id == userData.Id);
//    // если не найден, отправляем статусный код и сообщение об ошибке
//    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });
//    // если пользователь найден, изменяем его данные и отправляем обратно клиенту
//    user.Age = userData.Age;
//    user.Name = userData.Name;
//    return Results.Json(user);
//});

//  Таким образом, мы определили простейший API. Теперь добавим код клиента.
#endregion

#region Определение клиента
//  Теперь создадим в проекте новую папку wwwroot, в которую добавим новый файл index.html

//  Определим в файле index.html следующим код для взаимодействия с сервером ASP.NET Core:

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

//  Основная логика здесь заключена в коде javascript. При загрузке страницы в браузере получаем все объекты из БД с помощью
//  функции getUsers():

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

//  Для добавления строк в таблицу используется функция row(), которая возвращает строку. В этой строке будут определены ссылки
//  для изменения и удаления пользователя.

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
//}

//  И функция deleteUser() посылает приложению ASP.NET Core запрос типа DELETE на удаление пользователя, и при успешном удалении на
//  сервере удаляет объект по id из списка объектов Person.

//  Теперь запустим проект, и по умолчанию приложение отправит браузеру веб-страницу index.html, которая загрузит список объектов:

//  После этого мы сможем выполнять все базовые операции с пользователями - получение, добавление, изменение, удаление. 
#endregion