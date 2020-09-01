# F# Data Access options

* F# provides a broad range of database access support
  * FSharp.Data.SqlClient
    * MicroORM like Dapper
  * Fsharp.Data.SQLProvider
    * Linq to SQL support
  * SqlEntityConnection Type Provider
    * Entity Framework support
  * ADO.NET

# FSharp.Data.SqlClient library

* Manual SQL management
  * SQL-injection attacks are handled by the TypeProvider
  * Joins and IN statements are supported
* Fast
* F# type inferencing
  * No need to create Domain Model classes between layers
  * Still requires mapping to Request/Response classes at application boundary
* No database abstraction layer

# FSharp.Data.SqlClient library limitations

* Overhead for lots of small operations due to manual maintenance of SQL and F# generated Record types are specific to SQL query (see demo)
  * SELECT * operations
  * SELECT * WHERE operations
* Only supports SQL Server
* Does NOT generate static Domain Model classes for cross language support
* Requires LIVE connection to database for type inferencing to work
  * Build server requires access to a SQL Server instance

# Query database

* SELECT *
[<Literal>]
let selectAllSql = """select [Id],[UserName] from [dbo].[User]""“
```
let getUsers =
    use cmd = new SqlCommandProvider<selectAllSql, connectionString>(connectionString)
    cmd.Execute()
        |> Seq.toList
```
* SELECT * WHERE
```
[<Literal>]
let selectByUserNameSql = """select [Id],[UserName] from [dbo].[User]
where [UserName]=@userName"""

let getUserByUserName (userName : string) =
    use cmd = new SqlCommandProvider<selectByUserNameSql, connectionString, SingleRow = true>(connectionString)
    cmd.Execute(userName = userName)
```

# Insert/Update database

* INSERT INTO
```
[<Literal>]
let insertSql = """insert into [dbo].[User] ([UserName]) values (@userName)""“

let addUser (request : UserInbound) =
    use cmd = new SqlCommandProvider<insertSql, connectionString>(connectionString)
    cmd.Execute(userName = request.UserName)
```

# SQL injection attacks (Little Bobby Tables)

![Little Bobby Tables](https://imgs.xkcd.com/comics/exploits_of_a_mom.png)
https://xkcd.com/327/

```
{
  "userId": 1,
  "name": "Robert');drop table [dbo].[User];--",
  "appointment": "2020-08-30T17:35:08.336Z",
  "durationInMins": 30
}
```

![SQL Injection Swagger](/images/sql-injection-swagger.png)

![SQL Injection Database](/images/sql-injection-database.png)

# References

* Quickstart: Use Visual Studio to create your first ASP.NET Core web service in F#
  * https://docs.microsoft.com/en-us/visualstudio/ide/quickstart-fsharp?view=vs-2019
* Routing to controller actions in ASP.NET Core
  * https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-3.1
* Intro to F# Web APIs
  * https://nabeelvalley.netlify.app/blog/2019/30-10/fsharp-webapi/
* Entity Framework Core Database-First Tutorial for .NET Core
  * https://www.devart.com/dotconnect/postgresql/docs/EFCore-Database-First-NET-Core.html
* Guide - Data Access with F#
  * https://fsharp.org/guides/data-access/
* Not your grandfather's ORM
  * http://fsprojects.github.io/FSharp.Data.SqlClient/
* A comparison on SQL Server data access methods
  * http://fsprojects.github.io/FSharp.Data.SqlClient/comparison.html
* Exploits of a Mom
  * https://xkcd.com/327/
