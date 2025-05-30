using Domain.Repositories.UserAPI;
using Entities.Commands.UserAPICommands.AdressCommands;
using Entities.Entities.UserAPI;
using Entities.Queries.UserAPIQueries.AdressQueries;
using LocalNuggetTools.ResultPattern;
using System.Net.Http.Json;

namespace Domain.Services.UserAPI
{
    public class AdressService : IAdressRepository
    {

        private readonly HttpClient _httpClient;

        public AdressService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("UserAPI");   
        }

        public async Task<CommandResult> ExecuteAsync(AddAdressCommand command)
        {
            try
            {
                JsonContent jsonContent = JsonContent.Create(command);

                using (HttpResponseMessage response = await _httpClient.PostAsync("/api/user/Adress", jsonContent))
                {

                    if (!response.IsSuccessStatusCode)
                    {
                        return CommandResult.Failure($"Error: {response.StatusCode} - {response.ReasonPhrase}");


                    }

                    return CommandResult.Success();
                }

            }
            catch (Exception ex)
            {
                return CommandResult.Failure(ex.Message);
            }
        }

        public async Task<QueryResult<Adress>> ExecuteAsync(GetAdressQuery query)
        {
            try
            {

                using (HttpResponseMessage response = await _httpClient.GetAsync($"api/user/Adress/{query.UserId}"))
                {

                    if ( !response.IsSuccessStatusCode)
                    {
                        return QueryResult<Adress>.Failure($"Error code : {response.StatusCode} -  {response.ReasonPhrase}");

                    }

                    Adress? adress = await response.Content.ReadFromJsonAsync<Adress>();

                    if (adress is null)
                    {
                        return QueryResult<Adress>.Failure($"No object found, Id doesn't exist");
                    }



                    return QueryResult<Adress>.Success(adress);


                }

            }
            catch (Exception ex)
            {
                return QueryResult<Adress>.Failure(ex.Message);
            }
        }

        public async Task<CommandResult> ExecuteAsync(UpdateAdressCommand command)
        {

            try
            {
                JsonContent jsonContent = JsonContent.Create(command);

                using (HttpResponseMessage response = await _httpClient.PutAsync("/api/user/Adress", jsonContent))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return CommandResult.Failure($"Error code : {response.StatusCode} - {response.ReasonPhrase}");

                    }
                    return CommandResult.Success();
                }
            }

            catch (Exception ex)
            {
                return CommandResult.Failure(ex.Message);
            }

        }
    }
}

