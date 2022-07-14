//  �������� �������� � ������� � Entity Framework Core

//  ����� � ��� ������� ��������� ����� User, ������� ����� ������������ ������:

//public class User
//{
//    public int Id { get; set; }
//    public string Name { get; set; } = ""; // ��� ������������
//    public int Age { get; set; } // ������� ������������
//}

//  ��� �������������� � ����� ������ MS SQL Server � �������� ��������� ������ ��������� ��������� ����� ApplicationContext:

//using Microsoft.EntityFrameworkCore;
//public class ApplicationContext : DbContext
//{
//    public DbSet<User> Users { get; set; } = null!;
//    public ApplicationContext(DbContextOptions<ApplicationContext> options)
//        : base(options)
//    {
//        Database.EnsureCreated();   // ������� ���� ������ ��� ������ ���������
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

//  ����� � ����� Program.cs ��������� �������� ��� ����������, ������� ����� ������������ ������� � ������������ � ���� ������:

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
    // �������� ������������ �� id
    User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (user == null) return Results.NotFound(new { message = "������������ �� ������" });

    // ���� ������������ ������, ���������� ���
    return Results.Json(user);
});

app.MapDelete("/api/users/{id:int}", async (int id, ApplicationContext db) =>
{
    // �������� ������������ �� id
    User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (user == null) return Results.NotFound(new { message = "������������ �� ������" });

    // ���� ������������ ������, ������� ���
    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.Json(user);
});

app.MapPost("/api/users", async (User user, ApplicationContext db) =>
{
    // ��������� ������������ � ������
    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
    return user;
});

app.MapPut("/api/users", async (User userData, ApplicationContext db) =>
{
    // �������� ������������ �� id
    var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userData.Id);

    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (user == null) return Results.NotFound(new { message = "������������ �� ������" });

    // ���� ������������ ������, �������� ��� ������ � ���������� ������� �������
    user.Age = userData.Age;
    user.Name = userData.Name;
    await db.SaveChangesAsync();
    return Results.Json(user);
});

app.Run();

//  ������� ��������� ����� ApplicationContext � ������� ����������:
//var builder = WebApplication.CreateBuilder();
//string connection = "Server=(localdb)\\mssqllocaldb;Database=applicationdb;Trusted_Connection=True;";
//builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

//  ����� ����� �������� ������� WebApplication ���������� ���������������� ����������� ������:
//app.UseDefaultFiles();
//app.UseStaticFiles();

//  ����� � ������� ������� MapGet/MapPost/MapPut/MapDelete ������������ ����� �������� �����, ������� ����� ������������
//  ������ ���� ��������.

//  ������ �������� ����� ������������ ������ ���� GET �� �������� "api/users":
//  app.MapGet("/api/users", async (ApplicationContext db) => await db.Users.ToListAsync());

//  ��������� ���� � ���� �������� ������ ApplicationContext ��� �������� � �������� �������, �� �� ����� ��� �������� �����
//  �������� ����������� �������� ����� � ����� ���������� �������� ������ �������� �� �� ������ �������� User � ��������� �� �������.

//  ����� ������ ���������� � ���������� ��� ��������� ������ ������� �� id � ������ ���� GET �� ������ "api/users/{id}", ��
//  ����������� ������ �������� �����:

//app.MapGet("/api/users/{id:int}", async (int id, ApplicationContext db) =>
//{
//    // �������� ������������ �� id
//    User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

//    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
//    if (user == null) return Results.NotFound(new { message = "������������ �� ������" });

//    // ���� ������������ ������, ���������� ���
//    return Results.Json(user);
//});

//  ����� ����� �������� id �������� �� ���� ������� ������������� ������� User � �� ����� �������������� ���� ������ ������ � ����
//  ������, ��������� �������� ������ ApplicationContext. ���� ������ �� Id �� ��� ������, �� ���������� � ������� ������
//  Results.NotFound() ��������� ��� 404 � ��������� ���������� � ������� JSON. ���� ������ ������, �� � ������� ������ Results.Json()
//  ���������� ��������� ������ �������.

