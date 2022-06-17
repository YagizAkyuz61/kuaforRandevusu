using Mobil.Service;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobil.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservationDetailPage : ContentPage
    {
        public ReservationDetailPage(int id)
        {
            InitializeComponent();
            GetDetail(id);
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

        private async void GetDetail(int id)
        {
            var detail = await ApiService.GetReservation(id);
            idTXT.Text = Convert.ToString(detail.Id);
            kidTXT.Text = Convert.ToString(detail.KId);
            cidTXT.Text = Convert.ToString(detail.CId);
            nameTXT.Text = detail.CName;
            operationTXT.Text = detail.Operation;
            dateTXT.Text = Convert.ToString(detail.Date.ToShortDateString());
            timeTXT.Text = detail.Time;
        }

        private async void cancelButton_Clicked(object sender, EventArgs e)
        {
            var cancel = await ApiService.CancelReservation(Convert.ToInt32(idTXT.Text), Convert.ToInt32(kidTXT.Text), Convert.ToInt32(cidTXT.Text), "Rezervasyon iptal", "Rezervasyon iptal", "Rezervasyon iptal", Convert.ToDateTime(dateTXT.Text));
            if (idTXT.Text != null)
            {
                await DisplayAlert("Onaylandı", "İptal işlemi başarılı.", "Tamam");
                await Navigation.PopAsync();
            }
        }

        private async void hdButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new reservationPage(Convert.ToInt32(kidTXT.Text)));
        }
    }
}