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
    public partial class TimeListPage : ContentPage
    {
        private ObservableCollection<ReservationClass> ReservationClasses;
        private bool First = true;
        public TimeListPage()
        {
            InitializeComponent();
            kIdLBL.Text = Preferences.Get("id", string.Empty);
            ReservationClasses = new ObservableCollection<ReservationClass>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (First)
            {
                var times = await ApiService.GetKIdReservation(Convert.ToInt32(kIdLBL.Text));
                foreach (var time in times)
                {
                    ReservationClasses.Add(time);
                }
                list.ItemsSource = ReservationClasses;
            }
            First = false;
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
    }
}