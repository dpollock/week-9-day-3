namespace Sokoban.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var g = new Game(new ConsoleInputHandler(),
                             new ConsoleGameRender(),
                             new TextMapLoader(@"C:\IronYard\sokoban_levels.txt"));

            g.Play();
        }
    }
}