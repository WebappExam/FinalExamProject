using System.Security.Claims;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using StudyRoomBooking.Mvc.Controllers;
using StudyRoomBooking.Mvc.DAL;
using StudyRoomBooking.Mvc.Models;
using StudyRoomBooking.Mvc.viewModel;
using Microsoft.Extensions.Logging.Abstractions;


namespace StudyRoomBooking.Tests.Controllers;

public class RoomBookingControllerTests
{
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomBookingRepository _bookingRepository;
    private readonly RoomBookingController _controller;

    public RoomBookingControllerTests()
    {
        _roomRepository = Substitute.For<IRoomRepository>();
        _bookingRepository = Substitute.For<IRoomBookingRepository>();

        _controller = new RoomBookingController(
            _roomRepository,
            _bookingRepository,
            NullLogger<RoomBookingController>.Instance);

        // Start each test with an anonymous user
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // Configure asynchronous write operations
        _bookingRepository
            .Create(Arg.Any<RoomBooking>())
            .Returns(Task.CompletedTask);

        _bookingRepository
            .DeleteAsync(Arg.Any<RoomBooking>())
            .Returns(Task.CompletedTask);

        // Unless a test specifies otherwise, the room has no overlap
        _bookingRepository
            .HasOverlappingBookingsAsync(
                Arg.Any<int>(),
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>(),
                Arg.Any<int?>())
            .Returns(Task.FromResult(false));
    }

    // POSITIVE 1
    [Fact]
    public async Task Create_WhenRoomExists_ReturnsBookingForm()
    {
        // Arrange
        var room = new Room
        {
            RoomId = 1,
            RoomName = "C146"
        };

        _roomRepository.GetRoomById(1)
            .Returns(Task.FromResult<Room?>(room));

        // Act
        var result = await _controller.Create(1);

        // Assert
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<RoomBookingCreateViewModel>(view.Model);

        Assert.Equal(1, model.RoomId);
        Assert.Equal("C146", model.RoomName);
        Assert.True(model.StartTime.HasValue);
        Assert.Equal(model.StartTime.Value.AddHours(1), model.EndTime);
    }

    // POSITIVE 2
    [Fact]
    public async Task Create_WhenBookingIsValid_SavesBookingAndRedirects()
    {
        // Arrange
        SetLoggedInUser("student-1");
        var model = ArrangeValidBookingForm();

        var startTime = model.StartTime
        ?? throw new InvalidOperationException("Test setup requires a start time");

        var endTime = model.EndTime
        ?? throw new InvalidOperationException("Test setup requires an end time");

        // Act
        var result = await _controller.Create(model);

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("Index", redirect.ActionName);
        Assert.Equal("RoomLocation", redirect.ControllerName);

        await _bookingRepository.Received(1).Create(
            Arg.Is<RoomBooking>(booking =>
                booking.RoomId == model.RoomId &&
                booking.StartTime == startTime &&
                booking.EndTime == endTime &&
                booking.Course == model.Course &&
                booking.Description == model.Description &&
                booking.OrganizerId == "student-1"));
    }

    // POSITIVE 3
    [Fact]
    public async Task Index_WhenBookingsExist_ReturnsBookingViewModels()
    {
        // Arrange
        var start = DateTime.Now.AddDays(1);

        var bookings = new List<RoomBooking>
        {
            new RoomBooking
            {
                Id = 10,
                RoomId = 1,
                Course = "Web applications",
                StartTime = start,
                EndTime = start.AddHours(1)
            }
        };

        _bookingRepository.GetAllAsync()
            .Returns(Task.FromResult(bookings));

        _roomRepository.GetRoomById(1)
            .Returns(Task.FromResult<Room?>(
                new Room { RoomId = 1, RoomName = "C146" }));

        // Act
        var result = await _controller.Index();

        // Assert
        var view = Assert.IsType<ViewResult>(result);
        var models = Assert.IsType<List<RoomBookingDetailsViewModel>>(
            view.Model);

        var booking = Assert.Single(models);
        Assert.Equal(10, booking.Id);
        Assert.Equal("C146", booking.RoomName);
        Assert.Equal("Web applications", booking.Course);
        Assert.Equal(start, booking.StartTime);
    }

