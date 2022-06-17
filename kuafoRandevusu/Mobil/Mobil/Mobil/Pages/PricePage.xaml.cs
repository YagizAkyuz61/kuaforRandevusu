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
    public partial class PricePage : ContentPage
    {
        private ObservableCollection<HOperationClass> HOperationClasses;
        private ObservableCollection<OperationClass> OperationClasses;
        private ObservableCollection<GenderClass> GenderClasses;
        public PricePage()
        {
            InitializeComponent();
            HOperationClasses = new ObservableCollection<HOperationClass>();
            OperationClasses = new ObservableCollection<OperationClass>();
            GenderClasses = new ObservableCollection<GenderClass>();
            LoadTable();
        }

        private async void ListLoad()
        {
            var pre = Preferences.Get("userId", string.Empty);
            var operations = await ApiService.GetLHO(Convert.ToInt32(pre));
            foreach (var operation in operations)
            {
                HOperationClasses.Add(operation);
            }
            list.ItemsSource = HOperationClasses;
        }

        /*protected async override void OnAppearing()
        {
            base.OnAppearing();
            var pre = Preferences.Get("userId", string.Empty);
            var operations = await ApiService.GetLHO(Convert.ToInt32(pre));
            foreach(var operation in operations)
            {
                HOperationClasses.Add(operation);
            }
            list.ItemsSource = HOperationClasses;
        }*/

        private async void LoadTable()
        {
            var genders = await ApiService.GetGender();
            foreach (var gender in genders)
            {
                GenderClasses.Add(gender);
            }
            genderPC.ItemsSource = GenderClasses;
        }

        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            var pre = Preferences.Get("userId", string.Empty);
            var operations = operationPC.SelectedItem as OperationClass;
            var operation = new HOperationClass();
            operation.KId = Convert.ToInt32(pre);
            operation.Operation = operations.Operation;
            operation.Price = "(" + priceENT.Text + " TL)";

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
            else
            {
                var response = await ApiService.PostHOperation(operation);
                if(pre == null & operationPC.SelectedItem == null & priceENT.Text == null)
                {
                    await DisplayAlert("Hata", "Lütfen tekrar deneyin.", "Tamam");
                }
                else
                {
                    HOperationClasses.Clear();
                    await DisplayAlert("Onaylandı", "Fiyat belirleme başarılı.", "Tamam");
                    ListLoad();
                }
            }
        }

        private async void updateButton_Clicked(object sender, EventArgs e)
        {
            var kid = Preferences.Get("userId", string.Empty);
            var operation = await ApiService.PutHoperation(Convert.ToInt32(idTXT.Text), Convert.ToInt32(kid), opTXT.Text, "(" + updateENT.Text + " TL)" );
            if(idTXT.Text != null && opTXT.Text != null && updateENT.Text != null)
            {
                HOperationClasses.Clear();
                await DisplayAlert("Onaylandı", "Fiyat değiştirme başarılı.", "Tamam");
                OnAppearing();
            }
            else
                await DisplayAlert("Hata", "Fiyat değiştirme başarısız.", "Tamam");
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
                var selected = e.SelectedItem as HOperationClass;
                if (selected != null)
                {
                    idTXT.Text = Convert.ToString(selected.Id);
                    opTXT.Text = Convert.ToString(selected.Operation);
                }
                ((ListView)sender).SelectedItem = null;
            }
        }

        private async void genderPC_SelectedIndexChanged(object sender, EventArgs e)
        {
            OperationClasses.Clear();
            var gender = genderPC.SelectedItem as GenderClass;
            var operations = await ApiService.GetOperation(Convert.ToString(gender.Gender));
            foreach (var operation in operations)
            {
                OperationClasses.Add(operation);
            }
            operationPC.ItemsSource = OperationClasses;
            operationPC.IsEnabled = true;
        }
    }
}