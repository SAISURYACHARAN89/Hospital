using Hospital.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Hospital.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly string _connectionString;

        public AppointmentService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HospitalDB") 
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string not found");
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            var appointments = new List<Appointment>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT Id, PatientName, DoctorName, AppointmentDate, Status FROM Appointments", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            appointments.Add(MapReaderToAppointment(reader));
                        }
                    }
                }
            }

            return appointments;
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(int id)
        {
            Appointment? appointment = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT Id, PatientName, DoctorName, AppointmentDate, Status FROM Appointments WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            appointment = MapReaderToAppointment(reader);
                        }
                    }
                }
            }

            return appointment;
        }

        public async Task<int> CreateAppointmentAsync(Appointment appointment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(
                    "INSERT INTO Appointments (PatientName, DoctorName, AppointmentDate, Status) " +
                    "VALUES (@PatientName, @DoctorName, @AppointmentDate, @Status); " +
                    "SELECT CAST(SCOPE_IDENTITY() as int);", connection))
                {
                    command.Parameters.AddWithValue("@PatientName", appointment.PatientName);
                    command.Parameters.AddWithValue("@DoctorName", appointment.DoctorName);
                    command.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                    command.Parameters.AddWithValue("@Status", appointment.Status);

                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }

        public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(
                    "UPDATE Appointments SET PatientName = @PatientName, DoctorName = @DoctorName, " +
                    "AppointmentDate = @AppointmentDate, Status = @Status WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", appointment.Id);
                    command.Parameters.AddWithValue("@PatientName", appointment.PatientName);
                    command.Parameters.AddWithValue("@DoctorName", appointment.DoctorName);
                    command.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                    command.Parameters.AddWithValue("@Status", appointment.Status);

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("DELETE FROM Appointments WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        private Appointment MapReaderToAppointment(SqlDataReader reader)
        {
            return new Appointment
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                PatientName = reader.GetString(reader.GetOrdinal("PatientName")),
                DoctorName = reader.GetString(reader.GetOrdinal("DoctorName")),
                AppointmentDate = reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                Status = reader.GetString(reader.GetOrdinal("Status"))
            };
        }
    }
}
