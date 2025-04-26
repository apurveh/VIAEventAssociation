using ViaEventAssociation.Core.Domain.Agregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using Xunit.Abstractions;

public class RegisterGuest {
    private readonly ITestOutputHelper _testOutputHelper;

    public RegisterGuest(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    // Given via-email, first name, and last name, and email ends with “@via.dk”, and email is in correct email format, and email.text1 is between 3 and 6 (inclusive) characters long, and email.text1 is either: 3 or 4 uppercase/lowercase English letters, or 6 digits from 0 to 9, and first name is between 2 and 25 characters, and last name is between 2 and 25 characters (i.e. letters, not numbers or symbols)
    //ID:UC10.S1
    [Theory]
    [InlineData("John", "Doe", "JDO@via.dk")]
    [InlineData("Jo", "Do", "308833@via.dk")]
    [InlineData("thisisatwentyfivecharacte", "thisisatwentyfivecharacte", "313330@via.dk")]
    public void RegisterGuest_WithValidData_ShouldCreateNewAccount(string firstName, string lastName, string email) {
        //Arrange
        var validId = GuestId.GenerateId().Payload;
        var validName = NameType.Create(firstName).Payload;
        var validLastName = NameType.Create(lastName).Payload;
        var validEmail = Email.Create(email).Payload;

        //Act
        var result = Guest.Create(validId, validName, validLastName, validEmail);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(firstName, result.Payload.FirstName.Value);
        Assert.Equal(lastName, result.Payload.LastName.Value);
        Assert.Equal(email, result.Payload.Email.Value);
    }

    // Given email, and email does not end with “@via.dk”
    // ID:UC10.F1
    [Fact]
    public void RegisterGuest_WithInvalidEmailDomain_ShouldReturnError() {
        //Arrange
        var email = "john@gmail.com";

        //Act
        var result = Email.Create(email);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidEmailDomain, result.Error.GetAllErrors());
    }

    // Given email, and email is not in correct format (see S1)
    // ID:UC10.F2
    [Theory]
    [InlineData("John", "Doe", "johndoe@via")]
    [InlineData("John", "Doe", "johndoe@via.")]
    [InlineData("John", "Doe", "johndoe@via.d")]
    [InlineData("John", "Doe", "")] // Empty string
    [InlineData("John", "Doe", "TOOMANYCHARS@via.dk")] // Too many characters
    public void RegisterGuest_WithInvalidEmailFormat_ShouldReturnError(string firstName, string lastName, string email) {
        //Arrange

        //Act
        var result = Email.Create(email);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidEmail, result.Error.GetAllErrors());
    }

    // Given first name, and first name is invalid (see S1)
    // ID:UC10.F3
    [Theory]
    [InlineData("J", "Doe", "JDO@via.dk")]
    [InlineData("", "Doe", "JDO@via.dk")]
    [InlineData("this_is_an_extremely_long_", "Doe", "JDO@via.dk")]
    public void RegisterGuest_WithInvalidFirstName_ShouldReturnError(string firstName, string lastName, string email) {
        //Arrange


        //Act
        var result = NameType.Create(firstName);

        //Assert
        Assert.True(result.IsFailure);
        var errors = result.Error.GetAllErrors();
        var containsRequiredError = errors.Any(err => err.Equals(Error.TooShortName(2)) || err.Equals(Error.TooLongName(25)));
        Assert.True(containsRequiredError, "Expected error for too short or too long name not found in errors");
    }

    // Given last name, and last name is invalid (see S1)
    // ID:UC10.F4
    [Theory]
    [InlineData("John", "D", "JDO@via.dk")]
    [InlineData("John", "", "JDO@via.dk")]
    [InlineData("John", "this_is_an_extremely_long_", "JDO@via.dk")]
    public void RegisterGuest_WithInvalidLastName_ShouldReturnError(string firstName, string lastName, string email) {
        //Arrange

        //Act
        var result = NameType.Create(lastName);

        //Assert
        Assert.True(result.IsFailure);
        var errors = result.Error.GetAllErrors();
        var containsRequiredError = errors.Any(err => err.Equals(Error.TooShortName(2)) || err.Equals(Error.TooLongName(25)));
        Assert.True(containsRequiredError, "Expected error for too short or too long name not found in errors");
    }

    // Given email, and the email is already registered
    //should not this be tested outside? in the handler TODO troels
    // ID:UC10.F5
    [Fact]
    public void RegisterGuest_WithAlreadyRegisteredEmail_ShouldReturnError() {
        //Arrange
        var validName = "John";
        var validLastName = "Doe";
        var email = "john@via.dk";

        //Act
        // var result = Guest.Create(validName, validLastName, email);

        //Assert
        //TODO NOT IMPLEMENTED
        Assert.True(true);
    }

    // Given first name or last name, and the name contains numbers
    // ID:UC10.F6
    [Theory]
    [InlineData("John1", "Doe", "JD1@via.dk")]
    [InlineData("John", "Doe2", "JD1@via.dk")]
    [InlineData("324", "Doe", "JD1@via.dk")]
    [InlineData("john", "234", "JD1@via.dk")]
    public void RegisterGuest_WithNumbersInName_ShouldReturnError(string firstName, string lastName, string email) {
        //Arrange
        var errors = new HashSet<Error>();

        //Act
        var resultName = NameType.Create(firstName)
            .OnFailure(error => errors.Add(error));
        var resultLastName = NameType.Create(lastName)
            .OnFailure(error => errors.Add(error));

        var error = Error.Add(errors);

        //Assert
        Assert.True(resultName.IsFailure || resultLastName.IsFailure);
        Assert.Contains(Error.InvalidName, error.GetAllErrors());
    }

    // Given first name or last name, and the name contains symbols
    // ID:UC10.F7
    [Theory]
    [InlineData("John!", "Doe", "JD1@via.dk")]
    [InlineData("John", "Doe@", "JD1@via.dk")]
    [InlineData("!", "Doe", "JD1@via.dk")]
    [InlineData("john", "@", "JD1@via.dk")]
    public void RegisterGuest_WithSymbolsInName_ShouldReturnError(string firstName, string lastName, string email) {
        //Arrange
        var errors = new HashSet<Error>();

        //Act
        var resultName = NameType.Create(firstName)
            .OnFailure(error => errors.Add(error));
        var resultLastName = NameType.Create(lastName)
            .OnFailure(error => errors.Add(error));

        var error = Error.Add(errors);

        //Assert
        Assert.True(resultName.IsFailure || resultLastName.IsFailure);
        Assert.Contains(Error.InvalidName, error.GetAllErrors());
    }
}