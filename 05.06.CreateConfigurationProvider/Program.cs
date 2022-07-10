//  Создание провайдера конфгурации

//  Фреймворк ASP.NET Core по умолчанию предоставляет богатый функционал для работы с конфигурацией - для работы с json, xml и так
//  далее. Однако, в каких-то ситуациях этого может быть недостаточно. И в этом случае мы можем определить свои источники и провайдеры
//  конфигурации.

//  Создание конфигурации вовлекает три компонента: IConfigurationSource(определяет источник конфигурации),
//  ConfigurationProvider(сам провайдер конфигурации) и некий класс, который добавляет метод расширения к объекту IConfiguration.

//  Допустим, мы хотим хранить конфигурацию в простом текстовом файле. И для этого добавим новый класс, который назовем
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

//  Класс TextConfigurationProvider будет представлять провайдер конфигурации и поэтому должен быть унаследован от класса
//  ConfigurationProvider.

//  В этом классе с помощью StreamReader происходит считывание текстового файла и добавление данных в словарь data. Для загрузки данных
//  переопределяется метод Load(), определенный в базовом классе.

//  В данном случае мы предполагаем, что на первой строке будет располагаться ключ, а на следующей строке значение. И после считывания
//  эти ключ и значение будут попадать в словарь data.

//  После считывания словарь data присваивается свойству Data, которое унаследовано от ConfigurationProvider. Это свойство как раз и
//  хранит все те конфигурационные настройки, которые потом используются в программе.

//  И чтобы получить путь к файлу, он передается через параметр конструктора.

//  Далее нам надо обращаться к этому провайдеру. Для этого определим класс источника конфигурации, который назовем
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

//  Источник конфигурации должен реализовать интерфейс IConfigurationSource, и в частности, его метод Build. В этот метод в качестве
//  параметра передается строитель конфигурации.

//  В данном случае этот объект нам позволяет получить полный путь к текстовому файлу. Краткое название файла (относительный путь)
//  передается в класс источника через конструктор и хранится в свойстве FilePath. После создания полного пути к файлу этот путь
//  передается в конструктор TextConfigurationProvider.

//  Чтобы задействовать функционал источника конфигурации создадим вспомогательный класс TextConfigurationExtensions:

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
            throw new ArgumentException("Путь к файлу не указан");
        }

        var source = new TextConfigurationSource(path);
        builder.Add(source);
        return builder;
    }
}

//  Этот класс определяет для объекта IConfigurationBuilder метод расширения AddTextFile(), в котором создается источник
//  конфигурации TextConfigurationSource, который затем добавляется к строителю конфигурации:
//      var source = new TextConfigurationSource(path);
//      builder.Add(source);

//  Теперь добавим в проект сам файл конфигурации. Пусть он будет называться config.txt и будет иметь следующее содержимое:
//  name
//  Tom
//  age
//  33
//  То есть на первой строке идет ключ настройки, а на второй ее значение и так далее.

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
//  При обращении к приложению будут использованы настройки из текстового файла