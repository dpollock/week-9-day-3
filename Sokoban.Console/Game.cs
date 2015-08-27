namespace Sokoban.Console
{
    public class Game
    {
        private readonly IGameRender _gameRender;
        private readonly IInputHandler _inputHandler;
        private readonly IMapLoader _mapLoader;
        private World _world;
        public bool RequiresRedraw = true;

        public Game(IInputHandler inputHandler, IGameRender gameRender, IMapLoader mapLoader)
        {
            _inputHandler = inputHandler;
            _gameRender = gameRender;
            _mapLoader = mapLoader;
        }

        public void Play()
        {
            _world = new World(_mapLoader.Load(0));

            while (true)
            {
                if (RequiresRedraw)
                {
                    _gameRender.Draw(_world);
                    RequiresRedraw = false;
                }

                bool wantsToQuit = GetInput();
                if (wantsToQuit)
                    break;


                if (!_world.AnyUnsolvedBarrels())
                {
                    _gameRender.Draw(_world);
                    _gameRender.DrawWin();
                    break;
                }
            }
        }

        private bool GetInput()
        {
            var command = _inputHandler.GetCommand();
            switch (command)
            {
                case Command.MoveLeft:
                case Command.MoveRight:
                case Command.MoveUp:
                case Command.MoveDown:
                    var playerPoint = _world.GetPlayer();
                    bool result =  MoveObject(command, playerPoint);
                    break;
                case Command.Quit:
                    return true;
                case Command.Restart:
                    _world.Reset();
                    RequiresRedraw = true;
                    break;
            }

            return false;
        }

        private bool MoveObject(Command cmd, Point point)
        {
            var tile = _world.GetTile(point);
            var newPosition = GetNewPosition(cmd, point);
            if (IsValidMove(cmd, newPosition, tile))
            {
                UpdateOldAndNewSpots(point, newPosition, tile);
                RequiresRedraw = true;
                return true;
            }

            return false;

        }

        private void UpdateOldAndNewSpots(Point oldPoint, Point newPoint, TileType type)
        {
            var oldSpot = _world.GetTile(oldPoint);
            var newSpot = _world.GetTile(newPoint);

            switch (oldSpot)
            {
                case TileType.Player:
                case TileType.Barrel:
                    _world.SetTile(oldPoint, TileType.Blank);
                    break;
                case TileType.Storage:
                case TileType.BarrelInStorage:
                case TileType.PlayerOverStorage:
                    _world.SetTile(oldPoint, TileType.Storage);
                    break;
            }


            switch (newSpot)
            {
                case TileType.Blank:
                    _world.SetTile(newPoint, type & ~TileType.Storage);
                    break;
                case TileType.Storage:
                    _world.SetTile(newPoint, type | TileType.Storage );
                    break;
            }
        }

        private Point GetNewPosition(Command cmd, Point newPoint)
        {
            var position = new Point(newPoint.Y, newPoint.X);
            switch (cmd)
            {
                case Command.MoveLeft:
                    position.X -= 1;
                    break;
                case Command.MoveUp:
                    position.Y -= 1;
                    break;
                case Command.MoveRight:
                    position.X += 1;
                    break;
                case Command.MoveDown:
                    position.Y += 1;
                    break;
            }

            return position;
        }

        private bool IsValidMove(Command cmd, Point newPoint, TileType type)
        {
            var newSpot = _world.GetTile(newPoint);

            if (newSpot.IsWall())
                return false;

            if (newSpot.IsPassable())
                return true;

            if (newSpot.ContainsBarrel())
            {
                if (!type.ContainsPlayer())
                    return false;

                var newBarrelPosition = GetNewPosition(cmd, newPoint);
                if (IsValidMove(cmd, newBarrelPosition, newSpot)) //can barrel move here?
                {
                    return MoveObject(cmd, newPoint);
                }
            }

            return false;
        }
    }
}