-- Create Test Database for Unit Tests
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'HospitalTestDB')
BEGIN
    CREATE DATABASE HospitalTestDB;
END
GO

USE HospitalTestDB;
GO

-- Create Appointments Table for Testing
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Appointments')
BEGIN
    CREATE TABLE Appointments (
        Id INT PRIMARY KEY IDENTITY(1,1),
        PatientName NVARCHAR(100) NOT NULL,
        DoctorName NVARCHAR(100) NOT NULL,
        AppointmentDate DATETIME NOT NULL,
        Status NVARCHAR(50) NOT NULL
    );
END
GO

-- Note: No sample data needed for test database
-- Tests will create their own test data
