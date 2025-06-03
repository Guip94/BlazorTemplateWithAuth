using LocalNuggetTools.Commands;

public class UpdateUserFirstname : ICommandDefinition
{
    public UpdateUserFirstname(int id, string firstname)
    {
        Id = id;
        Firstname = firstname;
    }

    public int Id { get; }
    public string Firstname { get; }


}