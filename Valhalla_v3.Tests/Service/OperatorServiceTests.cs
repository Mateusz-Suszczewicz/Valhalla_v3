using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Linq.Expressions;
using Valhalla_v3.Database;
using Valhalla_v3.Services;
using Valhalla_v3.Shared;

public class OperatorServiceTests
{
    private readonly Mock<DbSet<Operator>> _mockDbSet;
    private readonly Mock<ValhallaContext> _mockContext;
    private readonly OperatorService _operatorService;

    public OperatorServiceTests()
    {
        _mockDbSet = new Mock<DbSet<Operator>>();
        Mock<ValhallaContext> mock = new Mock<ValhallaContext>();
        _mockContext = mock;
        _mockContext.Setup(c => c.Operator).Returns(_mockDbSet.Object);
        _operatorService = new OperatorService(_mockContext.Object);
    }
    private Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
    {
        var queryable = data.AsQueryable();
        var mockDbSet = new Mock<DbSet<T>>();

        mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

        mockDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<T, bool>>>(), default))
                 .ReturnsAsync((Expression<Func<T, bool>> predicate, CancellationToken _) =>
                 {
                     return queryable.FirstOrDefault(predicate);
                 });

        return mockDbSet;
    }

    [Fact]
    public async Task Create_ValidOperator_ReturnsId()
    {
        // Arrange
        var newOperator = new Operator
        {
            Name = "Test Operator",
            Password = "Password123"
        };

        _mockDbSet.Setup(m => m.AddAsync(It.IsAny<Operator>(), default))
                  .ReturnsAsync((Operator op, CancellationToken _) =>
                  {
                      op.Id = 1; // Symulacja przypisania ID podczas zapisu w bazie
                      return Mock.Of<EntityEntry<Operator>>(e => e.Entity == op);
                  });


        // Act
        var result = await _operatorService.Create(newOperator);

        // Assert
        Assert.Equal(1, result);
        _mockDbSet.Verify(m => m.AddAsync(It.IsAny<Operator>(), default), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Create_NullOperator_ThrowsArgumentNullException()
    {
        // Arrange
        Operator nullOperator = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _operatorService.Create(nullOperator));
    }

    [Fact]
    public async Task Create_OperatorWithId_ThrowsArgumentException()
    {
        // Arrange
        var invalidOperator = new Operator
        {
            Id = 1,
            Name = "Invalid Operator",
            Password = "Password123"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _operatorService.Create(invalidOperator));
    }

    [Fact]
    public async Task Delete_ValidId_DeletesOperator()
    {
        // Arrange
        var operatorToDelete = new Operator { Id = 1, Name = "Test Operator" };
        _mockDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Operator, bool>>>(), default))
                  .ReturnsAsync(operatorToDelete);


        // Act
        await _operatorService.Delete(1);

        // Assert
        _mockDbSet.Verify(m => m.Remove(It.IsAny<Operator>()), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Delete_InvalidId_ThrowsKeyNotFoundException()
    {
        // Arrange
        _mockDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Operator, bool>>>(), default))
                  .ReturnsAsync((Operator)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _operatorService.Delete(99));
    }

    [Fact]
    public async Task Get_ValidId_ReturnsOperator()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ValhallaContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new ValhallaContext(new ConfigurationBuilder().Build());
        context.Operator.Add(new Operator { Id = 1, Name = "Test Operator" });
        await context.SaveChangesAsync();

        var service = new OperatorService(context);

        // Act
        var result = await service.Get(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Operator", result.Name);
    }

    [Fact]
    public async Task Get_InvalidId_ThrowsKeyNotFoundException()
    {
        // Arrange
        _mockDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Operator, bool>>>(), default))
                  .ReturnsAsync((Operator)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _operatorService.Get(99));
    }

    [Fact]
    public async Task Update_ValidOperator_UpdatesOperator()
    {
        // Arrange
        var existingOperator = new Operator { Id = 1, Name = "Old Name", Password = "OldPassword" };
        _mockDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Operator, bool>>>(), default))
                  .ReturnsAsync(existingOperator);

        var updatedOperator = new Operator { Id = 1, Name = "New Name", Password = "NewPassword" };

        // Act
        await _operatorService.Update(updatedOperator);

        // Assert
        Assert.Equal("New Name", existingOperator.Name);
        Assert.Equal("NewPassword", existingOperator.Password);
        _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Update_NullOperator_ThrowsArgumentNullException()
    {
        // Arrange
        Operator nullOperator = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _operatorService.Update(nullOperator));
    }

    [Fact]
    public async Task Update_InvalidId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var invalidOperator = new Operator { Id = 99, Name = "Invalid Operator", Password = "InvalidPassword" };
        _mockDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Operator, bool>>>(), default))
                  .ReturnsAsync((Operator)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _operatorService.Update(invalidOperator));
    }
}
