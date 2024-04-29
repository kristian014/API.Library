using Application.BookLibrary.Books.Requests;
using Application.BookLibrary.Lookups.Dtos;
using Application.BookLibrary.Lookups.Dtos.Spec;
using Application.BookLibrary.LookupTypes.Dto;
using Application.BookLibrary.LookupTypes.Spec;
using Application.Common.Exceptions;
using Application.Common.Interface;
using Application.Repository;
using Application.User;
using Domain.Models;
using Moq;

namespace Test.BookLibraryAPI.APIController
{
    public class BookControllerTesrt
    {
        private readonly Mock<IRepository<Book>> _mockBookRepository;
        private readonly Mock<IRepository<LookupType>> _mockLookupTypeRepository;
        private readonly Mock<IRepository<Lookup>> _mockLookupRepository;
        private readonly Mock<IUserContext> _mockUserContext;
        private readonly CreateBookRequestHandler _handler;
        public BookControllerTesrt()
        {
            _mockBookRepository = new Mock<IRepository<Book>>();
            _mockLookupTypeRepository = new Mock<IRepository<LookupType>>();
            _mockLookupRepository = new Mock<IRepository<Lookup>>();
            _mockUserContext = new Mock<IUserContext>();

            _handler = new CreateBookRequestHandler(_mockBookRepository.Object, _mockLookupTypeRepository.Object, _mockLookupRepository.Object, _mockUserContext.Object);
        }
        [Test]
        public async Task Handle_SuccessfulCreation_ReturnsBookId()
        {
            // Arrange
            var request = new CreateBookRequest
            {
                Title = "Sample Book",
                ISBN = "123-4567890123",
                Price = 15.99,
                AuthorId = Guid.NewGuid(),
            };

            Guid expectedBookId = new Guid("24c180f1-ae05-45cf-bed7-625dab922c22");
            _mockUserContext.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser(
                  "24c180f1-ae05-45cf-bed7-625dab922c62",
                  "Test1@gmail.com",
                  new List<string> { "Role1", "Role2" }
              ));

            _mockLookupTypeRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<LookTypeByNameSpec>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new LookupTypeDto { Id = Guid.NewGuid() });

            _mockLookupRepository.Setup(r => r.ListAsync(It.IsAny<LookupByTypeIdSpec>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<LookupDto> { new LookupDto { Id = Guid.NewGuid(), Label = "Available" } });

            _mockBookRepository.Setup(r => r.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
                .Returns((Book book, CancellationToken _) => {
                    book.Id = expectedBookId;
                    return Task.FromResult(book);
                });

            // Act
            Guid result = await _handler.Handle(request, new CancellationToken());

            // Assert
            Assert.AreEqual(expectedBookId, result);
            _mockBookRepository.Verify(m => m.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Handle_StatusNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var request = new CreateBookRequest
            {
                Title = "Sample Book",
                ISBN = "123-4567890123",
                Price = 15.99,
                AuthorId = Guid.NewGuid(),
            };

            _mockUserContext.Setup(uc => uc.GetCurrentUser()).Returns(new CurrentUser(
                  "24c180f1-ae05-45cf-bed7-625dab922c62",
                  "Test1@gmail.com",
                  new List<string> { "Role1", "Role2" }
              ));
            _mockLookupTypeRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<LookTypeByNameSpec>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new LookupTypeDto { Id = Guid.NewGuid() });

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, new CancellationToken()));
        }

    }
}
