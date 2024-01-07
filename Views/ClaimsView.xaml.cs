
using Microsoft.Identity.Client;


namespace PizzaAppMaui.SignInMaui.Views
{
    public partial class ClaimsView : ContentPage
    {
        public IEnumerable<string> IdTokenClaims { get; set; } = new string[] { "No claims found in ID token" };
        public PublicClientSingleton PublicClient { get; private set; } 

        public ClaimsView()
        {
            BindingContext = this;
            InitializeComponent();

            PublicClient = new PublicClientSingleton(); 
            _ = SetViewDataAsync();
        }

        private async Task SetViewDataAsync()
        {
            try
            {
                var authResult = await PublicClient.Instance.AcquireTokenSilentAsync();

                if (PublicClient.Instance.MSALClientHelper != null && authResult != null && authResult.ClaimsPrincipal != null)
                {
                    IdTokenClaims = authResult.ClaimsPrincipal.Claims.Select(c => c.Value);
                }

                Claims.ItemsSource = IdTokenClaims;
            }
            catch (MsalUiRequiredException)
            {
                await Shell.Current.GoToAsync("claimsview");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        protected override bool OnBackButtonPressed() { return true; }

        private async void SignOutButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                await PublicClient.Instance.SignOutAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during sign out: " + ex.Message);
            }

            await Shell.Current.GoToAsync("mainview");
        }
    }
}
