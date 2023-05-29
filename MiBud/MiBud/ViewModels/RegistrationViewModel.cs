using Acr.UserDialogs;
using MiBud.Interfaces;
using MiBud.Models;
using MiBud.Services;
using MiBud.StaticInfo;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly INavigation navigationService;
        readonly IDeviceMacAddress device_mac_id;
        readonly Page page;
        MediaFile file = null;
        const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
          @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        const string passwordRegex = @"^(?=.*[a-z])(?=.*\d).{8,}$";
        public RegistrationViewModel(Page page) : base(page)
        {
            try
            {

                apiServices = new ApiServices();

                this.page = page;
                this.navigationService = page.Navigation;
                user_profile_pic = ImageSource.FromFile("ic_user.png");
                //this.device_mac_id = DependencyService.Get<IDeviceMacAddress>();
                //user_profile_pic = ImageSource.FromFile("ic_user.png");

                InitializeCommands();
                GetDeviceType();
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    await GetMacId();
                });
            }
            catch (Exception ex)
            {
            }
        }


        #region Properties

        private string _first_name;
        public string first_name
        {
            get => _first_name;
            set
            {
                _first_name = value;
                OnPropertyChanged("first_name");
            }
        }


        private string _last_name;
        public string last_name
        {
            get => _last_name;
            set
            {
                _last_name = value;
                OnPropertyChanged("last_name");
            }
        }


        private string _email;
        public string email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged("email");
            }
        }


        private string _mobile;
        public string mobile
        {
            get => _mobile;
            set
            {
                _mobile = value;
                OnPropertyChanged("mobile");
            }
        }


        private string _password;
        public string password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged("password");
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


        private string _country = "Select country";
        public string country
        {
            get => _country;
            set
            {
                _country = value;

                if (!string.IsNullOrEmpty(country) && !country.Contains("Select country"))
                {
                    country_text_color = (Color)Application.Current.Resources["text_color"];
                }

                OnPropertyChanged("country");
            }
        }


        private Color _country_text_color = (Color)Application.Current.Resources["placeholder_color"];
        public Color country_text_color
        {
            get => _country_text_color;
            set
            {
                _country_text_color = value;
                OnPropertyChanged("country_text_color");
            }
        }


        private string _device_type;
        public string device_type
        {
            get => _device_type;
            set
            {
                _device_type = value;
                OnPropertyChanged("device_type");
            }
        }


        private string _mac_id;
        public string mac_id
        {
            get => _mac_id;
            set
            {
                _mac_id = value;
                OnPropertyChanged(" mac_id");
            }
        }


        private ImageSource _user_profile_pic;
        public ImageSource user_profile_pic
        {
            get => _user_profile_pic;
            set
            {
                _user_profile_pic = value;
                OnPropertyChanged("user_profile_pic");
            }
        }


        private string _pin_code;
        public string pin_code
        {
            get => _pin_code;
            set
            {
                _pin_code = value;
                OnPropertyChanged("pin_code");
            }
        }

        //private string _rs_agent_id = "Select RSAgent";
        //public string rs_agent_id
        //{
        //    get => _rs_agent_id;
        //    set
        //    {
        //        _rs_agent_id = value;

        //        if (!string.IsNullOrEmpty(_rs_agent_id) && !_rs_agent_id.Contains("Select RSAgent"))
        //        {
        //            rs_agent_text_color = (Color)Application.Current.Resources["text_color"];
        //        }

        //        OnPropertyChanged("rs_agent_id");
        //    }
        //}


        //private Color _rs_agent_text_color = (Color)Application.Current.Resources["placeholder_color"];
        //public Color rs_agent_text_color
        //{
        //    get => _rs_agent_text_color;
        //    set
        //    {
        //        _rs_agent_text_color = value;
        //        OnPropertyChanged("rs_agent_text_color");
        //    }
        //}

        private UserModel _user_detail;
        public UserModel user_detail
        {
            get => _user_detail;
            set
            {
                _user_detail = value;
                OnPropertyChanged("user_detail");
            }
        }

        //private AgentUserType _agent_detail;
        //public AgentUserType agent_detail
        //{
        //    get => _agent_detail;
        //    set
        //    {
        //        _agent_detail = value;
        //        OnPropertyChanged("agent_detail");
        //    }
        //}

        private RsUserTypeCountry _country_detail;
        public RsUserTypeCountry country_detail
        {
            get => _country_detail;
            set
            {
                _country_detail = value;
                OnPropertyChanged("country_detail");
            }
        }

        private string _password_image = "\U000F06D1";
        public string password_image
        {
            get => _password_image;
            set
            {
                _password_image = value;
                OnPropertyChanged("password_image");
            }
        }

        private bool _is_password = true;
        public bool is_password
        {
            get => _is_password;
            set
            {
                _is_password = value;
                OnPropertyChanged("is_password");
            }
        }
        #endregion


        #region Methods

        [Obsolete]
        public void InitializeCommands()
        {
            OpenPrivacyPolicyCommand = new Command(async (obj) =>
            {
                GoToPrivacyPolicyPage();
            });

            MessagingCenter.Subscribe<CountyViewModel, RsUserTypeCountry>(this, "selected_country_registrationVM", async (sender, arg) =>
            {
                country = arg.name;
                country_detail = arg;
            });

            //MessagingCenter.Subscribe<AgentViewModel, AgentUserType>(this, "selected_agent_registrationVM", async (sender, arg) =>
            //{
            //    agent_detail = arg;
            //    rs_agent_id = arg.name;
            //});

            ProfileCommand = new Command(async (obj) =>
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await page.DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "profile.jpg",
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                    MaxWidthHeight = 70,
                    CompressionQuality = 50,
                });

                if (file == null)
                    return;

                //await page.DisplayAlert("File Location", file.Path, "OK");

                user_profile_pic = ImageSource.FromFile(file.Path);

                //user_profile_pic = ImageSource.FromStream(() =>
                //{
                //    var stream = file.GetStream();
                //    return stream;
                //});
            });

            IsPasswordCommand = new Command(async (obj) =>
            {
                if (is_password)
                {
                    password_image = "\U000F06D0";
                    //password_image_color = Color.Red;
                    is_password = false;
                }
                else
                {
                    is_password = true;
                    //password_image_color = Color.FromHex("#01FE2F");
                    password_image = "\U000F06D1";//
                }
            });

            SubmitCommand = new Command(async (obj) =>
            {

                await UserRegistration();
                //string description = "User Registration authentication An OTP has been sent to your mobile and will be valid for 10 mins.Pls enter the OTP here";
                //await this.navigationService.PushAsync(new Views.Otp.OtpPage(true, description));
            });

            //RSAgentCommand = new Command(async (obj) =>
            //{
            //    if (country_detail == null)
            //    {
            //        await page.DisplayAlert("Alert", "Please select country", "Ok");
            //    }
            //    else if (string.IsNullOrEmpty(pin_code))
            //    {
            //        await page.DisplayAlert("Alert", "Please select country", "Ok");
            //    }
            //    else
            //    {
            //        using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            //        {
            //            try
            //            {
            //                await Task.Delay(200);
            //                await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.AgentListPopupPage(country_detail.id, pin_code));
            //            }
            //            catch (Exception ex)
            //            {
            //            }
            //        }
            //    }
            //});

            //NewRSAgentCommand = new Command(async (obj) =>
            //{
            //    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            //    {
            //        await Task.Delay(200);
            //        await navigationService.PushAsync(new Views.CreateNewAgent.CreateNewAgentPage());
            //    }
            //});

            CountryCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);
                    StaticMethods.last_page = "registration";
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.CountyPopupPage());
                }
            });
        }

        async void GoToPrivacyPolicyPage()
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                await Task.Delay(200);
                await navigationService.PushAsync(new Views.PrivacyPolicy.PrivacyPolicyPage());
            }
        }

        public async Task UserRegistration()
        {

            try
            {
                var IsError = await Validation();
                if (!IsError)
                {
                    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(200);
                        //user_detail = new UserModel();
                        //user_detail.first_name = first_name;
                        //user_detail.last_name = last_name;
                        //user_detail.email = email;
                        //user_detail.mobile = mobile;
                        //user_detail.password = password;
                        //user_detail.device_type = device_type;
                        //user_detail.mac_id = mac_id;
                        //user_detail.user_profile_pic = user_profile_pic;
                        ////user_detail.pin_code = pin_code;
                        //user_detail.rs_agent_id = "WK10445094094";
                        //user_detail.user_type.Add(2);

                        user_detail = new UserModel();
                        user_detail.first_name = first_name;
                        user_detail.last_name = last_name;
                        user_detail.email = email;
                        user_detail.mobile = mobile;
                        user_detail.password = password;
                        user_detail.device_type = device_type;
                        user_detail.mac_id = mac_id;
                        user_detail.user_profile_pic = user_profile_pic;
                        //user_detail.pin_code = pin_code;
                        //user_detail.rs_agent_id = "WK2397950556"; //workshop_detail.code;
                        user_detail.user_type = "mibud";//"wikitekMechanik";
                        //user_detail.segment = "3W";
                        //user_detail.segment_id = 2;
                        user_detail.role = "mibudconsumer";

                        var response = await apiServices.UserRegistration(file, user_detail);
                        if (response == null)
                        {
                            await page.DisplayAlert("Success!", "Please check your internet connection!", "Ok");
                            return;
                        }

                        var api_status_code = StaticMethods.http_status_code(response.status_code);

                        if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                        {
                            await page.DisplayAlert("Success!", "User Created successfully!", "Ok");
                            await page.Navigation.PopAsync();
                        }
                        else
                        {
                            await page.DisplayAlert(api_status_code, response.error, "Ok");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void GetDeviceType()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    break;
                case Device.Android:
                    device_type = "android";
                    break;
                case Device.UWP:
                    device_type = "windows";
                    break;
                default:
                    break;
            }
        }

        public async Task GetMacId()
        {
            // mac_id = await device_mac_id.GetMacAddress();
            mac_id = "6E285B000000";
        }

        public async Task<bool> Validation()
        {
            bool IsError = false;

            if (file == null)
            {
                await page.DisplayAlert("Alert", "Click your profile image", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(first_name))
            {
                await page.DisplayAlert("Alert", "Please enter your first name", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(last_name))
            {
                await page.DisplayAlert("Alert", "Please enter your last name", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(email))
            {
                await page.DisplayAlert("Alert", "Please enter your email id", "Ok");
                IsError = true;
            }
            else if (!Regex.IsMatch(email, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
            {
                await page.DisplayAlert("Alert", "Please enter correct email id", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(mobile))
            {
                await page.DisplayAlert("Alert", "Please enter your mobile number", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(password))
            {
                await page.DisplayAlert("Alert", "Please enter your password", "Ok");
                IsError = true;
            }
            else if (!Regex.IsMatch(password, passwordRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
            {
                await page.DisplayAlert("Alert", "Password must has at least 8 character that include at least 1 number.", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(confirm_password))
            {
                await page.DisplayAlert("Alert", "Please enter your confirm password", "Ok");
                IsError = true;
            }
            else if (password != confirm_password)
            {
                await page.DisplayAlert("Alert", "Password and confirm password mismatch", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(device_type))
            {
                await page.DisplayAlert("Alert", "Device type not found", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(mac_id))
            {
                await page.DisplayAlert("Alert", "Mac address not found", "Ok");
                IsError = true;
            }
            else if (user_profile_pic == null)
            {
                await page.DisplayAlert("Alert", "please click your profile", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(pin_code))
            {
                await page.DisplayAlert("Alert", "Please enter pin code", "Ok");
                IsError = true;
            }
            //else if (string.IsNullOrEmpty(rs_agent_id))
            //{
            //    await page.DisplayAlert("Alert", "Please select RSAgent.\n\nIf RSAgent is not created so create firstly RSAgent.", "Ok");
            //    IsError = true;
            //}

            return IsError;
        }
        #endregion

        #region ICommands
        public ICommand OpenPrivacyPolicyCommand { get; set; }
        public ICommand ProfileCommand { get; set; }
        public ICommand CountryCommand { get; set; }
        //public ICommand RSAgentCommand { get; set; }
        //public ICommand NewRSAgentCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public ICommand IsPasswordCommand { get; set; }
        #endregion
    }
}
