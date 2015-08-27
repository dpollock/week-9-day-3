using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sokoban.Console
{
    public interface IMapLoader
    {
        Tile[,] Load(int mapNumber);
    }

    public class TextMapLoader : IMapLoader
    {
        private readonly string _fileName;

        public TextMapLoader(string fileName)
        {
            _fileName = fileName;
        }

        public Tile[,] Load( int mapNumber)
        {
            List<Tile[,]> maps = new List<Tile[,]>();
            string[] rows = File.ReadAllLines(_fileName);

            List<char[]> currentMapRows = new List<char[]>();
            foreach (var row in rows)
            {

                if (String.IsNullOrWhiteSpace(row))
                {
                    int xSize = currentMapRows.Max(x => x.Length);
                    int ySize = currentMapRows.Count;

                    var currentMap = new Tile[ySize, xSize];
                    for (int i = 0; i < ySize; i++)
                    {
                        for (int j = 0; j < xSize; j++)
                        {
                            currentMap[i, j] = new Tile() { Type = TileType.Blank };
                            if (j <= currentMapRows[i].Length - 1)
                            {
                                switch (currentMapRows[i][j])
                                {
                                    case ' ':
                                        currentMap[i, j].Type = TileType.Blank;
                                        break;
                                    case '@':
                                        currentMap[i, j].Type = TileType.Player;
                                        break;
                                    case '*':
                                        currentMap[i, j].Type = TileType.BarrelInStorage;
                                        break;
                                    case 'o':
                                        currentMap[i, j].Type = TileType.Barrel;
                                        break;
                                    case '+':
                                        currentMap[i, j].Type = TileType.PlayerOverStorage;
                                        break;
                                    case '.':
                                        currentMap[i, j].Type = TileType.Storage;
                                        break;
                                    case '#':
                                        currentMap[i, j].Type = TileType.Wall;
                                        break;


                                }


                            }

                        }
                    }

                    maps.Add(currentMap);

                    currentMapRows = new List<char[]>();
                }
                else
                {
                    currentMapRows.Add(row.ToCharArray());
                }

            }

            var loadMap = maps[mapNumber];

            return loadMap;

        
    }
    }
}