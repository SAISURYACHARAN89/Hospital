using Hospital.Models;
using Hospital.Services;
using Microsoft.Extensions.Configuration;

namespace Hospital.Tests.Services
{
    [TestClass]
    public class AppointmentServiceTests
    {
        private IConfiguration? _configuration;
        private IAppointmentService? _appointmentService;

        [TestInitialize]
        public void Setup()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"ConnectionStrings:HospitalDB", "Server=(localdb)\\mssqllocaldb;Database=HospitalTestDB;Trusted_Connection=True;MultipleActiveResultSets=true"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();

            _appointmentService = new AppointmentService(_configuration);
        }

        [TestMethod]
        public async Task CreateAppointmentAsync_ShouldReturnNewId()
        {
            var appointment = new Appointment
            {
                PatientName = "Test Patient",
                DoctorName = "Dr. Test",
                AppointmentDate = DateTime.Now.AddDays(1),
                Status = "Scheduled"
            };

            var id = await _appointmentService!.CreateAppointmentAsync(appointment);

            Assert.IsTrue(id > 0, "Created appointment should have a valid ID");
        }

        [TestMethod]
        public async Task GetAppointmentByIdAsync_WithValidId_ShouldReturnAppointment()
        {
            var appointment = new Appointment
            {
                PatientName = "John Doe",
                DoctorName = "Dr. Smith",
                AppointmentDate = DateTime.Now.AddDays(2),
                Status = "Scheduled"
            };

            var id = await _appointmentService!.CreateAppointmentAsync(appointment);
            var retrievedAppointment = await _appointmentService.GetAppointmentByIdAsync(id);

            Assert.IsNotNull(retrievedAppointment);
            Assert.AreEqual(appointment.PatientName, retrievedAppointment.PatientName);
            Assert.AreEqual(appointment.DoctorName, retrievedAppointment.DoctorName);
            Assert.AreEqual(appointment.Status, retrievedAppointment.Status);
        }

        [TestMethod]
        public async Task GetAppointmentByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            var appointment = await _appointmentService!.GetAppointmentByIdAsync(999999);

            Assert.IsNull(appointment);
        }

        [TestMethod]
        public async Task GetAllAppointmentsAsync_ShouldReturnList()
        {
            var appointments = await _appointmentService!.GetAllAppointmentsAsync();

            Assert.IsNotNull(appointments);
            Assert.IsInstanceOfType(appointments, typeof(IEnumerable<Appointment>));
        }

        [TestMethod]
        public async Task UpdateAppointmentAsync_WithValidAppointment_ShouldReturnTrue()
        {
            var appointment = new Appointment
            {
                PatientName = "Jane Smith",
                DoctorName = "Dr. Johnson",
                AppointmentDate = DateTime.Now.AddDays(3),
                Status = "Scheduled"
            };

            var id = await _appointmentService!.CreateAppointmentAsync(appointment);
            appointment.Id = id;
            appointment.Status = "Completed";

            var result = await _appointmentService.UpdateAppointmentAsync(appointment);

            Assert.IsTrue(result);

            var updatedAppointment = await _appointmentService.GetAppointmentByIdAsync(id);
            Assert.IsNotNull(updatedAppointment);
            Assert.AreEqual("Completed", updatedAppointment.Status);
        }

        [TestMethod]
        public async Task UpdateAppointmentAsync_WithInvalidId_ShouldReturnFalse()
        {
            var appointment = new Appointment
            {
                Id = 999999,
                PatientName = "Non Existent",
                DoctorName = "Dr. None",
                AppointmentDate = DateTime.Now,
                Status = "Scheduled"
            };

            var result = await _appointmentService!.UpdateAppointmentAsync(appointment);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DeleteAppointmentAsync_WithValidId_ShouldReturnTrue()
        {
            var appointment = new Appointment
            {
                PatientName = "Bob Wilson",
                DoctorName = "Dr. Brown",
                AppointmentDate = DateTime.Now.AddDays(4),
                Status = "Scheduled"
            };

            var id = await _appointmentService!.CreateAppointmentAsync(appointment);
            var result = await _appointmentService.DeleteAppointmentAsync(id);

            Assert.IsTrue(result);

            var deletedAppointment = await _appointmentService.GetAppointmentByIdAsync(id);
            Assert.IsNull(deletedAppointment);
        }

        [TestMethod]
        public async Task DeleteAppointmentAsync_WithInvalidId_ShouldReturnFalse()
        {
            var result = await _appointmentService!.DeleteAppointmentAsync(999999);

            Assert.IsFalse(result);
        }
    }
}
