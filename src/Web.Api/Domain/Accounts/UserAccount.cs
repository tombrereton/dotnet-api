using Web.Api.Common;

namespace Web.Api.Domain.Accounts;

public sealed class UserAccount : Entity
{
    private UserAccount(Guid id, string fullName, bool isActive, bool isDeleted)
    {
        Id = id;
        FullName = fullName;
        IsActive = isActive;
        IsDeleted = isDeleted;
    }

    private UserAccount()
    {
    }

    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    public static UserAccount Create(string fullName)
    {
        var userAccount = new UserAccount(Guid.NewGuid(), fullName, true, false);

        userAccount.RaiseDomainEvent(new UserAccountCreatedDomainEvent(userAccount.Id));

        return userAccount;
    }
}
