using System;
using System.Collections.Generic;
using Xamarin.Forms;
using MiBud.Services;
using Acr.UserDialogs;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MiBud.StaticInfo;

namespace MiBud.ViewModels
{
    public class ResetPasswordViewModel : ViewModelBase
    {
        ApiServices apiServices;
        string url = string.Empty;
        readonly Page page;
        public ResetPasswordViewModel(Page page, string url) : base(page)
        {
            try
            {
                this.page = page;
                apiServices = new ApiServices();
                this.url = url;
            }
            catch (Exception ex)
            {
            }
        }

        #region Properties

        private string _new_password;
        public string new_password
        {
            get => _new_password;
            set
            {
                _new_password = value;
                OnPropertyChanged("new_password");
            }
        }

        private string _confirm_password;
        public string confirm_password
        {
            get => _confirm_password;
            set
            {
                _confirm_password = value;
                OnPropertyChanged("confirm_password");
            }
        }
        #endregion

        #region ICommands
        public ICommand SubmitCommand => new Command(async (obj) =>
        {
            if(! await Validation())
            {
                var response = await apiServices.ResetPassword(url,new_password);

                if (response == null)
                {
                    await page.DisplayAlert("Success!", "Please check your internet connection!", "Ok");
                    return;
                }

                if (!string.IsNullOrEmpty(response.error))
                {
                    var api_status_code = StaticMethods.http_status_code(response.status_code);
                    await page.DisplayAlert(api_status_code, response.error, "Ok");
                    return;
                }
                await page.DisplayAlert("Success!", "Password reset successfully.", "Ok");
                Application.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
            }
        });
        #endregion

        #region Methods
        public async Task<bool> Validation()
        {
            const string passwordRegex = @"^(?=.*[a-z])(?=.*\d).{8,}$";
            bool IsError = false;

            if (string.IsNullOrEmpty(new_password))
            {
                await page.DisplayAlert("Alert", "Please enter your first name", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(confirm_password))
            {
                await page.DisplayAlert("Alert", "Please enter your last name", "Ok");
                IsError = true;
            }
            else if (!Regex.IsMatch(new_password, passwordRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
            {
                await page.DisplayAlert("Alert", "Password must has at least 8 character that include at least 1 number.", "Ok");
                IsError = true;
            }
            else if (new_password!= confirm_password)
            {
                await page.DisplayAlert("Alert", "Password and confirm password mismatch", "Ok");
                IsError = true;
            }
            return IsError;
        }
        #endregion
    }
}
