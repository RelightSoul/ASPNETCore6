//  Отправка статусных кодов в Results API

//  Нередко возникает необходимость отправить в ответ на запрос какой-либо статусный код. Например, если пользователь
//  пытается получить доступ к ресурсу, который недоступен или для которого у пользователя нету прав. Либо просто необходимо
//  уведомить пользователя с помощью статусного кода об успешном выполнении операции. И для этого Results API предоставляет ряд методов.

#region StatusCode
//  Метод StatusCode() позволяет отправить любой статусный код, числовой код которого передается в метод в качестве параметра:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/", () => Results.StatusCode(401));
//app.Map("/", () => "Hello World");

//app.Run();

//  Подобным образом мы можем послать клиенту любой другой статусный код. Но для некоторых отдельных статусных кодов имеются свои
//  отдельные методы.
#endregion

#region Метод NotFound
//  Метод NotFound() посылает код 404, уведомляя клиента о том, что ресурс не найден. В качестве параметра в метод можно передать
//  некоторый объект для отправки клиенту, например, сообщение об ошибке:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/about", () => Results.NotFound(new { messsage = "Resource not found"}));
//app.Map("/contacts", () => Results.NotFound("Error 404. Ivalid address"));
//app.Map("/", () => "Hello World");

//app.Run();

//  Стоит отметить, что сложные объекты по умолчанию сериализуются в формат json.
#endregion

#region Unauthorized
//  Метод Unauthorized() посылает код 401, уведомляя пользователя, что он не авторизован для доступа к ресурсу:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/contacts", () => Results.Unauthorized());
//app.Map("/", () => "Hello World");

//app.Run();
#endregion

#region BadRequest
//  Метод BadRequest() посылает код 400, который говорит о том, что запрос некорректный. В качестве параметра можно передать
//  некоторый объект для отправки клиенту:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/contacts/{age:int}", (int age) =>
//{
//    if (age < 18)
//        return Results.BadRequest(new { message = "Invalid age" });
//    else
//        return Results.Content("Access is available");
//});
//app.Map("/", () => "Hello World");

//app.Run();

//  В данном случае если параметр маршрута age меньше 18, то посылаем клиенту статусный код 400 с сообщением "Invalid age".
//  Иначе посылаем некоторое стандартное сообщение
#endregion

#region Ok
//  Метод Ok() посылает статусный код 200, уведомляя об успешном выполнении запроса. В качестве параметра метод принимает
//  отправляемую информацию:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Map("/about", () => Results.Ok("Laudate omnes gentes laudate"));
app.Map("/contacts", () => Results.Ok(new { message = "Success!" }));
app.Map("/", () => "Hello World");


app.Run();
#endregion