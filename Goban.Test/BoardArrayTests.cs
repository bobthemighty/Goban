using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;

namespace Goban.Test
{
    public class When_initializing_a_one_by_one_board
    {
        Because we_create_a_one_by_one_board = () =>
            The_board = new BoardArray(1, 1);

        /// <remarks>
        /// A la gnugo and tesuji we create an array with padding around the edges
        /// A one element board expands to 
        /// 
        /// oo
        /// ox
        /// ooo
        /// 
        /// where o are Grey blocks and x are empty.
        /// </remarks>
        It should_create_a_board_with_padding_around_the_top_left_and_bottom = () =>
            The_board.Elements.Length.ShouldEqual(7);

        It should_contain_a_single_empty_element = ()
            => The_board.Elements.Count(x => x == State.Empty).ShouldEqual(1);

        It should_place_the_empty_element_in_the_4th_position_of_the_array = () =>
            The_board.Elements[3].ShouldEqual(State.Empty);

        It should_return_the_coords_for_the_empty_point = () =>
            The_board.Coordinates(3).ShouldEqual(new Point(0, 0));

        It should_return_the_correct_index_for_the_empty_point = () =>
            The_board.Index(new Point(0, 0)).ShouldEqual(3);

        It should_return_no_neighbours_for_the_empty_point = () =>
            The_board.NeighboursOf(new Point(0, 0)).ShouldBeEmpty();

        private static BoardArray The_board;
    }


    public class When_initializing_a_two_by_one_board
    {
        Because we_create_a_two_by_one_board = () =>
            The_board = new BoardArray(2, 1);

        /// <remarks>
        /// A la gnugo and tesuji we create an array with padding around the edges
        /// A 2 * 1 element board expands to 
        /// 
        /// ooo
        /// oxx
        /// oooo
        /// 
        /// where o are Grey blocks and x are empty.
        /// </remarks>
        It should_create_a_board_with_padding_around_the_top_left_and_bottom = () =>
            The_board.Elements.Length.ShouldEqual(10);

        It should_contain_two_empty_elements = ()
            => The_board.Elements.Count(x => x == State.Empty).ShouldEqual(2);

        It should_place_the_first_empty_element_in_the_5th_position_of_the_array = () =>
            The_board.Elements[4].ShouldEqual(State.Empty);

        It should_place_the_second_empty_element_in_the_6th_position_of_the_array = () =>
            The_board.Elements[5].ShouldEqual(State.Empty);

        It should_return_the_coords_for_the_first_empty_point = () =>
            The_board.Coordinates(4).ShouldEqual(new Point(0, 0));


        It should_return_the_coords_for_the_second_empty_point = () =>
            The_board.Coordinates(5).ShouldEqual(new Point(1, 0));

        It should_return_the_correct_index_for_the_first_empty_point = () =>
            The_board.Index(new Point(0, 0)).ShouldEqual(4);

        It should_return_the_correct_index_for_the_second_empty_point = () =>
            The_board.Index(new Point(1, 0)).ShouldEqual(5);

        private static BoardArray The_board;
    }

    public class When_initializing_a_one_by_two_board
    {
        Because we_create_a_two_by_one_board = () =>
            The_board = new BoardArray(1, 2);

        /// <remarks>
        /// A la gnugo and tesuji we create an array with padding around the edges
        /// A 1 * 2 element board expands to 
        /// 
        /// oo
        /// ox
        /// ox
        /// ooo
        /// 
        /// where o are Grey blocks and x are empty.
        /// </remarks>
        It should_create_a_board_with_padding_around_the_top_left_and_bottom = () =>
            The_board.Elements.Length.ShouldEqual(9);

        It should_contain_two_empty_elements = ()
            => The_board.Elements.Count(x => x == State.Empty).ShouldEqual(2);

        It should_place_the_first_empty_element_in_the_4th_position_of_the_array = () =>
            The_board.Elements[3].ShouldEqual(State.Empty);

        It should_place_the_second_empty_element_in_the_6th_position_of_the_array = () =>
            The_board.Elements[5].ShouldEqual(State.Empty);

        It should_return_the_coords_for_the_first_empty_point = () =>
            The_board.Coordinates(3).ShouldEqual(new Point(0, 0));


        It should_return_the_coords_for_the_second_empty_point = () =>
            The_board.Coordinates(5).ShouldEqual(new Point(0, 1));

        It should_return_the_correct_index_for_the_first_empty_point = () =>
            The_board.Index(new Point(0, 0)).ShouldEqual(3);

        It should_return_the_correct_index_for_the_second_empty_point = () =>
            The_board.Index(new Point(0, 1)).ShouldEqual(5);

        It should_return_the_second_point_as_a_neighbour_of_the_first = () =>
            The_board.NeighboursOf(new Point(0, 0)).ShouldContainOnly(5);

        It should_return_the_first_point_as_a_neighbour_of_the_second = () =>
            The_board.NeighboursOf(new Point(0, 1)).ShouldContainOnly(3);


        private static BoardArray The_board;
    }

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
    }
}