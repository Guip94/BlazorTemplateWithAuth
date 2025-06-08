using Domain.Repositories.UserAPI;
using Entities.Commands.UserAPICommands.AdressCommands.UpdateUserNamesCommand;
using Entities.Entities.UserAPI;
using Entities.Queries.UserAPIQueries.UserQueries;
using LocalNuggetTools.ResultPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.UserAPI
{
    public class UserService : IUserRepository
    {

        private readonly HttpClient _httpClient;

        public UserService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("UserAPI");
        }

        public async Task<CommandResult> ExecuteAsync(UpdateUserNamesCommand command)
        {
            try
            {
                JsonContent content = JsonContent.Create(command);

                using (HttpResponseMessage response = await _httpClient.PutAsync($"/api/User/updateusernames/{command.Id}", content))
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

   
        public async Task<QueryResult<User>> ExecuteAsync(GetUserQuery query)
        {
            try
            {
                using (HttpResponseMessage response = await _httpClient.GetAsync($"api/User/userinfos/{query.Id}"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return QueryResult<User>.Failure($"Error code : {response.StatusCode} -  {response.ReasonPhrase}");
                    }

                    User? user = await response.Content.ReadFromJsonAsync<User>();
                    if (user is null)
                    {
                        return QueryResult<User>.Failure($"No object found, Id doesn't exist");
                    }


                    return QueryResult<User>.Success(user);



                }
            }
            catch (Exception ex)
            {
                return QueryResult<User>.Failure(ex.Message);
            }
        }
    }
}
