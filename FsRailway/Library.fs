module FsRailway

[<AutoOpenAttribute>]
module Result =
    open System.Threading.Tasks
    open FSharp.Control.Tasks.ContextInsensitive

    let bindAsync (a : Task<Result<'a, 'b>>) (b : 'a -> Task<Result<'c, 'b>>) =
        task {
            match! a with
            | Ok a -> return! b a
            | Error e -> return Error e
        }

    let bindSyncToAsync (a : Result<'a, 'b>) (b : 'a -> Task<Result<'c, 'b>>) =
        task {
            match a with
            | Ok a -> return! b a
            | Error e -> return Error e
        }