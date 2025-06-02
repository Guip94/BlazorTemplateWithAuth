using BlazorApp2.AuthSystems.Services.Interfaces;
using BlazorApp2.UI.Components.UserData.Models;
using Domain.Repositories.UserAPI;
using Entities.Commands.UserAPICommands.AdressCommands;
using LocalNuggetTools.ResultPattern;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;

namespace BlazorApp2.UI.Components.UserData
{
    public partial class AddAdressComponent
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


        private MudForm _registerAdressForm { get; set; }

        private AdressDTO _adress = new();




        protected override async Task OnInitializedAsync()
        {
            await _authorizingService.GetCurrentAuthorization();

            var isAuthenticateduser = await _authenticationStateProvider.GetAuthenticationStateAsync();

            if (isAuthenticateduser.User.Identity.IsAuthenticated)
            {





            }

        }

            private async Task HandleValidSubmit()
        {
          

            await _registerAdressForm.Validate();

            if (_registerAdressForm.IsValid)
            {

                try
                {

                    var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();

                    if (authenticationState.User.Identity.IsAuthenticated)
                    {
                        await _authorizingService.GetCurrentAuthorization();

                        int userId = int.Parse(authenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

                        try
                        {
                            var addAdressRslt = await _adressRepository.ExecuteAsync(new AddAdressCommand(_adress.Country, _adress.Zipcode, _adress.City, _adress.Street, userId));

                            if (!(addAdressRslt.IsSuccess))
                            {
                                CommandResult.Failure($"Error: {addAdressRslt.ErrorMessage}");


                            }
                            else
                            {
                                _snackbar.Add("Address added successfully");
                            }
                        }
                        catch (Exception ex)
                        {
                            CommandResult.Failure($"Error: {ex.Message}");
                        }


                    }







                }
                catch (Exception ex)
                {
                    _snackbar.Add($"Error while adding address: {ex.Message}", Severity.Error);
                }
            }


        }

        private void Cancel()
        {
            _navigationManager.NavigateTo("/profile");
            _snackbar.Add("No address added");
        }
    }
}
