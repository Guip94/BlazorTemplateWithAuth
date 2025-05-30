using BlazorApp2.AuthSystems.Models;
using BlazorApp2.AuthSystems.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace BlazorApp2.UI.Components.Auth
{
    public partial class LoginComponent
    {

        private MudForm loginForm;
        private bool _loginSuccess;
        private bool isLoading = false;
        private LoginModel loginModel = new();

        [Inject]
        ISnackbar _snackBar {  get; set; }

        [Inject]
        IAuthService authService { get; set; }

        [Inject]
        NavigationManager _navigation {  get; set; }

        [Inject]
        IJSRuntime _jsRuntime { get; set; }



        [Parameter]
        [SupplyParameterFromQuery(Name = "returnUrl")]
        public string ReturnUrl { get; set; }




        private async Task SubmitLogin()
        {
            try
            {
                isLoading = true;
                await loginForm.Validate();
                if (_loginSuccess)
                {
                    try
                    {
                        
                        AuthResult authRslt = await authService.Login(loginModel);

                        if (!authRslt.Success)
                        {
                            _snackBar.Add("Error when trying to login", Severity.Error);
                        }
                        else
                        {
                            StateHasChanged();

                            _navigation.NavigateTo("/");
                            _snackBar.Add("Connexion réussie");

                        }
                    }

                    catch (Exception ex)
                    {
                        _snackBar.Add("Une erreur est survenue lors de la connexion", Severity.Error);
                    }

                    finally
                    {
                        isLoading = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _snackBar.Add("Une erreur est survenue lors de la connexion", Severity.Error);
                isLoading = false;
            }

        }


    }
}
