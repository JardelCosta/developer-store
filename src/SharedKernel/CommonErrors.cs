namespace SharedKernel;

public static class CommonErrors
{
    public static Error AlreadyDisabled(EntityType entity, string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException("Id cannot be null or empty.", nameof(id));
        }
        string entityName = entity.ToString();
        return Error.Problem(
            $"{entityName}.AlreadyDisabled",
            $"The {entityName} item with Id = '{id}' is already disabled.");
    }

    public static Error NotFound(EntityType entity, string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException("Id cannot be null or empty.", nameof(id));
        }
        string entityName = entity.ToString();
        return Error.NotFound(
            $"{entityName}.NotFound",
            $"The {entityName} with Id = '{id}' was not found.");
    }
}
