namespace UserAPI.Infrastructure
{
    public class BlackListService : IBlackList
    {

        private readonly Dictionary<string, DateTime> _blackListedTokens = new Dictionary<string, DateTime>();


        public void AddToken(string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {

                // Cela permet d'ajouter une durée de vie d'invalidité au token avant qu'il ne puisse être réutilisé
                _blackListedTokens.Add(token, DateTime.UtcNow.AddMinutes(15));
            }
        }

        public bool IsTokenBlacklisted(string token)
        {

            if (_blackListedTokens.TryGetValue(token, out DateTime expire) && expire > DateTime.UtcNow) 
            { 
                
                return true;
            }


             _blackListedTokens.Remove(token);
            return false;
        }
    }
}
