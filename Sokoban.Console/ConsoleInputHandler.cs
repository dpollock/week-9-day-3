using System;

namespace Sokoban.Console
{
    public class ConsoleInputHandler : IInputHandler
    {
        public Command GetCommand()
        {
            if (!System.Console.KeyAvailable)
            {
                return Command.None;
            }

            var keyPressed = System.Console.ReadKey(true);
            switch (keyPressed.Key)
            {
                case ConsoleKey.LeftArrow:
                    return Command.MoveLeft;
                case ConsoleKey.UpArrow:
                    return Command.MoveUp;
                case ConsoleKey.RightArrow:
                    return Command.MoveRight;
                case ConsoleKey.DownArrow:
                    return Command.MoveDown;
                case ConsoleKey.Escape:
                    return Command.Quit;
                case ConsoleKey.R:
                    return Command.Restart;
                default:
                    return Command.None;
            }
        }
    }
}