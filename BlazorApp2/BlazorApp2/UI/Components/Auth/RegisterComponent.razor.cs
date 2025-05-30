using BlazorApp2.AuthSystems.Models;
using BlazorApp2.AuthSystems.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp2.UI.Components.Auth
{
    public partial class RegisterComponent
    {

        [Inject]
        AuthenticationStateProvider _authSatetProvider {  get; set; }

        [Inject]
        IJSRuntime _jsRuntime { get; set; }
        [Inject]
        private ISnackbar _snackbar {  get; set; }

        [Inject]
        private IAuthService _authService { get; set; }

        [Inject]

        private NavigationManager _navigation {  get; set; }


        private MudForm registerForm;

        private RegisterModel registerModel = new RegisterModel("", "", "", "");
            
        
        private bool registerSuccess;
        private bool isLoading = false;

        [DataType(DataType.Password)]
        [Compare(nameof(registerModel.Password))]
        private string confirmPassword { get; set; }


        protected async override Task OnInitializedAsync()
        {
            var user = (await _authSatetProvider.GetAuthenticationStateAsync()).User;

            string lastname = user.Identity.Name;
            registerModel.Lastname = lastname;

            string? mail = user.Claims.FirstOrDefault(c => c.Type =="Mail")?.Value;
            registerModel.Mail = mail;

            string? firstname = user.Claims.FirstOrDefault(c => c.Type == "Firstname")?.Value;

            StateHasChanged();
        }

        private async Task SubmitRegister()
        {
            await registerForm.Validate();
            if (registerForm.IsValid)
            {

                try
                {
                    isLoading = true;
                    AuthResult Authrslt = await _authService.Register(registerModel);

                    _snackbar.Add("Register succeeded", Severity.Success);
                    isLoading = false;
                    _navigation.NavigateTo("/");
                }
                catch (Exception ex)
                {
                    _snackbar.Add("Error while registering");
                    isLoading=false;
                }


            }
        }








    }
}
