using System;
using System.Collections.Generic;
using System.Linq;

namespace MemoryTiles
{
    [Serializable]
    public class Board
    {
        private int rows;
        private int columns;
        private Tile[,] tiles;

        public int Rows { get { return rows; } }
        public int Columns { get { return columns; } }

        public Board()
        {
            rows = 0;
            columns = 0;
            tiles = new Tile[rows, columns];
        }

        public Board(int numRows, int numColumns)
        {
            rows = numRows;
            columns = numColumns;
            tiles = new Tile[rows, columns];

            // Initialize the board with random tiles
            List<int> values = new List<int>();
            for (int i = 0; i < rows * columns / 2; i++)
            {
                values.Add(i);
                values.Add(i);
            }
            values = values.OrderBy(i => Guid.NewGuid()).ToList();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    tiles[i, j] = new Tile(values[i * columns + j], false);
                }
            }
        }

        public bool IsComplete()
        {
            foreach (Tile tile in tiles)
            {
                if (!tile.IsMatched)
                {
                    return false;
                }
            }
            return true;
        }

        public void FlipTile(int row, int column)
        {
            Tile tile = GetTile(row, column);
            tile.Flip();
        }

        public void UnflipTile(int row, int column)
        {
            Tile tile = GetTile(row, column);
            if (tile != null)
            {
                tile.Unflip();
            }
        }

        public Tile GetTile(int row, int column)
        {
            if (row < 0 || row >= Rows || column < 0 || column >= Columns)
            {
                return null; // return null if the row or column index is out of bounds
            }

            return tiles[row, column];
        }
    }
}
