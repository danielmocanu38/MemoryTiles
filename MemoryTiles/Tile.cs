using System;

namespace MemoryTiles
{
    [Serializable]
    public class Tile
    {
        public int Value { get; set; }
        public bool IsFlipped { get; private set; }
        public bool IsMatched { get; set; }
        public string FrontImagePath { get; set; }

        public int Row { get; set; }
        public int Column { get; set; }

        public Tile(int value, bool isMatched)
        {
            Value = value;
            IsMatched = isMatched;
            FrontImagePath = "";
        }

        public Tile(int value, bool isFlipped, bool isMatched, string frontImagePath)
        {
            Value = value;
            IsFlipped = isFlipped;
            IsMatched = isMatched;
            FrontImagePath = frontImagePath;
        }

        public void Flip()
        {
            IsFlipped = true;
        }

        public void Unflip()
        {
            IsFlipped = false;
        }
    }
}
