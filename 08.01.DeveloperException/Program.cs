//  Обработка ошибок
//  Обработка исключений

//  Ошибки в приложении можно условно разделить на два типа: исключения, которые возникают в процессе выполнения кода (например,
//  деление на 0), и стандартные ошибки протокола HTTP (например, ошибка 404).

//  Обычные исключения могут быть полезны для разработчика в процессе создания приложения, но простые пользователи не должны будут
//  их видеть.

#region UseDeveloperExceptionPage
//  Для обработки исключений в приложении, которое находится в процессе разработки, предназначен специальный middleware -
//  DeveloperExceptionPageMiddleware. Однако вручную нам его не надо добавлять, поскольку оно добавляется автоматически. Так, если
//  мы посмотрим на код класса WebApplicationBuilder, который применяется для создания приложения, то мы там можем увидеть там
//  следующие строки:

//if (context.HostingEnvironment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}

//  Если приложение находится в состоянии разработки, то с помощью middleware app.UseDeveloperExceptionPage() приложение перехватывает
//  исключения и выводит информацию о них разработчику.

//  Например, изменим код приложения следующим образом:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseDeveloperExceptionPage();

//app.Run(async context => 
//{
//    int a = 5;
//    int b = 0;
//    int c = a / b;
//    await context.Response.WriteAsync($"result = {c}");
//});

//app.Run();

//  В middleware в методе app.Run() симулируется генерация исключения путем деления на ноль. И если мы запустим проект, то в браузере
//  мы увидим информацию об исключении. Этой информации достаточно, чтобы определить где именно в коде произошло исключение.

//  Теперь посмотрим, как все это будет выглядеть для простого пользователя. Для этого изменим код приложения:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Environment.EnvironmentName = "Production";

//app.Run(async context =>
//{
//    int a = 5;
//    int b = 0;
//    int c = a / b;
//    await context.Response.WriteAsync($"result = {c}");
//});

//app.Run();

//  Выражение app.Environment.EnvironmentName = "Production" меняет имя окружения с "Development" (которое установлено по умолчанию) на
//  "Production". В этом случае среда ASP.NET Core больше не будет расценивать приложение как находящееся в стадии разработки.
//  Соответственно middleware из метода app.UseDeveloperExceptionPage() не будет перехватывать исключение. А приложение будет
//  возвращать пользователю ошибку 500, которая указывает, что на сервере возникла ошибка при обработке запроса. Однако реально
//  природа подобной ошибки может заключать в чем угодно.
#endregion

#region UseExceptionHandler
//  Это не самая лучшая ситуация, и нередко все-таки возникает необходимость дать пользователям некоторую информацию о том, что же
//  все-таки произошло. Либо потребуется как-то обработать данную ситуацию. Для этих целей можно использовать еще один встроенный
//  middleware - ExceptionHandlerMiddleware , который подключается с помощью метода UseExceptionHandler(). Он перенаправляет при
//  возникновении исключения на некоторый адрес и позволяет обработать исключение. Например, изменим код приложения следующим образом:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//// меняем имя окружения
//app.Environment.EnvironmentName = "Production";

//// если приложение не находится в процессе разработки
//// перенаправляем по адресу "/error"
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/error");
//}

//app.Map("/error", appEr => appEr.Run(async context => 
//{
//    context.Response.StatusCode = 500;
//    await context.Response.WriteAsync("Error 500. DivideByZeroException occurred!");
//}));

//app.Run(async (context) => 
//{
//    int a = 5;
//    int b = 0;
//    int c = a / b;
//    await context.Response.WriteAsync($"Result: {c}");
//});

//app.Run();

//  Метод app.UseExceptionHandler("/error"); перенаправляет при возникновении ошибки на адрес "/error".

//  Для обработки пути по определенному адресу здесь использовался метод app.Map(). В итоге при возникновении исключения будет
//  срабатывать делегат из метода app.Map(), в котором пользователю будет отправляться некотоое сообщение со статусным кодом 500.

//  Однако данный способ имеет небольшой недостаток - мы можем напрямую обратиться по адресу "/error" и получить тот же ответ от сервера.
//  Но в этом случае мы можем использовать другую версию метода UseExceptionHandler(), которая принимает делегат, параметр которого
//  представляет объект IApplicationBuilder:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

// меняем имя окружения
app.Environment.EnvironmentName = "Production";

// если приложение не находится в процессе разработки
// перенаправляем по адресу "/error"
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(appEr => appEr.Run(async context =>
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("Error 500. DivideByZeroException occurred!");
    }));
}

app.Run(async (context) =>
{
    int a = 5;
    int b = 0;
    int c = a / b;
    await context.Response.WriteAsync($"Result: {c}");
});

app.Run();

//  Следует учитывать, что app.UseExceptionHandler() следует помещать ближе к началу конвейера middleware.
#endregion