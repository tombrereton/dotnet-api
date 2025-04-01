> Under Development. The Features section lists completed work

# Introduction

This is a simple appointment booking api to demonstrate preferred patterns and architecture.

Users create an account with a default calendar and can add, remove, update or delete events to this calendar.
Users can only have 1 account but can have multiple calendars.

## Motivation

I want to have a starter template for building enterprise grade Dotnet Apis. Things that are important:

- Excellent developer experience
- Zero config setup for running integration tests locally against a datastore
- Zero config setup for running locally with a datastore and message broker
- Established application architecture e.g. Vertical Slices or Clean Architecture
- Contains examples for common problems e.g. Validation, Logging, Authentication, Data Access

## Features or Patterns

- [x] [Zero config integration testing](/tests/Web.Api.IntegrationTests/Helpers/AppointerWebApplicationFactory.cs)
  against the database
- [ ] Zero config integration testing with events
- [ ] Zero config integration test with authentication
- [x] [Zero config when running Api locally](/src/AppHost/Program.cs) and depends on databases or a message broker
- [x] [Consistent connection strings](/src/AppHost/Program.cs#L7) when running locally so it's easier to use database
  clients e.g. SSMS
- [x] Architecture tests to enforce intended architecture
  e.g. [Domain layer cannot depend on Infrastructure or Features](/tests/Web.Api.ArchitectureTests/DomainTests.cs)
- [x] [Vertical Slice Architecture](/src/Web.Api/Features/UserAccounts/CreateAccount.cs)
- [x] Use [problem details](/src/Web.Api/Features/UserAccounts/CreateAccount.cs#L23) when returning bad responses
- [ ] Validation
- [x] Use Repositories to enforce
  write [data access through Aggregate Roots](/src/Web.Api/Infrastructure/Repositories/Repository.cs)
- [ ] Use Dapper or EF Core Raw SQL for read data access
- [ ] Logging
- [ ] Error Handling
- [ ] Semantic Versioning
- [ ] Client generation from OpenApi spec
- [ ] Infra code (Terraform) for Azure
- [ ] Build Pipeline which runs tests and builds the Api
- [ ] Release pipeline which generates clients libraries and deploys the Api and Background Worker
- [ ] Tracks performance metrics to detect performance regressions

## Technologies

- Testcontainers
- xUnit
- EF Core for writing to the datastore
- EF Core SQL for reading from datastore
- FluentValidation
- Serilog

## Patterns

- Vertical Slice
- CQRS
- REPR (Request Endpoint Response)
- Repository Pattern
- Feature Folders

## Architectural Decision Records

- _November 2024_: Use thin endpoints or controllers and move the business logic into a handler class, so the logic is
  decoupled from the endpoint framework. This makes testing the business logic less brittle and refactoring easier e.g.
  moving the handlers to a common project which can be used by both the Api and Background Worker.
- ~~_November 2024 (Exploring)_:
  Use [Result](https://www.milanjovanovic.tech/blog/functional-error-handling-in-dotnet-with-the-result-pattern) objects
  for control flow instead of exceptions. In common code paths like validation or business rules use Result.Failure() or
  Result.Success(); use Exceptions for situations like failed connection, out of memory, access array incorrectly~~
- _November 2024_: Separate validation concerns by handling input validation (syntax and structure) with
  FluentValidator, while implementing business rule validation (such as preventing duplicate calendar names) directly
  within your Domain Entity. The key distinction is that business rules requiring database information belong in the
  aggregate, which by definition has all relevant data already loaded in memory—allowing these validations to occur
  without additional database queries.
- _November 2024_: Use Mediatr with CQS. This is more in line with SOLID and Open Closed principles; where each handler
  is responsible for 1 use case e.g. GetAccount or CreateAccount. This is in contrast to a service class, which would be
  responsible for both of these use cases and, as more use cases are added, the service adopts too much responsibility,
  becomes less cohesive, and consequently becomes hard to maintain or modify. We also use Mediatr to prevent constructor
  explosion and implement cross-cutting concerns for both the Api and Background Worker. It's important to colocate
  Mediatr Handlers with their corresponding Request and Response objects.
- _November 2024_: For writing to the database, use the **Repository Pattern** instead of naked EF Core. We use
  Repositories for a number of reasons; we enforce data access through Aggregate Roots, we enable simple
  unit testing of Domain logic by testing against an interface, and we keep a clean Domain layer without any
  infrastructure code so when changing databases etc, only the repository code would change not the Domain code.  
- _February 2025 (Exploring)_: Use [Discriminated Unions](https://github.com/mcintyre321/OneOf) (previously explored
  [Result](https://www.milanjovanovic.tech/blog/functional-error-handling-in-dotnet-with-the-result-pattern)
  objects) for control flow instead of exceptions. In common code paths like validation or business rules use Oneof<
  CreateAccountResponse, InvalidUserAccount> and leave Exceptions for situations like failed connection, out of
  memory, or access array incorrectly. We use discriminated unions instead of the Result object because it's more
  expressive, handling the result is simpler, type safety for errors,
  and [Microsoft may add them to C#](https://github.com/dotnet/csharplang/blob/main/proposals/TypeUnions.md).


## Prerequisites

- .NET 8 SDK
- The LATEST Docker
  for [Mac](https://docs.docker.com/desktop/install/mac-install/)/[Windows](https://docs.docker.com/desktop/install/windows-install/)

On Mac enable Rosetta in the beta features as shown in the image below:

![Enable Rosetta](./imgs/dockerForMac.png)

## Running the Tests

- Run `dotnet test` from the root of the project
