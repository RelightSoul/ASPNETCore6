//public class TimerMiddleware
//{    
//    TimeService timeService;
//    public TimerMiddleware(RequestDelegate next, TimeService timeService)
//    {
//        this.timeService = timeService;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        await context.Response.WriteAsync($"Time: {timeService.GetTime()}");
//    }

//}

public class TimerMiddleware
{
    public TimerMiddleware(RequestDelegate next) { }

    public async Task Invoke(HttpContext context, TimeService timeService)
    {
        await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");
    }
}