# One Hotel Booking

This project requires [.NET 5.0 SDK](https://dotnet.microsoft.com/download/dotnet/5.0) and [SQL Server 2019 Developer](https://go.microsoft.com/fwlink/?linkid=866662).

Check a `ConnectionString` in `appsettings.json` `ConnectionStrings.DefaultConnection`, and set the server name in parameter `Server`.
Currently it is `Server=localhost\\MSSQLSERVER01`

Then run migrations to create database.
Execute `update-database` in **Package Manager Console**.

Or just switch to `in_memory_db` branch and run project with **database in memory**.