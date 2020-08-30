namespace FSharpDataPrimer.Data

open FSharp.Data
open FSharpDataPrimer.Model.User

module UserRepository =
    [<Literal>]
    let selectAllSql = """select
[Id]
,[UserName]
from [dbo].[User]"""

    [<Literal>]
    let selectByUserNameSql = """select
[Id]
,[UserName]
from [dbo].[User]
where [UserName]=@userName"""

    [<Literal>]
    let insertSql = """insert into [dbo].[User] (
[UserName])
values (
@userName)"""

    [<Literal>]
    let connectionString =
        @"Data Source=.;Initial Catalog=FSharpDataPrimer;Integrated Security=True"

    let mapUsersRecord (record : SqlCommandProvider<selectAllSql, connectionString>.Record) =
        {
            Id = record.Id;
            UserName = record.UserName
        } : User

    let mapUsersRecordOption (recordOption : Option<SqlCommandProvider<selectAllSql, connectionString>.Record>) =
        match recordOption with
            | None -> None
            | Some record ->
                Some (mapUsersRecord record)

    let mapUserByUserNameRecord (record : SqlCommandProvider<selectByUserNameSql, connectionString, SingleRow = true>.Record) =
        {
            Id = record.Id;
            UserName = record.UserName
        } : User

    let mapUserByUserNameRecordOption (recordOption : Option<SqlCommandProvider<selectByUserNameSql, connectionString, SingleRow = true>.Record>) =
        match recordOption with
            | None -> None
            | Some record ->
                Some (mapUserByUserNameRecord record)

    let getUsers =
        use cmd = new SqlCommandProvider<selectAllSql, connectionString>(connectionString)
        // Error InvalidOperationException: The ConnectionString property has not been initialized.
        //      encountered if SQL result is not iterated before connection is disposed: |> Seq.toList.
        cmd.Execute()
            |> Seq.toList

    let getUserByUserName (userName : string) =
        use cmd = new SqlCommandProvider<selectByUserNameSql, connectionString, SingleRow = true>(connectionString)
        // Error InvalidOperationException: The ConnectionString property has not been initialized.
        //      encountered if SQL result is not iterated before connection is disposed: |> Seq.toList.
        cmd.Execute(userName = userName)

    let addUser (request : UserInbound) =
        use cmd = new SqlCommandProvider<insertSql, connectionString>(connectionString)
        cmd.Execute(userName = request.UserName)
