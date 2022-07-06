//  Загрузка файлов на сервер

//  Рассмотрим, как загружать файлы на сервер в ASP.NET Core. Все загружаемые файлы в ASP.NET Core представлены типом IFormFile из
//  пространства имен Microsoft.AspNetCore.Http. Соответственно для получения отправленного файла в контроллере необходимо использовать
//  IFormFile. Затем с помощью методов IFormFile мы можем произвести различные манипуляции файлом - получит его свойства, сохранить,
//  получить его поток и т.д. Некоторые его свойства и методы:
//  ContentType: тип файла
//  FileName: название файла
//  Length: размер файла
//  CopyTo / CopyToAsync: копирует файл в поток
//  OpenReadStream: открывает поток файла для чтения

//  Для тестирования данной возможности определим в проекте папку html, в которой создадим файл index.html

//< !DOCTYPE html >
//< html >
//< head >
//    < meta charset = "utf-8" />
//    < meta name = "viewport" content = "width=device-width" />
//    < title > METANIT.COM </ title >
//</ head >
//< body >
//    < h2 > Выберите файл для загрузки</h2>
//    <form action="upload" method="post" enctype="multipart/form-data">
//        <input type="file" name="uploads" /><br>
//        <input type="file" name="uploads" /><br>
//        <input type="file" name="uploads" /><br>
//        <input type="submit" value="Загрузить" />
//    </form>
//</body>
//</html>

//  В данном случае форма содержит набор элементов с типом file, через которые можно выбрать файлы для загрузки. В данном случае
//  на форме три таких элемента, но их может быть и меньше и больше. А благодаря установке атрибута формы enctype="multipart/form-data"
//  браузер будет знать, что вместе с формой надо передать файлы.

//  Отправляться файлы будут в запросе типа POST на адрес "/upload".

//  Теперь в файле Program.cs определим код, который будет получать загружаемые файлы:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();


app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;

    response.ContentType = "text/html; charset=utf-8";
    if (request.Path == "/upload" && request.Method == "POST")
    {
        IFormFileCollection files = request.Form.Files;
        // путь к папке, где будут храниться файлы
        var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
        // создаем папку для хранения файлов
        Directory.CreateDirectory(uploadPath);

        foreach (var file in files)
        {
            // путь к папке uploads
            string fullPath = $"{uploadPath}/{file.FileName}";

            // сохраняем файл в папку uploads
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }
        await response.WriteAsync("Файлы успешно загружены");
    }
    else
    {
        await response.SendFileAsync("html/index.html");
    }
});

app.Run();

//  Здесь если запрос приходит по адресу "/upload", а сам запрос представляет запрос типа POST, то приложение получает коллекцию
//  загруженных файлов с помощью свойства Request.Form.Files, которое представляет тип IFormFileCollection:
//  IFormFileCollection files = request.Form.Files;

//  Далее определяем каталог для загружаемых файлов (предполагается, что файлы будут храниться в каталоге "uploads", которая
//  располагается в папке приложения)
//  var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";

//  Если такой папки нет, то создаем ее. Затем перебираем всю коллекцию файлов.
//  foreach (var file in files)

//  Каждый отдельный файл в этой коллекции представляет тип IFormFile. Для копирования файла в нужный каталог создается поток
//  FileStream, в который записывается файл с помощью метода CopyToAsync.
//  using (var fileStream = new FileStream(fullPath, FileMode.Create))
//  {
//      await file.CopyToAsync(fileStream);
//  }

//  Если запрос идет по другому адресу и/или не представляет тип POST, то отправляем клиенту html-страницу index.html.