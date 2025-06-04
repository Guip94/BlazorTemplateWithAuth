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


        private string? _editing { get; set; }

      



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
                                Firstname = result.Result.Firstname,
                                Lastname = result.Result.Lastname,
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




        private void Editing(string propertyName)
        {
            _editing = propertyName;
        }


        private void OnEventDone()
        {

            _editing = string.Empty;
        }
        RenderFragment RenderFragmentField(string field) => builder =>
        {
            builder.OpenComponent(0, typeof(UpdateUserComponent));
            builder.AddAttribute(1, "FieldName", field);
            builder.AddAttribute(2, "UserDTO_", _user);
            builder.AddAttribute(3, "OnCancelOrUpdateEventTriggered", EventCallback.Factory.Create(this, OnEventDone));
            builder.CloseComponent();
        };
    }
}
