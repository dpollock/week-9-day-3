using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace barrelgame
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderWorld(Map);
            while (true)
            {
                GetMove();
            }
        }
        public static void RenderWorld(char[,] map)
        {
            Console.Clear();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }
        public static void GetMove()
        {
            var keypressed = System.Console.ReadKey(true);

            MovePiece(keypressed);
        }

        public static void MovePiece(ConsoleKeyInfo key)
        {
            Location currentspace = new Location();
            currentspace = CurrentPlayerSpace();
            Location nextlocation = new Location();
            nextlocation = GetNextSpace(currentspace, key);
            char[] nextsymbol = GetNextSymbols(currentspace, nextlocation);

            if (IsMoveValid(nextlocation, key))
            {
               
                if (Map[nextlocation.Y, nextlocation.X] == 'o' || Map[nextlocation.Y, nextlocation.X] == '*')
                {
                    MoveBarrel(nextlocation, key);
                }
                Map[currentspace.Y, currentspace.X] = nextsymbol[1];
                Map[nextlocation.Y, nextlocation.X] = nextsymbol[0];
            }
            RenderWorld(Map);
        }
        public static Location CurrentPlayerSpace()
        {
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j] == '@'
                    || Map[i, j] == '+')
                    {
                        return new Location { X = j, Y = i };
                    }
                }
            }
            return new Location { X = 0, Y = 0 };
        }
        public static Location GetNextSpace(Location oldlocation, ConsoleKeyInfo key)
        {
            Location newlocation = new Location();
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    newlocation.X = oldlocation.X - 1;
                    newlocation.Y = oldlocation.Y;
                    break;

                case ConsoleKey.DownArrow:
                    newlocation.X = oldlocation.X;
                    newlocation.Y = oldlocation.Y + 1;
                    break;

                case ConsoleKey.RightArrow:
                    newlocation.X = oldlocation.X + 1;
                    newlocation.Y = oldlocation.Y;
                    break;

                case ConsoleKey.UpArrow:
                    newlocation.X = oldlocation.X;
                    newlocation.Y = oldlocation.Y - 1;
                    break;

                default:
                    newlocation.X = oldlocation.X;
                    newlocation.Y = oldlocation.Y;
                    break;
            }
            return newlocation;
        }
        private static char[] GetNextSymbols(Location currentspace, Location nextlocation)
        {
            //man moves from blank
            if (Map[currentspace.Y, currentspace.X] == '@') 
            {
                if (Map[nextlocation.Y, nextlocation.X] == ' ' || Map[nextlocation.Y, nextlocation.X] == 'o')//man moves to blank
                {
                    return new char[] { '@', ' ' };
                }
                if (Map[nextlocation.Y, nextlocation.X] == '.' || Map[nextlocation.Y, nextlocation.X] == '*')//man moves to storage
                {
                    return new char[] { '+', ' ' };
                }
            }
          
            //man moves from storage
            if (Map[currentspace.Y, currentspace.X] == '+')
            {
                if (Map[nextlocation.Y, nextlocation.X] == ' ' || Map[nextlocation.Y, nextlocation.X] == 'o')//man moves to blank
                {
                    return new char[] { '@', '.' };
                }
                if (Map[nextlocation.Y, nextlocation.X] == '.' || Map[nextlocation.Y, nextlocation.X] == '*')//man moves to storage
                {
                    return new char[] { '+', '.' };
                }
            }
           
            //barrel moves from blank
            if ((Map[currentspace.Y, currentspace.X] == 'o') )
            {
                if (Map[nextlocation.Y, nextlocation.X] == ' ')//barrel moves to blank
                {
                    return new char[] { 'o', '@' };
                }
                if (Map[nextlocation.Y, nextlocation.X] == '.')//barrel moves to storage
                {
                    return new char[] { '*', '@' };
                }
            }
           
            //barrel moves from storage
            if (Map[currentspace.Y, currentspace.X] == '*')
            {
                if (Map[nextlocation.Y, nextlocation.X] == ' ')//barrel moves to blank
                {
                    return new char[] { 'o', '+' };
                }
                if (Map[nextlocation.Y, nextlocation.X] == '.')//barrel moves to storage
                {
                    return new char[] { '*', '+' };
                }
            }
          
            return new char[] { ' ', ' ' };
        }
        public static bool IsMoveValid(Location loc, ConsoleKeyInfo key)
        {
            if (loc.X >= Map.GetLength(1) || loc.Y >= Map.GetLength(0) || loc.X < 0 || loc.Y < 0 || Map[loc.Y, loc.X] == '#') //check out of bounds or a wall
            {
                return false;
            }

            if (Map[loc.Y, loc.X] == 'o' || Map[loc.Y, loc.X] == '*') //check if pushing a barrel
            {
                Location nextbarrelspace = new Location();
                nextbarrelspace = GetNextSpace(loc, key);
                if (Map[nextbarrelspace.Y, nextbarrelspace.X] == 'o' //barrel pushing barrel
                    || Map[nextbarrelspace.Y, nextbarrelspace.X] == '#' //barrel against wall
                    || Map[nextbarrelspace.Y, nextbarrelspace.X] == '*' //barrel pushing barrel
                    || nextbarrelspace.X < 0 || nextbarrelspace.Y < 0 //barrel out of bounds
                    || nextbarrelspace.X >= Map.GetLength(1) || nextbarrelspace.Y >= Map.GetLength(0))
                {
                    return false;
                }
            }
            return true;
        }
        public static void MoveBarrel(Location oldloc, ConsoleKeyInfo key)
        {
            Location nextloc = new Location();
            nextloc = GetNextSpace(oldloc, key);
            var barrel = GetNextSymbols(oldloc, nextloc);
            Map[oldloc.Y, oldloc.X] = barrel[1];
            Map[nextloc.Y, nextloc.X] = barrel[0];
        }
        public static char[,] Map =
               {
                { '#','#','#','#','#','#' },
                {  '#',' ', ' ',' ',' ','#'},
                {  '#','o', '@','.',' ','#'},
                {  '#',' ', ' ',' ',' ','#'},
                 {  '#',' ', ' ',' ',' ','#'},
                { '#','#','#','#','#','#' }
            };
        public class Location
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
