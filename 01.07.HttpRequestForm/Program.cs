//  �������� ����

//  ������� ������ ������������ �� ������ � ������� ���� html, ������ � ������� ���� POST. ��� ��������� ��������
//  ������ � ������ HttpRequest ���������� �������� Form. ����������, ��� �� ����� �������� �������� ������.
//  ������ ����� ��������� � ������� � ����� html ���� index.html. � �� ���������� ����� ������� ��� ����� ������
//  ������������, ������� � ������� ���� POST (������� method="post") ���������� ������ �� ������ "postuser"
//  (������� action="postuser"). �� ����� ���������� ��� ���� �����. ������ ���� ������������� ��� ����� �����
//  ������������. ������ ���� - ��� ����� �������� ������������.

//  ��� ��������� ���� ������ ��������� � ����� Program.cs ��������� ���:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async context => 
//{
//    context.Response.ContentType = "text/html; charset=utf-8";

//    // ���� ��������� ���� �� ������ "/postuser", �������� ������ �����
//    if (context.Request.Path == "/postuser")
//    {
//        var form = context.Request.Form;
//        string name = form["name"];
//        string age = form["age"];
//        await context.Response.WriteAsync($"<din><p>Name: {name}</p><p>Age: {age}</p></div>");
//    }
//    else
//    {
//        await context.Response.SendFileAsync("html/index.html");
//    }
//});
//app.Run();

//  �����, ���� �������� ����� "/postuser", �� ��������������, ��� ���������� ��������� �����. ������� ��������
//  ������������ ����� � ���������� form:
//  var form = context.Request.Form;
//  �������� Request.Form ���������� ������ IFormCollection - ������ ���� �������, ��� �� ����� ����� ��������
//  �������� ��������.��� ���� � �������� ������ ��������� �������� ����� ���� (�������� ��������� name ��������� �����):
//  /< input name = "age" type = "number" />
//  ���, � ������ ������ �������� ���� (�������� �������� name) ����� "age".�������������� � Request.Form �� �����
//  ����� �� ����� �������� ��� ��������:
//  string age = form["age"];
//  ����� ��������� ������ ����� ��� ������������ ������� �������

#region ��������� ��������
//  �������� ������ � ������� � ����� �� �������� index.html ��������� �����, ������� ����� ������������ ������.
//<!DOCTYPE html>
//< html >
//< head >
//    < meta charset = "utf-8" />
//    < title > METANIT.COM </ title >
//</ head >
//< body >
//    < h2 > User form </ h2 >
//    < form method = "post" action = "postuser" >
//        < p > Name: < br />
//            < input name = "name" />
//        </ p >
//        < p > Age: < br />
//            < input name = "age" type = "number" />
//        </ p >
//        < p >
//            Languages:< br />
//            < input name = "languages" />< br />
//            < input name = "languages" />< br />
//            < input name = "languages" />< br />
//        </ p >
//        < input type = "submit" value = "Send" />
//    </ form >
//</ body >
//</ html >
//  ��������� ��� ���� �����, ������� ����� ���� � �� �� ���. ������� ��� �� �������� ����� ������������� ������
//  �� ���� ��������. ������ ������� ��� �������� � ���� C#:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";

    // ���� ��������� ���� �� ������ "/postuser", �������� ������ �����
    if (context.Request.Path == "/postuser")
    {
        var form = context.Request.Form;
        string name = form["name"];
        string age = form["age"];
        string[] languages = form["languages"];
        // ������� �� ������� languages ���� ������
        string langList = "";
        foreach (var lang in languages)
        {
            langList += $" {lang}";
        }
        await context.Response.WriteAsync($"<div><p>Name: {name}</p>" +
            $"<p>Age: {age}</p>" +
            $"<div>Languages:{langList}</ul></div>");
    }
    else
    {
        await context.Response.SendFileAsync("html/index.html");
    }
});
app.Run();
//  ��������� �������� "languages" ������������ ������, �� � ������������� �� ����� � �������� �����


//  �������� ������� ����� ���������� �������� ������� ����� ������ �����, ���� �����, ������� ������������ �����
//  ���������, ��������, �������� select, ������� ������������ ������������� �����
//< !DOCTYPE html >
//< html >
//< head >
//    < meta charset = "utf-8" />
//    < title > METANIT.COM </ title >
//</ head >
//< body >
//    < h2 > User form </ h2 >
//    < form method = "post" action = "postuser" >
//        < p > Name: < br />
//            < input name = "name" />
//        </ p >
//        < p > Age: < br />
//            < input name = "age" type = "number" />
//        </ p >
//        < p >
//            Languages:< br />
//            < select multiple name = "languages" >
//                < option > C#</option>
//                < option > JavaScript </ option >
//                < option > Kotlin </ option >
//                < option > Java </ option >
//             </ select >
//        </ p >
//        < input type="submit" value="Send" />
//    </form>
//</body>
//</html>
#endregion
