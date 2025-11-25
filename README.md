# Project_Exchange

Golden Scale is a three-layered ASP.NET Core MVC 8.0 solution that showcases a premium currency exchange and valuation service. The project demonstrates:

- **Backend**: Clean separation between domain entities (`BusinessLogic`), persistence (`Infrastructure` with Entity Framework Core + SQL Server), and the MVC front-end (`Project_Exchange`).
- **Features**:
  - Modern landing page with service highlights and gallery
  - Live exchange-rate table sourced from the Hungarian National Bank (MNB) API
  - Secure registration/login backed by hashed credentials and cookie authentication
  - Authenticated appointment booking workflow with conflict checks

## Getting started

1. Restore dependencies and tools
   ```bash
   dotnet restore
   ```
2. Update the database (applies the existing migrations)
   ```bash
   dotnet ef database update --project Infrastructure --startup-project Project_Exchange
   ```
3. Start the web app
   ```bash
   dotnet run --project Project_Exchange
   ```

Default configuration points to the Azure SQL Database defined in `Project_Exchange/Project_Exchange/appsettings.json`. Replace the connection string locally if required.

## Project structure

| Project          | Responsibility                                          |
|------------------|---------------------------------------------------------|
| `BusinessLogic`  | Domain entities, DTOs, and service interfaces           |
| `Infrastructure` | EF Core DbContext, migrations, and manager implementations |
| `Project_Exchange` | MVC UI, controllers, view models, and static assets       |

## Next steps

- Add automated UI and integration tests
- Extend the scheduling module with reminders and admin management
- Harden the error handling and monitoring around the external MNB service
