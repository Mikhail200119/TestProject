using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Event.Api.Controllers;
using Event.Api.Models.Request;
using Event.Api.Models.Response;
using Event.Bll.Models;
using Event.Bll.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Event.Api.Tests;

public class EventsControllerTests
{
    private readonly EventsController _eventsController;
    private readonly Mock<IEventManager> _mockEventManager;
    private readonly Mock<IMapper> _mockMapper;

    public EventsControllerTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockEventManager = new Mock<IEventManager>();

        _eventsController = new EventsController(_mockEventManager.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Should_Return_All_Events()
    {
        // Arrange
        var eventModelList = new List<EventModel>
        {
            new() { Id = 1 },
            new() { Id = 2 },
            new() { Id = 3 }
        };

        var eventGetResponseList = new List<EventGetResponse>
        {
            new() { Id = 1 },
            new() { Id = 2 },
            new() { Id = 3 }
        };

        _mockEventManager
            .Setup(em => em.GetAllEventsAsync())
            .ReturnsAsync(eventModelList);

        _mockMapper
            .Setup(m => m.Map<IEnumerable<EventModel>, IEnumerable<EventGetResponse>>(eventModelList))
            .Returns(eventGetResponseList);

        // Act
        var getAllEventsResponse = await _eventsController.GetAllEventsAsync();

        // Assert
        getAllEventsResponse
            .Should()
            .BeOfType<ActionResult<IEnumerable<EventGetResponse>>>().Which.Result
            .Should()
            .BeOfType<OkObjectResult>().Which.Value
            .Should()
            .BeEquivalentTo(eventGetResponseList);

        _mockEventManager.VerifyAll();
        _mockMapper.VerifyAll();
    }

    [Fact]
    public async Task Should_Successfully_Return_Event_By_Id()
    {
        // Arrange
        var eventModel = new EventModel { Id = 1, Description = "Test event" };

        _mockEventManager
            .Setup(em => em.GetEventByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(eventModel);

        var eventGetResponse = new EventGetResponse { Id = 1, Description = "Test event" };

        _mockMapper
            .Setup(m => m.Map<EventGetResponse>(It.IsAny<EventModel>()))
            .Returns(eventGetResponse);

        // Act
        var getEventByIdResponse = await _eventsController.GetEventByIdAsync(1);

        // Assert
        getEventByIdResponse
            .Should()
            .BeOfType<ActionResult<EventGetResponse>>().Which.Result
            .Should()
            .BeOfType<OkObjectResult>().Which.Value
            .Should()
            .BeEquivalentTo(eventGetResponse);

        _mockEventManager.VerifyAll();
        _mockMapper.VerifyAll();
    }

    [Fact]
    public async Task Should_Create_New_Event()
    {
        // Arrange
        var createRequest = new EventCreateRequest { Description = "Test", Location = "Brest" };

        _mockMapper
            .Setup(m => m.Map<EventCreateModel>(It.IsAny<EventCreateRequest>()))
            .Returns(new EventCreateModel { Description = "Test", Location = "Brest" });

        _mockEventManager
            .Setup(em => em.CreateAsync(It.IsAny<EventCreateModel>()))
            .ReturnsAsync(new EventModel { Description = "Test", Location = "Brest" });

        var createResponse = new EventCreateResponse { Description = "Test", Location = "Brest" };
        
        _mockMapper
            .Setup(m => m.Map<EventCreateResponse>(It.IsAny<EventModel>()))
            .Returns(createResponse);

        // Act
        var result = await _eventsController.CreateEventAsync(createRequest);

        // Assert
        result
            .Should()
            .BeOfType<ActionResult<EventCreateResponse>>().Which.Result
            .Should()
            .BeOfType<CreatedAtActionResult>().Which.Value
            .Should()
            .BeEquivalentTo(createResponse);
            
        _mockMapper.VerifyAll();
        _mockEventManager.VerifyAll();
    }

    [Fact]
    public async Task Should_Successfully_Update_Event()
    {
        // Arrange
        var updateRequest = new EventUpdateRequest { Description = "Test", Location = "Brest" };
        var updateModel = new EventUpdateModel { Description = "Test", Location = "Brest" };
        var eventModel = new EventModel { Description = "Test", Location = "Brest" };
        var eventUpdateResponse = new EventUpdateResponse { Description = "Test", Location = "Brest" };

        _mockMapper
            .Setup(m => m.Map<EventUpdateModel>(updateRequest))
            .Returns(updateModel);

        _mockEventManager
            .Setup(em => em.UpdateAsync(It.IsAny<int>(), updateModel))
            .ReturnsAsync(eventModel);

        _mockMapper
            .Setup(m => m.Map<EventUpdateResponse>(eventModel))
            .Returns(eventUpdateResponse);

        // Act
        var result = await _eventsController.UpdateEventAsync(1, updateRequest);

        // Assert
        result
            .Should()
            .BeOfType<ActionResult<EventUpdateResponse>>().Which.Result
            .Should()
            .BeOfType<OkObjectResult>().Which.Value
            .Should()
            .BeEquivalentTo(eventUpdateResponse);

        _mockMapper.VerifyAll();
        _mockEventManager.VerifyAll();
    }

    [Fact]
    public async Task Should_Delete_Event()
    {
        // Act
        var result = await _eventsController.DeleteEventAsync(1);

        // Assert
        result
            .Should()
            .BeOfType<NoContentResult>();

        _mockEventManager.Verify(em => em.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Theory]
    [MemberData(nameof(GetEventCreateRequests))]
    public void Should_Return_Bad_Request_When_Model_Is_Not_Valid(EventCreateRequest request, bool expectedIsValid)
    {
        // Act
        var isValid = Validator.TryValidateObject(request, new ValidationContext(request), new List<ValidationResult>());

        // Assert
        isValid
            .Should()
            .Be(expectedIsValid);
    }

    public static IEnumerable<object[]> GetEventCreateRequests()
    {
        yield return new object[]
        {
            new EventCreateRequest
            {
                Name = "Test", 
                Description = "Test", 
                Location = "Test",
                Date = DateTime.UtcNow,
                Organizers = new List<OrganizerCreateRequest>(),
                Speakers = new List<SpeakerCreateRequest>()
            },
            true
        };
        
        yield return new object[]
        {
            new EventCreateRequest
            {
                Name = "Test", 
                Description = "Test", 
                Location = "Test",
                Date = DateTime.UtcNow,
                Speakers = new List<SpeakerCreateRequest>()
            },
            true
        };
     
        yield return new object[]
        {
            new EventCreateRequest
            {
                Name = "Test", 
                Location = "Test",
                Date = DateTime.UtcNow,
            },
            true
        };
        
        yield return new object[]
        {
            new EventCreateRequest
            {
                Location = "Test",
            },
            false
        };
        
        yield return new object[]
        {
            new EventCreateRequest
            {
                Name = "Test", 
                Description = "Test",
                Date = DateTime.UtcNow,
                Organizers = new List<OrganizerCreateRequest>(),
                Speakers = new List<SpeakerCreateRequest>()
            },
            false
        };
    }
}