//  �������� ������������ � �������� �����

//  ��������� ASP.NET Core ������������� ������� � ������� ������ ��� �������� ������������ � �������� �����. ��� ����������� �
//  ��������� �������� ���������� ����������� ����� �������� ����� ��������� ��������, ������� �������� �� ��������� �������.

//  ��������, ��������� ��������� ����������:

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

//  ����� ����� TimeService ��������� � �������� �������, �������� Time �������� ���������� ������� ����� � ������� "hh:mm:ss".

//  ���� ������ ����������� � ��������� �������� ����������:
//      builder.Services.AddTransient<TimeService>();

//  ����� ����� �������� ��������, ������� ���������� � �������� ������� ��������� � ����� Map() �� ����� �������� ��� �����������:
//      app.Map("/time", (TimeService timeService) => $"Time: {timeService.Time}");

//  ����� �������, ��� ��������� �� ������ "/time" ���������� ��������� ������� ������� �����

//  �������� ������� ����� �������� �����������, ���� ��������� �������� �������� ����� ������� � ��������� �����:

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