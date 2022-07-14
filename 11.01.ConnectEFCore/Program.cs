//  Работа с базой данных
//  Подключение Entity Framework Core

//  Entity Framework представляет ORM-решение, которое позволяет автоматически связать классы языка C# с таблицами в базе данных.
//  Entity Framework Core поддерживает большинство популярных СУБД, таких как MS SQL Server, SQLite, MySQL, Postres. В данном случае
//  мы будем работать через Entity Framework Core с базами данных в MS SQL Server.

//  Детально ознакомиться с работой с Entity Framework Core можно в соответствуюшем руководстве, здесь же мы сосредоточимся прежде
//  всего на тех моментах, которые характерны для веб-приложения ASP.NET Core.

//  Для работы с базами данных MS SQL Server через Entity Framework Core необходим пакет Microsoft.EntityFrameworkCore.SqlServer.
//  По умолчанию он отсутствует в проекте, поэтому его надо добавить, например, через пакетный менеджер Nuget:

//  Далее добавим в проект класс, которые будет представлять данные. Пусть он будет называться User и будет иметь следующий код:

//public class User
//{
//    public int Id { get; set; }
//    public string Name { get; set; } = "";
//    public int Age { get; set; }
//}
//  Этот класс представляет те объекты, которые будут храниться в базе данных.

//  Для взаимодействия с базой данных через Entity Framework Core необходим контекст данных - класс, унаследованный от класса
//  Microsoft.EntityFrameworkCore.DbContext. Поэтому добавим в проект новый класс, который назовем ApplicationContext (название
//  класса контекста произвольное):

//using Microsoft.EntityFrameworkCore;

//public class ApplicationContext : DbContext
//{
//    public DbSet<User> Users { get; set; } = null!;
//    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
//    {
//        Database.EnsureCreated(); // создаем базу данных при первом обращении
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

//  Свойство DbSet представляет собой коллекцию объектов, которая сопоставляется с определенной таблицей в базе данных. То есть
//  свойство Users будет представлять таблицу, в которой будут храниться объекты User.

//  Класс DbSet, как и другие типы, является ссылочным. А, начиная с C# 10 и .NET 6 автоматически применяется функциональность
//  ссылочных nullable-типов. И переменные/свойства тех типов, которые не являются nullable, следует инициализировать некотором
//  значением перед их использованием. Чтобы выйти из этой ситуации мы можем инициализировать свойство с помощью выражения null!,
//  которое говорит, что данное свойство в принципе не будет иметь значение null. Потому что в реальности конструктор базового
//  класса DbContext гарантирует, что все свойства типа DbSet будут инициализированы и соответственно в принципе не будут иметь
//  значение null.

//  В конструкторе класса ApplicationContext через параметр options будут передаваться настройки контекста данных. А в самом
//  конструкторе с помощью вызова Database.EnsureCreated() по определению модели будет создаваться база данных (если она отсутствует).

//  Кроме того, в методе OnModelCreating() настраивается некоторая начальная конфигурация модели. В частности, с помощью метода
//  modelBuilder.Entity<User>().HasData() в базу данных добавляются три начальных объекта

#region Конфигурация и строка подключения
//  Чтобы подключаться к базе данных, нам надо задать параметры подключения. Это можно сделать либо в коде C#, либо в файле конфигурации.
//  Выберем второй способ. Для этого изменим файл appsettings.json. Изменим его, добавив определение строки подключения:

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

//  В данном случае мы будем использовать упрощенный движок базы данных LocalDB, который представляет легковесную версию SQL
//  Server Express, предназначенную специально для разработки приложений. Об этом говорит параметр Server=(localdb)\\mssqllocaldb.
//  Ну а сама база данных будет называться applicationdb.
#endregion

#region Добавление контекста данных EF Core в качестве сервиса
//  Контекст данных, через который идет взаимодействие с базой данных, передается в приложение в качестве сервиса через механизм внедрения
//  зависимостей. Поэтому для работы с Entity Framework Core необходимо добавить класс контекста данных в коллекцию сервисов приложения:

//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder();

//// получаем строку подключения из файла конфигурации
//string connection = builder.Configuration.GetConnectionString("DefaultConnection");

//// добавляем контекст ApplicationContext в качестве сервиса в приложение
//builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

//var app = builder.Build();

//app.Run();

//  Для использования контекста данных ему надо передать строку подключения, которая выше была определена в файл конфигурации
//  appsettings.json. Поэтому сначала считываем строку подключения под названием "DefaultConnection":
//  string connection = builder.Configuration.GetConnectionString("DefaultConnection");

//  Для получения строки конфигурации для объекта IConfiguration, который представляет конфигурацию, определен метод расширения
//  GetConnectionString(), в который передается название строки подключения.

//  Для добавления контекста данных в качестве сервиса у объекта коллекции сервисов IServiceCollection определен метод AddDbContext(),
//  который типизируется классом контекста данных:
//  builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

//  Этот метод в качестве параметра принимает делегат, который настраивает подключение. В частности, с помощью вызова
//  options.UseSqlServer() настраивается подключение к серверу MS SQL Server, а в качестве параметра в этот вызов передается
//  строка подключения.
#endregion

#region Обращение к базе данных
//  Подключив контекст данных мы можем с его помощью обращаеться к базе данных. Например, в качестве теста получим добавляемые
//  по умолчанию данные:

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

// получение данных
app.MapGet("/", (ApplicationContext db) => db.Users.ToList());

app.Run();

//  Поскольку контекст данных добавлен в сервисы, то мы можем его получить в обработчике конечной точки:
//      app.MapGet("/", (ApplicationContext db) => db.Users.ToList());

//  В данном случае клиенту отправляется список добавленных в БД по умолчанию объектов User в формате JSON
#endregion