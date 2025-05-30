using BlazorApp2.AuthSystems.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace BlazorApp2.Layout
{
    public partial class NavMenu
    {

        [Inject]
        private IAuthService _authService {  get; set; }
        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; }
        [Inject]
        private ISnackbar _snackbar { get; set; }

        [Inject]
        private NavigationManager _navigation {  get; set; }




        private async Task Logout()
        {
            try
            {
                    var state = await _authenticationStateProvider.GetAuthenticationStateAsync();

                if (state.User.Identity.IsAuthenticated)
                {
             
                        await _authService.Logout();


                        _snackbar.Add("Log out succesfull");
                        _navigation.NavigateTo("/");

                    }
            

            }
            catch (Exception ex)
            {
                _snackbar.Add($"Error during logout : {ex.Message}", Severity.Error);
                throw;
            }
        }





    }
}
