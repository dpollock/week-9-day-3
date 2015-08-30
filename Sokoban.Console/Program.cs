using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.Console
{
    class Program
    {
        // convert positions to key value pairs for more clarity

        public static char[][] Map
        {
            get; set;
        }
        public static bool MapChanged { get; set; }
        public static bool GameInProgress { get; private set; }

        static void Main(string[] args)
        {
            Map = new char[][] {
           new [] { '#','#','#','#','#','#'},
           new [] { '#',' ',' ',' ',' ','#'},
           new [] { '#',' ','.','o',' ','#'},
           new [] { '#',' ',' ',' ','@','#'},
           new [] { '#','#','#','#','#','#'} };
            // There isn't a rendered map yet, so the map is changed, from blank to initial draw.
            MapChanged = true;
            GameInProgress = true;
            do
            {
                DrawMap();
                //if (Map.Any(x => x.Contains('o')))
                //{
                //    Moves.GetUserInput();
                //} else
                //{
                //    GameInProgress = false;
                //}
                if (GameInProgress)
                    Moves.GetUserInput();
            } while (GameInProgress);
            System.Console.WriteLine("Game over!");
            System.Console.ReadLine();
        }


        private static void DrawMap()
        {
            if (!MapChanged)
                return;

            int barrelCount = 0;
            System.Console.Clear();
            for (int i = 0; i < Map.Length; i++)
            {
                for (int j = 0; j < Map[i].Length; j++)
                {
                    if (Map[i][j] == '@')
                    {
                        Moves.PlayersPosition[0] = i;
                        Moves.PlayersPosition[1] = j;
                    }
                    if (Map[i][j] == 'o')
                        barrelCount++;
                    System.Console.Write(Map[i][j]);
                }
                System.Console.WriteLine();
            }
            MapChanged = false;
            if (barrelCount == 0)
                GameInProgress = false;
        }
    }

    public class EndGame
    {

    }

    public class Moves
    {
        public static int x = 1, y = 0;
        public static int[] PlayersPosition = new int[2];

        public static void GetUserInput()
        {
            bool validKey = false;
            while (!validKey)
            {
                var keyPressed = System.Console.ReadKey(true);
                switch (keyPressed.Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.DownArrow:
                        validKey = true;
                        MovePlayer(keyPressed.Key);
                        return;
                    default:
                        return;
                }
            }
        }

        private static void MovePlayer(ConsoleKey key)
        {
            int[] newPlayerPosition = new int[2];
            newPlayerPosition[0] = PlayersPosition[0];
            newPlayerPosition[1] = PlayersPosition[1];
            int[] direction = new int[] { 0, 0 };
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    newPlayerPosition[x]--;
                    direction[x] = -1;
                    direction[y] = 0;
                    ChangePlayerPosition(newPlayerPosition, direction);
                    break;
                case ConsoleKey.RightArrow:
                    newPlayerPosition[x]++;
                    direction[x] = 1;
                    direction[y] = 0;
                    ChangePlayerPosition(newPlayerPosition, direction);
                    break;
                case ConsoleKey.UpArrow:
                    newPlayerPosition[y]--;
                    direction[x] = 0;
                    direction[y] = -1;
                    ChangePlayerPosition(newPlayerPosition, direction);
                    break;
                case ConsoleKey.DownArrow:
                    newPlayerPosition[y]++;
                    direction[x] = 0;
                    direction[y] = 1;
                    ChangePlayerPosition(newPlayerPosition, direction);
                    break;
                default:
                    break;
            }
        }

        private static void ChangePlayerPosition(int[] newPlayerPosition, int[] direction)
        {
            if (AttemptMove(newPlayerPosition, direction))
            {
                PlayersPosition = newPlayerPosition;
                Program.MapChanged = true;
            }
        }

        private static bool AttemptMove(int[] playersNewPosition, int[] direction)
        {
            char spot = Program.Map[playersNewPosition[y]][playersNewPosition[x]];
            switch (spot)
            {
                case '#':
                    return false;
                case '.':
                    MovePlayer(playersNewPosition, true);
                    return true;
                case ' ':
                    MovePlayer(playersNewPosition, false);
                    return true;
                // If moving barrels
                case 'o':
                case '*':
                    char nextSpot = Program.Map[(playersNewPosition[y] + direction[y])][(playersNewPosition[x] + direction[x])];
                    switch (nextSpot)
                    {
                        case '#':
                        case 'o':
                        case '*':
                            return false;
                        case '.':
                            MovePlayerAndBarrel(spot, playersNewPosition, direction, true);
                            break;
                        case ' ':
                            MovePlayerAndBarrel(spot, playersNewPosition, direction, false);
                            break;
                        default:
                            break;
                    }
                    return true;
                default:
                    return false;
            }
        }

        private static void MovePlayer(int[] playersNewPosition, bool isCombiningPlayerAndStorage)
        {
            // Next spot
            if (isCombiningPlayerAndStorage)
            {
                Program.Map[playersNewPosition[y]][playersNewPosition[x]] = '+';
            }
            else if (!isCombiningPlayerAndStorage)
            {
                Program.Map[playersNewPosition[y]][playersNewPosition[x]] = '@';
            }
            // Current spot
            if (Program.Map[PlayersPosition[y]][PlayersPosition[x]] == '@')
            {
                Program.Map[PlayersPosition[y]][PlayersPosition[x]] = ' ';
            }
            else if (Program.Map[PlayersPosition[y]][PlayersPosition[x]] == '+')
            {
                Program.Map[PlayersPosition[y]][PlayersPosition[x]] = '.';
            }
        }

        private static void MovePlayerAndBarrel(char spot, int[] playersNewPosition, int[] direction, bool isCombiningBarrelAndStorage)
        {
            // Next spot
            if (isCombiningBarrelAndStorage)
            {
                Program.Map[(playersNewPosition[y] + direction[y])][(playersNewPosition[x] + direction[x])] = '*';
            }
            else
            {
                Program.Map[(playersNewPosition[y] + direction[y])][(playersNewPosition[x] + direction[x])] = 'o';
            }
            // This spot
            if (spot == 'o')
            {
                Program.Map[playersNewPosition[y]][playersNewPosition[x]] = '@';
            }
            else if (spot == '*')
            {
                Program.Map[playersNewPosition[y]][playersNewPosition[x]] = '+';
            }
            // Current spot

            // If the current position is just the player
            if (Program.Map[PlayersPosition[y]][PlayersPosition[x]] == '@')
            {
                Program.Map[PlayersPosition[y]][PlayersPosition[x]] = ' ';
            }
            // If the current position is the player and storage
            if (Program.Map[PlayersPosition[y]][PlayersPosition[x]] == '+')
            {
                Program.Map[PlayersPosition[y]][PlayersPosition[x]] = '.';
            }
        }
    }
}
