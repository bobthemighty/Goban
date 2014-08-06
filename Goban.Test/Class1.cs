using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;

namespace Goban.Test
{
    public class When_creating_an_empty_board
    {
        Because we_create_a_new_board = () =>
            The_board = new Board();
    }
}
