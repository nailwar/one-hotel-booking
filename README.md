# One Hotel Booking

This project requires [.NET 5.0 SDK](https://dotnet.microsoft.com/download/dotnet/5.0) and [SQL Server 2019 Developer](https://go.microsoft.com/fwlink/?linkid=866662).

Check a `ConnectionString` in `appsettings.json` `ConnectionStrings.DefaultConnection`, and set the server name in parameter `Server`.
Currently it is `Server=localhost\\MSSQLSERVER01`

Then run migrations to create database.
Execute `update-database` in **Package Manager Console**.

Or just switch to `in_memory_db` branch and run project with **database in memory**.
But there are unit tests do not work.

## Testing
Main functionality has been covered by unit tests.

## Tech stack
* Web Framework:
    * Microsoft.AspNetCore version 5.0.5
    * Microsoft.EntityFrameworkCore version 5.0.5
    * Microsoft.EntityFrameworkCore.SqlServer version 5.0.5
* Testing:
    * Microsoft.EntityFrameworkCore.InMemory version 5.0.5
    * NUnit version 3.12.0
    * Unit tests:
        * NSubstitute version 4.2.2 for mocks
        * FluentAssertions version 5.10.3 for asserts

## Task
You’ve been given the responsibility to develop a booking API for the very last hotel in Cancun.
The requirements are:
   - API will be maintained by the hotel’s IT department.
   - As it’s the very last hotel, the quality of service must be 99.99 to 100% => no downtime
   - For the purpose of the test, we assume the hotel has only one room available
   - To give a chance to everyone to book the room, the stay can’t be longer than 3 days and
   can’t be reserved more than 30 days in advance.
   - All reservations start at least the next day of booking,
   - To simplify the use case, a “DAY’ in the hotel room starts from 00:00 to 23:59:59.
   - Every end-user can check the room availability, place a reservation, cancel it or modify it.
   - To simplify the API is insecure
