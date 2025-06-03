using BlazorApp2.AuthSystems.Services.Interfaces;
using BlazorApp2.UI.Components.UserData.Models;
using Domain.Repositories.UserAPI;
using Entities.Commands.UserAPICommands.AdressCommands;
using Entities.Entities.UserAPI;
using Entities.Queries.UserAPIQueries.AdressQueries;
using LocalNuggetTools.ResultPattern;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;

namespace BlazorApp2.UI.Components.UserData.Address
{
    public partial class UpdateAddressComponent
    {

        [Inject]
        private IAuthorizingService _authorizingService { get; set; }

        [Inject]
        private ISnackbar _snackbar { get; set; }

        [Inject]
        private IAdressRepository _adressRepository { get; set; }

        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }



        [Parameter]
        public EventCallback<bool> OnCancelOrUpdateEventTriggered { get; set; }


        private AdressDTO _adress;

        private MudForm _updateAdressForm { get; set; } = new MudForm();
        protected override async Task OnInitializedAsync()
        {

            try
            {
                // get the authentication state of the user
                AuthenticationState authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

                if (authState.User.Identity!.IsAuthenticated)
                {
                    // Get current authorization to validate the process on the api end
                    await _authorizingService.GetCurrentAuthorization();

                    // Get UserId
                    int userId = int.Parse(authState.User.Claims.FirstOrDefault(c => c.Type is ClaimTypes.NameIdentifier)!.Value);

                    // fetch the address for the user

                    QueryResult<Adress> result = await _adressRepository.ExecuteAsync(new GetAdressQuery(userId));


                    _adress = new AdressDTO
                    {
                        Country = result.Result.Country,
                        City = result.Result.City,
                        Zipcode = result.Result.Zipcode,
                        Street = result.Result.Street
                    };
                }

            }
            catch (Exception ex)
            {
                _snackbar.Add($"An error occurred while initializing the component: {ex.Message}", Severity.Error);
                _navigationManager.NavigateTo("/profile");

            }
        }


        private async Task OnSubmit()
        {

            try
            {
                // Make sure the user is still authenticated
                AuthenticationState authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();

                if (authenticationState.User.Identity!.IsAuthenticated)
                {

                    await _authorizingService.GetCurrentAuthorization();

                    int userId = int.Parse(authenticationState.User.Claims.FirstOrDefault(c => c.Type is ClaimTypes.NameIdentifier)!.Value);

                    await _updateAdressForm.Validate();

                    if (_updateAdressForm.IsValid)
                    {

                        // Execute the update address command

                        try
                        {

                            CommandResult rslt = await _adressRepository.ExecuteAsync(new UpdateAdressCommand(userId, _adress.Country, _adress.Zipcode, _adress.City, _adress.Street));

                            if (!rslt.IsSuccess) { CommandResult.Failure("An error occured while updating the address"); }

                            CommandResult.Success();





                            //Trigger EventCallBack to notify the parent component

                            await OnCancelOrUpdateEventTriggered.InvokeAsync(true);


                        }
                        catch (Exception ex)
                        {

                            CommandResult.Failure(ex.Message, ex);

                        }

                    }


                }




            }
            catch (Exception ex)
            {

                _snackbar.Add($"An error occurred while updating the address: {ex.Message}", Severity.Error);
            }
        }

        private void OnCancel()
        {
            //Trigger the event callback to notify the parent component

            OnCancelOrUpdateEventTriggered.InvokeAsync(true);

            _navigationManager.NavigateTo("/profile");
        }
    }
}
