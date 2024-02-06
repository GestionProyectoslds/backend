# Project Name

This is a .NET project. Follow the instructions below to run the project and create migrations.

## Prerequisites

- .NET Core 3.1 or later
- Entity Framework Core
- SQL Server 2022
- SSMS 2019 (recommended)

## Running the Project

1. Open the project in Visual Studio.
2. Press `F5` or click `Start Debugging` in the Debug menu.

Alternatively, you can run the project from the terminal:

```bash
dotnet run
```

## Creating Migrations

You'll need to create and run a migration to run this project in a development environment

### dotnet package manager

```dotnet package manager
Add-Migration MigrationName
Update-Database
```

### dotnet tools on your OS CLI

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```
