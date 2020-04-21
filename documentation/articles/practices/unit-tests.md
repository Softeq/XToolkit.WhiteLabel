# Unit Tests

Unit testing is a great idea. It provides for code coverage, is a resource for documentation, and it provides a vehicle for good design.

## Arranging your tests

### Arrange-Act-Assert

The AAA (Arrange, Act, Assert) pattern is a common way of writing unit tests for a method under test.

The **Arrange** section of a unit test method initializes objects and sets the value of the data that is passed to the method under test.

The **Act** section invokes the method under test with the arranged parameters.

The **Assert** section verifies that the action of the method under test behaves as expected.

Following this pattern does make the code quite well structured and easy to understand. In general lines, it would look like this:

```cs
// arrange
var repository = Substitute.For<IClientRepository>();
var client = new Client(repository);

// act
client.Save();

// assert
mock.Received.SomeMethod();
```

> Comments `//` can be skipped.

### Test Names

The name of the test should tell you what the test is doing without you having to read every line of code to figure it out. It should explain the prerequisites. It should explain what behavior we expect from the function under test. It should help you frame your mind so that when a test fails you can more easily figure out what broke.

#### Gherkin Syntax

Gherkin Syntax explains behavior using the GivenWhenThen approach.

This approach aligns with the pattern used to structure the unit test, via AAA.

Format:

- **Given**: _Given a certain setup or precondition exists for the test._
- **When**: _When an action is performed._
- **Then**: _Then the indicated result is expected._

...

### Structure

#### Basic

...

#### Advanced

...

## Tools

- [xUnit.Net](https://github.com/xunit/xunit) - unit testing framework
- [NSubstitute](https://github.com/nsubstitute/NSubstitute) - mocking framework

## Resources

- [Unit testing best practices with .NET Core and .NET Standard](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
- [Book - The Art of Unit Testing](https://www.manning.com/books/the-art-of-unit-testing-second-edition)

---
