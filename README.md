# Project Name

This is a .NET project. Follow the instructions below to run the project and create migrations.

## Prerequisites

- .NET Core 8
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

## Configuration

The application's configuration is stored in the `appsettings.json` file. Here's what the structure of this file looks like:

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "SqlServer connection string"
    },
    "AppSettings": {
        "Token": "256-bit Signing token for JWT",
        "BackEndUrl": ""
    },
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "",
    "EmailSettings": {
        "MailgunAPIKey": "",
        "MailgunDomain": ""
    }
}
```

## Creating Migrations

This needs a database connection to function, you may need to create and run a migration to run this project in a development environment.
This can be done via the dotnet package manager console or through dotnet ef tools in your OS' Command Line Interface

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

### Appsettings structure
