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
using Mobil.Pages;

namespace Mobil.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommentPage : ContentPage
    {
        private ObservableCollection<CommentClass> CommentClasses;
        private ObservableCollection<CommentsClass> CommentsClasses;
        public CommentPage()
        {
            InitializeComponent();
            kIdLBL.Text = Preferences.Get("kid", string.Empty);
            CommentClasses = new ObservableCollection<CommentClass>();
            CommentsClasses = new ObservableCollection<CommentsClass>();
            LoadTable();
            List();
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

        private async void List()
        {
            CommentClasses.Clear();
            var comments = await ApiService.GetComment(Convert.ToInt32(kIdLBL.Text));
            foreach (var comment in comments)
            {
                CommentClasses.Add(comment);
            }
            list.ItemsSource = CommentClasses;
        }

        private async void LoadTable()
        {
            var comments = await ApiService.GetComments();
            foreach (var comment in comments)
            {
                CommentsClasses.Add(comment);
            }
            commentPC.ItemsSource = CommentsClasses;
        }

        private async void commentButton_Clicked(object sender, EventArgs e)
        {
            var pre = Preferences.Get("customerNames", string.Empty);
            var comments = commentPC.SelectedItem as CommentsClass;
            var comment = new CommentClass();
            comment.KId = Convert.ToInt32(kIdLBL.Text);
            comment.Comment = comments.Comment;
            comment.CName = pre;
            comment.Date = DateTime.Now;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Hata", "İnternet bağlantısı yok.", "Tamam");
                return;
            }
            else
            {
                var response = await ApiService.PostComment(comment);
                if (commentPC.SelectedItem == null)
                {
                    await DisplayAlert("Hata", "Lütfen yorum ekleme alanını doldurun.", "Tamam");
                }
                else
                {
                    await DisplayAlert("Onaylandı", "Yaptığınız yorum onaylanmıştır. Şimdi sizi kuaför sayfasına yönlendiriyoruz.", "Tamam");
                    await Navigation.PushAsync(new reservationPage(Convert.ToInt32(kIdLBL.Text)));
                }
            }
        }
    }
}