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

```
Given there is an account for user A
And there is a calendar for user A
And there is an appointment Z at 11am
When a user creates another appointment X at 11am
Then appointment X is also persisted
```

```
Given there is an account for user A
And there is a calendar for user A
And there is an appointment Z at 11am
And there is an appointment X at 11am
When a user gets all appointments
Then all appoints can be viewed
And the conflicting appointments can also be viewed
```
