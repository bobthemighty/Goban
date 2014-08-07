using System.Linq;
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
}
