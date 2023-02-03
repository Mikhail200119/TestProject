using AutoMapper;
using Event.Bll.Exceptions;
using Event.Bll.Models;
using Event.Bll.Services;
using Event.Bll.Services.Interfaces;
using Event.Dal.Entities;
using Event.Dal.Repositories.Interfaces;
using FluentAssertions;
using Moq;

namespace Event.Bll.Tests;

public class EventManagerTests
{
    private const string UserEmail = "test_user@gmail.com";

    private readonly IEventManager _eventManager;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly Mock<IEventUnitOfWork> _mockEventUnitOfWork;

    public EventManagerTests()
    {
        var user = new User
        {
            Email = UserEmail
        };

        _mockEventUnitOfWork = new Mock<IEventUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockUserService = new Mock<IUserService>();

        _mockEventRepository = new Mock<IEventRepository>();

        _mockUserService
            .Setup(us => us.GetCurrentUser())
            .Returns(user);

        _mockEventUnitOfWork
            .Setup(uow => uow.Events)
            .Returns(_mockEventRepository.Object);

        _eventManager = new EventManager(_mockEventUnitOfWork.Object, _mockUserService.Object, _mockMapper.Object);
    }

    private readonly EventDbModel _eventDbModel = new()
    {
        Description = "test event",
        Date = DateTime.UtcNow,
        Organizers = new List<OrganizerDbModel> { new() { Name = "TestOrganizer" } },
        Speakers = new List<SpeakerDbModel> { new() { Name = "TestSpeaker" } },
        Location = "Brest",
        CreatedBy = UserEmail
    };

    private readonly EventCreateModel _eventCreateModel = new()
    {
        Description = "test event",
        Date = DateTime.UtcNow,
        Organizers = new List<OrganizerCreateModel> { new() { Name = "TestOrganizer" } },
        Speakers = new List<SpeakerCreateModel> { new() { Name = "TestSpeaker" } },
        Location = "Brest"
    };

    private readonly EventUpdateModel _eventUpdateModel = new()
    {
        Description = "test event",
        Date = DateTime.UtcNow,
        Organizers = new List<OrganizerUpdateModel> { new() { Name = "TestOrganizer" } },
        Speakers = new List<SpeakerUpdateModel> { new() { Name = "TestSpeaker" } },
        Location = "Brest",
    };

    private readonly EventModel _eventModel = new()
    {
        Description = "test event",
        Date = DateTime.UtcNow,
        Organizers = new List<OrganizerModel> { new() { Name = "TestOrganizer" } },
        Speakers = new List<SpeakerModel> { new() { Name = "TestSpeaker" } },
        Location = "Brest"
    };

    [Fact]
    public async Task Should_Successfully_Create_A_New_Event()
    {
        // Arrange
        _mockMapper
            .Setup(m => m.Map<EventDbModel>(It.IsAny<EventCreateModel>()))
            .Returns(_eventDbModel);
        
        var expectedResponseModel = _eventModel;

        _mockMapper
            .Setup(m => m.Map<EventModel>(It.IsAny<EventDbModel>()))
            .Returns(expectedResponseModel);
        
        // Act
        var response = await _eventManager.CreateAsync(_eventCreateModel);

        // Assert
        response
            .Should()
            .BeEquivalentTo(expectedResponseModel);

        _mockMapper.VerifyAll();
        _mockUserService.Verify(us => us.GetCurrentUser());
    }

    [Fact]
    public async Task Should_Successfully_Return_An_Event_By_Id()
    {
        // Arrange
        _mockEventRepository
            .Setup(er => er.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_eventDbModel);

        _mockMapper
            .Setup(m => m.Map<EventModel>(_eventDbModel))
            .Returns(_eventModel);
        
        // Act
        var response = await _eventManager.GetEventByIdAsync(1);

        // Assert
        response
            .Should()
            .BeEquivalentTo(_eventModel);

        _mockEventRepository.VerifyAll();
        _mockMapper.VerifyAll();
    }

