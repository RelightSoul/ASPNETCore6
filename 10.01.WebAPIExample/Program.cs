//  Web API
//  ������ ���������� Web API

//  Web API ������������ ������ ���������� ���������� � ����� REST (Representation State Transfer ��� "�������� ��������� �������������").
//  REST - ����������� ������������ ���������� ��������� ������� ��� ����� �������� HTTP ��� �������������� � ��������:
//      GET(��������� ������)
//      POST(���������� ������)
//      PUT(��������� ������)
//      DELETE(�������� ������)

//  ��� ���������� �������� ����������� ��������� ASP.NET Core ������������� ��� ���������� �������, ������� ��� � ����� Map()
//  ����������� ��� ������ ���������� ��� ���� Microsoft.AspNetCore.Routing.IEndpointRouteBuilder (� �������������� � ��� ����
//  WebApplication). ��� ������ ����� ���������� � �������� ��������� ������� �������� �����, ������� ������������ ������������
//  ���� ��������:
//      MapGet(������ GET)
//      MapPost(������ POST)
//      MapPut(������ PUT)
//      MapDelete(������ DELETE)

//  ����������, ��� �� ����� ����������� � ������� ���� ������� ���������� API.

#region �������� �������
//  ������� ��������� ���-���������� �� ASP.NET Core, ������� � ����� ���������� ������������ Web API:

// ��������� ������
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
    // �������� ������������ �� id
    Person? user = users.FirstOrDefault(u => u.Id == id);
    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (user == null)
    {
        return Results.NotFound(new { message = "������������ �� ������"});
    }

    // ���� ������������ ������, ���������� ���
    return Results.Json(user);
});

app.MapDelete("/api/users/{id}", (string id) => 
{
    // �������� ������������ �� id
    Person? user = users.FirstOrDefault(u => u.Id == id);

    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (user == null)
    {
        return Results.NotFound(new { message ="������������ �� ������"});
    }

    // ���� ������������ ������, ������� ���
    users.Remove(user);
    return Results.Json(user);
});

app.MapPost("/api/users", (Person user) =>
{
    // ������������� id ��� ������ ������������
    user.Id = Guid.NewGuid().ToString();
    // ��������� ������������ � ������
    users.Add(user);
    return user;
});

