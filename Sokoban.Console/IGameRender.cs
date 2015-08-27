namespace Sokoban.Console
{
    public interface IGameRender
    {
        void Draw(World map);
        void DrawWin();
    }
}