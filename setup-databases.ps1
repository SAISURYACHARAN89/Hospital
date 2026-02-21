Write-Host "Setting up Hospital Management System Databases..." -ForegroundColor Cyan
Write-Host ""

# Setup main database
Write-Host "Creating HospitalDB database..." -ForegroundColor Yellow
sqlcmd -S "(localdb)\mssqllocaldb" -i "Hospital\Database\setup.sql"

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Failed to create HospitalDB database" -ForegroundColor Red
    Write-Host "Please ensure SQL Server LocalDB is installed and running" -ForegroundColor Red
    Write-Host ""
    Write-Host "To install LocalDB, run: sqllocaldb create MSSQLLocalDB" -ForegroundColor Yellow
    Write-Host "To start LocalDB, run: sqllocaldb start MSSQLLocalDB" -ForegroundColor Yellow
    pause
    exit 1
}

Write-Host ""
Write-Host "Creating HospitalTestDB database for unit tests..." -ForegroundColor Yellow
sqlcmd -S "(localdb)\mssqllocaldb" -i "Hospital\Database\setup-test-db.sql"

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Failed to create HospitalTestDB database" -ForegroundColor Red
    pause
    exit 1
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "Database setup completed successfully!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Databases created:" -ForegroundColor Cyan
Write-Host "  - HospitalDB (main database with sample data)" -ForegroundColor White
Write-Host "  - HospitalTestDB (test database)" -ForegroundColor White
Write-Host ""
Write-Host "You can now:" -ForegroundColor Cyan
Write-Host "  - Run API: cd Hospital; dotnet run" -ForegroundColor White
Write-Host "  - Run Tests: cd Hospital.Tests; dotnet test" -ForegroundColor White
Write-Host ""
