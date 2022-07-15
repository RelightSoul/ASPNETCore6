//  Авторизация с помощью JWT-токенов в клиенте JavaScript

//  В прошлой статье был рассмотрен процесс конфигурации и генерации JWT-токенов. Теперь посмотрим, как мы можем применить
//  JWT-токен для авторизации в приложении. Для этого определим в файле Program.cs следующий код:

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// условная бд с пользователями
var people = new List<Person>
 {
    new Person("tom@gmail.com", "12345"),
    new Person("bob@gmail.com", "55555")
};

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        };
    });
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", (Person loginData) =>
{
    // находим пользователя 
    Person? person = people.FirstOrDefault(p => p.Email == loginData.Email && p.Password == loginData.Password);
    // если пользователь не найден, отправляем статусный код 401
    if (person is null) return Results.Unauthorized();

    var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Email) };
    // создаем JWT-токен
    var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

    // формируем ответ
    var response = new
    {
        access_token = encodedJwt,
        username = person.Email
    };

    return Results.Json(response);
});
app.Map("/data", [Authorize] () => new { message = "Hello World!" });

app.Run();

public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // издатель токена
    public const string AUDIENCE = "MyAuthClient"; // потребитель токена
    const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}

record class Person(string Email, string Password);

//  Для предствления пользователя в приложении здесь определен record-класс Person, который имеет два свойства: email и пароль.
//  И для упрощения ситуации вместо базы данных все пользователи приложения хранятся в списке people. Условно говоря у нас есть два
//  пользователя.

//  Для описания некоторых настроек генерации токена, как и в прошлой теме, в коде определен специальный класс AuthOptions, и также,
//  как и в прошлой теме, с помощью метода AddJwtBearer() в приложение добавляется конфигурация токена.

//  В конечной точке "\login", которая обрабатывает POST-запросы, получаем отправленные клиентом аутентификационные данные опять же
//  для простоты в виде объекта Person:
//  app.MapPost("/login", (Person loginData) =>

//  Используя полученные данные, пытаемся найти в списке people пользователя:
//  Person ? person = people.FirstOrDefault(p => p.Email == loginData.Email && p.Password == loginData.Password);

//  Если пользователь не найден, то есть переданы некорректные email и/или пароль, то оправляем статусный код 401, который говорит
//  о том, что доступ запрещен:
//  if (person is null) return Results.Unauthorized();

//  Если пользователь найден, то создается список объектов Claim с одним Claim, который представляет email пользователя.
//  Генерируем jwt-токен:
//  var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Email) };
//  var jwt = new JwtSecurityToken(
//      issuer: AuthOptions.ISSUER,
//      audience: AuthOptions.AUDIENCE,
//      claims: claims,
//      expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),  // действие токена истекает через 2 минуты
//      signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
//  var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

//  Далее формирует ответ клиенту. Он отправляется в виде объекта в формате json, который содержит два свойства:
//  access_token - собственно токен и username - email аутентифицированного пользователя
//  var response = new { access_token = encodedJwt, username = person.Email }; return Results.Json(response);

//  Еще одна конечная точка - "/data" использует атрибут Authorize, поэтому для обращения к ней необходимо в запросе отправлять
//  полученный jwt-токен.
//  /app.Map("/data", [Authorize] (HttpContext context) => $"Hello World!");

//  Создание клиента на javascript
//  Теперь определим клиент для тестирования авторизации с помощью токена. Итак, в коде приложения определено подключение
//  статических файлов по умолчанию:
//  app.UseDefaultFiles();
//  app.UseStaticFiles();

//  В качестве веб-страницы по умолчанию добавим в проект для статических файлов папку wwwroot, а в нее - новый файл index.html:

