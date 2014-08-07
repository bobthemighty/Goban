using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CuttingEdge.Conditions;
using Machine.Specifications;

namespace Goban.Test
{
    public class When_creating_an_empty_board
    {
        Because we_create_a_new_board = () =>
            The_board = new Board();

        It has_no_groups = () =>
            The_board.Groups.ShouldBeEmpty();

        It should_be_black_to_move = () =>
            The_board.ToPlay.ShouldEqual(Colour.Black);

        static Board The_board;
    }

    public class When_setting_the_only_point_on_a_one_by_one_board
    {
        Establish context = () => The_board = new Board(3, 3);

        Because we_set_the_only_available_stone = () => The_board.Set(0, 0, Colour.White);

        It should_have_the_correct_colour = () => 
            The_board.Get(0, 0).State.ShouldEqual(State.White);
        
        private static Board The_board;
    }

    public class When_getting_coords_for_a_silly_board_size
    {
        Establish context = () =>
            the_board = new Board(1, 7);

        It should_return_the_correct_coordinates_for_the_first_non_grey_position = () =>
            the_board.Coordinates(4).ShouldEqual(new Point(0, 0));



        private static Board the_board;
    }

    public class When_we_set_a_stone_in_a_valid_position
    {
        Establish context = () =>
            The_board = new Board();

        Because we_set_a_stone = () =>
            The_board.Set(9, 9, Colour.Black);

        It should_have_one_group = () =>
            The_board.Groups.Count().ShouldEqual(1);

        It should_return_the_correct_colour_for_the_position = () =>
            The_board.Get(9, 9).State.ShouldEqual(State.Black);

        It should_have_the_correct_move_number_for_the_position = () =>
            The_board.Get(9, 9).MoveNumber.ShouldEqual(1);

        private static Board The_board;
    }

    internal enum State
    {
        Grey = 1,
        Empty = 0,
        Black =2 ,
        White = 3
    }

    internal enum Colour
    {
        Empty = 0,
        Black =2,
        White =3
    }


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

    internal struct Point
    {
        private readonly int _x;
        private readonly int _y;

        public Point(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X
        {
            get { return _x; }
        }

        public int Y
        {
            get { return _y; }
        }

        public bool Equals(Point other)
        {
            return _x == other._x && _y == other._y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Point && Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_x*397) ^ _y;
            }
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", _x, _y);
        }
    }

    internal class Stone
    {
        public Stone(State state, int moveNumber)
        {
            State = state;
            MoveNumber = moveNumber;
        }

        public State State { get; private set; }
        public int MoveNumber { get; private set; }
    }
}
