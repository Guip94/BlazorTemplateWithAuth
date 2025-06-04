using BlazorApp2.AuthSystems.Services.Interfaces;
using BlazorApp2.UI.Components.UserData.Models;
using Domain.Repositories.UserAPI;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using LocalNuggetTools.ResultPattern;
using Entities.Queries.UserAPIQueries.UserQueries;
using Entities.Entities.UserAPI;
using System.Security.Claims;
using MudBlazor.Extensions;
using Entities.Commands.UserAPICommands.UserCommands;
using System.Linq.Expressions;

namespace BlazorApp2.UI.Components.UserData.UserInfos
{
    public partial class UpdateUserComponent
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


        private MudForm _updateUserForm { get; set; } = new MudForm();



        [Parameter]
        public UserDTO UserDTO_ { get; set; }



        [Parameter]
        public string FieldName { get; set; }


        [Parameter]
        public EventCallback OnCancelOrUpdateEventTriggered { get; set; }


        private string _editing;




       

        protected override async Task OnInitializedAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();


            if (authState.User.Identity!.IsAuthenticated)
            {

                await _authorizingService.GetCurrentAuthorization();



            }



        }


        private async Task OnSubmit(Expression<Func<UserDTO, object>> propertySelector)
        {

            // Get the value of UserDTO_ property through Reflection
            // using  Expression<Func<UserDTO_, object>>
            var member = (propertySelector.Body as MemberExpression ?? ((UnaryExpression)propertySelector.Body).Operand as MemberExpression);

            // Get the property name from the MemberExpression
            string propertyName = member?.Member.Name ?? throw new ArgumentException("Invalid property selector");


            try
            {
                // keep verifying the user is authenticated
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

                if (authState.User.Identity!.IsAuthenticated)
                {
                    // need authorization from current token to update user profile
                    await _authorizingService.GetCurrentAuthorization();


                    int userId = int.Parse(authState.User.Claims.FirstOrDefault(c => c.Type is ClaimTypes.NameIdentifier)!.Value);


                    // Get the value of the property
                    var listOfTypes = UserDTO_.GetType().GetProperties().ToList();


                    foreach (var type in listOfTypes)
                    {


                        CommandResult rslt = type.Name switch
                        {
                            nameof(UserDTO.Firstname) => await _userRepository.ExecuteAsync(new UpdateUserFirstnameCommand(userId, propertySelector.ToString())),
                            nameof(UserDTO.Lastname) => await _userRepository.ExecuteAsync(new UpdateUserLastnameCommand(userId, propertySelector.ToString())),
                            _ => CommandResult.Failure("Invalid property name")


                        };

                        if (!rslt.IsSuccess)
                        {
                            _snackbar.Add($"An error occrued while updating : {rslt.ErrorMessage}");
                        }
                        else
                        {

                            _snackbar.Add($"{propertyName} updated with success");
                            StateHasChanged();
                            await OnCancelOrUpdateEventTriggered.InvokeAsync(true);
                        }


                    }

                    _editing = string.Empty;

                }




            }
            catch (Exception ex)
            {
                _snackbar.Add($"An error occurred while updating the user: {ex.Message}", Severity.Error);

            }
        }

        private async Task OnCancel()
        {

            _editing = string.Empty;
            await OnCancelOrUpdateEventTriggered.InvokeAsync(true);
        }



        protected override void OnParametersSet()
        {
            _editing = FieldName switch
            {
                "Firstname" => nameof(UserDTO_.Firstname),
                "Lastname" => nameof(UserDTO_.Lastname),
                _ => ""
            };

        

        }


    }
}