    // POSITIVE 4
    [Fact]
    public async Task Details_WhenBookingExists_ReturnsCorrectDetails()
    {
        // Arrange
        var start = DateTime.Now.AddDays(1);

        var booking = new RoomBooking
        {
            Id = 10,
            RoomId = 1,
            Course = "Programming",
            Description = "Group assignment",
            StartTime = start,
            EndTime = start.AddHours(2)
        };

        _bookingRepository.GetByIdAsync(10)
            .Returns(Task.FromResult<RoomBooking?>(booking));

        _roomRepository.GetRoomById(1)
            .Returns(Task.FromResult<Room?>(
                new Room { RoomId = 1, RoomName = "C146" }));

        // Act
        var result = await _controller.Details(10);

        // Assert
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<RoomBookingDetailsViewModel>(view.Model);

        Assert.Equal(10, model.Id);
        Assert.Equal(1, model.RoomId);
        Assert.Equal("C146", model.RoomName);
        Assert.Equal("Programming", model.Course);
        Assert.Equal("Group assignment", model.Description);
        Assert.Equal(start, model.StartTime);
        Assert.Equal(start.AddHours(2), model.EndTime);
    }

    // POSITIVE 5
    [Fact]
    public async Task MyBookings_WhenUserIsLoggedIn_ReturnsUsersBookings()
    {
        // Arrange
        SetLoggedInUser("student-1");

        var start = DateTime.Now.AddDays(1);
        var bookings = new List<RoomBooking>
        {
            new RoomBooking
            {
                Id = 10,
                OrganizerId = "student-1",
                RoomId = 1,
                Room = new Room
                {
                    RoomId = 1,
                    RoomName = "C146",
                    Building = "Karethe Johnsens hus"
                },
                Course = "Programming",
                StartTime = start,
                EndTime = start.AddHours(1)
            }
        };

        _bookingRepository.GetByOrganizerIdAsync("student-1")
            .Returns(Task.FromResult<IEnumerable<RoomBooking>>(bookings));

        // Act
        var result = await _controller.MyBookings();

        // Assert
        var view = Assert.IsType<ViewResult>(result);
        var models = Assert.IsType<List<RoomBookingListViewModel>>(
            view.Model);

        var booking = Assert.Single(models);
        Assert.Equal(10, booking.BookingId);
        Assert.Equal("C146", booking.RoomName);
        Assert.Equal("Karethe Johnsens hus", booking.Building);
        Assert.Equal("Programming", booking.Course);

        await _bookingRepository.Received(1)
            .GetByOrganizerIdAsync("student-1");
    }

    // POSITIVE 6
    [Fact]
    public async Task Cancel_WhenUserOwnsBooking_DeletesAndRedirects()
    {
        // Arrange
        SetLoggedInUser("student-1");

        var booking = new RoomBooking
        {
            Id = 10,
            OrganizerId = "student-1"
        };

        _bookingRepository.GetRoomBookingAsync(10)
            .Returns(Task.FromResult<RoomBooking?>(booking));

        // Act
        var result = await _controller.Cancel(10);

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("MyBookings", redirect.ActionName);

        await _bookingRepository.Received(1).DeleteAsync(booking);
    }

    // NEGATIVE 1
    [Fact]
    public async Task Create_WhenRoomDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var model = ArrangeValidBookingForm();

        _roomRepository.GetRoomById(model.RoomId)
            .Returns(Task.FromResult<Room?>(null));

        // Act
        var result = await _controller.Create(model);

        // Assert
        Assert.IsType<NotFoundResult>(result);

