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
            while (GameOn())
            {
                ConsoleKeyInfo direction = GetMove();
                Location currentSpace = CurrentPlayerSpace();
                Location nextSpace = GetNextSpace(currentSpace, direction);
                if (IsMoveValid(nextSpace, direction))
                {
                   
                   
                    MovePiece(currentSpace, nextSpace, direction);
                    RenderWorld(Map);
                    Console.WriteLine(Score());

                }
               
            }
            Console.WriteLine("Congratulations, You win!");
            Console.Read();
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
        public static ConsoleKeyInfo GetMove()
        {
            var keypressed = System.Console.ReadKey(true);
            return keypressed;
        }

        public static void MovePiece(Location currentspace, Location nextlocation, ConsoleKeyInfo direction)
        {
            char[] nextmansymbol = GetNextSymbols(currentspace, nextlocation);
            if (IsPushingBarrel(nextlocation))
            {
                Location nextBarrelSpace = GetNextSpace(nextlocation, direction);
                char[] nextBarrelSymbol = GetNextSymbols(nextlocation, nextBarrelSpace);
                Map[nextBarrelSpace.Y, nextBarrelSpace.X] = nextBarrelSymbol[1];
            }
           
            Map[currentspace.Y, currentspace.X] = nextmansymbol[0];
            Map[nextlocation.Y, nextlocation.X] = nextmansymbol[1];
           
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
        public static bool IsPushingBarrel(Location nextlocation)
        {
            if (Map[nextlocation.Y, nextlocation.X] == 'o' || Map[nextlocation.Y, nextlocation.X] == '*') //check if pushing a barrel
            {
                return true;
            }
            return false;
        }
        private static char[] GetNextSymbols(Location currentspace, Location nextlocation)
        {
            //man moves from blank
            if (Map[currentspace.Y, currentspace.X] == '@') 
            {
                if (Map[nextlocation.Y, nextlocation.X] == ' ' || Map[nextlocation.Y, nextlocation.X] == 'o')//man moves to blank
                {
                    return new char[] { ' ', '@' };
                }
                if (Map[nextlocation.Y, nextlocation.X] == '.' || Map[nextlocation.Y, nextlocation.X] == '*')//man moves to storage
                {
                    return new char[] { ' ', '+' };
                }
            }
          
            //man moves from storage
            if (Map[currentspace.Y, currentspace.X] == '+')
            {
                if (Map[nextlocation.Y, nextlocation.X] == ' ' || Map[nextlocation.Y, nextlocation.X] == 'o')//man moves to blank
                {
                    return new char[] { '.', '@' };
                }
                if (Map[nextlocation.Y, nextlocation.X] == '.' || Map[nextlocation.Y, nextlocation.X] == '*')//man moves to storage
                {
                    return new char[] { '.', '+' };
                }
            }
           
            //barrel moves from blank
            if ((Map[currentspace.Y, currentspace.X] == 'o') )
            {
                if (Map[nextlocation.Y, nextlocation.X] == ' ')//barrel moves to blank
                {
                    return new char[] { '@', 'o' };
                }
                if (Map[nextlocation.Y, nextlocation.X] == '.')//barrel moves to storage
                {
                    return new char[] { '@', '*' };
                }
            }
           
            //barrel moves from storage
            if (Map[currentspace.Y, currentspace.X] == '*')
            {
                if (Map[nextlocation.Y, nextlocation.X] == ' ')//barrel moves to blank
                {
                    return new char[] { '+', 'o' };
                }
                if (Map[nextlocation.Y, nextlocation.X] == '.')//barrel moves to storage
                {
                    return new char[] { '+', '*' };
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

            if (IsPushingBarrel(loc)) //check if pushing a barrel
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
      
        public static bool GameOn()
        {
            if (CountPiece('o') == 0)
            {
                return false;
            }
            return true;
        }
        public static string Score()
        {
            int barrels = CountPiece('o');
            int spots = CountPiece('.');
            int stored = CountPiece('*');
            return string.Format("You have {0} barrels to put away, {1} empty spots and {2} barrels stored!", barrels, spots, stored);
        }
        public static int CountPiece(char piece)
        {
            int k = 0;
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j] == piece)
                    
                    {
                        k++;
                    }
                }
            }
            return k;
        }
        public static char[,] Map =
              {{'#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#'},
{'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#',' ',' ',' ',' ',' ',' ',' ','#'},
{'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#',' ',' ',' ',' ',' ',' ',' ','#'},
{'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#',' ',' ',' ','#',' ',' ',' ','#'},
{'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#',' ',' ',' ','#',' ',' ',' ','#'},
{'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#',' ',' ',' ','#'},
{'#',' ',' ','o',' ',' ','o',' ',' ',' ',' ',' ',' ',' ',' ','#',' ',' ',' ','#'},
{'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#',' ','.',' ','#'},
{'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#',' ',' ',' ','#'},
{'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#',' ',' ',' ','#'},
{'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#',' ','.',' ','#'},
{'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#',' ',' ',' ','#'},
{'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#',' ',' ',' ','#'},
{'#',' ',' ',' ',' ',' ',' ',' ','o',' ',' ',' ',' ',' ',' ','#',' ',' ',' ','#'},
{'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#',' ',' ',' ','#'},
{'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
{'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','.',' ','#'},
{'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
{'#','@',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#',' ',' ',' ',' ',' ','#'},
{'#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#'}}
;
        public class Location
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
