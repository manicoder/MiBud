using System;
using Xamarin.Forms;
using MiBud.Services;
using System.Windows.Input;
using Xamarin.Essentials;
using System.Text.RegularExpressions;
using MiBud.StaticInfo;
using MiBud.Models;

namespace MiBud.ViewModels
{
    public class OtpViewModel : ViewModelBase
    {
        ApiServices apiServices;

        readonly Page page;
        public OtpViewModel(Page page, bool termcondition_visible, bool otp_visible, string description) : base(page)
        {
            try
            {
                this.page = page;
                apiServices = new ApiServices();
                this.description = description;
                this.otp_visible = otp_visible;
                this.email_visible = !otp_visible;
                this.termcondition_visible = termcondition_visible;
            }
            catch (Exception ex)
            {
            }
        }

        #region Properties

        private bool _otp_visible;
        public bool otp_visible
        {
            get => _otp_visible;
            set
            {
                _otp_visible = value;
                OnPropertyChanged("otp_visible");
            }
        }

        private bool _email_visible;
        public bool email_visible
        {
            get => _email_visible;
            set
            {
                _email_visible = value;
                OnPropertyChanged("email_visible");
            }
        }

        private bool _termcondition_visible;
        public bool termcondition_visible
        {
            get => _termcondition_visible;
            set
            {
                _termcondition_visible = value;
                OnPropertyChanged("termcondition_visible");
            }
        }

        private string _email = "deep.yogi@gmail.com";
        public string email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged("email");
            }
        }

        private string _otp;
        public string otp
        {
            get => _otp;
            set
            {
                _otp = value;
                OnPropertyChanged("otp");
            }
        }

        private string _description;
        public string description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged("description");
            }
        }

        private string _otpFirstDigit;
        public string otpFirstDigit
        {
            get => _otpFirstDigit;
            set
            {
                _otpFirstDigit = value;
                OnPropertyChanged("otpFirstDigit");
            }
        }

        private string _otpSecondDigit;
        public string otpSecondDigit
        {
            get => _otpSecondDigit;
            set
            {
                _otpSecondDigit = value;
                OnPropertyChanged("otpSecondDigit");
            }
        }

        private string _otpThirdDigit;
        public string otpThirdDigit
        {
            get => _otpThirdDigit;
            set
            {
                _otpThirdDigit = value;
                OnPropertyChanged("otpThirdDigit");
            }
        }

        private string _otpFourthDigit;
        public string otpFourthDigit
        {
            get => _otpFourthDigit;
            set
            {
                _otpFourthDigit = value;
                OnPropertyChanged("otpFourthDigit");
            }
        }

        private string _otpFifthDigit;
        public string otpFifthDigit
        {
            get => _otpFifthDigit;
            set
            {
                _otpFifthDigit = value;
                OnPropertyChanged("otpFifthDigit");
            }
        }

        private string _otpSixthDigit;
        public string otpSixthDigit
        {
            get => _otpSixthDigit;
            set
            {
                _otpSixthDigit = value;
                OnPropertyChanged("_otpSixthDigit");
            }
        }
        #endregion

        #region ICommands
        public ICommand SubmitCommand => new Command(async (obj) =>
        {
            string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

            if (email_visible)
            {
                if (!Regex.IsMatch(email, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
                {
                    await page.DisplayAlert("Alert", "Please enter valid email", "Ok");
                    return;
                }

                var response = await apiServices.SendEmailToServer(email);

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

                string[] otp_list = response.message.Split(' ');
                otp = otp_list[6];
                otp_visible = true;
                email_visible = false;
                description = "Forgot Password An OTP has been sent to your mobile and will be valid for 10 mins.Pls enter the OTP here";
            }
            else
            {
                if (otp != $"{otpFirstDigit}{otpSecondDigit}{otpThirdDigit}{otpFourthDigit}{otpFifthDigit}{otpSixthDigit}")
                {
                    await page.DisplayAlert("Alert", "Invalid otp", "Ok");
                    return;
                }

                VerifyOtpRequestModel verifyOtpRequestModel = new VerifyOtpRequestModel
                {
                    email = email,
                    otp = otp
                };
                var response = await apiServices.VerifyOtp(verifyOtpRequestModel);
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

                await page.Navigation.PushAsync(new Views.ResetPassword.ResetPasswordPage(response.reset_url));
            }
        });

        public ICommand PrivacyPolicyCommand => new Command(async (obj) =>
        {
            await page.Navigation.PushAsync(new Views.PrivacyPolicy.PrivacyPolicyPage());
        });
        public ICommand TermConditionCommand => new Command(async (obj) =>
        {
            await page.Navigation.PushAsync(new Views.TermsAndConditions.TermsAndConditionsPage());
        });
        #endregion
    }
}
