public class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; } = 0;
    public List<string> Languages { get; set; } = new();
    public Company? Company { get; set; }
}