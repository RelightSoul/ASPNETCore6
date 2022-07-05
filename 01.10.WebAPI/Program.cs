//  Создание простейшего API

//  Рассмотренного в прошлых темах материала достаточно для создания примитивного приложения. В этой теме попробуем реализовать
//  простейшее приложение Web API в стиле REST. Архитектура REST предполагает применение следующих методов или типов запросов HTTP
//  для взаимодействия с сервером, где каждый тип запроса отвечает за определенное действие:
//  GET(получение данных)
//  POST(добавление данных)
//  PUT(изменение данных)    
//  DELETE(удаление данных)

//  Поскольку в приложении ASP.NET Core мы можем легко получить и адрес запроса и тип запроса, то реализовать подобную архитектуру
//  не составит труда.

#region Создание сервера на ASP.NET Core
List<Person> users = new List<Person>
{ 
    new Person() {Id= Guid.NewGuid().ToString(), Name = "Tom", Age = 37 },
    new Person() {Id= Guid.NewGuid().ToString(), Name = "Bob", Age = 41 },
    new Person() {Id= Guid.NewGuid().ToString(), Name = "Sam", Age = 24 }
};

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async context =>
{
    var response = context.Response;
    var request = context.Request;
    var path = context.Request.Path;
    //string expressionForNumber = "^/api/users/([0 - 9]+)$";   // если id представляет число

    // 2e752824-1657-4c7f-844b-6ec2e168e99c
    string exressionForGuid = @"^/api/users/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";
    if (path == "/api/users" && request.Method == "GET")
    {
       
    }
});
app.Run();

// получение всех пользователей
async Task GetAllPeople(HttpResponse response)
{
    await response.WriteAsJsonAsync(users);
}
// получение одного пользователя по id
async Task GetPerson(string? id, HttpResponse response)
{
    // получаем пользователя по id
    Person? user = users.FirstOrDefault((u) => u.Id == id);
    // если пользователь найден, отправляем его
    if (user != null)
    {
        await response.WriteAsJsonAsync(user);
    }
    // если не найден, отправляем статусный код и сообщение об ошибке
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
    }
}
async Task DeletePerson(string? id, HttpResponse response)
{
    Person? user = users.FirstOrDefault(u => u.Id == id);
    if (user != null)
    {
        users.Remove(user);
        await response.WriteAsJsonAsync(user);
    }
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
    }
}


public class Person
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }
}
#endregion