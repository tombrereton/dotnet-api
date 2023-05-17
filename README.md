# Introduction

Appointer is a simple appointment scheduling api that allows users to create, edit,
and delete appointments to a calendar.
Appointer is built with .NET 6.

# Getting Started

## Prerequisites

- .NET 6 SDK
- Docker for Mac/Windows/Linux

## Note

- The application does not run yet! This is the starting point for the exercise
- An example test is included to show how to write tests using EF Core & Testcontainers

## The Exercise

We will build a basic online appointment api. The api will allow users to create an account
with an associated calendar. Then users can create, modify & view appointments in that
calendar. We will ignore authentication in this exercise.

- Using TDD, we will build out the functionality of the application
- The functionality is defined by the requirements in the `Requirements` section

## Running the Tests

- Run `dotnet test` from the root of the project

# Requirements
The requirements are written in the Given When Then format. This is a common format used
for acceptance criteria & tests. The requirements are written in a way that is not specific to any 
particular technology. This allows us to write the tests first and then implement the
functionality to make the tests pass.

Writing Acceptance Tests will be out of the scope of this exercise. However, the requirements
can still guide our Integration & Unit Tests.

```
Given there is no account for user A
When user A creates an account
Then the account is persisted
And a calendar is created
```

```
Given there is an account for user A
When user A deletes their account
Then the account is not active
And the calendar is not active
```

```
Given there is an account for user A
And there is a calendar for user A
When a user creates an appointment in the calendar for user A
Then the appointment can be viewed
```

```
Given there is an account for user A
And there is a calendar for user A
When a user creates an appointment in the calendar for user A
Then the appointment can be viewed
```

```
Given there is an account for user A
And there is a calendar for user A
And there is an appointment Z
When a user updates appointment Z
Then the updated appointment is persisted
```

```
Given there is an account for user A
And there is a calendar for user A
And there is an appointment Z
When a user deletes appointment Z
Then appointment Z is not active
```

