public class SetReadyEvenCommandTest {
    //ID:UC8.S1
    [Fact]
    public void Create_SetReadyEventCommand_WithValidData() {
        //Arrange
        var guid = "EID" + Guid.NewGuid();

        //Act
        var result = SetReadyEventCommand.Create(guid);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guid, result.Payload.Id.Value);
    }

    //ID:UC8.F1
    [Fact]
    public void Create_SetReadyEventCommand_WithInvalidEventId() {
        //Arrange
        var guid = "EID" + "InvalidGuid";

        //Act
        var result = SetReadyEventCommand.Create(guid);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.InvalidLength, result.Error);
    }
}