//  ��� ��������� ������� ���� DELETE �� �������� "/api/users/{id}" ����������� ������ �������� �����:

//app.MapDelete("/api/users/{id:int}", async (int id, ApplicationContext db) =>
//{
//    // �������� ������������ �� id
//    User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

//    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
//    if (user == null) return Results.NotFound(new { message = "������������ �� ������" });

//    // ���� ������������ ������, ������� ���
//    db.Users.Remove(user);
//    await db.SaveChangesAsync();
//    return Results.Json(user);
//});

//  ����� ���� ������ �� Id �� ������ � ���� ������, �� ���������� ��������� ��� 404. ���� �� ������ ������, �� � �������
//  ������ db.Users.Remove(user) ���������, ��� ������ ������ ���� ������� �� ��. � � ������� ������������ ������ db.SaveChangesAsync()
//  ��������� ��������� � ���� ������ (�� ���� ������� ������). � � ����� �������� ��������� ������ �������.

//  ��� ��������� ������� � ������� POST �� ������ "/api/users" ����������� ��������� �������� �����:

//app.MapPost("/api/users", async (User user, ApplicationContext db) =>
//{
//    // ��������� ������������ � ������
//    await db.Users.AddAsync(user);
//    await db.SaveChangesAsync();
//    return user;
//});

//����� �� �������, ��� � ������� ���� POST ������ ����� ���������� �� ������ ������, ������� ������������� ����������� ���� User.
//� ������� �������������� ASP.NET Core ������ ������������� ������� �� ��� ������ User. � ���� ������ �� ������ �������� � ��������
//��������� � ����������� �������� ����� ������ � �������� ApplicationContext.

//����� ��������� ������� User � ������� ������ db.Users.AddAsync(user) ���������, ��� ������ ������ ���� �������� � ��. � � �������
//������������ ������ db.SaveChangesAsync() ��������� ��������� � ���� ������ (�� ���� ��������� ������). ����� ���������� ����������
//������ User ������� �������.

//���� ���������� �������� PUT-������ �� ������ "/api/users", �� ������ ������������ ��������� �������� �����:

//app.MapPut("/api/users", async (User userData, ApplicationContext db) =>
//{
//    // �������� ������������ �� id
//    var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userData.Id);

//    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
//    if (user == null) return Results.NotFound(new { message = "������������ �� ������" });

//    // ���� ������������ ������, �������� ��� ������ � ���������� ������� �������
//    user.Age = userData.Age;
//    user.Name = userData.Name;
//    await db.SaveChangesAsync();
//    return Results.Json(user);
//});

//  ����� ����������� ������� �������� ������������ �������� ������ � ���� ������� User � ������ ApplicationContext. ����� ��������
//  ����� �������� ������ � ���� ������. ���� ������ �� ������, ���������� ��������� ��� 404. ���� ������ ������, �� �������� ��� ������,
//  � ������� ������ db.SaveChangesAsync() ��������� ��������� � ���� ������ � ���������� ���������� ������ ������� �������

//  ������ ������� ��� �������. ��� ����� �������� � ������� ����� ����� wwwroot, � ������� ������� ����� ���� index.html

//  ��������� � ����� index.html ��������� ��� ��� �������������� � ���-����������� ASP.NET Core:

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

//  �������� ������ ����� ��������� � ���� javascript. ��� �������� �������� � �������� �������� ��� ������� �� �� � ������� �������
//  getUsers():

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

//  ��� ���������� ����� � ������� ������������ ������� row(), ������� ���������� ������. � ���� ������ ����� ���������� ������ ���
//  ��������� � �������� ������������.

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

//  � ������� deleteUser() �������� ���������� ASP.NET Core ������ ���� DELETE �� �������� ������������, � ��� �������� �������� ��
//  ������� ������� ������������ �� id �� ������� �������������.

//  ������ �������� ������, � �� ��������� ���������� �������� �������� ���-�������� index.html, ������� �������� ������ ��������:

//  ����� ����� �� ������ ��������� ��� ������� �������� � �������������� - ���������, ����������, ���������, ��������