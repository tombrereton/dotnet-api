> Under Development. The Features section lists completed work

# Introduction

This is a simple appointment booking api to demonstrate preferred patterns and architecture.

Users create an account with a default calendar and can add, remove, update or delete events to this calendar.
Users can only have 1 account but can have multiple calendars.

## Motivation
I want to have a starter template for building Dotnet Apis. Things that are important:
- Excellent developer experience
- Zero config setup for running integration tests locally against a datastore
- Zero config setup for running locally with a datastore and message broker
- Established application architecture e.g. Vertical Slices or Clean Architecture
- Contains examples for common problems e.g. Validation, Logging, Authentication, Data Access

## Features
- [x] Zero config integration testing against the database
- [ ] Zero config integration testing with events
- [ ] Zero config integration test with authentication
- [x] Architecture tests to enforce intended architecture i.e. Domain code cannot use code from Infrastructure
- [x] Vertical Slice Architecture
- [ ] Validation
- [ ] Use Repositorities to enforce write data access through Aggregate Roots
- [ ] Use Dapper or EF Core Raw SQL for read data access
- [ ] Logging
- [ ] Error Handling

## Technologies
- Testcontainers
- xUnit
- EF Core for writing to the datastore
- Dapper for reading from the datastore
- FluentValidation
- Serilog

## Patterns
- Vertical Slice
- CQRS
- REPR (Request Endpoint Response)
- Repository Pattern
- Feature Folders

## Architectural Decision Records
- Have a thin endpoint/controller and move the logic into a handler/service class so it's (1) unit testable and (2) decoupled from the endpoint framework to ease with refactoring e.g. moving the handlers to a `Core` or `Application` project and changing the executable project to function app
- (Exploring) Use [Result](https://www.milanjovanovic.tech/blog/functional-error-handling-in-dotnet-with-the-result-pattern) objects for control flow instead of exceptions. In common code paths like validation or business rules use Result.Failure() or Result.Success(); use Exceptions for situations like failed connection, out of memory, access array incorrectly
- Perform all validation you can in with Input Validation, i.e. the FluentValidator. For validation on business rules i.e. if x is 3, then y can only be 5, or no duplicate calendar names, perform this in the Domain Entity. In other words, if you rely on data from the database to perform the validation it's a business rule and should be validated within the aggregate. Being an Aggregate means all its information is loaded in memory to perform these checks without reaching into the database again.
- Use Mediatr with CQS. This is more in line with SOLID and Open Closed princinples; where each handler is responsible for 1 use case e.g. GetAccount or CreateAccount. This is in contrast to Service class which would be responible for both of these use cases, as more use cases are added the service adopts too much responsibility, becomes less cohesive, and consequently becomes hard to maintain or modify.

## Prerequisites

- .NET 8 SDK
- The LATEST Docker
  for [Mac](https://docs.docker.com/desktop/install/mac-install/)/[Windows](https://docs.docker.com/desktop/install/windows-install/)

On Mac enable Rosetta in the beta features as shown in the image below:

![Enable Rosetta](./imgs/dockerForMac.png)

## Running the Tests

- Run `dotnet test` from the root of the project



