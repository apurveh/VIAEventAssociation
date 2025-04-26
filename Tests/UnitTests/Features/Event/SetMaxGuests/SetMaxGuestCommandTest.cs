namespace UnitTests.Features.Event.SetMaxGuests;

public class SetMaxGuestCommandTest {
    //ID:UC7.S1
    [Fact]
    public void Create_SetMaxGuestCommand_WithValidData() {
        //Arrange
        var guid = "EID" + Guid.NewGuid();
        var maxGuests = "10";

        //Act
        var result = SetMaxGuestCommand.Create(guid, maxGuests);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guid, result.Payload.Id.Value);
    }

    //ID:UC7.F1
    [Fact]
    public void Create_SetMaxGuestCommand_WithNegativeMaxGuests() {
        //Arrange
        var guid = "EID" + Guid.NewGuid();
        var maxGuests = "-10";

        //Act
        var result = SetMaxGuestCommand.Create(guid, maxGuests);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidMaxGuests(maxGuests).Message, result.Error.Message);
    }
}