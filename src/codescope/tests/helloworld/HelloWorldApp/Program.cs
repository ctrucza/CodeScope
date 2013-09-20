using HelloWorldLib;

namespace HelloWorldApp
{
    internal interface IHelloWorld
    {
        
    }

    enum WorldType
    {
        
    }

    class Program
    {
        static void Main(string[] args)
        {
            HelloWorld helloWorld = new HelloWorld();
            helloWorld.SayHi();
        }
    }
}
