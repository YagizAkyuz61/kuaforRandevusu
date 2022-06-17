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
    public partial class hdPage : ContentPage
    {
        private ObservableCollection<ReservationClass> ReservationClasses;
        public hdPage()
        {
            InitializeComponent();
            ReservationClasses = new ObservableCollection<ReservationClass>();
        }
        protected override async void OnAppearing()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            base.OnAppearing();

            ReservationClasses.Clear();
            var reservations = await ApiService.GetKIdReservation(Convert.ToInt32(Preferences.Get("userId", string.Empty)));
            foreach (var reservation in reservations)
            {
                ReservationClasses.Add(reservation);
            }
            list.ItemsSource = ReservationClasses;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
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
                var selected = e.SelectedItem as ReservationClass;
                if (selected != null)
                {
                    Navigation.PushAsync(new ReservationDetailPage(Convert.ToInt32(selected.Id)));
                }
                ((ListView)sender).SelectedItem = null;
            }
        }

        private async void menuBTN_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SInPage());
        }

        private async void profileBTN_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
            else
                await Navigation.PushAsync(new ProfilePage());
        }
    }
}