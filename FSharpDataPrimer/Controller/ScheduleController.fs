namespace FSharpDataPrimer.Controller

open System
open System.Net.Http
open Microsoft.AspNetCore.Mvc
open FSharp.Data
// To open module, must be organizationally, above Controller folder.
open FSharpDataPrimer.Model.Schedule
open FSharpDataPrimer.Data.ScheduleRepository

[<Route("api/[controller]")>]
[<ApiController>]
type ScheduleController () =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() =
        let dbResult = getSchedules
        let result =
            dbResult
            |> Seq.map (fun x -> mapGetSchedulesRecord x)
        ActionResult<seq<Schedule>>(result)

    [<HttpGet("/key(name,appointment,durationInMins)")>]
    member this.Get(name : string, appointment : DateTime, durationInMins : int) =
        match (if name = null then None else Some name) with
        | Some name ->
            let dbResult = getScheduleByNameAndAppointmentAndDurationInMins(name, appointment, durationInMins)
            let result = mapGetScheduleByNameAndAppointmentAndDurationInMinsRecordOption dbResult
            ActionResult<Option<Schedule>>(result)
        | _ ->
            let dbResult = getScheduleByAppointmentAndDurationInMins(appointment, durationInMins)
            let result = mapGetScheduleByAppointmentAndDurationInMinsRecordOption dbResult
            ActionResult<Option<Schedule>>(result)

    [<HttpPost>]
    member this.Post(request : ScheduleInbound) =
        // F# compiler generates a warning because function result is used if |> ignore is not explicitly used.
        addSchedule(request) |> ignore
        let dbResult = getScheduleByNameAndAppointmentAndDurationInMins(request.Name, request.Appointment, request.DurationInMins)
        let result = mapGetScheduleByNameAndAppointmentAndDurationInMinsRecordOption dbResult
        // Need custom JSON converter for Option type.
        ActionResult<Option<Schedule>>(result)
