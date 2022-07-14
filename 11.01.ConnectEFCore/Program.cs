//  ������ � ����� ������
//  ����������� Entity Framework Core

//  Entity Framework ������������ ORM-�������, ������� ��������� ������������� ������� ������ ����� C# � ��������� � ���� ������.
//  Entity Framework Core ������������ ����������� ���������� ����, ����� ��� MS SQL Server, SQLite, MySQL, Postres. � ������ ������
//  �� ����� �������� ����� Entity Framework Core � ������ ������ � MS SQL Server.

//  �������� ������������ � ������� � Entity Framework Core ����� � ��������������� �����������, ����� �� �� �������������� ������
//  ����� �� ��� ��������, ������� ���������� ��� ���-���������� ASP.NET Core.

//  ��� ������ � ������ ������ MS SQL Server ����� Entity Framework Core ��������� ����� Microsoft.EntityFrameworkCore.SqlServer.
//  �� ��������� �� ����������� � �������, ������� ��� ���� ��������, ��������, ����� �������� �������� Nuget:

//  ����� ������� � ������ �����, ������� ����� ������������ ������. ����� �� ����� ���������� User � ����� ����� ��������� ���:

//public class User
//{
//    public int Id { get; set; }
//    public string Name { get; set; } = "";
//    public int Age { get; set; }
//}
//  ���� ����� ������������ �� �������, ������� ����� ��������� � ���� ������.

//  ��� �������������� � ����� ������ ����� Entity Framework Core ��������� �������� ������ - �����, �������������� �� ������
//  Microsoft.EntityFrameworkCore.DbContext. ������� ������� � ������ ����� �����, ������� ������� ApplicationContext (��������
//  ������ ��������� ������������):

//using Microsoft.EntityFrameworkCore;

//public class ApplicationContext : DbContext
//{
//    public DbSet<User> Users { get; set; } = null!;
//    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
//    {
//        Database.EnsureCreated(); // ������� ���� ������ ��� ������ ���������
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

//  �������� DbSet ������������ ����� ��������� ��������, ������� �������������� � ������������ �������� � ���� ������. �� ����
//  �������� Users ����� ������������ �������, � ������� ����� ��������� ������� User.

//  ����� DbSet, ��� � ������ ����, �������� ���������. �, ������� � C# 10 � .NET 6 ������������� ����������� ����������������
//  ��������� nullable-�����. � ����������/�������� ��� �����, ������� �� �������� nullable, ������� ���������������� ���������
//  ��������� ����� �� ��������������. ����� ����� �� ���� �������� �� ����� ���������������� �������� � ������� ��������� null!,
//  ������� �������, ��� ������ �������� � �������� �� ����� ����� �������� null. ������ ��� � ���������� ����������� ��������
//  ������ DbContext �����������, ��� ��� �������� ���� DbSet ����� ���������������� � �������������� � �������� �� ����� �����
//  �������� null.

//  � ������������ ������ ApplicationContext ����� �������� options ����� ������������ ��������� ��������� ������. � � �����
//  ������������ � ������� ������ Database.EnsureCreated() �� ����������� ������ ����� ����������� ���� ������ (���� ��� �����������).

//  ����� ����, � ������ OnModelCreating() ������������� ��������� ��������� ������������ ������. � ���������, � ������� ������
//  modelBuilder.Entity<User>().HasData() � ���� ������ ����������� ��� ��������� �������

#region ������������ � ������ �����������
//  ����� ������������ � ���� ������, ��� ���� ������ ��������� �����������. ��� ����� ������� ���� � ���� C#, ���� � ����� ������������.
//  ������� ������ ������. ��� ����� ������� ���� appsettings.json. ������� ���, ������� ����������� ������ �����������:

//{
//    "ConnectionStrings": {
//        "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=applicationdb;Trusted_Connection=True;"
//    },
//  "Logging": {
//        "LogLevel": {
//            "Default": "Information",
//      "Microsoft.AspNetCore": "Warning"
//        }
//    },
//  "AllowedHosts": "*"
//}

//  � ������ ������ �� ����� ������������ ���������� ������ ���� ������ LocalDB, ������� ������������ ����������� ������ SQL
//  Server Express, ��������������� ���������� ��� ���������� ����������. �� ���� ������� �������� Server=(localdb)\\mssqllocaldb.
//  �� � ���� ���� ������ ����� ���������� applicationdb.
#endregion

#region ���������� ��������� ������ EF Core � �������� �������
//  �������� ������, ����� ������� ���� �������������� � ����� ������, ���������� � ���������� � �������� ������� ����� �������� ���������
//  ������������. ������� ��� ������ � Entity Framework Core ���������� �������� ����� ��������� ������ � ��������� �������� ����������:

//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder();

//// �������� ������ ����������� �� ����� ������������
//string connection = builder.Configuration.GetConnectionString("DefaultConnection");

//// ��������� �������� ApplicationContext � �������� ������� � ����������
//builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

//var app = builder.Build();

//app.Run();

//  ��� ������������� ��������� ������ ��� ���� �������� ������ �����������, ������� ���� ���� ���������� � ���� ������������
//  appsettings.json. ������� ������� ��������� ������ ����������� ��� ��������� "DefaultConnection":
//  string connection = builder.Configuration.GetConnectionString("DefaultConnection");

//  ��� ��������� ������ ������������ ��� ������� IConfiguration, ������� ������������ ������������, ��������� ����� ����������
//  GetConnectionString(), � ������� ���������� �������� ������ �����������.

//  ��� ���������� ��������� ������ � �������� ������� � ������� ��������� �������� IServiceCollection ��������� ����� AddDbContext(),
//  ������� ������������ ������� ��������� ������:
//  builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

//  ���� ����� � �������� ��������� ��������� �������, ������� ����������� �����������. � ���������, � ������� ������
//  options.UseSqlServer() ������������� ����������� � ������� MS SQL Server, � � �������� ��������� � ���� ����� ����������
//  ������ �����������.
#endregion

#region ��������� � ���� ������
//  ��������� �������� ������ �� ����� � ��� ������� ����������� � ���� ������. ��������, � �������� ����� ������� �����������
//  �� ��������� ������:

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

// ��������� ������
app.MapGet("/", (ApplicationContext db) => db.Users.ToList());

app.Run();

//  ��������� �������� ������ �������� � �������, �� �� ����� ��� �������� � ����������� �������� �����:
//      app.MapGet("/", (ApplicationContext db) => db.Users.ToList());

//  � ������ ������ ������� ������������ ������ ����������� � �� �� ��������� �������� User � ������� JSON
#endregion