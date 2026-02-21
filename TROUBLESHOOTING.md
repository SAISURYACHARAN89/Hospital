# Troubleshooting Guide

## Issue: Test Database Connection Failure

**Error Message:**
```
Cannot open database "HospitalTestDB" requested by the login. The login failed.
```

**Solution:**

1. **Create the test database** by running one of these commands:

   **Using PowerShell (Recommended):**
   ```powershell
   .\setup-databases.ps1
   ```

   **Using Command Prompt:**
   ```cmd
   setup-databases.bat
   ```

   **Manual Method:**
   ```powershell
   sqlcmd -S "(localdb)\mssqllocaldb" -i "Hospital\Database\setup-test-db.sql"
   ```

2. **Verify LocalDB is running:**
   ```powershell
   sqllocaldb info
   sqllocaldb start MSSQLLocalDB
   ```

3. **Check if databases exist:**
   ```powershell
   sqlcmd -S "(localdb)\mssqllocaldb" -Q "SELECT name FROM sys.databases WHERE name IN ('HospitalDB', 'HospitalTestDB')"
   ```

## Issue: Git Add Permission Denied for .vs folder

**Error Message:**
```
error: open(".vs/Hospital.slnx/FileContentIndex/...vsidx"): Permission denied
```

**Solution:**

1. **Remove .vs folder from git tracking:**
   ```powershell
   git rm -r --cached .vs
   ```

2. **The .gitignore file has been created** to prevent this in the future

3. **Re-initialize git properly:**
   ```powershell
   # Clean up
   Remove-Item -Path .git -Recurse -Force
   
   # Re-initialize
   git init
   git add .
   git commit -m "Initial commit"
   ```

## Issue: LocalDB Not Installed

**Solution:**

1. **Install SQL Server Express LocalDB:**
   - Download from: https://aka.ms/ssefordevs
   - Or install via Visual Studio Installer (SQL Server Express LocalDB component)

2. **Create LocalDB instance:**
   ```powershell
   sqllocaldb create MSSQLLocalDB
   sqllocaldb start MSSQLLocalDB
   ```

## Issue: Connection String Problems

**Symptom:** Cannot connect to database

**Solution:**

Check your connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "HospitalDB": "Server=(localdb)\\mssqllocaldb;Database=HospitalDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

Alternative connection strings:
- `Server=.\\SQLEXPRESS;Database=HospitalDB;Trusted_Connection=True;`
- `Server=localhost;Database=HospitalDB;Integrated Security=True;`

## Running Only Controller Tests (No Database Required)

If you want to run tests without setting up databases:

```powershell
# Run only controller tests (these use mocks)
dotnet test --filter FullyQualifiedName~AppointmentsControllerTests

# Skip service tests
dotnet test --filter FullyQualifiedName!~AppointmentServiceTests
```

## Verifying Everything Works

1. **Build the solution:**
   ```powershell
   dotnet build
   ```

2. **Run the API:**
   ```powershell
   cd Hospital
   dotnet run
   ```

3. **Test an endpoint:**
   ```powershell
   # In another terminal
   curl https://localhost:7xxx/api/appointments
   ```

4. **Run controller tests only:**
   ```powershell
   dotnet test --filter FullyQualifiedName~AppointmentsControllerTests
   ```

## Common Commands

```powershell
# Check LocalDB instances
sqllocaldb info

# Start LocalDB
sqllocaldb start MSSQLLocalDB

# Stop LocalDB
sqllocaldb stop MSSQLLocalDB

# Delete and recreate databases
sqlcmd -S "(localdb)\mssqllocaldb" -Q "DROP DATABASE IF EXISTS HospitalDB"
sqlcmd -S "(localdb)\mssqllocaldb" -Q "DROP DATABASE IF EXISTS HospitalTestDB"
.\setup-databases.ps1

# Build solution
dotnet build

# Run API
cd Hospital; dotnet run

# Run all tests
cd Hospital.Tests; dotnet test

# Run controller tests only
dotnet test --filter "FullyQualifiedName~AppointmentsControllerTests"

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"
```
