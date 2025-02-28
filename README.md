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

- [x] [Zero config integration testing](/tests/Web.Api.IntegrationTests/Helpers/AppointerWebApplicationFactory.cs) against the database
- [ ] Zero config integration testing with events
- [ ] Zero config integration test with authentication
- [x] Architecture tests to enforce intended architecture i.e. [Domain layer cannot depend on Infrastructure or Feature](tests/Web.Api.ArchitectureTests/DomainTests.cs)
- [x] [Vertical Slice Architecture](src/Web.Api/Features/UserAccounts/CreateAccount.cs)
- [ ] Validation
- [x] Use Repositories to enforce write [data access through Aggregate Roots](src/Web.Api/Infrastructure/Repositories/Repository.cs)
- [ ] Use Dapper or EF Core Raw SQL for read data access
- [ ] Logging
- [ ] Error Handling
- [ ] Semantic Versions
- [ ] Client generation from OpenApi spec
- [ ] Infra code (Terraform) for Azure
- [ ] Build Pipeline which also runs Unit and Integration Tests
- [ ] Release pipeline which generates Clients and Deploys Api and Background Worker

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

- _November 2024_: Have a thin endpoint/controller and move the logic into a handler/service class so it's (1) unit testable and (2) decoupled from the endpoint framework to ease with refactoring e.g. moving the handlers to a `Core` or `Application` project and changing the executable project to function app
- _November 2024 (Exploring)_: Use [Result](https://www.milanjovanovic.tech/blog/functional-error-handling-in-dotnet-with-the-result-pattern) objects for control flow instead of exceptions. In common code paths like validation or business rules use Result.Failure() or Result.Success(); use Exceptions for situations like failed connection, out of memory, access array incorrectly
- _November 2024_: Perform all validation you can in with Input Validation, i.e. the FluentValidator. For validation on business rules i.e. no duplicate calendar names, perform this in the Domain Entity. In other words, if you rely on data from the database to perform the validation it's a business rule and should be validated within the aggregate. Being an Aggregate means all its information is loaded in memory to perform these checks without reaching into the database again.
- _November 2024_: Use Mediatr with CQS. This is more in line with SOLID and Open Closed princinples; where each handler is responsible for 1 use case e.g. GetAccount or CreateAccount. This is in contrast to Service class which would be responible for both of these use cases, as more use cases are added the service adopts too much responsibility, becomes less cohesive, and consequently becomes hard to maintain or modify.
- _February 2025 (Exploring)_: Use Discrimated Unions (instead of [Result](https://www.milanjovanovic.tech/blog/functional-error-handling-in-dotnet-with-the-result-pattern) objects) with the [Oneof](https://github.com/mcintyre321/OneOf) package for control flow instead of exceptions. In common code paths like validation or business rules use Oneof<CreateAccountResponse, InvalidUserAccount> and use Exceptions for situations like failed connection, out of memory, access array incorrectly

## Prerequisites

- .NET 8 SDK
- The LATEST Docker
  for [Mac](https://docs.docker.com/desktop/install/mac-install/)/[Windows](https://docs.docker.com/desktop/install/windows-install/)

On Mac enable Rosetta in the beta features as shown in the image below:

![Enable Rosetta](./imgs/dockerForMac.png)

## Running the Tests

- Run `dotnet test` from the root of the project
