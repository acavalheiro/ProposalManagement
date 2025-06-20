using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using ProposalManagement.Application.Commands;
using ProposalManagement.Application.Commands.Handlers;
using ProposalManagement.Application.Core.Validators;
using ProposalManagement.Application.Validators;
using ProposalManagement.Domain.Entities;
using ProposalManagement.Domain.Enums;
using ProposalManagement.Infrastructure.Data;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Tests.Commands.Handlers
{
    [TestFixture]
    public class CreateProposalCommandHandlerTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private Mock<ILogger<CreateProposalCommandHandler>> _mockLogger;
        private CreateProposalValidator _createProposalValidator;
        private CreateProposalCommandHandler _handler;

        private List<User> _users = new List<User>()
        {
            new User()
            {
                UserId = Guid.Parse("e9a06151-7f87-4670-a95a-aab83bebea66"),
                PartyId = Guid.Parse("f892b8a5-45dc-4320-baf8-ce0a9011f78a"),
            },
        };

        private List<Item> _items = new List<Item>()
        {
            new Item()
            {
                ItemId = Guid.Parse("069ec4ec-eb7a-465e-8c27-394cdacf7940"), Name = "Item 1",
                Parties = { new Party() { PartyId = Guid.Parse("f892b8a5-45dc-4320-baf8-ce0a9011f78a")} }
                
            },
            new Item()
            {
                ItemId = Guid.Parse("ab0f4d4d-a995-4c35-9a4c-95d5f7f01efd"), Name = "Item 1",
                Parties = { new Party() { PartyId = Guid.Parse("7e22cc45-79da-4ac1-9a63-34b32201bf08") }}
                
            },
        };

        private List<Proposal> _proposals = new List<Proposal>()
        {
            new Proposal() { ItemId = Guid.Parse("069ec4ec-eb7a-465e-8c27-394cdacf7940") }
        };


        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _mockContext.Setup(u => u.Users).ReturnsDbSet(_users);
            _mockContext.Setup(i => i.Items).ReturnsDbSet(_items);
            _mockContext.Setup(p => p.Proposals).ReturnsDbSet(_proposals);
            _mockLogger = new Mock<ILogger<CreateProposalCommandHandler>>();
            _createProposalValidator = new CreateProposalValidator(_mockContext.Object, _mockLogger.Object);
            _handler = new CreateProposalCommandHandler(_mockContext.Object, _mockLogger.Object, _createProposalValidator);
        }



        [Test]
        public async Task Handle_UserNotFound_ReturnsNotFoundError()
        {
            // Arrange
            var nonExistentUserId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var partyId = Guid.NewGuid();


            var command = new CreateProposalCommand
            {
                ItemId = itemId,
                Information = "Test proposal information",
                AllocationType = ProposalAllocationType.Amount,
                AllocationQuantity = 50,
                AuthenticatedUserId = nonExistentUserId
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error.Description, Is.EqualTo($"{nameof(User)} with Id: {nonExistentUserId} not found"));
            Assert.That(result.Error.Code, Is.EqualTo("Error.NotFound"));
        }
        
        [Test]
        public async Task Handle_ItemNotFound_ReturnsNotFoundError()
        {
            // Arrange
            var existingUserId = Guid.Parse("e9a06151-7f87-4670-a95a-aab83bebea66");
            var nonExistentItemId = Guid.NewGuid();
            var partyId = Guid.NewGuid();


            var command = new CreateProposalCommand
            {
                ItemId = nonExistentItemId,
                Information = "Test proposal information",
                AllocationType = ProposalAllocationType.Amount,
                AllocationQuantity = 50,
                AuthenticatedUserId = existingUserId
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error.Description, Is.EqualTo($"{nameof(Item)} with Id: {nonExistentItemId} not found"));
            Assert.That(result.Error.Code, Is.EqualTo("Error.NotFound"));
        }
        
        [Test]
        public async Task Handle_ItemNotBelongToParty_ReturnsItemNotBelongToPartyError()
        {
            // Arrange
            var existingUserId = Guid.Parse("e9a06151-7f87-4670-a95a-aab83bebea66");
            var existentItemId = Guid.Parse("ab0f4d4d-a995-4c35-9a4c-95d5f7f01efd");
            var partyId = Guid.NewGuid();


            var command = new CreateProposalCommand
            {
                ItemId = existentItemId,
                Information = "Test proposal information",
                AllocationType = ProposalAllocationType.Amount,
                AllocationQuantity = 50,
                AuthenticatedUserId = existingUserId
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error.Description, Is.EqualTo($"The item does not belong to the party of the authenticated user."));
            Assert.That(result.Error.Code, Is.EqualTo("ItemNotBelongToParty"));
        }
        
        [Test]
        [TestCase(-1)]
        [TestCase(101)]
        public async Task Handle_InvalidAllocationQuantity_ReturnsInvalidAllocationPercentageError(int value)
        {
            // Arrange
            var existingUserId = Guid.Parse("e9a06151-7f87-4670-a95a-aab83bebea66");
            var existentItemId = Guid.Parse("069ec4ec-eb7a-465e-8c27-394cdacf7940");
            var partyId = Guid.NewGuid();


            var command = new CreateProposalCommand
            {
                ItemId = existentItemId,
                Information = "Test proposal information",
                AllocationType = ProposalAllocationType.Percentage,
                AllocationQuantity = value,
                AuthenticatedUserId = existingUserId
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error.Description, Is.EqualTo($"Allocation percentage must be between 0 and 100."));
            Assert.That(result.Error.Code, Is.EqualTo("InvalidAllocationPercentage"));
        }
        
        [Test]
        [TestCase(-1)]
        public async Task Handle_InvalidAllocationQuantity_ReturnsInvalidAllocationQuantityError(int value)
        {
            // Arrange
            var existingUserId = Guid.Parse("e9a06151-7f87-4670-a95a-aab83bebea66");
            var existentItemId = Guid.Parse("069ec4ec-eb7a-465e-8c27-394cdacf7940");
            var partyId = Guid.NewGuid();


            var command = new CreateProposalCommand
            {
                ItemId = existentItemId,
                Information = "Test proposal information",
                AllocationType = ProposalAllocationType.Amount,
                AllocationQuantity = value,
                AuthenticatedUserId = existingUserId
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error.Description, Is.EqualTo($"Allocation quantity must be greater than 0."));
            Assert.That(result.Error.Code, Is.EqualTo("InvalidAllocationQuantity"));
        }
        
        [Test]
        public async Task Handle_ProposalAlreadyExists_ProposalAlreadyExistsError()
        {
            // Arrange
            var existingUserId = Guid.Parse("e9a06151-7f87-4670-a95a-aab83bebea66");
            var existentItemId = Guid.Parse("069ec4ec-eb7a-465e-8c27-394cdacf7940");
            var partyId = Guid.NewGuid();


            var command = new CreateProposalCommand
            {
                ItemId = existentItemId,
                Information = "Test proposal information",
                AllocationType = ProposalAllocationType.Amount,
                AllocationQuantity = 5,
                AuthenticatedUserId = existingUserId
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error.Description, Is.EqualTo($"A proposal for this item already exists."));
            Assert.That(result.Error.Code, Is.EqualTo("ProposalAlreadyExists"));
        }
    }
}