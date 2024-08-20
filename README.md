# Bill's coding test for CAA

## How to run the API
1. Open the `CaaCodingChallenge/CaaCodingChallenge.sln` solution file in Visual Studio.
1. Create a user secrets file for the FlightsApi.  See below for the content of this file.
1. Make sure the FlightsApi is selected as the default startup project.
1. Press F5 to start the API.
1. Use Postman to test the API.  A Postman collection is included in the repository root.

### Secrets
Place something similar to the following in your secrets file:
```
{
  "Jwt": {
    "Key": "kjnhflPowjhFD44ignlofjgojfd879DHHWKikn7"
  }
}
```

## How to run the tests
The unit tests can be executed using the Visual Studio Test Explorer.

A Postman collection accompanies the solution.  It was my intention to implement a complete set of tests for the API,
but I discovered that there's a limit on the number of times one can run the tests using a free account. My intention is to now create another free account and complete the work.

## Notes about the solution

### Structure
The solution structure follows a Clean Architecture approach.  The outermost layer is the Web API.  Next comes the Use Cases layer, which I've called `UnitsOfWork`.  The innermost layer contains the entity model and handles persistence.

The Mediator pattern is used in this solution.  This pattern does a nice job of reducing the coupling between the API and the Use Cases.  It may be a bit overkill in the context of this exercise, but it's a pattern I like, and I wanted to illustrate how I have used it in the past.

### Logging
I took a small liberty with the logging.  The instructions said to use `ILogger`, which I have done, but I'm using Serilog's rather than Microsoft's.
