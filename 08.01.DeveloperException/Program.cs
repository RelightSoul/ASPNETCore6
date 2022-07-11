//  ��������� ������
//  ��������� ����������

//  ������ � ���������� ����� ������� ��������� �� ��� ����: ����������, ������� ��������� � �������� ���������� ���� (��������,
//  ������� �� 0), � ����������� ������ ��������� HTTP (��������, ������ 404).

//  ������� ���������� ����� ���� ������� ��� ������������ � �������� �������� ����������, �� ������� ������������ �� ������ �����
//  �� ������.

#region UseDeveloperExceptionPage
//  ��� ��������� ���������� � ����������, ������� ��������� � �������� ����������, ������������ ����������� middleware -
//  DeveloperExceptionPageMiddleware. ������ ������� ��� ��� �� ���� ���������, ��������� ��� ����������� �������������. ���, ����
//  �� ��������� �� ��� ������ WebApplicationBuilder, ������� ����������� ��� �������� ����������, �� �� ��� ����� ������� ���
//  ��������� ������:

//if (context.HostingEnvironment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}

//  ���� ���������� ��������� � ��������� ����������, �� � ������� middleware app.UseDeveloperExceptionPage() ���������� �������������
//  ���������� � ������� ���������� � ��� ������������.

//  ��������, ������� ��� ���������� ��������� �������:

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

//  � middleware � ������ app.Run() ������������ ��������� ���������� ����� ������� �� ����. � ���� �� �������� ������, �� � ��������
//  �� ������ ���������� �� ����������. ���� ���������� ����������, ����� ���������� ��� ������ � ���� ��������� ����������.

//  ������ ���������, ��� ��� ��� ����� ��������� ��� �������� ������������. ��� ����� ������� ��� ����������:

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

//  ��������� app.Environment.EnvironmentName = "Production" ������ ��� ��������� � "Development" (������� ����������� �� ���������) ��
//  "Production". � ���� ������ ����� ASP.NET Core ������ �� ����� ����������� ���������� ��� ����������� � ������ ����������.
//  �������������� middleware �� ������ app.UseDeveloperExceptionPage() �� ����� ������������� ����������. � ���������� �����
//  ���������� ������������ ������ 500, ������� ���������, ��� �� ������� �������� ������ ��� ��������� �������. ������ �������
//  ������� �������� ������ ����� ��������� � ��� ������.
#endregion

#region UseExceptionHandler
//  ��� �� ����� ������ ��������, � ������� ���-���� ��������� ������������� ���� ������������� ��������� ���������� � ���, ��� ��
//  ���-���� ���������. ���� ����������� ���-�� ���������� ������ ��������. ��� ���� ����� ����� ������������ ��� ���� ����������
//  middleware - ExceptionHandlerMiddleware , ������� ������������ � ������� ������ UseExceptionHandler(). �� �������������� ���
//  ������������� ���������� �� ��������� ����� � ��������� ���������� ����������. ��������, ������� ��� ���������� ��������� �������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//// ������ ��� ���������
//app.Environment.EnvironmentName = "Production";

//// ���� ���������� �� ��������� � �������� ����������
//// �������������� �� ������ "/error"
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

//  ����� app.UseExceptionHandler("/error"); �������������� ��� ������������� ������ �� ����� "/error".

//  ��� ��������� ���� �� ������������� ������ ����� ������������� ����� app.Map(). � ����� ��� ������������� ���������� �����
//  ����������� ������� �� ������ app.Map(), � ������� ������������ ����� ������������ �������� ��������� �� ��������� ����� 500.

//  ������ ������ ������ ����� ��������� ���������� - �� ����� �������� ���������� �� ������ "/error" � �������� ��� �� ����� �� �������.
//  �� � ���� ������ �� ����� ������������ ������ ������ ������ UseExceptionHandler(), ������� ��������� �������, �������� ��������
//  ������������ ������ IApplicationBuilder:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

// ������ ��� ���������
app.Environment.EnvironmentName = "Production";

// ���� ���������� �� ��������� � �������� ����������
// �������������� �� ������ "/error"
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

//  ������� ���������, ��� app.UseExceptionHandler() ������� �������� ����� � ������ ��������� middleware.
#endregion