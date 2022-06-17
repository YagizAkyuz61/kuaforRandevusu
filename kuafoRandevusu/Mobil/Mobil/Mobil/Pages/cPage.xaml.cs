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
    public partial class cPage : ContentPage
    {
        private ObservableCollection<UserClass> UserClasses;
        private ObservableCollection<CityClass> CityClasses;
        private ObservableCollection<IlceClass> IlceClasses;
        private ObservableCollection<GenderClass> GenderClasses;
        float max = 1, pmax = 2, prg = 0;
        bool run = true;
        int counter = 1;
        public cPage()
        {
            InitializeComponent();
            UserClasses = new ObservableCollection<UserClass>();
            CityClasses = new ObservableCollection<CityClass>();
            IlceClasses = new ObservableCollection<IlceClass>();
            GenderClasses = new ObservableCollection<GenderClass>();
            LoadTable();
            lastBTN.Text = Preferences.Get("selectedName", string.Empty);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
        }

        private async void LoadTable()
        {
            var citys = await ApiService.GetCity();
            var genders = await ApiService.GetGender();
            var ilces = await ApiService.GetIlce();

            foreach (var city in citys)
            {
                CityClasses.Add(city);
            }
            cityPC.ItemsSource = CityClasses;

            foreach (var gender in genders)
            {
                GenderClasses.Add(gender);
            }
            genderPC.ItemsSource = GenderClasses;

            foreach (var ilce in ilces)
            {
                IlceClasses.Add(ilce);
            }
        }

        private async void cityPC_SelectedIndexChanged(object sender, EventArgs e)
        {
            var city = cityPC.SelectedItem as CityClass;
            var ilces = await ApiService.GetIlces(city.Id);
            IlceClasses.Clear();

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
            else
            {
                Device.StartTimer(TimeSpan.FromSeconds(0.5), () =>
                {
                    if (prg >= 1)
                    {
                        run = false;
                        cityPC.IsEnabled = true;
                        ilcePC.IsEnabled = true;
                        waitLBL.IsVisible = false;
                        foreach (var ilce in ilces)
                        {
                            if (ilce.CId == city.Id)
                                IlceClasses.Add(ilce);
                        }
                        ilcePC.ItemsSource = IlceClasses;
                    }
                    else
                    {
                        waitLBL.IsVisible = true;
                        cityPC.IsEnabled = false;
                        ilcePC.IsEnabled = false;
                        prg += max / pmax;
                        progressBar.ProgressTo(prg, 5, Easing.Linear);
                        counter += 1;
                    }
                    return run;
                });
            }
        }
        
        private async void searchButton_Clicked(object sender, EventArgs e)
        {
            var cpc = cityPC.SelectedItem as CityClass;
            var ipc = ilcePC.SelectedItem as IlceClass;
            var gpc = genderPC.SelectedItem as GenderClass;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
            else
            {
                if (cityPC.SelectedItem == null || ilcePC.SelectedItem == null || genderPC.SelectedItem == null)
                {
                    await DisplayAlert("Hata", "Şehir, ilçe ve kuaför türünü seçiniz.", "Tamam");
                }
                else
                {
                    UserClasses.Clear();
                    var users = await ApiService.SearchUser(searchENT.Text, cpc.Name, ipc.IName, gpc.Gender);
                    foreach (var user in users)
                    {
                        UserClasses.Add(user);
                    }
                    list.ItemsSource = UserClasses;
                }
            }
        }

        private async void lastBTN_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
            else
            {
                var pre = Preferences.Get("selectedUser", string.Empty);
                if (lastBTN.Text == "")
                {
                    await DisplayAlert("Hata", "Bu buton ile en son randevu aldığınız kuaföre hızlı şekilde gidersiniz. Sen yenisin galiba. Lütfen manuel şekilde arama yap.", "Tamam");
                }
                else
                {
                    await Navigation.PushAsync(new reservationPage(Convert.ToInt32(pre)));
                }
            }
        }

        private void list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
            else
            {
                var selectedUser = e.SelectedItem as UserClass;
                if (selectedUser != null)
                {
                    Navigation.PushAsync(new reservationPage(selectedUser.Id));
                }
                ((ListView)sender).SelectedItem = null;
            }
        }

        private async void menuBTN_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SInPage());
        }
    }
}