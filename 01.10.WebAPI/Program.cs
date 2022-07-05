//  �������� ����������� API

//  �������������� � ������� ����� ��������� ���������� ��� �������� ������������ ����������. � ���� ���� ��������� �����������
//  ���������� ���������� Web API � ����� REST. ����������� REST ������������ ���������� ��������� ������� ��� ����� �������� HTTP
//  ��� �������������� � ��������, ��� ������ ��� ������� �������� �� ������������ ��������:
//  GET(��������� ������)
//  POST(���������� ������)
//  PUT(��������� ������)    
//  DELETE(�������� ������)

//  ��������� � ���������� ASP.NET Core �� ����� ����� �������� � ����� ������� � ��� �������, �� ����������� �������� �����������
//  �� �������� �����.

#region �������� ������� �� ASP.NET Core
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
    //string expressionForNumber = "^/api/users/([0 - 9]+)$";   // ���� id ������������ �����

    // 2e752824-1657-4c7f-844b-6ec2e168e99c
    string exressionForGuid = @"^/api/users/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";
    if (path == "/api/users" && request.Method == "GET")
    {
       
    }
});
app.Run();

// ��������� ���� �������������
async Task GetAllPeople(HttpResponse response)
{
    await response.WriteAsJsonAsync(users);
}
// ��������� ������ ������������ �� id
async Task GetPerson(string? id, HttpResponse response)
{
    // �������� ������������ �� id
    Person? user = users.FirstOrDefault((u) => u.Id == id);
    // ���� ������������ ������, ���������� ���
    if (user != null)
    {
        await response.WriteAsJsonAsync(user);
    }
    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "������������ �� ������" });
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
        await response.WriteAsJsonAsync(new { message = "������������ �� ������" });
    }
}


public class Person
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }
}
#endregion