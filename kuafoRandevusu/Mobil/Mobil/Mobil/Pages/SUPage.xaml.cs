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
    public partial class SUPage : ContentPage
    {
        private ObservableCollection<CityClass> CityClasses;
        private ObservableCollection<IlceClass> IlceClasses;
        private ObservableCollection<GenderClass> GenderClasses;
        float max = 1, pmax = 2, prg = 0;
        bool run = true;
        int counter = 1;
        public SUPage()
        {
            InitializeComponent();
            CityClasses = new ObservableCollection<CityClass>();
            IlceClasses = new ObservableCollection<IlceClass>();
            GenderClasses = new ObservableCollection<GenderClass>();
            LoadTable();
            Address();
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

        private async void Address()
        {
            var result = await Geolocation.GetLocationAsync(new
                GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(30)));
            addressEnt.Text = $"{result.Latitude}, {result.Longitude}";
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
                        waitLBL.IsVisible = false;
                        cityPC.IsEnabled = true;
                        ilcePC.IsEnabled = true;
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

        private async void singUpButton_Clicked(object sender, EventArgs e)
        {
            var city = cityPC.SelectedItem as CityClass;
            var ilce = ilcePC.SelectedItem as IlceClass;
            var gender = genderPC.SelectedItem as GenderClass;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
            else
            {
                if (nameEnt.Text == null || nickEnt.Text == null || passEnt.Text == null || phoneEnt.Text == null || addressEnt.Text == null || cityPC.SelectedItem == null || ilcePC.SelectedItem == null || genderPC.SelectedItem == null || emailEnt.Text == null)
                {
                    await DisplayAlert("Hata", "Lütfen tüm alanları doldurun.", "Kapat");
                }
                else
                {
                    bool option = await DisplayAlert("Uyarı", "Olduğunuz konum dükkanınız mı?", "Evet", "Hayır");
                    if (option == true)
                    {
                        var response = await ApiService.RegisterUser(nameEnt.Text, emailEnt.Text, nickEnt.Text, passEnt.Text, phoneEnt.Text, addressEnt.Text, Convert.ToString(city.Name), Convert.ToString(ilce.IName), Convert.ToString(gender.Gender), DateTime.Now);
                        if (response)
                        {
                            await DisplayAlert("İşlem Tamam", "Hesabınız başarıyla oluşturuldu", "Kapat");
                            Application.Current.MainPage = new NavigationPage(new SInPage());
                        }
                        else
                        {
                            await DisplayAlert("Hata", "Hatalı giriş yaptınız. Her şeyi girdiğinize emin olunuz", "Kapat");
                        }
                    }
                }
            }
        }
    }
}