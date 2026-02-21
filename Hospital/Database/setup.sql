-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'HospitalDB')
BEGIN
    CREATE DATABASE HospitalDB;
END
GO

USE HospitalDB;
GO

-- Create Appointments Table
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

-- Insert Sample Data
INSERT INTO Appointments (PatientName, DoctorName, AppointmentDate, Status)
VALUES 
    ('Surya', 'Dr', '2025-02-15 10:00:00', 'Scheduled'),
    ('MB', 'Mr', '2025-02-16 14:30:00', 'Scheduled'),
    ('PSPK', 'Dr', '2025-02-14 09:00:00', 'Completed');
GO
