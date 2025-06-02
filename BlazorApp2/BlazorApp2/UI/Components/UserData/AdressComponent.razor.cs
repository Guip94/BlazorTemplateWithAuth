using BlazorApp2.AuthSystems.Services.Interfaces;
using BlazorApp2.UI.Components.UserData.Models;
using Domain.Repositories.UserAPI;
using Entities.Entities.UserAPI;
using Entities.Queries.UserAPIQueries.AdressQueries;
using LocalNuggetTools.ResultPattern;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;

namespace BlazorApp2.UI.Components.UserData
{
    public partial class AdressComponent
    {

        [Inject]
        private IAuthorizingService _AuthorizingService { get; set; }


        [Inject]
        private ISnackbar _Snackbar { get; set; }

        [Inject]
        private IAdressRepository _AdressRepository { get; set; }

        [Inject]
        private AuthenticationStateProvider _AuthenticationStateProvider { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Parameter]
        public AdressDTO Adress { get; set; }



        private bool renderAddAddressComponent = false;

        protected override async Task OnInitializedAsync()
        {

            var authState = await _AuthenticationStateProvider.GetAuthenticationStateAsync();

            if (authState.User.Identity.IsAuthenticated)
            {
                await _AuthorizingService.GetCurrentAuthorization();
                    
                int userId = int.Parse(authState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
                QueryResult<Adress> result = await _AdressRepository.ExecuteAsync(new GetAdressQuery(userId));

                if (result.IsSuccess)
                {

                    Adress = new AdressDTO
                    {
                        Country = result.Result.Country,
                        City = result.Result.City,
                        Zipcode = result.Result.Zipcode,
                        Street = result.Result.Street,
                    };

                }

            }
        }


        private void RenderAddAddressComponent()
        {

            renderAddAddressComponent = !renderAddAddressComponent;
            StateHasChanged();
        }

    }
}
