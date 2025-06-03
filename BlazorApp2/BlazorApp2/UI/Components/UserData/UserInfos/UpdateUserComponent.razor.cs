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

        [Parameter]
        public UserDTO _user { get; set; }




        private bool _renderUpdateUserLastnameField { get; set; } = false;


       
        private bool _renderUpdateUserFirstnameField { get; set; } = false;


      
        private bool _renderUpdateUserMailField { get; set; } = false;


        private MudForm _updateUserForm { get; set; } = new MudForm();




        [Parameter]
        public EventCallback<bool> OnCancelOrUpdateEventTriggered { get; set; }




        [Parameter]
        public UserDTO UserDTO_ { get; set; }


        private UserDTO _userDTO;

        protected override async Task OnInitializedAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();


            if (authState.User.Identity!.IsAuthenticated)
            {

                await _authorizingService.GetCurrentAuthorization();

                _userDTO = UserDTO_;




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

                  
                        var listOfTypes = UserDTO_.GetType().GetProperties()
                            .Where(p => p.GetValue(UserDTO_) is not null)
                            .Select(p => p.PropertyType)
                            .ToList();

        

                    foreach (var type in listOfTypes)
                        {
                        if (propertyName == type.Name)
                            {
                                
                                CommandResult rslt = await _userRepository.ExecuteAsync(new UpdateUserFirstnameCommand(userId, propertySelector.ToString()));
                                if (!rslt.IsSuccess) { CommandResult.Failure($"Error: {rslt.ErrorMessage}"); }
                                else
                                {
                                    _snackbar.Add("Firstname updated successfully", Severity.Success);
                                    _renderUpdateUserFirstnameField = false;
                                    await InvokeAsync(StateHasChanged);
                                    await OnCancelOrUpdateEventTriggered.InvokeAsync(true);
                                }


                            }
                            if (propertyName == type.Name)
                            {
                                CommandResult rslt = await _userRepository.ExecuteAsync(new UpdateUserLastnameCommand(userId, propertySelector.ToString()));
                                if (!rslt.IsSuccess) { CommandResult.Failure($"Error: {rslt.ErrorMessage}"); }
                                else
                                {
                                    _snackbar.Add("Lastname updated successfully", Severity.Success);
                                    _renderUpdateUserLastnameField = false;
                                    await InvokeAsync(StateHasChanged);
                                    await OnCancelOrUpdateEventTriggered.InvokeAsync(true);
                            }
                            }

                        }

                    }
                  



            }
            catch (Exception ex)
            {
                _snackbar.Add($"An error occurred while updating the user: {ex.Message}", Severity.Error);

            }
        }

        private async Task OnCancel() 
        {
            _renderUpdateUserLastnameField = false;
            _renderUpdateUserFirstnameField = false;
            _renderUpdateUserMailField = false;

            await OnCancelOrUpdateEventTriggered.InvokeAsync(true);
        }







    }
}
