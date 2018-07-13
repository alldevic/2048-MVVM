namespace _2048.Core.Model
{
    public class Cell
    {
        public int Value { get; set; }

        public bool WasMerged { get; set; }

        public bool WasCreated { get; set; }

        public Coordinate Position { get; set; }

        public int X
        {
            get => Position.X;
            set => Position.X = value;
        }

        public int Y
        {
            get => Position.Y;
            set => Position.Y = value;
        }

        public Coordinate PreviousPosition { get; set; }

        public Cell(int X, int Y)
        {
            Position = new Coordinate(X, Y);
        }

        public bool IsEmpty()
        {
            return Value == 0;
        }
    }
}