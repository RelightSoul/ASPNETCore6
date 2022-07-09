//  ��������� ���� ������������

//  ASP.NET Core ��������� ��������� ��������� ������ ���������� � ���������� ��������. � ����� ������ ���������� ����� ������� �����
//  ������������ ���� �� ��������� �����:
//      Transient: ��� ������ ��������� � ������� ��������� ����� ������ �������. � ������� ������ ������� ����� ���� ��������� ���������
//      � �������, �������������� ��� ������ ��������� ����� ����������� ����� ������. �������� ������ ���������� ����� �������� ��������
//      ��� ����������� ��������, ������� �� ������ ������ � ���������
//      Scoped: ��� ������� ������� ��������� ���� ������ �������. �� ���� ���� � ������� ������ ������� ���� ��������� ��������� � ������
//      �������, �� ��� ���� ���� ���������� ����� �������������� ���� � ��� �� ������ �������.
//      Singleton: ������ ������� ��������� ��� ������ ��������� � ����, ��� ����������� ������� ���������� ���� � ��� �� ����� ���������
//      ������ �������

//  ��� �������� ������� ���� ������� ������������ ��������������� ����� AddTransient(), AddScoped() � AddSingleton().

//  ��� ������������ ��������� ��������� ������������ � ���������� ����� ������� ��������� ��������� ICounter:

//public interface ICounter
//{
//    int Value { get; }
//}

//  ����� ��������� ���������� ����� ���������� - ����� RandomCounter

//public class RandomCounter : ICounter
//{
//    static Random rnd = new Random();
//    private int _value;
//    public RandomCounter()
//    {
//        _value = rnd.Next(0, 1000000);
//    }
//    public int Value
//    {
//        get => _value;
//    }
//}
//  ���� ������ RandomCounter ������� � ��������� ���������� ���������� ����� � ��������� �� 0 �� 1000000.

//  � ����� ��������� ����� �����, ������� ��� ����� �������� ������� ����������� � ��������� Depedency Injection. ���� �����
//  ������� CounterService � ��������� � ��� ��������� ���:

//public class CounterService
//{
//    public ICounter Counter { get; }
//    public CounterService(ICounter counter)
//    {
//        Counter = counter;  
//    }
//}
//  ������ ����� ������ ������������� ������ ICounter, ������������ ����� �����������.

//  ��� ������ � ��������� ��������� ��������� middleware, ������� ������� CounterMiddleware:

//public class CounterMiddleware
//{
//    RequestDelegate next;
//    int i = 0; // ������� ��������
//    public CounterMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }
//    public async Task InvokeAsync(HttpContext httpContext, ICounter counter, CounterService counterService)
//    {
//        i++;
//        httpContext.Response.ContentType = "text/html; charset=utf-8";
//        await httpContext.Response.WriteAsync($"������ {i}; Counter: {counter.Value}; Service: {counterService.Counter.Value}");
//    }
//}

//  ��� ��������� ������������ ����� ������������ ����� InvokeAsync, � ������� ���������� ��� ����������� ICounter � CounterService.
//  � ����� ������ ��������� �������� Value �� ����� ������������. ������ ������ CounterService ��� ���������� ����������� ICounter.

//  ������ �� ������� ���� ������� ���������� ��������� ��������� ������ ��������.

#region AddTransient
//  ����� AddTransient() ������� transient-�������. ����� ������� ��������� ��� ������ ��������� � ���. ������ ����� ����� ���
//  ������������� ������:
//  AddTransient(Type serviceType)
//  AddTransient(Type serviceType, Type implementationType)
//  AddTransient(Type serviceType, Func<IServiceProvider, object> implementationFactory)
//  AddTransient<TService>()
//  AddTransient<TService, TImplementation>()
//  AddTransient<TService>(Func < IServiceProvider, TService > implementationFactory)
//  AddTransient<TService, TImplementation>(Func < IServiceProvider, TImplementation > implementationFactory)

//  ���������� ������ ����� ��� ���������� ��������:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddTransient<ICounter, RandomCounter>();
//builder.Services.AddTransient<CounterService>();

//var app = builder.Build();

//app.UseMiddleware<CounterMiddleware>();

//app.Run();

//  � ����� ������ CounterMiddleware �������� ������ ICounter, ��� �������� ��������� ���� ��������� ������ RandomCounter.
//  CounterMiddleware ����� �������� ������ CounterService, ������� ����� ���������� ICounter. � ��� ����� ICounter �����
//  ����������� ������ ��������� ������ RandomCounter. ������� ������������ ��������� ����� ������ ������������ �� ���������.
//  ����� �������, ���������� AddTransient ������� ��� ������ ������� RandomCounter.

//  ��� ������ � ����������� �������� � ����������� ����� ����������� ����� ������� RandomCounter.
#endregion

#region AddScoped
//  ����� AddScoped ������� ���� ��������� ������� ��� ����� �������. �� ����� �� �� ������������� ������, ��� � AddTransient.
//  ��� ��� ���������� ������� ��� ���������� ��������� �������:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddScoped<ICounter, RandomCounter>();
//builder.Services.AddScoped<CounterService>();

//var app = builder.Build();

//app.UseMiddleware<CounterMiddleware>();

//app.Run();

//  ������ � ������ ������ � ���� �� ������� � CounterMiddleware � ������ CounterService ����� ������������ ���� � ��� �� ������
//  RandomCounter. ��� ��������� ������� � ���������� ����� �������������� ����� ������ RandomCounter.
#endregion

#region AddSingleton
//  AddSingleton ������� ���� ������ ��� ���� ����������� ��������, ��� ���� ������ ��������� ������ �����, ����� �� ���������������
//  ���������. ���� ����� ����� ��� �� �� ������������ ������, ��� � AddTransient � AddScoped.

//  ��� ���������� AddSingleton ������� ��� ����������:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddSingleton<ICounter, RandomCounter>();
//builder.Services.AddSingleton<CounterService>();

//var app = builder.Build();

//app.UseMiddleware<CounterMiddleware>();

//app.Run();

//  ��� �������� singleton-�������� ������������� ���������� �� �������� Depedency Injection. �� ��� ����� ���� ������� �
//  �������� � ������ �����:

var builder = WebApplication.CreateBuilder();

RandomCounter rndCounter = new RandomCounter();
builder.Services.AddSingleton<ICounter>(rndCounter);
builder.Services.AddSingleton<CounterService>(new CounterService(rndCounter));

var app = builder.Build();

app.UseMiddleware<CounterMiddleware>();

app.Run();
//  ������ ���������� � ������ ������ ����� ���������� ����������� �������.
#endregion