# Bill's coding test for CAA

## How to run the API
1. Open the `CaaCodingChallenge/CaaCodingChallenge.sln` solution file in Visual Studio.
1. Create a database, as described below.
1. Create a user secrets file for the FlightsApi.  See below for the content of this file.
1. Make sure the FlightsApi is selected as the default startup project.
1. Press F5 to start the API.
1. Use Postman to test the API.  A Postman collection is included in the repository root.  See below for details.

### Creating the database
1. Open a Visual Studio developer PowerShell CLI.
1. `cd FlightsData`
1. `dotnet ef database update`

### Secrets
Place something similar to the following in your secrets file:
```
{
  "Jwt": {
    "Key": "kjnhflPowjhFD44ignlofjgojfd879DHHWKikn7"
  }
}
```

## How to run the unit tests
The unit tests can be executed using the Visual Studio Test Explorer.

## How to use the Postman collection
A Postman collection accompanies the solution.  It can be used either for manual, call-by-call testing, or to run a complete automated set of tests.  For either scenario, first follow these steps:
1. Run the API (as described above).
1. Make sure the localhost port of the API running in your browser matches the port in the Pre-request script of the Postman collections' Setup request.

If you are running requests manually:
1. Make sure a Postman environment is selected.

## Notes about the solution

### Security
To make testing easier, the solution includes an API that provides JWT tokens.  In reality, a different, more secure approach should be used.

### Structure
The solution structure follows a Clean Architecture approach.  The outermost layer is the Web API.  Next comes the Use Cases layer, which I've called `UnitsOfWork`.  The innermost layer contains the entity model and handles persistence.

The Mediator pattern is used in this solution.  This pattern does a nice job of reducing the coupling between the API and the Use Cases.  It may be a bit overkill in the context of this exercise, but it's a pattern I like, and I wanted to illustrate how I have used it in the past.

### Logging
I took a small liberty with the logging.  The instructions said to use `ILogger`, which I have done, but I'm using Serilog's rather than Microsoft's.  Log output can be seen in the console window which opens when the API is running.
