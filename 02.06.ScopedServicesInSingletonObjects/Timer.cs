public class Timer : ITimer
{
    public string Time { get; }
    public Timer()
    {
        Time = DateTime.Now.ToLongTimeString(); 
    }
}