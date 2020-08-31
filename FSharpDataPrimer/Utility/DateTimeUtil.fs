namespace FSharpDataPrimer.Utility

open System

module DateTimeUtil =
    let toDateTimeUtc (dt : DateTime) =
        match dt.Kind with
        | DateTimeKind.Unspecified ->
            DateTime(dt.Ticks, DateTimeKind.Utc)
        | DateTimeKind.Local ->
            dt.ToUniversalTime()
        | _ -> dt

    let toDateTimeLocal (dt : DateTime) =
        match dt.Kind with
        | DateTimeKind.Unspecified ->
            DateTime(dt.Ticks, DateTimeKind.Local)
        | DateTimeKind.Utc ->
            dt.ToLocalTime()
        | _ -> dt
