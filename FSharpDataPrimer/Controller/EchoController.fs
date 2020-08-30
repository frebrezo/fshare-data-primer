namespace FSharpDataPrimer.Controller

open System
open Microsoft.AspNetCore.Mvc

[<Route("api/[controller]")>]
[<ApiController>]
type EchoController () =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get(s : string) =
        if s |> String.IsNullOrEmpty then
            ActionResult<string>("Hello World")
        else
            ActionResult<string>(s)
