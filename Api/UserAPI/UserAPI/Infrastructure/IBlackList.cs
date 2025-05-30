namespace UserAPI.Infrastructure
{
    public interface IBlackList
    {
        void AddToken(string token);
        bool IsTokenBlacklisted(string token);
    }
}
