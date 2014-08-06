namespace Baduk.Test
open FsUnit
open Fuseki
open NUnit.Framework

[<NUnit.Framework.TestFixture>]
module Board =

    [<Test>]
    let ``An empty board has no groups`` () =
        let board = Board()

        board.Groups |> should be Empty