    [Fact]
    public async Task Should_Throw_NotFoundException_When_Event_With_Passed_Id_Was_Not_Found()
    {
        // Arrange
        _mockEventRepository
            .Setup(er => er.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((EventDbModel?)null);

        // Act
        Func<Task<EventModel?>> getEventByIdAction = async () => await _eventManager.GetEventByIdAsync(1);

        // Assert
        await getEventByIdAction
            .Should()
            .ThrowExactlyAsync<NotFoundException>()
            .WithMessage("Event with id 1 has not been found.");

        _mockEventRepository.VerifyAll();
    }

    [Fact]
    public async Task Should_Throw_PermissionsException_When_Unauthorized_User_Tries_To_Get_Event_By_Id()
    {
        // Arrange
        _eventDbModel.CreatedBy = "Another user";

        _mockEventRepository
            .Setup(er => er.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_eventDbModel);

        // Act
        Func<Task<EventModel?>> getEventByIdAction = async () => await _eventManager.GetEventByIdAsync(1);

        // Assert
        await getEventByIdAction
            .Should()
            .ThrowExactlyAsync<PermissionsException>()
            .WithMessage("Only creator of the event is able manage it.");

        _mockEventRepository.VerifyAll();
    }

    [Fact]
    public async Task Should_Successfully_Update_Existing_Event()
    {
        // Arrange
        var updateModel = _eventUpdateModel;

        _mockEventRepository
            .Setup(er => er.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new EventDbModel { CreatedBy = UserEmail });

        _mockMapper
            .Setup(m => m.Map<EventDbModel>(updateModel))
            .Returns(_eventDbModel);
        
        _eventModel.Id = 1;

        _mockMapper
            .Setup(m => m.Map<EventModel>(It.IsAny<EventDbModel>()))
            .Returns(_eventModel);
        
        // Act
        var response = await _eventManager.UpdateAsync(1, updateModel);

        // Assert
        response
            .Should()
            .BeEquivalentTo(_eventModel);

        _mockMapper.VerifyAll();
        _mockEventRepository.VerifyAll();
    }

    [Fact]
    public async Task Should_Throw_NotFoundException_When_Event_With_Passed_Id_Is_Not_Exist_In_Update()
    {
        // Arrange
        _mockEventRepository
            .Setup(er => er.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((EventDbModel?)null);

        // Act
        Func<Task<EventModel?>> updateEventAction = async () => await _eventManager.UpdateAsync(1, new EventUpdateModel());

        // Assert
        await updateEventAction
            .Should()
            .ThrowExactlyAsync<NotFoundException>()
            .WithMessage("Event with id 1 has not been found.");

        _mockEventRepository.VerifyAll();
    }

    [Fact]
    public async Task Should_Successfully_Delete_An_Event()
    {
        // Arrange
        _mockEventRepository
            .Setup(er => er.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_eventDbModel);

        // Act
        Func<Task> deleteEventAction = async () => await _eventManager.DeleteAsync(1);

        // Assert
        await deleteEventAction
            .Should()
            .NotThrowAsync();

        _mockEventRepository.VerifyAll();
        _mockEventRepository.Verify(er => er.DeleteAsync(It.IsAny<int>()), Times.Once);
        _mockEventUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_Successfully_Return_All_Events()
    {
        // Arrange
        var existingEventsDbModels = new List<EventDbModel>
        {
            new() { Id = 1, Description = "First", CreatedBy = UserEmail },
            new() { Id = 2, Description = "Second", CreatedBy = UserEmail },
            new() { Id = 3, Description = "Third", CreatedBy = UserEmail },
            new() { Id = 4, Description = "Fourth", CreatedBy = UserEmail }
        };

        var eventsModels = new List<EventModel>
        {
            new() { Id = 1, Description = "First" },
            new() { Id = 2, Description = "Second" },
            new() { Id = 3, Description = "Third" },
            new() { Id = 4, Description = "Fourth" }
        };

        _mockEventRepository
            .Setup(er => er.GetAllAsync(UserEmail))
            .ReturnsAsync(existingEventsDbModels);

        _mockMapper
            .Setup(m => m.Map<IEnumerable<EventDbModel>, IEnumerable<EventModel>>(existingEventsDbModels))
            .Returns(eventsModels);
        
        // Act
        var response = await _eventManager.GetAllEventsAsync();

        // Assert
        var responseAsList = response.ToList();

        responseAsList
            .Should()
            .BeEquivalentTo(eventsModels);

        _mockEventRepository.VerifyAll();
        _mockMapper.VerifyAll();
    }

    [Fact]
    public async Task Should_Throw_Permissions_Exception_When_Unauthorized_User_Tries_Update_Event()
    {
        // Arrange
        _mockUserService
            .Setup(us => us.GetCurrentUser())
            .Returns(new User { Email = "UnauthorizedUser" });

        _mockEventRepository
            .Setup(er => er.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_eventDbModel);

        // Act
        Func<Task<EventModel?>> updateEventAction = async () => await _eventManager.UpdateAsync(1, new EventUpdateModel());

        // Assert
        await updateEventAction
            .Should()
            .ThrowExactlyAsync<PermissionsException>()
            .WithMessage("Only creator of the event is able manage it.");

        _mockUserService.VerifyAll();
        _mockEventRepository.VerifyAll();
    }

    [Fact]
    public async Task Should_Throw_PermissionsException_When_Unauthorized_User_Tries_To_Delete_Event()
    {
        // Arrange
        _mockUserService
            .Setup(us => us.GetCurrentUser())
            .Returns(new User { Email = "UnauthorizedUser" });

        _mockEventRepository
            .Setup(er => er.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_eventDbModel);

        // Act
        Func<Task> deleteEventAction = async () => await _eventManager.DeleteAsync(1);

        // Assert
        await deleteEventAction
            .Should()
            .ThrowExactlyAsync<PermissionsException>()
            .WithMessage("Only creator of the event is able manage it.");

        _mockUserService.VerifyAll();
    }
}