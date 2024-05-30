namespace Domain.Accounts;

public record UserAccount(Guid Id, string FullName, bool IsActive = true, bool IsDeleted = false);