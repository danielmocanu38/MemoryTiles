using System;

namespace MemoryTiles
{
    [Serializable]
    public class User
    {
        private string name;
        private string profileImagePath;
        private int playedGames;
        private int wonGames;
        private Game game;


        public User()
        {
            name = "";
            profileImagePath = "";
            playedGames = 0;
            wonGames = 0;
            game = new Game();
        }
        public User(string name, string profileImagePath)
        {
            this.name = name;
            this.profileImagePath = profileImagePath;
            playedGames = 0;
            wonGames = 0;
            game = new Game();
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            return Name;
        }

        public string ProfileImagePath
        {
            get { return profileImagePath; }
            set { profileImagePath = value; }
        }

        public int PlayedGames
        {
            get { return playedGames; }
            set { playedGames = value; }
        }
        public int WonGames
        {
            get { return wonGames; }
            set { wonGames = value; }
        }

        public Game Game
        {
            get { return game; }
            set { game = value; }
        }

        public void IncrementPlayedGames()
        {
            playedGames++;
        }

        public void IncrementWonGames()
        {
            wonGames++;
        }

        public void StartNewGame(int rows, int columns)
        {
            game.CreateBoard(rows, columns);
        }

        public void SaveBoardState(Board board)
        {
            game.SaveBoard(board);
        }

        public Board getCurrentBoard()
        {
            return game.getBoard();
        }
    }
}
