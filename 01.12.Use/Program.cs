//  Метод Use

//  Метод расширения Use() добавляет компонент middleware, который позволяет передать обработку запроса далее следующим в конвейере
//  компонентам. Он имеет следующие версии

//  public static IApplicationBuilder Use(this IApplicationBuilder app, Func<HttpContext, Func<Task>, Task> middleware);
//  public static IApplicationBuilder Use(this IApplicationBuilder app, Func<HttpContext, RequestDelegate, Task> middleware)

//  Метод Use() реализован как метод расширения для типа IApplicationBuilder, соответственно мы можем вызвать данный метод у объекта
//  WebApplication для добавления middleware в приложение. В обоих версиях метод Use принимает некоторое действие, которое имеет два
//  параметра и возвращает объект Task.

//  Первый параметр делегата Func, который передается в метод Use(), представляет объект HttpContext. Этот объект позволяет получить
//  данные запроса и управлять ответом клиенту.

//  Второй параметр делегата представляет другой делегат - Func<Task> или RequestDelegate. Этот делегат представляет следующий в
//  конвейере компонент middleware, которому будет передаваться обработка запроса.

//  В общем случае применение метода Use() выглядит следующим образом

//app.Use(async (context, next) =>
//{
//    // действия перед передачи запроса в следующий middleware
//    await next.Invoke();
//    // действия после обработки запроса следующим middleware
//});

//  Работа middleware разбивается на две части:
//  Middleware выполняет некоторую начальную обработку запроса до вызова await next.Invoke()
//  Затем вызывается метод next.Invoke(), который передает обработку запроса следующему компоненту в конвейере
//  Когда следующий в конвейере компонент закончил обработку запрос возвращается в обратно в текущий компонент, и выполняются действия,
//  которые идут после вызова await next.Invoke(

//  Таким образом, middleware в методе Use выполняет действия до следующего в конвейере компонента и после него.

//  Рассмотрим метод Use() на примере:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();
//WebApplication app = builder.Build();

//string date = "";

//app.Use(async (context, next) =>
//{
//    date = DateTime.Now.ToShortDateString();
//    await next.Invoke();   // вызываем middleware из app.Run
//    Console.WriteLine($"Current date: {date}");
//});

//app.Run(async context =>
//{
//    await context.Response.WriteAsync($"Date: {date}");
//});

//app.Run();

//  В данном случае мы используем перегрузку метода Use, которая в качестве параметров принимает контекст запроса - объект HttpContext
//  и делегат Func<Task>, который представляет собой ссылку на следующий в конвейере компонент middleware.

//  Middleware в методе app.Use() реализует простейшую задачу - присваивает переменной date текущую дату в виде строки и затем передает
//  обработку запроса следующим компонентам middleware в конвейере. То есть при вызове await next.Invoke() обработка запроса перейдет
//  к тому компоненту, который установлен в методе app.Run(). В итоге обработка запроса будет выглядеть следующим образом:

//      Вызов компонента app.Use

//      Установка значения переменной date:
//      date = DateTime.Now.ToShortDateString();

//      Вызов await next.Invoke(). Управление переходит следующему компоненту в конвейере - к app.Run.

//      В middleware из app.Run() отравляет клиенту текущую дату в качестве ответа с помощью метода context.Response.WriteAsync():
//      await context.Response.WriteAsync($"Date: {date}");

//      Метод app.Run закончил свою работу, и управление обработкой возвращается к middleware в методе app.Use.Начинает выполняться та
//      часть кода, которая идет после await next.Invoke(). В этой части выполняется условное логгирование - на консоль выводится значение
//      переменной date:
//      Console.WriteLine($"Current date: {date}");

//      После этого обработка запроса завершена

#region Отправка ответа
//  При использовании метода Use и передаче выполнения следующему делегату следует учитывать, что не рекомендуется вызывать метод
//  next.Invoke после метода Response.WriteAsync(). Компонент middleware должен либо генерировать ответ с помощью Response.WriteAsync,
//  либо вызывать следующий делегат посредством next.Invoke, но не выполнять оба этих действия одновременно. Так как согласно
//  документации последующие изменения объекта Response могут привести к нарушению протокола, например, будет послано больше байт,
//  чем указано в заголовке Content-Length, либо могут привести к нарушению тела ответа, например, футер страницы HTML запишется в
//  CSS-файл.

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("<p>Hello world!</p>");
//    await next.Invoke();
//});

//app.Run(async (context) =>
//{
//    //await Task.Delay(10000); // можно поставить задержку
//    await context.Response.WriteAsync("<p>Good bye, World...</p>");
//});

//app.Run();
#endregion

#region Использование делегат RequestDelegate
//  В примере выше использовалась версия метода Use(), которая использует делегат Func<Task>. Подобным образом можно использовать
//  и другую версию, где используется делегат RequestDelegate. Единственное - при вызове делегата ( то есть фактически следующего
//  в конвейере компонента) необходимо передавать делегату объект HttpContext:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//string date = "";

//app.Use(async (context, next) =>
//{
//    date = DateTime.Now.ToShortDateString();
//    await next.Invoke(context);                 // здесь next - RequestDelegate
//    Console.WriteLine($"Current date: {date}");  // Current date: 08.12.2021
//});

//app.Run(async (context) => await context.Response.WriteAsync($"Date: {date}"));

//app.Run();
#endregion

#region Терминальный компонент middleware
//  Middleware в методе Use() необязательно должен взывать к следующему в конвейере компоненту. Вместо этого он может завершить
//  обработку запроса. В этом случае он может выступать в роли такого же терминального компонента middleware, как и компоненты из
//  метода Run(). Например:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

string date = "";

app.Use(async (context, next) =>
{
    string? path = context.Request.Path.Value?.ToLower();
    if (path == "/date")
    {
        await context.Response.WriteAsync($"Date: {DateTime.Now.ToShortDateString()}");
    }
    else
    {
        await next.Invoke();
    }
});

app.Run(async (context) => await context.Response.WriteAsync($"Hello METANIT.COM"));

app.Run();
//  Здесь middleware в app.Use проверяет запрошенный адрес - если он содержит "/date", то клиенту отправляется текущая дата.
//  Иначе обработка запроса передается дальше в app.Run.

//  Причем в принципе мы можем использовать компонент в app.Use как единственный и соответственно терминальный:

//app.Use(async (HttpContext context, Func<Task> next) =>
//{
//    await context.Response.WriteAsync("Hello Work!");
//});

//  Однако в данном случае для большей производительости лучше использовать app.Run(), если нам надо определить лишь один компонент,
//  который в принципе не передает запрос дальше по конвейеру.
#endregion

#region Вынесение компонентов в методы
//  Также можно вынести все inline-компоненты в отдельные методы:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Use(GetDate);
//app.Run(async (context) => await context.Response.WriteAsync("Hello METANIT.COM"));
//app.Run();
async Task GetDate(HttpContext context, Func<Task> next)
{
    string? path = context.Request.Path.Value?.ToLower();
    if (path == "/date")
    {
        await context.Response.WriteAsync($"Date: {DateTime.Now.ToShortDateString()}");
    }
    else
    {
        await next.Invoke();
    }
}

//  Подобным образом можно использовать и другую версию метода Use, в которой используется делегат RequestDelegate:

async Task GetDate2(HttpContext context, RequestDelegate next)
{
    string? path = context.Request.Path.Value?.ToLower();
    if (path == "/date")
    {
        await context.Response.WriteAsync($"Date: {DateTime.Now.ToShortDateString()}");
    }
    else
    {
        await next.Invoke(context);
    }
}
#endregion