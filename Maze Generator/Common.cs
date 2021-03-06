﻿using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Maze_Generator
{
    class Common
    {
        // Path directions
        private const int N = 1 << 0;
        private const int E = 1 << 1;
        private const int S = 1 << 2;
        private const int W = 1 << 3;

        public static bool IsInBounds(int x, int y, int[,] grid)
        {
            return x >= 0 && x <= grid.GetLength(0) - 1 &&
                   y >= 0 && y <= grid.GetLength(1) - 1;
        }

        public static void PrintMazeCommandLine(int[,] maze)
        {
            Console.Clear();

            Console.WriteLine(" " + new string('_', maze.GetLength(0) * 2 - 1));
            for (var y = 0; y < maze.GetLength(1); y++)
            {
                Console.Write("|");
                for (var x = 0; x < maze.GetLength(0); x++)
                {
                    // Bottom wall
                    Console.Write((maze[x, y] & S) != 0 ? " " : "_");

                    if ((maze[x, y] & E) != 0)
                    {
                        // Connecting bottom wall
                        Console.Write(((maze[x, y] | maze[x + 1, y]) & S) == 0 ? "_" : " ");
                    }
                    else
                    {
                        // Right wall
                        Console.Write("|");
                    }
                }

                Console.WriteLine();
            }
        }

        public static void PrintMazeCommandLine(int[,] maze, int currentX, int currentY, char character = 'X')
        {
            Console.Clear();

            Console.WriteLine(" " + new string('_', maze.GetLength(0) * 2 - 1));
            for (var y = 0; y < maze.GetLength(1); y++)
            {
                Console.Write("|");
                for (var x = 0; x < maze.GetLength(0); x++)
                {
                    // Bottom wall
                    if (x == currentX && y == currentY)
                    {
                        Console.Write(character);
                    }
                    else
                    {
                        Console.Write((maze[x, y] & S) != 0 ? " " : "_");
                    }

                    if ((maze[x, y] & E) != 0)
                    {
                        // Connecting bottom wall
                        Console.Write(((maze[x, y] | maze[x + 1, y]) & S) == 0 ? "_" : " ");
                    }
                    else
                    {
                        // Right wall
                        Console.Write("|");
                    }
                }

                Console.WriteLine();
            }
        }
        
        public static void PrintMazePNG(int[,] maze, String filename, 
                int cellSize = 1, int wallSize = 1,
                Color? cell = null, Color? wall = null)
        {
            // Default to black and white maze
            Color cellColour = cell ?? Color.White;
            Color wallColour = wall ?? Color.Black;

            Bitmap image = new Bitmap(
                (maze.GetLength(0) * (cellSize + wallSize)) + wallSize, 
                (maze.GetLength(1) * (cellSize + wallSize)) + wallSize, 
                PixelFormat.Format24bppRgb);

            // Top-most wall
            ColourArea(image, wallColour,
                0, 0, 
                image.Width, wallSize);


            // Left-most wall
            ColourArea(image, wallColour,
                0, 0,
                wallSize, image.Height);


            for (var x = 0; x < maze.GetLength(0); x++)
            {
                for (var y = 0; y < maze.GetLength(1); y++)
                {
                    // Coordinates for start of cell 
                    var cellX = (x * (cellSize + wallSize)) + wallSize;
                    var cellY = (y * (cellSize + wallSize)) + wallSize;

                    // Coordinates for start of wall
                    var cellWallX = cellX + cellSize;
                    var cellWallY = cellY + cellSize;

                    // Cell
                    ColourArea(image, cellColour, 
                        cellX, cellY, 
                        cellWallX, cellWallY);

                    // Right wall
                    ColourArea(image, (maze[x, y] & E) == 0 ? wallColour : cellColour,
                        cellWallX, cellY,
                        cellWallX + wallSize, cellWallY);

                    // Bottom wall
                    ColourArea(image, (maze[x, y] & S) == 0 ? wallColour : cellColour,
                        cellX, cellWallY, 
                        cellWallX, cellWallY + wallSize);

                    // Bottom Right wall
                    ColourArea(image, Color.DimGray,
                        cellWallX, cellWallY, 
                        cellWallX + wallSize, cellWallY + wallSize);
                }

            }

            image.Save(filename + ".png", ImageFormat.Png);
        }

        private static void ColourArea(Bitmap image, Color colour, int startX, int startY, int endX, int endY)
        {
            for (var x = startX; x < endX; x++)
            {
                for (var y = startY; y < endY; y++)
                {
                    image.SetPixel(x, y, colour);
                }
            }
        }

    }
}