//<!DOCTYPE html>
//<html>
//<head>
//    <meta charset="utf-8" />
//    <title>METANIT.COM</title>
//</head>
//<body>
//    <div id="userInfo" style="display:none;">
//        <p>Добро пожаловать <span id="userName"></span>!</p>
//        <input type="button" value="Выйти" id="logOut" />
//    </div>
//    <div id="loginForm">
//        <h3>Вход на сайт</h3>
//        <p>
//            <label>Введите email</label><br />
//            <input type="email" id="email" />
//        </p>
//        <p>
//            <label>Введите пароль</label><br />
//            <input type="password" id="password" />
//        </p>
//        <input type="submit" id="submitLogin" value="Логин" />
//    </div>
//    <p>
//        <input type="submit" id="getData" value="Получить данные" />
//    </p>
//    <script>
//        var tokenKey = "accessToken";
//        // при нажатии на кнопку отправки формы идет запрос к /login для получения токена
//        document.getElementById("submitLogin").addEventListener("click", async e => {
//            e.preventDefault();
//            // отправляет запрос и получаем ответ
//            const response = await fetch("/login", {
//                method: "POST",
//                headers: { "Accept": "application/json", "Content-Type": "application/json" },
//                body: JSON.stringify({
//                    email: document.getElementById("email").value,
//                    password: document.getElementById("password").value
//                })
//            });
//            // если запрос прошел нормально
//            if (response.ok === true) {
//                // получаем данные
//                const data = await response.json();
//                // изменяем содержимое и видимость блоков на странице
//                document.getElementById("userName").innerText = data.username;
//                document.getElementById("userInfo").style.display = "block";
//                document.getElementById("loginForm").style.display = "none";
//                // сохраняем в хранилище sessionStorage токен доступа
//                sessionStorage.setItem(tokenKey, data.access_token);
//            }
//            else  // если произошла ошибка, получаем код статуса
//                console.log("Status: ", response.status);
//        });

//        // кнопка для обращения по пути "/data" для получения данных
//        document.getElementById("getData").addEventListener("click", async e => {
//            e.preventDefault();
//            // получаем токен из sessionStorage
//            const token = sessionStorage.getItem(tokenKey);
//            // отправляем запрос к "/data
//            const response = await fetch("/data", {
//                method: "GET",
//                headers: {
//                    "Accept": "application/json",
//                    "Authorization": "Bearer " + token  // передача токена в заголовке
//                }
//            });

//            if (response.ok === true) {
//                const data = await response.json();
//                alert(data.message);
//            }
//            else
//                console.log("Status: ", response.status);
//        });

//        // условный выход - просто удаляем токен и меняем видимость блоков
//        document.getElementById("logOut").addEventListener("click", e => {

//            e.preventDefault();
//            document.getElementById("userName").innerText = "";
//            document.getElementById("userInfo").style.display = "none";
//            document.getElementById("loginForm").style.display = "block";
//            sessionStorage.removeItem(tokenKey);
//        });
//    </script>
//</body>
//</html>

//  Первый блок на странице выводит информацию о вошедшем пользователе и ссылку для выхода. Второй блок содержит форму для логина.

//  После нажатия кнопки на форме логина запрос будет отправляться методом POST на адрес "/login". Конечная точка, которая отвечает
//  за обработку POST-запросов по этому маршруту, если переданы корректные email и пароль, отправит в ответ токен.

//  Ответом сервера в случае удачной аутентификации будет примерно следующий объект:

//{
//    access_token : "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93
//                    cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoicXdlcnR5IiwiaHR0cDovL3NjaGVtYXMub
//                    Wljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoidXNlciIsIm5iZi
//                    I6MTQ4MTYzOTMxMSwiZXhwIjoxNDgxNjM5MzcxLCJpc3MiOiJNeUF1dGhTZXJ2ZXIiLCJhdWQiOiJ
//                    odHRwOi8vbG9jYWxob3N0OjUxODg0LyJ9.dQJF6pALUZW3wGBANy_tCwk5_NR0TVBwgnxRbblp5Ho",
//    username: "tom@gmail.com"
//}

//  Параметр access_token как раз и будет представлять токен доступа. Также в объекте передается дополнительная информация о
//  нике пользователя.

//  Для того, чтобы в коде js данный токен в дальнейшем был доступен, то он сохраняется в хранилище sessionStorage.

//  Дополнительная кнопка с id="getData" на странице предназначена для тестирования авторизации с помощью токена. По ее нажатию
//  будет выполняться запрос по адресу "/data", для доступа к которому необходимо быть аутентифицированным. Чтобы отправить токен
//  в запросе, нам нужно настроить в запросе заголовок Authorization:

//headers:
//{
//    "Accept": "application/json",
//    "Authorization": "Bearer " + token  // передача токена в заголовке
//}

//  Запустим проект и введем данные одного из пользователя, который есть в списке people
//  При вводе корректных данных север пришлет клиенту объект с jwt-токеном и логином пользователя. И после этого мы можем нажать на
//  кпопку "Получить данные" и тем самым обратиться к ресурсу "/data", для доступа к которому требуется токен.
//  В то же время если мы попробуем обратиться к этому же ресурсу без токена или с токеном с истекшим сроком, то получим ошибку
//  401 (Unauthorized).
