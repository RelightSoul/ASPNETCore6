//  �������� ������ � Results API

//  ��� �������� ������ � ������� Results API ����� ����������� ����� File(), ������� ����� ��� ������.

#region �������� ����� ��� ������� ������
//  ��� �������� ����� ��� ������� ������ ����������� ��������� ������ ������:

//  public static IResult File (byte[] fileContents, string? contentType = default, 
//  string? fileDownloadName = default, bool enableRangeProcessing = false,
//  DateTimeOffset? lastModified = default, Microsoft.Net.Http.Headers.EntityTagHeaderValue? entityTag = default);

//  ��������� ������:
//      fileContents: ���������� ����� � ���� ������� ������
//      contentType: ��� ����������� ����� - ��������� Content-Type
//      fileDownloadName: ��� ����� ��� ��������
//      enableRangeProcessing: �������� ���� bool. ���� ��� ����� true, �� ��������� ��������� ������ �� ������
//      lastModified: �������� ���� Nullable<DateTimeOffset>>, ������� ��������� �� ���� ���������� ��������� �����
//      entityTag: �������� ���� EntityTagHeaderValue, ������� ������������� � ������

//  ��������, �������� � ������� ����� ����� Files, � ������� ������� �����-������ ����. ��������, � ���� ������ ��� ���� forest.jpg:

//  ��� �������� ����� ��������� ��������� ���:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/forest", async () =>
//{
//    string path = "Files/forest.jpg";
//    byte[] fileContent = await File.ReadAllBytesAsync(path);  // ��������� ���� � ������ ������
//    string contentType = "image/jpg";       // ��������� mime-����
//    string downloadName = "magic_forest.jpg";  // ��������� ������������ �����
//    return Results.File(fileContent, contentType, downloadName);
//});

//app.Map("/", () => "Hello World");

//app.Run();

//  � ��� ��������� �� ���� "/forest" ������������ ����� ��������� ���� forest.png
#endregion

#region �������� � ���� ��������� ������
//  ��� ���� ������ ������ File() ��������� ��������� ������ � ���� ������ Stream:

//  public static IResult File(System.IO.Stream fileStream, string? contentType = default,
//  string? fileDownloadName = default, DateTimeOffset? lastModified = default,
//  Microsoft.Net.Http.Headers.EntityTagHeaderValue? entityTag = default, bool enableRangeProcessing = false);

//  ����� �������� ������ ������ ��������, ������� � ������ ������ ������������ ���������� ����� � ���� ������� Stream.
//  ��������� ��������� �������� �� ��, ��� � � ���������� ������

//  �����, ��� � � ���������� �������, �������� ���� forest.png �� ����� Files:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/forest", () =>
//{
//    string path = "Files/forest.jpg";
//    FileStream fileStream = new FileStream(path, FileMode.Open); 
//    string contentType = "image/jpg";       // ��������� mime-����
//    string downloadName = "magic_forest.jpg";  // ��������� ������������ �����
//    return Results.File(fileStream, contentType, downloadName);
//});

//app.Map("/", () => "Hello World");

//app.Run();
#endregion

#region �������� ����� �� ������������� ����
//  ���� � ��� ���� ���� � �����, �� ����� ������������ ������ ������ ������ File(), ������� � ��������� ������� ���������
//  ��������� ���� � �����:

//  public static IResult File(string path, string? contentType = default,
//  string? fileDownloadName = default, DateTimeOffset? lastModified = default,
//  Microsoft.Net.Http.Headers.EntityTagHeaderValue? entityTag = default, bool enableRangeProcessing = false);

//  ��������� ��������� �� ��, ��� � ����� ������������� ������� ������. ������ ����� ��������, ��� ������ ������ ������ ���������
//  ���� � ������ ������������ �����, ������� �������� ���������� WebRootPath - �� ��������� ��� ����� wwwroot. �� ���� ���� ���
//  �������� �� ��������� ������ ������������� � ���� �����:

//  ��������� ��������� �� ��, ��� � ����� ������������� ������� ������. ������ ����� ��������, ��� ������ ������ ������
//  ��������� ���� � ������ ������������ �����, ������� �������� ���������� WebRootPath - �� ��������� ��� ����� wwwroot.
//  �� ���� ���� ��� �������� �� ��������� ������ ������������� � ���� �����:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/forest", () =>
//{
//    string path = "Files/forest.jpg";
//    string contentType = "image/jpg";
//    string downloadName = "magic_forest.jpg";
//    return Results.File(path, contentType, downloadName);
//});

//app.Map("/", () => "Hello World");

//app.Run();

//  �� ��� ��������� �� ����� ��������������. ��������, � ������� ���� ����� Files. � �� �����, ��� ���������� �������������
//  ������������ �� ��� �����.

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions { WebRootPath = "Files" });  // ��������� ����� ��� �������� ������
var app = builder.Build();

app.Map("/forest", () =>
{
    string path = "forest.jpg";
    string contentType = "image/jpeg";
    string downloadName = "forest.jpg";
    return Results.File(path, contentType, downloadName);
});

app.Map("/", () => "Hello World");

app.Run();
#endregion