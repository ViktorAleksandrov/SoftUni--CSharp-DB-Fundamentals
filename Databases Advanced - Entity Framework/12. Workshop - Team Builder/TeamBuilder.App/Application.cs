using TeamBuilder.App.Core;

namespace TeamBuilder.App
{
    class Application
    {
        static void Main(string[] args)
        {
            var engine = new Engine(new CommandDispatcher());

            engine.Run();
        }
    }
}
