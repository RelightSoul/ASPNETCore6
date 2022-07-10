//  �������� ���������� �����������

//  ��������� ASP.NET Core �� ��������� ������������� ������� ���������� ��� ������ � ������������� - ��� ������ � json, xml � ���
//  �����. ������, � �����-�� ��������� ����� ����� ���� ������������. � � ���� ������ �� ����� ���������� ���� ��������� � ����������
//  ������������.

//  �������� ������������ ��������� ��� ����������: IConfigurationSource(���������� �������� ������������),
//  ConfigurationProvider(��� ��������� ������������) � ����� �����, ������� ��������� ����� ���������� � ������� IConfiguration.

//  ��������, �� ����� ������� ������������ � ������� ��������� �����. � ��� ����� ������� ����� �����, ������� �������
//  TextConfigurationProvider:

using System.IO;

public class TextConfigurationProvider : ConfigurationProvider
{
    public string FilePath { get; set; }
    public TextConfigurationProvider(string filePath)
    {
        FilePath = filePath;
    }
    public override void Load()
    {
        var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        using (StreamReader textReader = new StreamReader(FilePath))
        {
            string? line;
            while ((line = textReader.ReadLine()) != null)
            {
                string key = line.Trim();
                string? value = textReader.ReadLine() ?? "";
                data.Add(key, value);
            }
        }
        Data = data;
    }
}

//  ����� TextConfigurationProvider ����� ������������ ��������� ������������ � ������� ������ ���� ����������� �� ������
//  ConfigurationProvider.

//  � ���� ������ � ������� StreamReader ���������� ���������� ���������� ����� � ���������� ������ � ������� data. ��� �������� ������
//  ���������������� ����� Load(), ������������ � ������� ������.

//  � ������ ������ �� ������������, ��� �� ������ ������ ����� ������������� ����, � �� ��������� ������ ��������. � ����� ����������
//  ��� ���� � �������� ����� �������� � ������� data.

//  ����� ���������� ������� data ������������� �������� Data, ������� ������������ �� ConfigurationProvider. ��� �������� ��� ��� �
//  ������ ��� �� ���������������� ���������, ������� ����� ������������ � ���������.

//  � ����� �������� ���� � �����, �� ���������� ����� �������� ������������.

//  ����� ��� ���� ���������� � ����� ����������. ��� ����� ��������� ����� ��������� ������������, ������� �������
//  TextConfigurationSource:

public class TextConfigurationSource : IConfigurationSource
{
    public string FilePath { get; }
    public TextConfigurationSource(string filename)
    {
        FilePath = filename;
    }
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        string filePath = builder.GetFileProvider().GetFileInfo(FilePath).PhysicalPath;
        return new TextConfigurationProvider(filePath);
    }
}

//  �������� ������������ ������ ����������� ��������� IConfigurationSource, � � ���������, ��� ����� Build. � ���� ����� � ��������
//  ��������� ���������� ��������� ������������.

//  � ������ ������ ���� ������ ��� ��������� �������� ������ ���� � ���������� �����. ������� �������� ����� (������������� ����)
//  ���������� � ����� ��������� ����� ����������� � �������� � �������� FilePath. ����� �������� ������� ���� � ����� ���� ����
//  ���������� � ����������� TextConfigurationProvider.

//  ����� ������������� ���������� ��������� ������������ �������� ��������������� ����� TextConfigurationExtensions:

public static class TextConfigurationExtensions
{
    public static IConfigurationBuilder AddTextFile(
        this IConfigurationBuilder builder, string path)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException("���� � ����� �� ������");
        }

        var source = new TextConfigurationSource(path);
        builder.Add(source);
        return builder;
    }
}

//  ���� ����� ���������� ��� ������� IConfigurationBuilder ����� ���������� AddTextFile(), � ������� ��������� ��������
//  ������������ TextConfigurationSource, ������� ����� ����������� � ��������� ������������:
//      var source = new TextConfigurationSource(path);
//      builder.Add(source);

//  ������ ������� � ������ ��� ���� ������������. ����� �� ����� ���������� config.txt � ����� ����� ��������� ����������:
//  name
//  Tom
//  age
//  33
//  �� ���� �� ������ ������ ���� ���� ���������, � �� ������ �� �������� � ��� �����.

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        builder.Configuration.AddTextFile("config.txt");

        app.Map("/", (IConfiguration appConfig) => $"{appConfig["name"]} - {appConfig["age"]}");

        app.Run();
    }
}
//  ��� ��������� � ���������� ����� ������������ ��������� �� ���������� �����