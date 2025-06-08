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
using System.Linq.Expressions;
using Entities.Commands.UserAPICommands.AdressCommands.UpdateUserNamesCommand;

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



        [Parameter]
        public UserDTO UserDTO_ { get; set; }



        [Parameter]
        public string FieldName { get; set; }


        [Parameter]
        public EventCallback OnCancelOrUpdateEventTriggered { get; set; }


        private string _editing;



        private MudForm _updateUserForm { get; set; } = new MudForm();





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



            // keep verifying the user is authenticated
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();


            if (authState.User.Identity!.IsAuthenticated)
            {
                // need authorization from current token to update user profile
                await _authorizingService.GetCurrentAuthorization();

                await _updateUserForm.Validate();



                if (_updateUserForm.IsValid)
                {
                    try
                    {
                        int userId = int.Parse(authState.User.Claims.FirstOrDefault(c => c.Type is ClaimTypes.NameIdentifier)!.Value);

                        // Get the value of the property
                        var listOfTypes = UserDTO_.GetType().GetProperties().ToList();

                        foreach (var type in listOfTypes)
                        {

                            //lastname
                            if (type.Name == propertyName)
                            {

                                CommandResult rslt = await _userRepository.ExecuteAsync(new UpdateUserNamesCommand(userId, UserDTO_.Lastname, UserDTO_.Firstname));

                                if (!rslt.IsSuccess) { CommandResult.Failure($"An error occurred while updating the user: {rslt.ErrorMessage}"); }

                                CommandResult.Success();
                                _snackbar.Add($"{propertyName} updated with success");
                                StateHasChanged();
                                await OnCancelOrUpdateEventTriggered.InvokeAsync(true);

                            }

                            //firstname
                            if (type.Name == propertyName)
                            {
                                CommandResult rslt = await _userRepository.ExecuteAsync(new UpdateUserNamesCommand(userId, UserDTO_.Lastname,UserDTO_.Firstname));
                                if (!rslt.IsSuccess) { CommandResult.Failure($"An error occurred while updating the user: {rslt.ErrorMessage}"); }
                                CommandResult.Success();
                                _snackbar.Add($"{propertyName} updated with success");
                                StateHasChanged();
                                await OnCancelOrUpdateEventTriggered.InvokeAsync(true);
                            }


                        }

                        _editing = string.Empty;

                    }



                    catch (Exception ex)
                    {
                        _snackbar.Add($"An error occurred while updating the user: {ex.Message}", Severity.Error);
                        _editing = string.Empty;

                    }

                }

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
