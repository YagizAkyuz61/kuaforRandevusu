using Mobil.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobil.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CSUPage : ContentPage
    {
        public CSUPage()
        {
            InitializeComponent();
        }

        private async void singUpButton_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
            else
            {
                if (nameEnt.Text == null || nickEnt.Text == null || passEnt.Text == null || phoneEnt.Text == null || emailEnt.Text == null)
                {
                    await DisplayAlert("Hata", "Lütfen tüm alanları doldurun.", "Kapat");
                }
                else
                {
                    var response = await ApiService.RegisterCustomer(nameEnt.Text, nickEnt.Text, emailEnt.Text, phoneEnt.Text, passEnt.Text, DateTime.Now);
                    if (response)
                    {
                        await DisplayAlert("İşlem Tamam", "Hesabınız başarıyla oluşturuldu", "Kapat");
                        Application.Current.MainPage = new NavigationPage(new SInPage());
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Hatalı giriş yaptınız. Her şeyi girdiğinize emin olunuz.", "Kapat");
                    }
                }
            }
        }
    }
}