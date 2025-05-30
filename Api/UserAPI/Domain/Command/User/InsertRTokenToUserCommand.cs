using LocalNuggetTools.Commands;

namespace Domain.Command.User
{
    public class InsertRTokenToUserCommand : ICommandDefinition
    {
        public InsertRTokenToUserCommand(int id, string token)
        {
            Id = id;
            refreshToken = token;
        }

        public int Id { get; }
        public string refreshToken { get; }
    }
}
