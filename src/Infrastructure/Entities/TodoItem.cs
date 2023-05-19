namespace Appointer.Infrastructure.Entities;

public record TodoItem(Guid Id, string Title, string Description, bool Done);