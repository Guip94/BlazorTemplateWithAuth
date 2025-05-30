using BlazorApp2.AuthSystems.Services;
using Domain.Repositories.UserAPI;
using Entities.Entities.UserAPI;
using Entities.Queries.UserAPIQueries.AdressQueries;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;

namespace BlazorApp2.UI.Components.UserData
{
    public partial class ShowAdressComponent
    {
        [Inject]
        private NavigationManager _navigation { get; set; }
        [Inject]
        private IAdressRepository _adressRepo { get; set; }

        [Inject]
        private ISnackbar _snackbar { get; set; }
        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; }

        [Inject]
        private AuthorizingService _authorizingService { get; set; }

        [Inject]
        ILogger<Adress> _logger { get; set; }

        [Parameter]
        public int Zipcode { get; set; }
        [Parameter]
        public string City { get; set; }
        [Parameter]
        public string Country { get; set; }
        [Parameter]
        public string Street { get; set; }


        [Parameter]
        public Adress UserAdress { get; set; }



        protected async override Task OnInitializedAsync()
        {
            
            try
            {

                // Verify if the user is authenticated

                AuthenticationState currentState = await _authenticationStateProvider.GetAuthenticationStateAsync();

                if (currentState.User.Identity.IsAuthenticated)
                {

                    int? UserId = int.Parse(currentState.User.Claims.FirstOrDefault(c => c.Type is ClaimTypes.NameIdentifier)!.Value);

                    //Verifying if the user is authenticated when attacking the endpoint. IE : having API authorize status in the header
                    //else it's not going to work

                    await _authorizingService.GetCurrentAuthorization();

                    var result = await _adressRepo.ExecuteAsync(new GetAdressQuery(UserId.Value));

                    if (!result.IsSuccess)
                    {
                        _logger.LogError(result.ErrorMessage, "Error while fetching user adress");
                        _snackbar.Add("Error while fetching user adress", Severity.Error);
                        return;
                    }

                    var DTOResult = result.Result;

                    UserAdress = new Adress(
                        DTOResult.Id,
                        DTOResult.Country,
                        DTOResult.Zipcode,
                        DTOResult.City,
                        DTOResult.Street
                    );

                }

           

             

            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "Error while fetching user adress");
                _snackbar.Add("Error while fetching user adress", Severity.Error);
            }

        }
    }
}
