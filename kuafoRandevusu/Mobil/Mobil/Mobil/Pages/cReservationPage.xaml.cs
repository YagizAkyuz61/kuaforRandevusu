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
    public partial class cReservationPage : ContentPage
    {
        private ObservableCollection<ReservationClass> ReservationClasses;
        public cReservationPage()
        {
            InitializeComponent();
            ReservationClasses = new ObservableCollection<ReservationClass>();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            ReservationClasses.Clear();
            var ress = await ApiService.GetCIdReservation(Convert.ToInt32(Preferences.Get("customerId", string.Empty)));
            foreach (var res in ress)
            {
                ReservationClasses.Add(res);
            }
            reserList.ItemsSource = ReservationClasses;
        }

        private void reserList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
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
                    Navigation.PushAsync(new ReservationDetailPage(selected.Id));
                }
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}