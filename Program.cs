class Program
{
    static void Main(string[] args)
    {
        Example.Log("start");
        var example = new Alpha();
        example.Run().Wait();
        Example.Log("end");
    }
}
