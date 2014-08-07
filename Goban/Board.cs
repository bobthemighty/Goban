using System.Collections.Generic;
using System.Text.RegularExpressions;
using CuttingEdge.Conditions;

namespace Goban
{
    internal class Board
    {
        private readonly int _width;
        private readonly int _height;
        private int _nextGroup = 1;
        private State[] _board;
        private int[] _groups;
        private int[] _moveNumbers;
        private int _lastMove = 0;

        public Board(int width = 19, int height = 19)
        {
            Condition.Requires(width, "width").IsGreaterThan(0).IsLessThan(35);
            Condition.Requires(height, "height").IsGreaterThan(0).IsLessThan(35);
            _width = width;
            _height = height;
            ToPlay = Colour.Black;
            int size = ((height + 2) * (width+ 1) + 1);
            _board = InitBounds(new State[size]);
            _moveNumbers = new int[size];
            _groups = new int[size];
        }

        private State[] InitBounds(State[] p0)
        {
            Condition.Requires(p0).IsNotNull().IsLongerOrEqual(9);
            // set first row
            int p;

            for (p = 0; p < 2 + _width; p++)
            {
                p0[p] = State.Grey;
            }

            for (var row = 0; row < _height; row ++)
            {
                p0[p] = State.Grey;
                p += _width+1;
                p0[p++] = State.Grey;
            }

            for (var col = 0; col <= _width+1; col ++)
            {
                p0[p + col] = State.Grey;
            }
            return p0;
        }

        public IEnumerable<Group> Groups
        {
            get { yield break; }
        }

        public Colour ToPlay { get; private set; }

        public void Set(int x, int y, Colour state)
        {
            Condition.Requires(x).IsGreaterOrEqual(0).IsLessThan(_width);
            Condition.Requires(y).IsGreaterOrEqual(0).IsLessThan(_height);
            var pos = Index(x, y);
            _board[pos] = (State)state;
            _moveNumbers[pos] = (++_lastMove);
        }

        public Stone Get(int x, int y)
        {
            Condition.Requires(x).IsGreaterOrEqual(0).IsLessThan(_width);
            Condition.Requires(y).IsGreaterOrEqual(0).IsLessThan(_height);
            var pos = Index(x, y);
            var state = _board[pos];
            var moveNumber = _moveNumbers[pos];
            return new Stone(state, moveNumber);
        }   

        private int Index(int x, int y)
        {
            return 4 + (2*x) + (x*y) + (2*y);
        }

        public Point Coordinates(int idx)
        {
            return new Point((idx / (_width + 3 ) -1 ),  ((idx % (_height + 3)) - 1));
        }
    }
}