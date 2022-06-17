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
    public partial class reservationPage : ContentPage
    {
        private ObservableCollection<TimesClass> TimesClasses;
        private ObservableCollection<ReservationClass> ReservationClasses;
        private ObservableCollection<OperationClass> OperationClasses;
        private ObservableCollection<HOperationClass> HOperationClasses;
        public reservationPage(int id)
        {
            InitializeComponent();
            GetUserProfile(id);
            TimesClasses = new ObservableCollection<TimesClass>();
            ReservationClasses = new ObservableCollection<ReservationClass>();
            OperationClasses = new ObservableCollection<OperationClass>();
            HOperationClasses = new ObservableCollection<HOperationClass>();

            datePC.MinimumDate = DateTime.Now;
            datePC.MaximumDate = DateTime.Now.AddMonths(2);
            datePC.Date = DateTime.Now;
            datePC.Format = "dd-MM-yyyy";
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

        private async void GetUserProfile(int id)
        {
            var user = await ApiService.GetUser(id);
            var ads = user.Address;
            idLab.Text = Convert.ToString(user.Id);
            hairNameLab.Text = user.Name;

            var operations = await ApiService.GetHOperation(Convert.ToInt32(id));
            foreach (var operation in operations)
            {
                HOperationClasses.Add(operation);
            }
            operationPC.ItemsSource = HOperationClasses;

            var times = await ApiService.GetTimes(Convert.ToInt32(id));
            foreach (var time in times)
            {
                TimesClasses.Add(time);
            }
            timePC.ItemsSource = TimesClasses;
        }

        private async void reservationAddButton_Clicked(object sender, EventArgs e)
        {
            var pre = Preferences.Get("customerNames", string.Empty);
            var cid = Preferences.Get("customerId", string.Empty);

            var time = timePC.SelectedItem as TimesClass;
            var operation = operationPC.SelectedItem as HOperationClass;
            var reservation = new ReservationClass();
            reservation.KId = Convert.ToInt32(idLab.Text);
            reservation.CId = Convert.ToInt32(cid);
            reservation.CName = Convert.ToString(pre);
            reservation.Operation = Convert.ToString(operation.Operation);
            reservation.Date = datePC.Date;
            reservation.Time = Convert.ToString(time.Time);

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
            else
            {
                var response = await ApiService.PostReservation(reservation);
                if (response != null)
                {
                    await DisplayAlert("Hata", "Lütfen tekrar deneyin.", "Tamam");
                }
                else
                {
                    await DisplayAlert("Onaylandı", "kuaföRandevunuz onaylanmıştır.", "Tamam");
                    Application.Current.MainPage = new NavigationPage(new SInPage());
                    Preferences.Set("selectedUser", idLab.Text);
                    Preferences.Set("selectedName", hairNameLab.Text);
                }
            }
        }

        private async void timeListBTN_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
            else
            {
                if (idLab.Text == null)
                {
                    await DisplayAlert("1 Dakika", "Lütfen sayfanızın yüklenmesini bekleyiniz", "Tamam");
                }
                else
                {
                    Preferences.Set("id", idLab.Text);
                    await Navigation.PushAsync(new TimeListPage());
                }
            }
        }

        private async void commentButton_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
            else
            {
                if(idLab.Text == null)
                {
                    await DisplayAlert("1 Dakika", "Lütfen sayfanızın yüklenmesini bekleyiniz", "Tamam");
                }
                else
                {
                    Preferences.Set("kid", idLab.Text);
                    await Navigation.PushAsync(new CommentPage());
                }
            }
                
        }

        [Obsolete]
        private async void goAddressBTN_Clicked(object sender, EventArgs e)
        {
            var user = await ApiService.GetUser(Convert.ToInt32(idLab.Text));
            var ads = user.Address;
            if (idLab.Text == null)
            {
                await DisplayAlert("1 Dakika", "Lütfen sayfanızın yüklenmesini bekleyiniz", "Tamam");
            }
            else
            {
                Device.OpenUri(new Uri("https://www.google.com.tr/maps/place/" + ads));
            }
        }
    }
}