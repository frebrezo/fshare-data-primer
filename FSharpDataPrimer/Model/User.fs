namespace FSharpDataPrimer.Model

open FSharp.Data

module User =
    [<CLIMutable>]
    type User =
        {
            Id : int;
            UserName : string
        }

    [<CLIMutable>]
    type UserInbound =
        {
            UserName : string
        }

