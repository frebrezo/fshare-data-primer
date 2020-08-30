namespace FSharpDataPrimer.Model

open System

module Schedule = 
   [<CLIMutable>]
   type Schedule =
        {
            Id : int;
            UserId : int;
            Name : string;
            Appointment : DateTime;
            DurationInMins : int
        }

    [<CLIMutable>]
    type ScheduleInbound =
         {
             UserId : int;
             Name : string;
             Appointment : DateTime;
             DurationInMins : int
         }
