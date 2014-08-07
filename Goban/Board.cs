using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CuttingEdge.Conditions;

namespace Goban
{
    internal class Board
    {
        private readonly int _width;
        private readonly int _height;
        private readonly BoardArray _board;
        private short[] _moveNumbers;

        private short _lastMove = 0;

        public Board(int width = 19, int height = 19)
        {
            _width = width;
            _height = height;
            Condition.Requires(width, "width").IsGreaterThan(0).IsLessThan(35);
            Condition.Requires(height, "height").IsGreaterThan(0).IsLessThan(35);
            ToPlay = Colour.Black;
            _board = new BoardArray(width, height);
            _moveNumbers = new Int16[width*height];
        }

      

        public IEnumerable<Group> Groups
        {
            get { yield break; }
        }

        public Colour ToPlay { get; private set; }

        public void Play(int x, int y, Colour colour)
        {
            _lastMove ++;
            ToPlay = (ToPlay == Colour.Black ? Colour.White : Colour.Black);
            Set(x, y, colour);
        }

        public void Set(int x, int y, Colour state)
        {
            Condition.Requires(x).IsGreaterOrEqual(0).IsLessThan(_width);
            Condition.Requires(y).IsGreaterOrEqual(0).IsLessThan(_height);
            _board.Set(x, y, state);
            _moveNumbers[(y*_width) + x] = _lastMove;
        }

        public Stone Get(int x, int y)
        {
            Condition.Requires(x).IsGreaterOrEqual(0).IsLessThan(_width);
            Condition.Requires(y).IsGreaterOrEqual(0).IsLessThan(_height);
            var moveNum = _moveNumbers[(y*_width) + x];
            return new Stone(_board.StateAt(x,y), moveNum);
        }   
    }
}