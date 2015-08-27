namespace Sokoban.Console
{
    public class World
    {
        private readonly Tile[,] _orginalMap;
        private Tile[,] _map;

        public World(Tile[,] map)
        {
            _map = map;

            _orginalMap = _map.DeepCopy();
        }

        public int YSize => _map.GetLength(0);
        public int XSize => _map.GetLength(1);

        public bool AnyUnsolvedBarrels()
        {
            for (var i = 0; i < YSize; i++)
            {
                for (var j = 0; j < XSize; j++)
                {
                    if (GetTile(i, j) == TileType.Barrel)
                        return true;
                }
            }

            return false;
        }

        public Point GetPlayer()
        {
            for (var y = 0; y < YSize; y++)
            {
                for (var x = 0; x < XSize; x++)
                {
                    var spot = GetTile(y, x);
                    if (spot.ContainsPlayer())
                    {
                        return new Point(y, x);
                    }
                }
            }

            return new Point(-1, -1);
        }

        public void SetTile(Point p, TileType type)
        {
            if (CheckBounds(p.Y, p.X))
            {
                return;
            }

            _map[p.Y, p.X].Type = type;
        }

        public TileType GetTile(int y, int x)
        {
            if (CheckBounds(y, x))
            {
                return TileType.Blank;
            }

            return _map[y, x].Type;
        }

        private bool CheckBounds(int y, int x)
        {
            return x < 0 || x >= XSize || y < 0 || y >= YSize;
        }

        public TileType GetTile(Point p)
        {
            return GetTile(p.Y, p.X);
        }

        public void Reset()
        {
            _map = _orginalMap.DeepCopy();
        }
    }
}