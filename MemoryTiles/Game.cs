using System;

namespace MemoryTiles
{
    [Serializable]
    public class Game
    {
        private Board board;
        private int level;

        public Game()
        {
            board = new Board();
            level = 0;
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public void CreateBoard(int rows, int cols)
        {
            board = new Board(rows, cols);
        }

        public void SaveBoard(Board board)
        {
            this.board = board;
        }

        public Board getBoard()
        {
            return board;
        }

        public void incrementLevel()
        {
            level++;
        }
    }
}
