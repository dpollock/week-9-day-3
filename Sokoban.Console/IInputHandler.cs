namespace Sokoban.Console
{
    public enum Command
    {
        None,
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        Quit,
        Restart
    }

    public interface IInputHandler
    {
        Command GetCommand();
    }
}