        await _bookingRepository.DidNotReceive()
            .Create(Arg.Any<RoomBooking>());
    }

    // NEGATIVE 2
    [Fact]
    public async Task Create_WhenStartIsInPast_ReturnsFormWithoutSaving()
    {
        // Arrange
        SetLoggedInUser("student-1");
        var model = ArrangeValidBookingForm();
        model.StartTime = DateTime.Now.AddDays(-1);

        // Act
        var result = await _controller.Create(model);

        // Assert
        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(model, view.Model);

        Assert.True(_controller.ModelState.ContainsKey(
            nameof(RoomBookingCreateViewModel.StartTime)));

        Assert.False(_controller.ModelState.IsValid);

        await _bookingRepository.DidNotReceive()
            .Create(Arg.Any<RoomBooking>());
    }

    // NEGATIVE 3
    [Fact]
    public async Task Create_WhenEndIsBeforeStart_ReturnsFormWithoutSaving()
    {
        // Arrange
        SetLoggedInUser("student-1");
        var model = ArrangeValidBookingForm();

        var startTime = model.StartTime
        ?? throw new InvalidOperationException("Test setup requires a start time.");

model.EndTime = startTime.AddHours(-1);
        // Act
        var result = await _controller.Create(model);

        // Assert
        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(model, view.Model);
        Assert.False(_controller.ModelState.IsValid);

        await _bookingRepository.DidNotReceive()
            .Create(Arg.Any<RoomBooking>());
    }

    // NEGATIVE 4
    [Fact]
public async Task Create_WhenBookingOverlaps_ReturnsFormWithoutSaving()
{
    // Arrange
    SetLoggedInUser("student-1");
    var model = ArrangeValidBookingForm();

    var startTime = model.StartTime
        ?? throw new InvalidOperationException("Test setup requires a start time.");

    var endTime = model.EndTime
        ?? throw new InvalidOperationException("Test setup requires an end time.");

    _bookingRepository.HasOverlappingBookingsAsync(
            model.RoomId, startTime, endTime, null)
        .Returns(Task.FromResult(true));

    // Act
    var result = await _controller.Create(model);

    // Assert
    var view = Assert.IsType<ViewResult>(result);
    Assert.Same(model, view.Model);
    Assert.False(_controller.ModelState.IsValid);

    await _bookingRepository.Received(1)
        .HasOverlappingBookingsAsync(
            model.RoomId, startTime, endTime, null);

    await _bookingRepository.DidNotReceive()
        .Create(Arg.Any<RoomBooking>());
}

    // NEGATIVE 5
    [Fact]
    public async Task Create_WhenUserIsAnonymous_ReturnsFormWithoutSaving()
    {
        // Arrange
        var model = ArrangeValidBookingForm();
        // The constructor provides an anonymous user.

        // Act
        var result = await _controller.Create(model);

        // Assert
        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(model, view.Model);
        Assert.False(_controller.ModelState.IsValid);

        await _bookingRepository.DidNotReceive()
            .Create(Arg.Any<RoomBooking>());
    }

    // NEGATIVE 6
    [Fact]
    public async Task Cancel_WhenBookingBelongsToAnotherUser_ReturnsForbid()
    {
        // Arrange
        SetLoggedInUser("student-1");

        var booking = new RoomBooking
        {
            Id = 10,
            OrganizerId = "student-2"
        };

        _bookingRepository.GetRoomBookingAsync(10)
            .Returns(Task.FromResult<RoomBooking?>(booking));

        // Act
        var result = await _controller.Cancel(10);

        // Assert
        Assert.IsType<ForbidResult>(result);

        await _bookingRepository.DidNotReceive()
            .DeleteAsync(Arg.Any<RoomBooking>());

        await _bookingRepository.DidNotReceive()
            .DeleteAsync(Arg.Any<int>());
    }

    // Creates valid input and configures an existing room
    private RoomBookingCreateViewModel ArrangeValidBookingForm()
    {
        var room = new Room
        {
            RoomId = 1,
            RoomName = "C146"
        };

        _roomRepository.GetRoomById(room.RoomId)
            .Returns(Task.FromResult<Room?>(room));

        var start = DateTime.Now.AddDays(1);

        return new RoomBookingCreateViewModel
        {
            RoomId = room.RoomId,
            StartTime = start,
            EndTime = start.AddHours(1),
            Course = "Web applications",
            Description = "Group assignment"
        };
    }

    // Simulates an authenticated user without real login or Identity storage
    private void SetLoggedInUser(string userId)
    {
        var identity = new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            },
            authenticationType: "TestAuthentication");

        _controller.HttpContext.User = new ClaimsPrincipal(identity);
    }
}