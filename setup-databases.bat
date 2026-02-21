@echo off
echo Setting up Hospital Management System Databases...
echo.

REM Setup main database
echo Creating HospitalDB database...
sqlcmd -S (localdb)\mssqllocaldb -i "Hospital\Database\setup.sql"

if %errorlevel% neq 0 (
    echo ERROR: Failed to create HospitalDB database
    echo Please ensure SQL Server LocalDB is installed and running
    pause
    exit /b 1
)

echo.
echo Creating HospitalTestDB database for unit tests...
sqlcmd -S (localdb)\mssqllocaldb -i "Hospital\Database\setup-test-db.sql"

if %errorlevel% neq 0 (
    echo ERROR: Failed to create HospitalTestDB database
    pause
    exit /b 1
)

echo.
echo ========================================
echo Database setup completed successfully!
echo ========================================
echo.
echo You can now run the application and tests:
echo   - Run API: cd Hospital ^&^& dotnet run
echo   - Run Tests: cd Hospital.Tests ^&^& dotnet test
echo.
pause
