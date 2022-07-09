//  Передача зависимостей в конечные точки

//  Фреймворк ASP.NET Core предоставляет простой и удобный способ для передачи зависимостей в конечные точки. Все добавляемые в
//  коллекцию сервисов приложения зависимости можно получить через параметры делегата, который отвечает за обработку запроса.

//  Например, определим следующее приложение:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddTransient<TimeService>();

//var app = builder.Build();

//app.Map("/time", (TimeService timeService) => $"Time: {timeService.Time}");
//app.Map("/", () => "Index Page");

//app.Run();

//public class TimeService
//{
//    public string Time => DateTime.Now.ToLongTimeString();
//}

//  Здесь класс TimeService выступает в качестве сервиса, свойство Time которого возвращает текущее время в формате "hh:mm:ss".

//  Этот сервис добавляется в коллекцию сервисов приложения:
//      builder.Services.AddTransient<TimeService>();

//  Далее через параметр делегата, который передается в качестве второго параметра в метод Map() мы можем получить эту зависимость:
//      app.Map("/time", (TimeService timeService) => $"Time: {timeService.Time}");

//  Таким образом, при обращении по адресу "/time" приложение возвратит клиенту текущее время

//  Подобным образом можно получить зависимости, если обаботчик маршрута конечной точки вынесен в отдельный метод:

var builder = WebApplication.CreateBuilder();

builder.Services.AddTransient<TimeService>();

var app = builder.Build();

app.Map("/time", GetTime);
app.Map("/", () => "Index Page");

app.Run();

string GetTime(TimeService timeService)
{
    return $"Time: {timeService.Time}";
}
public class TimeService
{
    public string Time => DateTime.Now.ToLongTimeString();
}