namespace FSharpDataPrimer.Data

open System
open FSharp.Data
open FSharpDataPrimer.Model.Schedule

module ScheduleRepository =
    // Better to use columns instead of * because type inference system is bad at
    //      requerying the database.
    [<Literal>]
    let selectAllSql = """select
[Id]
,[UserId]
,[Name]
,[Appointment]
,[DurationInMins]
from [dbo].[Schedule]"""

    [<Literal>]
    let selectByNameAndAppointmentAndDurationInMinsSql = """select
[Id]
,[UserId]
,[Name]
,[Appointment]
,[DurationInMins]
from [dbo].[Schedule]
where [Name]=@name
and [Appointment]=@appointment
and [DurationInMins]=@durationInMins"""

    [<Literal>]
    let selectByAppointmentAndDurationInMinsSql = """select
[Id]
,[UserId]
,[Name]
,[Appointment]
,[DurationInMins]
from [dbo].[Schedule]
where [Name] is null
and [Appointment]=@appointment
and [DurationInMins]=@durationInMins"""

    [<Literal>]
    let insertSql = """insert into [dbo].[Schedule] (
[UserId]
,[Name]
,[Appointment]
,[DurationInMins])
values (
@userId
,@name
,@appointment
,@durationInMins)"""

    [<Literal>]
    let connectionString =
        @"Data Source=.;Initial Catalog=FSharpDataPrimer;Integrated Security=True"

    // Define long SqlComandProviders as a type.
    type CommandByNameAndAppointmentAndDurationInMins =
        SqlCommandProvider<selectByNameAndAppointmentAndDurationInMinsSql, connectionString, SingleRow = true>

    type CommandByAppointmentAndDurationInMins =
        SqlCommandProvider<selectByAppointmentAndDurationInMinsSql, connectionString, SingleRow = true>

    let mapGetSchedulesRecord (record : SqlCommandProvider<selectAllSql, connectionString>.Record) =
        {
            Id = record.Id;
            UserId = record.UserId;
            Name = if record.Name.IsNone then null else record.Name.Value;
            Appointment = record.Appointment;
            DurationInMins = record.DurationInMins
        } : Schedule

    let mapGetSchedulesRecordOption (recordOption : Option<SqlCommandProvider<selectAllSql, connectionString>.Record>) =
        match recordOption with
            | None -> None
            | Some record ->
                Some (mapGetSchedulesRecord record)

    let mapGetScheduleByNameAndAppointmentAndDurationInMinsRecord (record : CommandByNameAndAppointmentAndDurationInMins.Record) =
        {
            Id = record.Id;
            UserId = record.UserId;
            Name = if record.Name.IsNone then null else record.Name.Value;
            Appointment = record.Appointment;
            DurationInMins = record.DurationInMins
        } : Schedule

    let mapGetScheduleByNameAndAppointmentAndDurationInMinsRecordOption (recordOption : Option<CommandByNameAndAppointmentAndDurationInMins.Record>) =
        match recordOption with
            | None -> None
            | Some record ->
                Some (mapGetScheduleByNameAndAppointmentAndDurationInMinsRecord record)

    let mapGetScheduleByAppointmentAndDurationInMinsRecord (record : CommandByAppointmentAndDurationInMins.Record) =
        {
            Id = record.Id;
            UserId = record.UserId;
            Name = if record.Name.IsNone then null else record.Name.Value;
            Appointment = record.Appointment;
            DurationInMins = record.DurationInMins
        } : Schedule

    let mapGetScheduleByAppointmentAndDurationInMinsRecordOption (recordOption : Option<CommandByAppointmentAndDurationInMins.Record>) =
        match recordOption with
            | None -> None
            | Some record ->
                Some (mapGetScheduleByAppointmentAndDurationInMinsRecord record)

    let getSchedules =
        use cmd = new SqlCommandProvider<selectAllSql, connectionString>(connectionString)
        // Error InvalidOperationException: The ConnectionString property has not been initialized.
        //      encountered if SQL result is not iterated before connection is disposed: |> Seq.toList.
        cmd.Execute()
            |> Seq.toList

    let getScheduleByNameAndAppointmentAndDurationInMins (name : string, appointment : DateTime, durationInMins : int) =
        use cmd = new CommandByNameAndAppointmentAndDurationInMins(connectionString)
        cmd.Execute(
            name = name,
            appointment = appointment,
            durationInMins = durationInMins)

    let getScheduleByAppointmentAndDurationInMins (appointment : DateTime, durationInMins : int) =
        use cmd = new CommandByAppointmentAndDurationInMins(connectionString)
        cmd.Execute(
            appointment = appointment,
            durationInMins = durationInMins)

    let addSchedule (request : ScheduleInbound) =
        use cmd = new SqlCommandProvider<insertSql, connectionString>(connectionString)
        cmd.Execute(
            userId = request.UserId,
            name = request.Name,
            appointment = request.Appointment,
            durationInMins = request.DurationInMins)
