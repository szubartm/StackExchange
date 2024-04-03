using Moq;
using StackExchange.API.Data.Entities;
using StackExchange.API.Services;
using StackExchange.Api.UnitTests.Helpers;

namespace StackExchange.Api.UnitTests;

public class TagServiceTests
{
    [Fact]
    public async Task SetPercentageOfAllGivenTags_IsSettingValuesCorrectly()
    {
        var tags = DataHelper.GetFakeTagsList();
        var calculatorMock = new Mock<ICalculator>();
        calculatorMock.Setup(x => x.CalculatePercentage(It.IsAny<long>(), It.IsAny<long>()))
            .Returns(5);
        var service = new TagService(calculatorMock.Object);
        var result = service.SetPercentageOfAllGivenTags(tags);

        Assert.True(result[0].Share == 5);
    }

    [Fact]
    public async Task SetPercentageOfAllGivenTags_ReturnsEmptyList_If_InputIsEmpty()
    {
        var calculatorMock = new Mock<ICalculator>();
        var service = new TagService(calculatorMock.Object);
        var result = service.SetPercentageOfAllGivenTags(new List<TagDto>());

        Assert.True(result.Count == 0);
    }
}