using System.Globalization;
using ViaEventAssociation.Core.Application.Features.Commands.Event;

namespace UnitTests.Features.Event.UpdateTimeInterval;

public class UpdateTimeIntervalCommandTest {
    // ID:UC4.S1
    [Fact]
    public void Create_ValidInput_Success() {
        // Arrange
        var guid = "EID" + Guid.NewGuid();
        var start = "2025-01-01T08:20:00";
        var end = "2025-01-01T10:00:00";
        // The expected format for DateTime comparison
        var dateTimeFormat = "yyyy-MM-ddTHH:mm:ss";

        // Act
        var result = UpdateTimeIntervalCommand.Create(guid, start, end);
        var cmd = result.Payload;

        // Convert DateTime to strings using the expected format for comparison
        var formattedStart = cmd.TimeInterval.Start.ToString(dateTimeFormat,CultureInfo.InvariantCulture);
        var formattedEnd = cmd.TimeInterval.End.ToString(dateTimeFormat,CultureInfo.InvariantCulture);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guid, cmd.Id.Value);
        Assert.Equal(start, formattedStart);
        Assert.Equal(end, formattedEnd);
    }

    // ID:UC4.F1
    [Fact]
    public void Create_InvaditFormatInput_Failure() {
        // Arrange
        var guid = "EID" + Guid.NewGuid();
        var start = "20250101T08:20:00";
        var end = "2025-01-01T1333ES";

        // Act
        var result = UpdateTimeIntervalCommand.Create(guid, start, end);

        // Assert
        Assert.True(result.IsFailure);
    }
}