app.MapPut("/api/users", (Person userData) =>
{
    // �������� ������������ �� id
    var user = users.FirstOrDefault(u => u.Id == userData.Id);
    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (user == null) return Results.NotFound(new { message = "������������ �� ������" });
    // ���� ������������ ������, �������� ��� ������ � ���������� ������� �������
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

//  �������� � ����� ������ ���� ���. ������� ��������� ������ �������� Person - �� ������, � �������� ����� �������� ������������:

//var users = new List<Person>
//{
//    new() { Id = Guid.NewGuid().ToString(), Name = "Tom", Age = 37 },
//    new() { Id = Guid.NewGuid().ToString(), Name = "Bob", Age = 41 },
//    new() { Id = Guid.NewGuid().ToString(), Name = "Sam", Age = 24 }
//};

//  ����� �������� ��������, ��� ������ ������ Person ����� �������� Id, ������� � �������� �������� �������� Guid - ����������
//  �������������, �������� "2e752824-1657-4c7f-844b-6ec2e168e99c".

//  ��� ��������� ������ ���������� � ���� �������� ������ ��������, �� � �������� �������� ������ �������� ������ ����������� ��
//  �����-������ ���� ������.

//  ����� ����� �������� ������� WebApplication ���������� ���������������� ����������� ������:
//      app.UseDefaultFiles();
//      app.UseStaticFiles();

//  ����� � ������� ������� MapGet/MapPost/MapPut/MapDelete ������������ ����� �������� �����, ������� ����� ������������ ������ ����
//  ��������.

//  ������� ����������� �������� �����, ������� ������������ ������ ���� GET �� �������� "api/users":
//      app.MapGet("/api/users", () => users);

//  ������ GET ������������ ��������� ��������, � � ������ ������ ���������� ���� ������������ ������ �������� Person.

//  ����� ������ ���������� � ���������� ��� ��������� ������ ������� �� id � ������ ���� GET �� ������ "api/users/{id}", �� �����������
//  ������ �������� �����:

//app.MapGet("/api/users/{id}", (string id) =>
//{
//    // �������� ������������ �� id
//    Person? user = users.FirstOrDefault(u => u.Id == id);
//    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
//    if (user == null) return Results.NotFound(new { message = "������������ �� ������" });

//    // ���� ������������ ������, ���������� ���
//    return Results.Json(user);
//});

//����� ����� �������� id �������� �� ���� ������� ������������� ������� Person � �� ����� �������������� ���� ������ ������ � ������
//users. ���� ������ �� Id �� ��� ������, �� ���������� � ������� ������ Results.NotFound() ��������� ��� 404 � ��������� ���������� �
//������� JSON. ���� ������ ������, �� � ������� ������ Results.Json() ���������� ��������� ������ �������.

//  ��� ��������� ������� ���� DELETE �� �������� "/api/users/{id}" ����������� ������ �������� �����:

//app.MapDelete("/api/users/{id}", (string id) =>
//{
//    // �������� ������������ �� id
//    Person? user = users.FirstOrDefault(u => u.Id == id);

//    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
//    if (user == null) return Results.NotFound(new { message = "������������ �� ������" });

//    // ���� ������������ ������, ������� ���
//    users.Remove(user);
//    return Results.Json(user);
//});

//  ����� ��������� ����������� ������ - ���� ������ �� Id �� ������, ���������� ��������� ��� 404. ���� �� ������ ������,
//  �� ������� ��� �� ������ � �������� �������.

//  ��� ��������� ������� � ������� POST �� ������ "/api/users" ����������� ��������� �������� �����:

//app.MapPost("/api/users", (Person user) => {

//    // ������������� id ��� ������ ������������
//    user.Id = Guid.NewGuid().ToString();
//    // ��������� ������������ � ������
//    users.Add(user);
//    return user;
//});

//  ������ ���� POST ������������ �������� ���������� ������������ ������. ������ �� �������, ��� ������ �������� ������, �������
//  ������������� ����������� ���� Person. � ������� �������������� ASP.NET Core ������ ������������� ������� �� ��� ������ Person.
//  � ���� ������ �� ������ �������� � �������� ��������� � ����������� �������� �����.

//  ����� ��������� ������ ������������� � ������ ������� �������� Id, ��������� ��� � ������ users � ���������� ������� �������.

//  ���� ���������� �������� PUT-������ �� ������ "/api/users", �� ����������� ������� �������� ������������ �������� ������ � ����
//  ������� Person � �������� ����� �������� ������ � ������ users. ���� ������ �� ������, ���������� ��������� ��� 404. ���� ������
//  ������, �� �������� ��� ������ � ���������� ������� �������:

//app.MapPut("/api/users", (Person userData) => {

//    // �������� ������������ �� id
//    var user = users.FirstOrDefault(u => u.Id == userData.Id);
//    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
//    if (user == null) return Results.NotFound(new { message = "������������ �� ������" });
//    // ���� ������������ ������, �������� ��� ������ � ���������� ������� �������
//    user.Age = userData.Age;
//    user.Name = userData.Name;
//    return Results.Json(user);
//});

//  ����� �������, �� ���������� ���������� API. ������ ������� ��� �������.
#endregion

#region ����������� �������
//  ������ �������� � ������� ����� ����� wwwroot, � ������� ������� ����� ���� index.html

//  ��������� � ����� index.html ��������� ��� ��� �������������� � �������� ASP.NET Core:

//<!DOCTYPE html>
//<html>
//<head>
//����<meta charset="utf-8" />
//����<meta name="viewport" content="width=device-width" />
//����<title>������ �������������</title>
//����<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" />
//</head>
//<body>
//����<h2>������ �������������</h2>
//����<form name="userForm">
//��������<input type="hidden" name="id" value="0" />
//��������<div class="mb-3">
//������������<label class="form-label" for="name">���:</label>
//������������<input class="form-control" name="name" />
//��������</div>
//��������<div class="mb-3">
//������������<label for="age" class="form-label">�������:</label>
//������������<input class="form-control" name="age" />
//��������</div>
//��������<div class="mb-3">
//������������<button type="submit" class="btn btn-sm btn-primary">���������</button>
//������������<a id="reset" class="btn btn-sm btn-primary">��������</a>
//��������</div>
//����</form>
//����<table class="table table-condensed table-striped table-bordered">
//��������<thead><tr><th>���</th><th>�������</th><th></th></tr></thead>
//��������<tbody>
//��������</tbody>
//����</table>

//����<script>
//����// ��������� ���� �������������
//��������async function getUsers() {
//������������// ���������� ������ � �������� �����
//������������const response = await fetch("/api/users", {
//����������������method: "GET",
//����������������headers: { "Accept": "application/json" }
//������������});
//������������// ���� ������ ������ ���������
//������������if (response.ok === true) {
//����������������// �������� ������
//����������������const users = await response.json();
//����������������const rows = document.querySelector("tbody");
//����������������// ��������� ���������� �������� � �������
//����������������users.forEach(user => rows.append(row(user)));
//������������}
//��������}
//��������// ��������� ������ ������������
//��������async function getUser(id) {
//������������const response = await fetch("/api/users/" + id, {
//����������������method: "GET",
//����������������headers: { "Accept": "application/json" }
//������������});
//������������if (response.ok === true) {
//����������������const user = await response.json();
//����������������const form = document.forms["userForm"];
//����������������form.elements["id"].value = user.id;
//����������������form.elements["name"].value = user.name;
//����������������form.elements["age"].value = user.age;
//������������}
//������������else {
//����������������// ���� ��������� ������, �������� ��������� �� ������
//����������������const error = await response.json();
//����������������console.log(error.message); // � ������� ��� �� �������
//������������}
//��������}
//��������// ���������� ������������
//��������async function createUser(userName, userAge) {

//������������const response = await fetch("api/users", {
//����������������method: "POST",
//����������������headers: { "Accept": "application/json", "Content-Type": "application/json" },
//����������������body: JSON.stringify({
//��������������������name: userName,
//��������������������age: parseInt(userAge, 10)
//����������������})
//������������});
//������������if (response.ok === true) {
//����������������const user = await response.json();
//����������������reset();
//����������������document.querySelector("tbody").append(row(user));
//������������}
//������������else {
//����������������const error = await response.json();
//����������������console.log(error.message);
//������������}
//��������}
//��������// ��������� ������������
//��������async function editUser(userId, userName, userAge) {
//������������const response = await fetch("api/users", {
//����������������method: "PUT",
//����������������headers: { "Accept": "application/json", "Content-Type": "application/json" },
//����������������body: JSON.stringify({
//��������������������id: userId,
//��������������������name: userName,
//��������������������age: parseInt(userAge, 10)
//����������������})
//������������});
//������������if (response.ok === true) {
//����������������const user = await response.json();
//����������������reset();
//����������������document.querySelector("tr[data-rowid='" + user.id + "']").replaceWith(row(user));
//������������}
//������������else {
//����������������const error = await response.json();
//����������������console.log(error.message);
//������������}
//��������}
//��������// �������� ������������
//��������async function deleteUser(id) {
//������������const response = await fetch("/api/users/" + id, {
//����������������method: "DELETE",
//����������������headers: { "Accept": "application/json" }
//������������});
//������������if (response.ok === true) {
//����������������const user = await response.json();
//����������������document.querySelector("tr[data-rowid='" + user.id + "']").remove();
//������������}
//������������else {
//����������������const error = await response.json();
//����������������console.log(error.message);
//������������}
//��������}

//��������// ����� ������ ����� ����� ��������
//��������function reset() {
//������������const form = document.forms["userForm"];
//������������form.reset();
//������������form.elements["id"].value = 0;
//��������}
//��������// �������� ������ ��� �������
//��������function row(user) {

//������������const tr = document.createElement("tr");
//������������tr.setAttribute("data-rowid", user.id);

//������������const nameTd = document.createElement("td");
//������������nameTd.append(user.name);
//������������tr.append(nameTd);

//������������const ageTd = document.createElement("td");
//������������ageTd.append(user.age);
//������������tr.append(ageTd);

//������������const linksTd = document.createElement("td");

//������������const editLink = document.createElement("a");
//������������editLink.setAttribute("style", "cursor:pointer;padding:15px;");
//������������editLink.append("��������");
//������������editLink.addEventListener("click", e => {

//����������������e.preventDefault();
//����������������getUser(user.id);
//������������});
//������������linksTd.append(editLink);

//������������const removeLink = document.createElement("a");
//������������removeLink.setAttribute("style", "cursor:pointer;padding:15px;");
//������������removeLink.append("�������");
//������������removeLink.addEventListener("click", e => {

//����������������e.preventDefault();
//����������������deleteUser(user.id);
//������������});

//������������linksTd.append(removeLink);
//������������tr.appendChild(linksTd);

//������������return tr;
//��������}
//��������// ����� �������� �����
//��������document.getElementById("reset").addEventListener("click", e => {

//������������e.preventDefault();
//������������reset();
//��������})

//��������// �������� �����
//��������document.forms["userForm"].addEventListener("submit", e => {
//������������e.preventDefault();
//������������const form = document.forms["userForm"];
//������������const id = form.elements["id"].value;
//������������const name = form.elements["name"].value;
//������������const age = form.elements["age"].value;
//������������if (id == 0)
//����������������createUser(name, age);
//������������else
//����������������editUser(id, name, age);
//��������});

//��������// �������� �������������
//��������getUsers();
//����</script>
//</body>
//</html>

//  �������� ������ ����� ��������� � ���� javascript. ��� �������� �������� � �������� �������� ��� ������� �� �� � �������
//  ������� getUsers():

//async function getUsers()
//{
//    // ���������� ������ � �������� �����
//    const response = await fetch("/api/users", {
//    method: "GET",
//        headers: { "Accept": "application/json" }
//    });
//// ���� ������ ������ ���������
//if (response.ok === true)
//{
//    // �������� ������
//    const users = await response.json();
//    let rows = document.querySelector("tbody");
//    users.forEach(user => {
//        // ��������� ���������� �������� � �������
//        rows.append(row(user));
//    });
//}
//}

//  ��� ���������� ����� � ������� ������������ ������� row(), ������� ���������� ������. � ���� ������ ����� ���������� ������
//  ��� ��������� � �������� ������������.

//  ������ ��� ��������� ������������ � ������� ������� getUser() �������� � ������� ����������� ������������:

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
//    // ���� ��������� ������, �������� ��������� �� ������
//    const error = await response.json();
//    console.log(error.message); // � ������� ��� �� �������
//}
//}

//  � ���������� ������������ ����������� � ����� ��� ��������. ��� �� ����� ����������� � ��� ���������� �������. � ������� ��������
//  ����, ������� ������ id ������������, �� ����� ������, ����� �������� ����������� - ���������� ��� ��������������. ���� id ����� 0,
//  �� ����������� ������� createUser, ������� ���������� ������ � POST-�������:

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

//  ���� �� ����� ������������ ��� �������� �� �����, � � ������� ���� ���������� ��� id, �� ����������� ������� editUser, �������
//  ���������� PUT-������:

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

//  � ������� deleteUser() �������� ���������� ASP.NET Core ������ ���� DELETE �� �������� ������������, � ��� �������� �������� ��
//  ������� ������� ������ �� id �� ������ �������� Person.

//  ������ �������� ������, � �� ��������� ���������� �������� �������� ���-�������� index.html, ������� �������� ������ ��������:

//  ����� ����� �� ������ ��������� ��� ������� �������� � �������������� - ���������, ����������, ���������, ��������. 
#endregion