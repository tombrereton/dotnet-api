using MassTransit;
using Microsoft.EntityFrameworkCore;
using Teeitup.Core.Contracts;
using Teeitup.Core.Domain.Accounts;
using Teeitup.Core.Domain.Calendars;
using Teeitup.Core.Infrastructure.Database;

// ReSharper disable ClassNeverInstantiated.Global

namespace Worker.Consumers;

public class UserAccountCreatedConsumer(ILogger<UserAccountCreatedConsumer> logger, TeeitupDbContext dbContext)
    : IConsumer<UserAccountCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserAccountCreatedIntegrationEvent> context)
    {
        logger.LogInformation("Received user account created: {Message}", context.Message.FullName);
        
        var userAccount = await dbContext
            .UserAccounts
            .Include(x => x.Calendars)
            .FirstOrDefaultAsync(x => x.Id == context.Message.UserAccountId, context.CancellationToken);
        
        ArgumentNullException.ThrowIfNull(userAccount, nameof(userAccount));
        
        var calendar = Calendar.Create("default");
        userAccount.AddCalendar(calendar);

        dbContext.UserAccounts.Update(userAccount);
        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}