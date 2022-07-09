//  Создание ограничений маршрутов

//  Хотя фреймворк ASP.NET Core по умолчанию предоставляет большой набор встроенных ограничений, их может быть недостаточно. И в этом
//  случае мы можем определить свои кастомные ограничения маршрутов.

//  Для создания собственного ограничения маршрута нужно реализовать интерфейс IRouteConstraint с одним единственным методом Match,
//  который имеет следующее определение:

//public interface IRouteConstraint
//{
//    bool Match(HttpContext? httpContext,
//            IRouter? route,
//            string routeKey,
//            RouteValueDictionary values,
//            RouteDirection routeDirection);
//}

//  Параметры метода:
//  httpContext: объект HttpContext, который инкапсулирует информацию о HTTP-запросе
//  route: объект IRouter, который представляет маршрут, в рамках которого применяется ограничение
//  routeKey: объект String - название параметра маршрута, к которому применяется ограничение
//  values: объект RouteValueDictionary, который представляет набор параметров маршрута в виде словаря, где ключи - названия параметров,
//  а значения - значения параметров маршрута
//  routeDirection: объект перечисления RouteDirection, которое указывает, применяется ограничение при обработке запроса, либо при
//  генерации ссылки

//  В качестве результата метод возвращает значение типа bool: true, если зпрос удовлетворяет данному ограничению маршрута, и false,
//  если не удовлетворяет.

//  Ограничение маршрута применяет этот интерфейс IRouteConstraint. Это вынуждает движок маршрутизации вызвать для ограничения
//  маршрута метод IRouteConstraint.Match, чтобы определить, применяется ли данное ограничение к данному запросу или нет. И только
//  если данный метод возвращает true, запрос может быть сопоставлен с маршрутом (конечно, если запрос также удовлетворяет и другим
//  ограниченям, которые могут применяться).

#region Ограничение для параметра маршрута
//  Допустим, клиент в запросе через параметр должен передавать код, который равен некоторой строке. Для этого определим следующий класс
//  ограничения маршрута:

//public class SecretCodeConstraint : IRouteConstraint
//{
//    string secretCode;    // допустимый код
//    public SecretCodeConstraint(string secretCode)
//    {
//        this.secretCode = secretCode;
//    }

//    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values,
//        RouteDirection routeDirection)
//    {
//        return values[routeKey]?.ToString() == secretCode;
//    }
//}

//  Класс SecretCodeConstraint через конструктор принимает некий условно секретный код, которому должен соответствовать параметр
//  маршрута. В методе Match() с помощью выражения values[routeKey] получаем из словаря values значение параметра маршрута, имя
//  которого передается через routeKey. И затем это значение сравниванием с секретным кодом:
//      return values[routeKey]?.ToString() == secretCode;
//  Если оба значения равны, то возвращаем true, что указывает, что маршрут соответствует данному ограничению.

//var builder = WebApplication.CreateBuilder();
//// проецируем класс SecretCodeConstraint на inline-ограничение secretcode
//builder.Services.Configure<RouteOptions>(options =>
//                options.ConstraintMap.Add("secretcode", typeof(SecretCodeConstraint)));

//// альтернативное добавление класса ограничения
////builder.Services.AddRouting(options => options.ConstraintMap.Add("secretcode", typeof(SecretCodeConstraint)));

//var app = builder.Build();

//app.Map(
//    "/users/{name}/{token:secretcode(123466)}/",
//    (string name, int token) => $"Name: {name} \nToken: {token}"
//);
//app.Map("/", () => "Index Page");

//app.Run();

//  Здесь надо отметить следующий момент. Если мы хотим использовать класс ограничения как inline-ограничение внутри шаблона маршрута,
//  то нам необходимо изменить настройки для сервиса RouteOptions:
//  builder.Services.Configure<RouteOptions>(options =>
//      options.ConstraintMap.Add("secretcode", typeof(SecretCodeConstraint)));

//  В данном случае параметр options представляет объект RouteOptions. Его свойство ConstraintMap представляет коллекцию применяемых
//  ограничений. Метод Add() добавляет в эту коллекцию ограничение. Причем первый параметр этого метода представляет ключ ограничения
//  в коллекции ограничений, а второй параметр - собственно класс ограничения. Далее ключ ограничения затем можно будет применять как
//  inline-ограничение, на которое проецируется класс ограничения.

//  В качестве альтернативы вместо этого вызова мы могли бы обратиться к сервисам маршрутизации и также настроить объект RouteOptions:
//  builder.Services.AddRouting(options =>
//                options.ConstraintMap.Add("secretcode", typeof(SecretConstraint)));

//  После этого мы сможем использовать inline-ограничение в шаблоне маршрута:
//      "/users/{name}/{token:secretcode(123466)}/"
//  То есть здесь к параметру маршрута token будет применяться ограничение SecretCodeConstraint. А строка 123466 - это тот секретный
//  код, который будет передаваться в конструктор объекта SecretCodeConstraint.

//  Другой пример.Допустим, мы хотим, чтобы параметр не мог иметь одно из набора значений:

var builder = WebApplication.CreateBuilder();

builder.Services.AddRouting(options => options.ConstraintMap.Add("invalidnames", typeof(InvalidNamesConstraint)));

var app = builder.Build();

app.Map("/users/{name:invalidnames}",(string name) => $"Name: {name}");
app.Map("/", () => "Index Page");

app.Run();

public class InvalidNamesConstraint : IRouteConstraint
{
    string[] names = {"Tom","Sam","Bob" };    
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        return !names.Contains(values[routeKey]?.ToString());
    }
}
//  В данном случае параметр маршрута name НЕ должен представлять имена "Tom", "Sam" и "Bob":
#endregion