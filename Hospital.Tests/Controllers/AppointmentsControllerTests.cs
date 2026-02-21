using Hospital.Controllers;
using Hospital.Models;
using Hospital.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hospital.Tests.Controllers
{
    [TestClass]
    public class AppointmentsControllerTests
    {
        private Mock<IAppointmentService>? _mockService;
        private AppointmentsController? _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IAppointmentService>();
            _controller = new AppointmentsController(_mockService.Object);
        }

        [TestMethod]
        public async Task GetAllAppointments_ShouldReturnOkWithAppointments()
        {
            var appointments = new List<Appointment>
            {
                new Appointment { Id = 1, PatientName = "John Doe", DoctorName = "Dr. Smith", AppointmentDate = DateTime.Now, Status = "Scheduled" },
                new Appointment { Id = 2, PatientName = "Jane Smith", DoctorName = "Dr. Johnson", AppointmentDate = DateTime.Now, Status = "Completed" }
            };

            _mockService!.Setup(s => s.GetAllAppointmentsAsync()).ReturnsAsync(appointments);

            var result = await _controller!.GetAllAppointments();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(IEnumerable<Appointment>));
        }

        [TestMethod]
        public async Task GetAppointment_WithValidId_ShouldReturnOk()
        {
            var appointment = new Appointment
            {
                Id = 1,
                PatientName = "John Doe",
                DoctorName = "Dr. Smith",
                AppointmentDate = DateTime.Now,
                Status = "Scheduled"
            };

            _mockService!.Setup(s => s.GetAppointmentByIdAsync(1)).ReturnsAsync(appointment);

            var result = await _controller!.GetAppointment(1);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetAppointment_WithInvalidId_ShouldReturnNotFound()
        {
            _mockService!.Setup(s => s.GetAppointmentByIdAsync(999)).ReturnsAsync((Appointment?)null);

            var result = await _controller!.GetAppointment(999);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task CreateAppointment_WithValidData_ShouldReturnCreated()
        {
            var appointment = new Appointment
            {
                PatientName = "John Doe",
                DoctorName = "Dr. Smith",
                AppointmentDate = DateTime.Now,
                Status = "Scheduled"
            };

            _mockService!.Setup(s => s.CreateAppointmentAsync(appointment)).ReturnsAsync(1);

            var result = await _controller!.CreateAppointment(appointment);

            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAppointment_WithValidData_ShouldReturnNoContent()
        {
            var appointment = new Appointment
            {
                Id = 1,
                PatientName = "John Doe",
                DoctorName = "Dr. Smith",
                AppointmentDate = DateTime.Now,
                Status = "Completed"
            };

            _mockService!.Setup(s => s.UpdateAppointmentAsync(appointment)).ReturnsAsync(true);

            var result = await _controller!.UpdateAppointment(1, appointment);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAppointment_WithMismatchedId_ShouldReturnBadRequest()
        {
            var appointment = new Appointment
            {
                Id = 2,
                PatientName = "John Doe",
                DoctorName = "Dr. Smith",
                AppointmentDate = DateTime.Now,
                Status = "Completed"
            };

            var result = await _controller!.UpdateAppointment(1, appointment);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAppointment_WithNonExistentId_ShouldReturnNotFound()
        {
            var appointment = new Appointment
            {
                Id = 999,
                PatientName = "John Doe",
                DoctorName = "Dr. Smith",
                AppointmentDate = DateTime.Now,
                Status = "Completed"
            };

            _mockService!.Setup(s => s.UpdateAppointmentAsync(appointment)).ReturnsAsync(false);

            var result = await _controller!.UpdateAppointment(999, appointment);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteAppointment_WithValidId_ShouldReturnNoContent()
        {
            _mockService!.Setup(s => s.DeleteAppointmentAsync(1)).ReturnsAsync(true);

            var result = await _controller!.DeleteAppointment(1);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteAppointment_WithInvalidId_ShouldReturnNotFound()
        {
            _mockService!.Setup(s => s.DeleteAppointmentAsync(999)).ReturnsAsync(false);

            var result = await _controller!.DeleteAppointment(999);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }
    }
}
