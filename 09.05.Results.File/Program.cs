//  Отправка файлов в Results API

//  Для отправки файлов с помощью Results API может применяться метод File(), который имеет ряд версий.

#region Отправка файла как массива байтов
//  Для отправки файла как массива байтов применяется следующая версия метода:

//  public static IResult File (byte[] fileContents, string? contentType = default, 
//  string? fileDownloadName = default, bool enableRangeProcessing = false,
//  DateTimeOffset? lastModified = default, Microsoft.Net.Http.Headers.EntityTagHeaderValue? entityTag = default);

//  Параметры метода:
//      fileContents: содержимое файла в виде массива байтов
//      contentType: тип содержимого файла - заголовок Content-Type
//      fileDownloadName: имя файла для загрузки
//      enableRangeProcessing: значение типа bool. Если оно равно true, то допустима обработка данных по частям
//      lastModified: значение типа Nullable<DateTimeOffset>>, которое указывает на дату последнего изменения файла
//      entityTag: значение типа EntityTagHeaderValue, которое ассоциировано с файлом

//  Например, создадим в проекте новую папку Files, в которую добавим какой-нибудь файл. Например, в моем случае это файл forest.jpg:

//  Для отправки файла определим следующий код:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/forest", async () =>
//{
//    string path = "Files/forest.jpg";
//    byte[] fileContent = await File.ReadAllBytesAsync(path);  // считываем файл в массив байтов
//    string contentType = "image/jpg";       // установка mime-типа
//    string downloadName = "magic_forest.jpg";  // установка загружаемого имени
//    return Results.File(fileContent, contentType, downloadName);
//});

//app.Map("/", () => "Hello World");

//app.Run();

//  И при обращении по пути "/forest" пользователю будет отправлен файл forest.png
#endregion

#region Отправка в виде файлового потока
//  Еще одна версия метода File() позволяет отправить данные в виде потока Stream:

//  public static IResult File(System.IO.Stream fileStream, string? contentType = default,
//  string? fileDownloadName = default, DateTimeOffset? lastModified = default,
//  Microsoft.Net.Http.Headers.EntityTagHeaderValue? entityTag = default, bool enableRangeProcessing = false);

//  Здесь меняется только первый параметр, который в данной версии представляет содержимое файла в виде объекта Stream.
//  Остальные параметры остаются те же, что и в предыдущей версии

//  Также, как и в предыдущем примере, отправим файл forest.png из папки Files:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/forest", () =>
//{
//    string path = "Files/forest.jpg";
//    FileStream fileStream = new FileStream(path, FileMode.Open); 
//    string contentType = "image/jpg";       // установка mime-типа
//    string downloadName = "magic_forest.jpg";  // установка загружаемого имени
//    return Results.File(fileStream, contentType, downloadName);
//});

//app.Map("/", () => "Hello World");

//app.Run();
#endregion

#region Отправка файла по определенному пути
//  Если у нас есть путь к файлу, то проще использовать третью версию метода File(), которая в качествве первого параметра
//  принимает путь к файлу:

//  public static IResult File(string path, string? contentType = default,
//  string? fileDownloadName = default, DateTimeOffset? lastModified = default,
//  Microsoft.Net.Http.Headers.EntityTagHeaderValue? entityTag = default, bool enableRangeProcessing = false);

//  Остальные параметры те же, что в ранее рассмотренных версиях метода. Однако стоит отметить, что данная версия метода вычисляет
//  пути к файлам относительно папки, которая задается параметром WebRootPath - по умолчанию это папка wwwroot. То есть файл для
//  отправки по умолчанию должен располагаться в этой папке:

//  Остальные параметры те же, что в ранее рассмотренных версиях метода. Однако стоит отметить, что данная версия метода
//  вычисляет пути к файлам относительно папки, которая задается параметром WebRootPath - по умолчанию это папка wwwroot.
//  То есть файл для отправки по умолчанию должен располагаться в этой папке:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/forest", () =>
//{
//    string path = "Files/forest.jpg";
//    string contentType = "image/jpg";
//    string downloadName = "magic_forest.jpg";
//    return Results.File(path, contentType, downloadName);
//});

//app.Map("/", () => "Hello World");

//app.Run();

//  Но это поведение мы можем переопределить. Допустим, в проекте есть папка Files. И мы хотим, что приложение автоматически
//  подхватывало из нее файлы.

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions { WebRootPath = "Files" });  // добавляем папку для хранения файлов
var app = builder.Build();

app.Map("/forest", () =>
{
    string path = "forest.jpg";
    string contentType = "image/jpeg";
    string downloadName = "forest.jpg";
    return Results.File(path, contentType, downloadName);
});

app.Map("/", () => "Hello World");

app.Run();
#endregion