using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sokoban.Models
{
    public class GameBoard
    {
        public Square[,] Map { get; set; }
    }

    public class Square
    {
      public string Space { get; set; }
      public string Piece { get; set; }
    }  
    public class Gamestate
    {
        public bool IsGameOn { get; set; }
        public bool IsGameWinnable { get; set; }
        public int NumberOfMoves { get; set; }
        public string ScoreReport { get; set; }
        public TimeSpan ElapsedTime { get; set; }
    }
   public class GamePlay
    {
       
        public string Direction { get; set; }

        public static bool IsGameOn()
        {
            return true;
        }
        public static Square[,] Move(string direction, Square[,] map)
        {
          
                Location currentspace = GetCurrentPlayerSpace(map);
                Location nextspace = GetNextSpace(currentspace, direction);
                if (IsValidMove(nextspace, direction, map))
                {
                    if (IsPushingCrate(nextspace, map))
                    {
                        Location nextcratespace = GetNextSpace(nextspace, direction);
                        map[nextcratespace.Y, nextcratespace.X].Piece = "Crate";
                    }
                    map[nextspace.Y, nextspace.X].Piece = "Man";
                    map[currentspace.Y, currentspace.X].Piece = "None";
                }

           
            return map;
        }
        public static bool IsValidMove(Location nextspace, string direction, Square[,] map)
        {  
            
            //check out of bounds
            if (nextspace.X < 0 || nextspace.Y < 0 || nextspace.X > map.GetLength(1) || nextspace.Y > map.GetLength(0))
            {
                return false;
            }
            //check if hitting wall
            if (map[nextspace.Y,nextspace.X].Space == "Wall")
            {
                return false;
            }
            //check if pushing crate against a wall
            if (IsPushingCrate(nextspace, map))
            {
                var nextcratespace = GetNextSpace(nextspace, direction);
                if (map[nextcratespace.Y,nextcratespace.X].Space == "Wall" || IsPushingCrate(nextcratespace, map))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool IsPushingCrate(Location nextspace, Square[,] map)
        {
            if (map[nextspace.Y,nextspace.X].Piece == "Crate")
            {
                return true;
            }
            return false;
        }
        public static Location GetNextSpace(Location currentspace, string direction)
        {
            Location nextspace = new Location();
            switch (direction)
            {  
                case "up":
                    nextspace.X = currentspace.X;
                    nextspace.Y = currentspace.Y - 1;
                    break;

                case "down":
                    nextspace.X = currentspace.X;
                    nextspace.Y = currentspace.Y + 1;
                    break;

                case "left":
                    nextspace.X = currentspace.X - 1;
                    nextspace.Y = currentspace.Y;
                    break;

                case "right":
                    nextspace.X = currentspace.X + 1;
                    nextspace.Y = currentspace.Y;
                    break;
                    
                default:
                    nextspace.X = 1;
                    nextspace.Y = 1;
                    break;
            }
            return nextspace;
        }
       
        public static Location GetCurrentPlayerSpace(Square[,] map)
        {
            Location space = new Location();
            for (int i = 0; i< map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i,j].Piece == "Man")
                    {
                        space.X = j;
                        space.Y = i;
                        break;
                    }
                }
            }
            return space;
            
        }
        public Square[,] LoadMap(char[,] map)
        {
            Square[,] board = new Square[map.GetLength(0), map.GetLength(1)];
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {

                    Square square = new Square();
                    board[i, j] = square;
                    switch (map[i, j])
                    {

                        case '.':
                            square.Space = "Storage";
                            square.Piece = "None";
                            break;

                        case 'o':
                            square.Space = "Open";
                            square.Piece = "Crate";
                            break;

                        case '@':
                            square.Space = "Open";
                            square.Piece = "Man";
                            break;

                        case '*':
                            square.Space = "Storage";
                            square.Piece = "Crate";
                            break;

                        case '+':
                            square.Space = "Storage";
                            square.Piece = "Man";
                            break;

                        case '#':
                            square.Space = "Wall";
                            square.Piece = "None";
                            break;

                        default:
                            square.Space = "Open";
                            square.Piece = "None";
                            break;
                    }
                }

            }
            return board;
        }
       
    }

    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }
    }


}