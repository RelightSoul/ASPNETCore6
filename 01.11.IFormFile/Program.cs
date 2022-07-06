//  �������� ������ �� ������

//  ����������, ��� ��������� ����� �� ������ � ASP.NET Core. ��� ����������� ����� � ASP.NET Core ������������ ����� IFormFile ��
//  ������������ ���� Microsoft.AspNetCore.Http. �������������� ��� ��������� ������������� ����� � ����������� ���������� ������������
//  IFormFile. ����� � ������� ������� IFormFile �� ����� ���������� ��������� ����������� ������ - ������� ��� ��������, ���������,
//  �������� ��� ����� � �.�. ��������� ��� �������� � ������:
//  ContentType: ��� �����
//  FileName: �������� �����
//  Length: ������ �����
//  CopyTo / CopyToAsync: �������� ���� � �����
//  OpenReadStream: ��������� ����� ����� ��� ������

//  ��� ������������ ������ ����������� ��������� � ������� ����� html, � ������� �������� ���� index.html

//< !DOCTYPE html >
//< html >
//< head >
//    < meta charset = "utf-8" />
//    < meta name = "viewport" content = "width=device-width" />
//    < title > METANIT.COM </ title >
//</ head >
//< body >
//    < h2 > �������� ���� ��� ��������</h2>
//    <form action="upload" method="post" enctype="multipart/form-data">
//        <input type="file" name="uploads" /><br>
//        <input type="file" name="uploads" /><br>
//        <input type="file" name="uploads" /><br>
//        <input type="submit" value="���������" />
//    </form>
//</body>
//</html>

//  � ������ ������ ����� �������� ����� ��������� � ����� file, ����� ������� ����� ������� ����� ��� ��������. � ������ ������
//  �� ����� ��� ����� ��������, �� �� ����� ���� � ������ � ������. � ��������� ��������� �������� ����� enctype="multipart/form-data"
//  ������� ����� �����, ��� ������ � ������ ���� �������� �����.

//  ������������ ����� ����� � ������� ���� POST �� ����� "/upload".

//  ������ � ����� Program.cs ��������� ���, ������� ����� �������� ����������� �����:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();


app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;

    response.ContentType = "text/html; charset=utf-8";
    if (request.Path == "/upload" && request.Method == "POST")
    {
        IFormFileCollection files = request.Form.Files;
        // ���� � �����, ��� ����� ��������� �����
        var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
        // ������� ����� ��� �������� ������
        Directory.CreateDirectory(uploadPath);

        foreach (var file in files)
        {
            // ���� � ����� uploads
            string fullPath = $"{uploadPath}/{file.FileName}";

            // ��������� ���� � ����� uploads
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }
        await response.WriteAsync("����� ������� ���������");
    }
    else
    {
        await response.SendFileAsync("html/index.html");
    }
});

app.Run();

//  ����� ���� ������ �������� �� ������ "/upload", � ��� ������ ������������ ������ ���� POST, �� ���������� �������� ���������
//  ����������� ������ � ������� �������� Request.Form.Files, ������� ������������ ��� IFormFileCollection:
//  IFormFileCollection files = request.Form.Files;

//  ����� ���������� ������� ��� ����������� ������ (��������������, ��� ����� ����� ��������� � �������� "uploads", �������
//  ������������� � ����� ����������)
//  var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";

//  ���� ����� ����� ���, �� ������� ��. ����� ���������� ��� ��������� ������.
//  foreach (var file in files)

//  ������ ��������� ���� � ���� ��������� ������������ ��� IFormFile. ��� ����������� ����� � ������ ������� ��������� �����
//  FileStream, � ������� ������������ ���� � ������� ������ CopyToAsync.
//  using (var fileStream = new FileStream(fullPath, FileMode.Create))
//  {
//      await file.CopyToAsync(fileStream);
//  }

//  ���� ������ ���� �� ������� ������ �/��� �� ������������ ��� POST, �� ���������� ������� html-�������� index.html.