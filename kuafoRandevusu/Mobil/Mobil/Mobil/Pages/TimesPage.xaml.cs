using Mobil.Model;
using Mobil.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobil.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimesPage : ContentPage
    {
        private ObservableCollection<TimeClass> TimeClasses;
        public TimesPage()
        {
            InitializeComponent();
            TimeClasses = new ObservableCollection<TimeClass>();
            kidLBL.Text = Preferences.Get("userId", string.Empty);
            LoadTime();
        }

        private async void LoadTime()
        {
            var times = await ApiService.GetTime();
            foreach(var time in times)
                TimeClasses.Add(time);
            list.ItemsSource = TimeClasses;

        }

        private async void list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var lis = e.SelectedItem as TimeClass;
            var times = new TimesClass();
            times.KId = Convert.ToInt32(kidLBL.Text);
            times.Time = lis.Time;

            var post = await ApiService.PostTime(times);
            if (kidLBL.Text == null)
            {
                await DisplayAlert("Hata", "Lütfen tekrar deneyin.", "Tamam");
            }
            else
            {
                acceptLBL.TextColor = Color.Green;
                acceptLBL.Text = "Rezervasyon saati ayarlandı: " + lis.Time;
            }
        }
    }
}