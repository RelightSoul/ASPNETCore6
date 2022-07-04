//  ����� Run � ����������� ������������� middleware

//  ����� ������� ������ ���������� middleware � �������� ��������� ������� � ASP.NET Core ������������ ����� Run(),
//  ������� ��������� ��� ����� ���������� ��� ���������� IApplicationBuilder (������������� ��� ������������ � �����
//  WebApplication):
//  IApplicationBuilder.Run(RequestDelegate handler)

//  ����� Run ��������� ������������ ��������� - ����� ���������, ������� ��������� ��������� �������. �������
//  ������������� �� �� �������� ������� ������ ���������� � ��������� ������� ������ - ��������� � ��������� �����������
//  �� ��������. ������� ������ ����� ������� �������� � ����� ����� ���������� ��������� ��������� �������. �� ���� ��
//  ����� ���� �������� ������ ������, ������� ��������� ���������� middleware.

//  � �������� ��������� ����� Run ��������� ������� RequestDelegate. ���� ������� ����� ��������� �����������:
//  public delegate Task RequestDelegate(HttpContext context);

//  �� ��������� � �������� ��������� �������� ������� HttpContext � ���������� ������ Task.

//  ���������� ���� ����� ��� ����������� ����������� ����������:
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();

app.Run(async (context) => await context.Response.WriteAsync("Hello Metanit"));
//  ����� ��� �������� RequestDelegate ���������� ������-���������, �������� �������� - HttpContext ����� ������������ ���
//  �������� ������. � ���������, ����� context.Response.WriteAsync() ��������� ��������� ������� ��������� ����� - � ������
//  ������ ������������ ������� ������.
app.Run();
//  ������ ������ - ����� Run(), ������� ��������� ����������, ���������� ����� ���������� ���������� middleware. �� ��
//  ����� ��������� ��� ����� ����������� middleware, ����� � ��� �� ����� ������.

//  ��� ������������� ����������� �� ����� ������� ��� middleware � ��������� �����:
app.Run(HandleRequst);
async Task HandleRequst(HttpContext context)
{
    await context.Response.WriteAsync("Hello METANIT.COM 2");
}

#region ��������� ���� middleware
//  ���������� middleware ��������� ���� ��� � ���������� � ������� ����� ���������� ����� ����������. �� ���� ��� �����������
//  ��������� �������� ������������ ���� � �� �� ����������. ��������, ��������� � ����� Program.cs ��������� ���:
int x = 2;
app.Run(async (context) =>
{
    x = x * 2;  //  2 * 2 = 4
    await context.Response.WriteAsync($"Result: {x}");
});
//  ��� ������� ���������� �� ����������� �������, ��� ������� ������� ����� 4 � �������� ����������
//  ������ ��� ����������� ��������(�������� �������� ��� ������ �������) �� ������, ��� ��������� ���������� � �� ����� 4.

//  ����� ����� ��������, ��� ������� Google Chrome ����� �������� ��� ������� - ���� ���������� � ����������, � ������ -
//  � ����� ������ favicon.ico, ������� � Google Chrome ��������� ����� ���������� �� 2 ����, � ������� ������.
#endregion