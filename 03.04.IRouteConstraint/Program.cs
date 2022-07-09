//  �������� ����������� ���������

//  ���� ��������� ASP.NET Core �� ��������� ������������� ������� ����� ���������� �����������, �� ����� ���� ������������. � � ����
//  ������ �� ����� ���������� ���� ��������� ����������� ���������.

//  ��� �������� ������������ ����������� �������� ����� ����������� ��������� IRouteConstraint � ����� ������������ ������� Match,
//  ������� ����� ��������� �����������:

//public interface IRouteConstraint
//{
//    bool Match(HttpContext? httpContext,
//            IRouter? route,
//            string routeKey,
//            RouteValueDictionary values,
//            RouteDirection routeDirection);
//}

//  ��������� ������:
//  httpContext: ������ HttpContext, ������� ������������� ���������� � HTTP-�������
//  route: ������ IRouter, ������� ������������ �������, � ������ �������� ����������� �����������
//  routeKey: ������ String - �������� ��������� ��������, � �������� ����������� �����������
//  values: ������ RouteValueDictionary, ������� ������������ ����� ���������� �������� � ���� �������, ��� ����� - �������� ����������,
//  � �������� - �������� ���������� ��������
//  routeDirection: ������ ������������ RouteDirection, ������� ���������, ����������� ����������� ��� ��������� �������, ���� ���
//  ��������� ������

//  � �������� ���������� ����� ���������� �������� ���� bool: true, ���� ����� ������������� ������� ����������� ��������, � false,
//  ���� �� �������������.

//  ����������� �������� ��������� ���� ��������� IRouteConstraint. ��� ��������� ������ ������������� ������� ��� �����������
//  �������� ����� IRouteConstraint.Match, ����� ����������, ����������� �� ������ ����������� � ������� ������� ��� ���. � ������
//  ���� ������ ����� ���������� true, ������ ����� ���� ����������� � ��������� (�������, ���� ������ ����� ������������� � ������
//  �����������, ������� ����� �����������).

#region ����������� ��� ��������� ��������
//  ��������, ������ � ������� ����� �������� ������ ���������� ���, ������� ����� ��������� ������. ��� ����� ��������� ��������� �����
//  ����������� ��������:

//public class SecretCodeConstraint : IRouteConstraint
//{
//    string secretCode;    // ���������� ���
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

//  ����� SecretCodeConstraint ����� ����������� ��������� ����� ������� ��������� ���, �������� ������ ��������������� ��������
//  ��������. � ������ Match() � ������� ��������� values[routeKey] �������� �� ������� values �������� ��������� ��������, ���
//  �������� ���������� ����� routeKey. � ����� ��� �������� ������������ � ��������� �����:
//      return values[routeKey]?.ToString() == secretCode;
//  ���� ��� �������� �����, �� ���������� true, ��� ���������, ��� ������� ������������� ������� �����������.

//var builder = WebApplication.CreateBuilder();
//// ���������� ����� SecretCodeConstraint �� inline-����������� secretcode
//builder.Services.Configure<RouteOptions>(options =>
//                options.ConstraintMap.Add("secretcode", typeof(SecretCodeConstraint)));

//// �������������� ���������� ������ �����������
////builder.Services.AddRouting(options => options.ConstraintMap.Add("secretcode", typeof(SecretCodeConstraint)));

//var app = builder.Build();

//app.Map(
//    "/users/{name}/{token:secretcode(123466)}/",
//    (string name, int token) => $"Name: {name} \nToken: {token}"
//);
//app.Map("/", () => "Index Page");

//app.Run();

//  ����� ���� �������� ��������� ������. ���� �� ����� ������������ ����� ����������� ��� inline-����������� ������ ������� ��������,
//  �� ��� ���������� �������� ��������� ��� ������� RouteOptions:
//  builder.Services.Configure<RouteOptions>(options =>
//      options.ConstraintMap.Add("secretcode", typeof(SecretCodeConstraint)));

//  � ������ ������ �������� options ������������ ������ RouteOptions. ��� �������� ConstraintMap ������������ ��������� �����������
//  �����������. ����� Add() ��������� � ��� ��������� �����������. ������ ������ �������� ����� ������ ������������ ���� �����������
//  � ��������� �����������, � ������ �������� - ���������� ����� �����������. ����� ���� ����������� ����� ����� ����� ��������� ���
//  inline-�����������, �� ������� ������������ ����� �����������.

//  � �������� ������������ ������ ����� ������ �� ����� �� ���������� � �������� ������������� � ����� ��������� ������ RouteOptions:
//  builder.Services.AddRouting(options =>
//                options.ConstraintMap.Add("secretcode", typeof(SecretConstraint)));

//  ����� ����� �� ������ ������������ inline-����������� � ������� ��������:
//      "/users/{name}/{token:secretcode(123466)}/"
//  �� ���� ����� � ��������� �������� token ����� ����������� ����������� SecretCodeConstraint. � ������ 123466 - ��� ��� ���������
//  ���, ������� ����� ������������ � ����������� ������� SecretCodeConstraint.

//  ������ ������.��������, �� �����, ����� �������� �� ��� ����� ���� �� ������ ��������:

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
//  � ������ ������ �������� �������� name �� ������ ������������ ����� "Tom", "Sam" � "Bob":
#endregion