namespace FSharpDataPrimer.Controller

open System
open Microsoft.AspNetCore.Mvc
// To open module, must be organizationally, above Controller folder.
open FSharpDataPrimer.Model.User
open FSharpDataPrimer.Data.UserRepository

[<Route("api/[controller]")>]
[<ApiController>]
type UserController () =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() =
        let dbResult = getUsers
        let result : seq<User> =
            dbResult
            |> Seq.map (fun x -> mapUsersRecord x)
        ActionResult<seq<User>>(result)

    [<HttpGet("key(userName)")>]
    member this.Get(userName : string) =
        let dbResult = getUserByUserName userName
        let result = mapUserByUserNameRecordOption dbResult
        // Need custom JSON converter for Option type.
        ActionResult<Option<User>>(result)

    [<HttpPost>]
    member this.Post(request : UserInbound) =
        // F# compiler generates a warning because function result is used if |> ignore is not explicitly used.
        addUser(request) |> ignore
        let dbResult = getUserByUserName request.UserName
        let result = mapUserByUserNameRecordOption dbResult
        // Need custom JSON converter for Option type.
        ActionResult<Option<User>>(result)
