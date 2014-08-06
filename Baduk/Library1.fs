namespace Fuseki

type Board (?height: int, ?width: int) =

    let _height = match height with 
        | Some h -> h
        | None -> 19

    let _width = match width with 
        | Some w -> w
        | None -> _height

    member x.Groups = []


