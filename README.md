# Hospital Appointment Management System

A Hospital Appointment Management System built with ASP.NET Core Web API (.NET 10) and ADO.NET Core for database operations.

## Features

- **CRUD Operations** for hospital appointments
- **ADO.NET Core** implementation (SqlConnection, SqlCommand, SqlDataReader)
- **Dependency Injection** for services
- **Unit Tests** with MSTest and Moq
- **RESTful API** design

## Technologies Used

- .NET 10
- ASP.NET Core Web API
- ADO.NET Core (Microsoft.Data.SqlClient)
- SQL Server (LocalDB)
- MSTest for unit testing
- Moq for mocking

## Prerequisites

- .NET 10 SDK
- SQL Server LocalDB (installed with Visual Studio)
- Visual Studio 2022 or Visual Studio Code

## Database Setup

**Option 1: Automated Setup (Recommended)**

Run the provided batch script from the root directory:
```bash
setup-databases.bat
```

This will create both `HospitalDB` and `HospitalTestDB` databases.

**Option 2: Manual Setup**

1. Open SQL Server Management Studio (SSMS) or Visual Studio SQL Server Object Explorer
2. Connect to `(localdb)\mssqllocaldb`
3. Run the SQL scripts:
   - `Hospital/Database/setup.sql` (for main database)
   - `Hospital/Database/setup-test-db.sql` (for test database)

**Option 3: Using sqlcmd**

```bash
sqlcmd -S (localdb)\mssqllocaldb -i "Hospital\Database\setup.sql"
sqlcmd -S (localdb)\mssqllocaldb -i "Hospital\Database\setup-test-db.sql"
```

**Verify Database Creation:**

```sql
-- Connect to (localdb)\mssqllocaldb and run:
SELECT name FROM sys.databases WHERE name IN ('HospitalDB', 'HospitalTestDB');
```

## Configuration

Update the connection string in `appsettings.json` if needed:

```json
{
  "ConnectionStrings": {
    "HospitalDB": "Server=(localdb)\\mssqllocaldb;Database=HospitalDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

## Running the Application

```bash
cd Hospital
dotnet run
```

The API will be available at `https://localhost:7xxx` or `http://localhost:5xxx`

## API Endpoints

### Get All Appointments
```http
GET /api/appointments
```

### Get Appointment by ID
```http
GET /api/appointments/{id}
```

### Create Appointment
```http
POST /api/appointments
Content-Type: application/json

{
  "patientName": "John Doe",
  "doctorName": "Dr. Smith",
  "appointmentDate": "2025-02-15T10:00:00",
  "status": "Scheduled"
}
```

### Update Appointment
```http
PUT /api/appointments/{id}
Content-Type: application/json

{
  "id": 1,
  "patientName": "John Doe",
  "doctorName": "Dr. Smith",
  "appointmentDate": "2025-02-15T10:00:00",
  "status": "Completed"
}
```

### Delete Appointment
```http
DELETE /api/appointments/{id}
```

## Running Tests

### Service Tests (AppointmentServiceTests)
Tests the ADO.NET data access layer:
- Creating appointments
- Retrieving appointments
- Updating appointments
- Deleting appointments

### Controller Tests (AppointmentsControllerTests)
Tests the API endpoints using mocked services:
- GET all appointments
- GET appointment by ID
- POST new appointment
- PUT update appointment
- DELETE appointment

Run all tests:
```bash
cd Hospital.Tests
dotnet test
```

Run tests with detailed output:
```bash
dotnet test --logger "console;verbosity=detailed"
```

## Project Structure

```
Hospital/
├── Controllers/
│   └── AppointmentsController.cs    # REST API endpoints
├── Models/
│   └── Appointment.cs                # Data model
├── Services/
│   ├── IAppointmentService.cs        # Service interface
│   └── AppointmentService.cs         # ADO.NET implementation
├── Database/
│   └── setup.sql                     # Database setup script
├── appsettings.json                  # Configuration
└── Program.cs                        # Application startup

Hospital.Tests/
├── Controllers/
│   └── AppointmentsControllerTests.cs  # Controller unit tests
└── Services/
    └── AppointmentServiceTests.cs      # Service unit tests
```

## Key Implementation Details

### ADO.NET Usage

The `AppointmentService` class demonstrates:
- **SqlConnection**: Managing database connections
- **SqlCommand**: Executing SQL commands with parameters
- **SqlDataReader**: Reading data from queries
- **Async operations**: Using async/await for database operations
- **Parameterized queries**: Preventing SQL injection

### Dependency Injection

Services are registered in `Program.cs`:
```csharp
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
```

### Unit Testing

- **Service tests**: Test actual database operations (integration tests)
- **Controller tests**: Use Moq to mock the service layer
- **Test patterns**: Arrange-Act-Assert pattern
- **Edge cases**: Test invalid IDs, null values, etc.

## Status Values

- `Scheduled` - Appointment is scheduled
- `Completed` - Appointment has been completed
- `Cancelled` - Appointment was cancelled

## Troubleshooting

Having issues? Check the [TROUBLESHOOTING.md](TROUBLESHOOTING.md) guide for common problems and solutions:
- Database connection errors
- Git issues with .vs folder
- LocalDB installation
- Running tests without database setup

## Notes

- The service tests use a test database (HospitalTestDB) to avoid affecting production data
- Controller tests use mocking to isolate the controller logic from the service layer
- All database operations use parameterized queries to prevent SQL injection
- The API follows RESTful conventions with appropriate HTTP status codes

## Getting Started Checklist

- [ ] Install .NET 10 SDK
- [ ] Install SQL Server LocalDB
- [ ] Clone the repository
- [ ] Run `setup-databases.ps1` or `setup-databases.bat`
- [ ] Build the solution: `dotnet build`
- [ ] Run the API: `cd Hospital; dotnet run`
- [ ] Run tests: `cd Hospital.Tests; dotnet test`
- [ ] Test endpoints using browser, Postman, or curl
