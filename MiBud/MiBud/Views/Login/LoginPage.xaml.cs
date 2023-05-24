using MiBud.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel vm;
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = vm = new LoginViewModel(this);
        }

        //private void signup_clicked(object sender, EventArgs e)
        //{
        //    this.Navigation.PushAsync(new RegistrationPage());
        //}   

        //private void forgot_password_clicked(object sender, EventArgs e)
        //{
        //    this.Navigation.PushAsync(new ForgotPasswordPage());
        //}

        //private void login_clicked(object sender, EventArgs e)
        //{
        //    App.Current.MainPage = new NavigationPage(new MasterDetailView { Detail = new NavigationPage(new MyVehiclePage())});
        //}
    }
}