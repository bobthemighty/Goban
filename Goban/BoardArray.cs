using System.Collections.Generic;

namespace Goban
{
    /// <summary>
    /// Maps between 2D coordinates and 1D indices into the Elements array
    /// Coordinates off the edges are guaranteed to map to valid array indices
    /// which makes neighbour look ups trivial.
    /// </summary>
    internal class BoardArray
    {
        private readonly int _width;
        private readonly int _height;

        public BoardArray(int width, int height)
        {
            _width = width;
            _height = height;
            Elements = new State[1 + ((height + 2)* (width + 1))];
            InitBorders();
        }

        private void InitBorders()
        {
            var c = 0;
            var r = 0;
            while (c <= _width + 1)
            {
                Elements[c++] = State.Grey;
            }

            while (r ++ < _height)
            {
                c += _width;
                Elements[c++] = State.Grey;
            }

            while (c < Elements.Length)
                Elements[c++] = State.Grey;

        }

        public State[] Elements { get; private set; }

        public int Above(int index)
        {
            return index - (1 + _width);
        }

        public int Below(int index)
        {
            return index + (1 + _width);
        }

        public int Right(int index)
        {
            return index + 1;
        }

        public int Left(int index)
        {
            return index - 1;
        }

        public Point Coordinates(int index)
        {
            var y = (index/(_width + 1) - 1);
            var x = (index  %(_width + 1) - 1);
            return new Point(x, y);
        }

        public int Index(Point point)
        {
            return (_width + 2) + point.Y*(_width + 1) + point.X;
        }

        public IEnumerable<int> NeighboursOf(Point point)
        {
            var pos = Index(point);
            if (Elements[Above(pos)] != State.Grey) yield return Above(pos);
            if (Elements[Right(pos)] != State.Grey) yield return Right(pos);
            if (Elements[Below(pos)] != State.Grey) yield return Below(pos);
            if (Elements[Left(pos)] != State.Grey) yield return Left(pos);
        }

        public void Set(int x, int y, Colour state)
        {
            Elements[Index(new Point(x, y))] = (State)state;
        }

        public State StateAt(int x, int y)
        {
            return Elements[Index(new Point(x, y))];
        }
    }
}