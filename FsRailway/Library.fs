module FsRailway

open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive

[<AutoOpenAttribute>]
module Result =

    let bindTask (a : Task<Result<'a, 'b>>) (b : 'a -> Task<Result<'c, 'b>>) =
        task {
            match! a with
            | Ok a -> return! b a
            | Error e -> return Error e
        }

    let bindAsync (a : Async<Result<'a, 'b>>) (b : 'a -> Async<Result<'c, 'b>>) =
        async {
            match! a with
            | Ok a -> return! b a
            | Error e -> return Error e
        }

    let bindTaskToAsync (a : Task<Result<'a, 'b>>) (b : 'a -> Async<Result<'c, 'b>>) =
        task {
            match! a with
            | Ok a -> return! b a
            | Error e -> return Error e
        } 
        |> Async.AwaitTask

    let bindAsyncToTask (a : Async<Result<'a, 'b>>) (b : 'a -> Task<Result<'c, 'b>>) =
        task {
            match! a with
            | Ok a -> return! b a
            | Error e -> return Error e
        } 

    let bindSyncToTask (a : Result<'a, 'b>) (b : 'a -> Task<Result<'c, 'b>>) =
        task {
            match a with
            | Ok a -> return! b a
            | Error e -> return Error e
        }

    let bindSyncToAsync (a : Result<'a, 'b>) (b : 'a -> Async<Result<'c, 'b>>) =
        async {
            match a with
            | Ok a -> return! b a
            | Error e -> return Error e
        }

    let mapTask (mapping : 'T -> 'U) (a : Task<Result<'T, 'TError>>) =
        task {
            let! x = a
            return x |> Result.map mapping
        }

    let mapAsync (mapping : 'T -> 'U) (a : Async<Result<'T, 'TError>>) =
        async {
            let! x = a
            return x |> Result.map mapping
        }

    let mapErrorTask (mapping : 'TError -> 'U) (a : Task<Result<'a, 'TError>>) =
        task {
            let! x = a
            return x |> Result.mapError mapping
        }

    let mapErrorAsync (mapping : 'TError -> 'U) (a : Async<Result<'a, 'TError>>) =
        async {
            let! x = a
            return x |> Result.mapError mapping
        }

module Async =

    let bindTask (a : Task<'a>) (b : 'a -> 'b) =
        a.Result |> b

    let bindAsync (a : Async<'a>) (b : 'a -> 'b) =
        a |> Async.RunSynchronously |> b