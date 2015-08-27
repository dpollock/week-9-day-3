using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sokoban.Console
{
    [Flags]
    [Serializable]
    public enum TileType
    {
        Blank = 0,
        Wall = 1,
        Barrel = 2,
        Storage = 4,
        Player = 8,
        BarrelInStorage = Barrel | Storage,
        PlayerOverStorage = Player | Storage,
        Impassable = Wall,
        Passable = Blank | Storage
    }

    public static class TileTypeExtensions
    {
        public static T DeepCopy<T>(this T other)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T) formatter.Deserialize(ms);
            }
        }

        public static bool IsWall(this TileType spot)
        {
            return (spot & TileType.Wall) != 0;
        }

        public static bool IsPassable(this TileType spot)
        {
            return (TileType.Passable | spot) == TileType.Passable;
        }

        public static bool ContainsBarrel(this TileType spot)
        {
            return (spot & TileType.Barrel) != 0;
        }

        public static bool ContainsPlayer(this TileType spot)
        {
            return (spot & TileType.Player) != 0;
        }
    }

    [Serializable]
    public class Tile
    {
        public TileType Type { get; set; }
    }
}