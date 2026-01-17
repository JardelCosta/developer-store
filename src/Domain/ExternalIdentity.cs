namespace Domain;

public class ExternalIdentity
{
    public Guid ExternalId { get; private set; }
    public string Description { get; private set; }

    protected ExternalIdentity() { }

    public ExternalIdentity(Guid externalId, string description)
    {
        ExternalId = externalId;
        Description = description;
    }
}
