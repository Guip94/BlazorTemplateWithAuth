using BlazorApp2.AuthSystems.Services.Interfaces;
using BlazorApp2.UI.Components.UserData.Models;
using Domain.Repositories.UserAPI;
using Entities.Entities.UserAPI;
using Entities.Queries.UserAPIQueries.UserQueries;
using LocalNuggetTools.ResultPattern;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;

namespace BlazorApp2.UI.Components.UserData.UserInfos
{
    public partial class UserComponent
    {

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; }

        [Inject]

        private IAuthorizingService _authorizingService { get; set; }

        [Inject]
        private ISnackbar _snackbar { get; set; }

        [Inject]
        private IUserRepository _userRepository { get; set; }

        private UserDTO _user { get; set; }







        // bool variable to control the rendering of the UpdateUserComponent


        /// <summary>
        /// var to render the lastname update component
        /// </summary>
        private bool _renderUpdateUserLastnameComponent { get; set; } = false;


        /// <summary>
        /// var to render the firstname update component
        /// </summary>
        private bool _renderUpdateUserFirstnameComponent { get; set; } = false;


        /// <summary>
        /// var to render the mail update component
        /// </summary>
        private bool _renderUpdateUserMailComponent { get; set; } = false;


        protected override async Task OnInitializedAsync()
        {

            try
            {
                //Verify if the user is authenticated

                AuthenticationState authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

                if (authState.User.Identity.IsAuthenticated)
                {
                    // Verify if the user is authorized to access the user data
                    await _authorizingService.GetCurrentAuthorization();

                    // Get the user ID from the claims
                    int userId = int.Parse(authState.User.Claims.FirstOrDefault(c => c.Type is ClaimTypes.NameIdentifier)!.Value);

                    // Fetch the user data from the repository

                    try
                    {
                        QueryResult<User> result = await _userRepository.ExecuteAsync(new GetUserQuery(userId));

                        if (result.IsSuccess)
                        {
                            _user = new UserDTO
                            {
                                FirstName = result.Result.Firstname,
                                LastName = result.Result.Lastname,
                                Mail = result.Result.Mail

                            };
                        }

                    }
                    catch (Exception ex)
                    {
                        QueryResult<User> result = QueryResult<User>.Failure($"An error occurred while fetching user data: {ex.Message}");

                    }
                }





            }
            catch (Exception ex)
            {
                _snackbar.Add($"An error occurred while initializing the user component: {ex.Message}", Severity.Error);
            }

        }



        private void RenderUpdateUserFirstnameComponent()
        {
            _renderUpdateUserFirstnameComponent = !_renderUpdateUserFirstnameComponent;
            StateHasChanged();
        }


        private void RenderUpdateUserLastnameComponent()
        {
            _renderUpdateUserLastnameComponent = !_renderUpdateUserLastnameComponent;
            StateHasChanged();
        }


        private void RenderUpdateUserMailComponent()
        {
            _renderUpdateUserMailComponent = !_renderUpdateUserMailComponent;
            StateHasChanged();
        }
    }
}
