using Mobil.Model;
using Mobil.Service;
using System;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobil.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SInPage : ContentPage
    {
        public SInPage()
        {
            InitializeComponent();

            userNameTXT.Text = Preferences.Get("userName", string.Empty);
            passwordTXT.Text = Preferences.Get("password", string.Empty);
        }

        private async void loginBTN_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
            else
            {
                var response = await ApiService.Login(userNameTXT.Text, passwordTXT.Text);
                if (response)
                {
                    Application.Current.MainPage = new NavigationPage(new hdPage());
                    Preferences.Set("userName", userNameTXT.Text);
                    Preferences.Set("password", passwordTXT.Text);
                }
                else
                {
                    await DisplayAlert("Hata", "Hatalı giriş yaptınız.", "Kapat");
                }
            }
        }

        private async void singUpBTN_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
            else
                await Navigation.PushAsync(new SUPage());
        }

        private async void customerBTN_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
            else
            {
                var response = await ApiService.LoginCustomer(userNameTXT.Text, passwordTXT.Text);
                if (response)
                {
                    Application.Current.MainPage = new NavigationPage(new cPage());
                    Preferences.Set("userName", userNameTXT.Text);
                    Preferences.Set("password", passwordTXT.Text);
                }
                else
                {
                    await DisplayAlert("Hata", "Hatalı giriş yaptınız.", "Kapat");
                }
            }
        }

        private async void randevuButton_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
            else
            {
                var response = await ApiService.LoginCustomer(userNameTXT.Text, passwordTXT.Text);
                if (response)
                {
                    await Navigation.PushAsync(new cReservationPage());
                    Preferences.Set("userName", userNameTXT.Text);
                    Preferences.Set("password", passwordTXT.Text);
                }
                else
                {
                    await DisplayAlert("Hata", "Hatalı giriş yaptınız.", "Kapat");
                }
            }
        }

        private void csuButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CSUPage());
        }
    }
}