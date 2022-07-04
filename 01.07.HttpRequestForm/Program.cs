//  Отправка форм

//  Нередко данные отправляются на сервер с помощью форм html, обычно в запросе типа POST. Для получения подобных
//  данных в классе HttpRequest определено свойство Form. Рассмотрим, как мы можем получить подобные данные.
//  Прежде всего определим в проекте в папке html файл index.html. В нём определена форма условно для ввода данных
//  пользователя, которая в запросе типа POST (атрибут method="post") отправляет данные по адресу "postuser"
//  (атрибут action="postuser"). На форме определены два поля ввода. Первое поле предназначено для ввода имени
//  пользователя. Второе поле - для ввода возроста пользователя.

//  Для получения этих данных определим в файле Program.cs следующий код:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async context => 
//{
//    context.Response.ContentType = "text/html; charset=utf-8";

//    // если обращение идет по адресу "/postuser", получаем данные формы
//    if (context.Request.Path == "/postuser")
//    {
//        var form = context.Request.Form;
//        string name = form["name"];
//        string age = form["age"];
//        await context.Response.WriteAsync($"<din><p>Name: {name}</p><p>Age: {age}</p></div>");
//    }
//    else
//    {
//        await context.Response.SendFileAsync("html/index.html");
//    }
//});
//app.Run();

//  Здесь, если запрошен адрес "/postuser", то предполагается, что отправлена некоторая форма. Сначала получаем
//  отправленную форму в переменную form:
//  var form = context.Request.Form;
//  Свойство Request.Form возвращает объект IFormCollection - своего рода словарь, где по ключу можно получить
//  значение элемента.При этом в качестве ключей выступает названия полей форм (значения атрибутов name элементов формы):
//  /< input name = "age" type = "number" />
//  Так, в данном случае название поля (значение атрибута name) равно "age".Соответственно в Request.Form по этому
//  имени мы можем получить его значение:
//  string age = form["age"];
//  После получения данных формы они отправляются обратно клиенту

#region Получение массивов
//  Усложним задачу и добавим в форму на странице index.html несколько полей, которые будут представлять массив.
//<!DOCTYPE html>
//< html >
//< head >
//    < meta charset = "utf-8" />
//    < title > METANIT.COM </ title >
//</ head >
//< body >
//    < h2 > User form </ h2 >
//    < form method = "post" action = "postuser" >
//        < p > Name: < br />
//            < input name = "name" />
//        </ p >
//        < p > Age: < br />
//            < input name = "age" type = "number" />
//        </ p >
//        < p >
//            Languages:< br />
//            < input name = "languages" />< br />
//            < input name = "languages" />< br />
//            < input name = "languages" />< br />
//        </ p >
//        < input type = "submit" value = "Send" />
//    </ form >
//</ body >
//</ html >
//  Добавлено три поля ввода, которые имеют одно и то же имя. Поэтому при их отправке будет формироваться массив
//  из трех значений. Теперь получим эти значения в коде C#:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";

    // если обращение идет по адресу "/postuser", получаем данные формы
    if (context.Request.Path == "/postuser")
    {
        var form = context.Request.Form;
        string name = form["name"];
        string age = form["age"];
        string[] languages = form["languages"];
        // создаем из массива languages одну строку
        string langList = "";
        foreach (var lang in languages)
        {
            langList += $" {lang}";
        }
        await context.Response.WriteAsync($"<div><p>Name: {name}</p>" +
            $"<p>Age: {age}</p>" +
            $"<div>Languages:{langList}</ul></div>");
    }
    else
    {
        await context.Response.SendFileAsync("html/index.html");
    }
});
app.Run();
//  Поскольку параметр "languages" представляет массив, то и сопоствляться он будет с массивом строк


//  Подобным образом можно передавать значения массива полей других типов, либо полей, которые представляют набор
//  элементов, например, элемента select, который поддерживает множественный выбор
//< !DOCTYPE html >
//< html >
//< head >
//    < meta charset = "utf-8" />
//    < title > METANIT.COM </ title >
//</ head >
//< body >
//    < h2 > User form </ h2 >
//    < form method = "post" action = "postuser" >
//        < p > Name: < br />
//            < input name = "name" />
//        </ p >
//        < p > Age: < br />
//            < input name = "age" type = "number" />
//        </ p >
//        < p >
//            Languages:< br />
//            < select multiple name = "languages" >
//                < option > C#</option>
//                < option > JavaScript </ option >
//                < option > Kotlin </ option >
//                < option > Java </ option >
//             </ select >
//        </ p >
//        < input type="submit" value="Send" />
//    </form>
//</body>
//</html>
#endregion
