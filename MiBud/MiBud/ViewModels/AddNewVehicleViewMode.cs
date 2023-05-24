using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MiBud.Models;
using MiBud.Services;
using MiBud.StaticInfo;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class AddNewVehicleViewMode : ViewModelBase
    {
        ApiServices apiServices;
        readonly INavigation navigationService;
        readonly Page page;
        MediaFile file = null;
        bool is_edit = false;
        public AddNewVehicleViewMode(Page page, Vehicle vehicle) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                this.page = page;
                vehicle_request_model = new CreateVehicleRequestModel();
                segment_list = new ObservableCollection<VehicleSegment>();
                selected_vehicle = vehicle;
                InitializeCommands();

                if (vehicle != null)
                {
                    selected_brand = new OemResult { oem = new VehicleOem { name = vehicle.oem.name, id = vehicle.oem.id } };
                    selected_model = new VehicleModelResult { name = vehicle.vehicle_model.name, id = vehicle.vehicle_model.id };
                    selected_sub_model = new VehicleSubModel { name = vehicle.sub_model.name, id = vehicle.sub_model.id };
                    selected_model_year = new VehicleSubModel { model_year = vehicle.model_year.name, id = vehicle.model_year.id };
                    registration_number = vehicle.registration_id;
                    vin_number = vehicle.vin;
                    vehicle_pic = ImageSource.FromUri(new Uri(vehicle.vehicle_picture));
                    is_edit = true;
                }
                else
                {
                    selected_brand = new OemResult { oem = new VehicleOem { name = "Select vehicle brand" } };
                    selected_model = new VehicleModelResult { name = "Select model" };
                    selected_sub_model = new VehicleSubModel { name = "Select sub model" };
                    selected_model_year = new VehicleSubModel { model_year = "Select model year" };
                    is_edit = false;
                    vehicle_pic = "ic_car.png";
                }

                Device.InvokeOnMainThreadAsync(async () =>
                {
                    GetSegmentList(vehicle);
                });
            }
            catch (Exception ex)
            {
            }
        }

        #region Properties
        private string _registration_number;
        public string registration_number
        {
            get => _registration_number;
            set
            {
                _registration_number = value;
                OnPropertyChanged("vehicle_pic");
            }
        }

        private string _vin_number;
        public string vin_number
        {
            get => _vin_number;
            set
            {
                _vin_number = value;
                OnPropertyChanged("vin_number");
            }
        }

        private ObservableCollection<VehicleSegment> _segment_list;
        public ObservableCollection<VehicleSegment> segment_list
        {
            get => _segment_list;
            set
            {
                _segment_list = value;
                OnPropertyChanged("segment_list");
            }
        }

        private Vehicle _selected_vehicle;
        public Vehicle selected_vehicle
        {
            get => _selected_vehicle;
            set
            {
                _selected_vehicle = value;
                OnPropertyChanged("selected_vehicle");
            }
        }

        private VehicleSegment _selected_segment;
        public VehicleSegment selected_segment
        {
            get => _selected_segment;
            set
            {
                _selected_segment = value;
                OnPropertyChanged("selected_segment");
            }
        }

        //private string _vehicle_brand = "Select vehicle brand";
        //public string vehicle_brand
        //{
        //    get => _vehicle_brand;
        //    set
        //    {
        //        _vehicle_brand = value;

        //        if (!string.IsNullOrEmpty(vehicle_brand) && !vehicle_brand.Contains("Select vehicle brand"))
        //        {
        //            brand_text_color = (Color)Application.Current.Resources["text_color"];
        //        }

        //        OnPropertyChanged("vehicle_brand");
        //    }
        //}

        //private string _vehicle_model = "Select model";
        //public string vehicle_model
        //{
        //    get => _vehicle_model;
        //    set
        //    {
        //        _vehicle_model = value;

        //        if (!string.IsNullOrEmpty(vehicle_model) && !vehicle_model.Contains("Select model"))
        //        {
        //            model_text_color = (Color)Application.Current.Resources["text_color"];
        //        }

        //        OnPropertyChanged("vehicle_model");
        //    }
        //}

        //private string _vehicle_sub_model = "Select sub model";
        //public string vehicle_sub_model
        //{
        //    get => _vehicle_sub_model;
        //    set
        //    {
        //        _vehicle_sub_model = value;

        //        if (!string.IsNullOrEmpty(vehicle_sub_model) && !vehicle_sub_model.Contains("Select sub model"))
        //        {
        //            sub_model_text_color = (Color)Application.Current.Resources["text_color"];
        //        }

        //        OnPropertyChanged("vehicle_sub_model");
        //    }
        //}

        //private string _vehicle_model_year = "Select model year";
        //public string vehicle_model_year
        //{
        //    get => _vehicle_model_year;
        //    set
        //    {
        //        _vehicle_model_year = value;

        //        if (!string.IsNullOrEmpty(vehicle_model_year) && !vehicle_model_year.Contains("Select model year"))
        //        {
        //            model_year_text_color = (Color)Application.Current.Resources["text_color"];
        //        }

        //        OnPropertyChanged("vehicle_model_year");
        //    }
        //}

        //private Color _brand_text_color = (Color)Application.Current.Resources["placeholder_color"];
        //public Color brand_text_color
        //{
        //    get => _brand_text_color;
        //    set
        //    {
        //        _brand_text_color = value;
        //        OnPropertyChanged("brand_text_color");
        //    }
        //}

        //private Color _model_text_color = (Color)Application.Current.Resources["placeholder_color"];
        //public Color model_text_color
        //{
        //    get => _model_text_color;
        //    set
        //    {
        //        _model_text_color = value;
        //        OnPropertyChanged("model_text_color");
        //    }
        //}

        //private Color _sub_model_text_color = (Color)Application.Current.Resources["placeholder_color"];
        //public Color sub_model_text_color
        //{
        //    get => _sub_model_text_color;
        //    set
        //    {
        //        _sub_model_text_color = value;
        //        OnPropertyChanged("sub_model_text_color");
        //    }
        //}

        //private Color _model_year_text_color = (Color)Application.Current.Resources["placeholder_color"];
        //public Color model_year_text_color
        //{
        //    get => _model_year_text_color;
        //    set
        //    {
        //        _model_year_text_color = value;
        //        OnPropertyChanged("model_year_text_color");
        //    }
        //}

        private ImageSource _vehicle_pic;
        public ImageSource vehicle_pic
        {
            get => _vehicle_pic;
            set
            {
                _vehicle_pic = value;
                OnPropertyChanged("vehicle_pic");
            }
        }

        private OemResult _selected_brand;
        public OemResult selected_brand
        {
            get => _selected_brand;
            set
            {
                _selected_brand = value;
                OnPropertyChanged("selected_brand");
            }
        }

        private VehicleModelResult _selected_model;
        public VehicleModelResult selected_model
        {
            get => _selected_model;
            set
            {
                _selected_model = value;
                OnPropertyChanged("selected_model");
            }
        }

        private VehicleSubModel _selected_sub_model;
        public VehicleSubModel selected_sub_model
        {
            get => _selected_sub_model;
            set
            {
                _selected_sub_model = value;
                OnPropertyChanged("selected_sub_model");
            }
        }

        private VehicleSubModel _selected_model_year;
        public VehicleSubModel selected_model_year
        {
            get => _selected_model_year;
            set
            {
                _selected_model_year = value;
                OnPropertyChanged("selected_model_year");
            }
        }

        private ObservableCollection<VehicleSubModel> _sub_model_list;
        public ObservableCollection<VehicleSubModel> sub_model_list
        {
            get => _sub_model_list;
            set
            {
                _sub_model_list = value;
                OnPropertyChanged("sub_model_list");
            }
        }

        private ObservableCollection<VehicleSubModel> _model_year_list;
        public ObservableCollection<VehicleSubModel> model_year_list
        {
            get => _model_year_list;
            set
            {
                _model_year_list = value;
                OnPropertyChanged("model_year_list");
            }
        }

        private CreateVehicleRequestModel _vehicle_request_model;
        public CreateVehicleRequestModel vehicle_request_model
        {
            get => _vehicle_request_model;
            set
            {
                _vehicle_request_model = value;
                OnPropertyChanged("vehicle_request_model");
            }
        }
        #endregion

        #region Methods

        [Obsolete]
        public void InitializeCommands()
        {
            MessagingCenter.Subscribe<VehicleBrandViewModel, OemResult>(this, "selected_oem", async (sender, arg) =>
            {
                //vehicle_brand = arg.oem.name;
                selected_brand = arg;
                selected_model = new VehicleModelResult { name = "Select model" };
                selected_sub_model = new VehicleSubModel { name = "Select sub model" };
                selected_model_year = new VehicleSubModel { model_year = "Select model year" };
            });

            MessagingCenter.Subscribe<VehicleModelViewModel, VehicleModelResult>(this, "selected_vehicle_model", async (sender, arg) =>
            {
                //vehicle_model = arg.name;
                selected_model = arg;
                sub_model_list = new ObservableCollection<VehicleSubModel>(arg.sub_models);
                selected_sub_model = new VehicleSubModel { name = "Select sub model" };
                selected_model_year = new VehicleSubModel { model_year = "Select model year" };
            });

            MessagingCenter.Subscribe<VehicleSubModelViewModel, VehicleSubModel>(this, "selected_vehicle_sub_model", async (sender, arg) =>
            {
                //vehicle_sub_model = arg.name;
                selected_sub_model = arg;
                model_year_list = new ObservableCollection<VehicleSubModel>
                {
                    new VehicleSubModel
                    {
                        model_year = arg.model_year,
                        id = arg.id,
                    }
                };
                selected_model_year = new VehicleSubModel { model_year = "Select model year" };
            });

            MessagingCenter.Subscribe<VehicleModelYearViewModel, VehicleSubModel>(this, "selected_vehicle_model_year", async (sender, arg) =>
           {
               ///vehicle_model_year = arg.vehicle_model_year;
               selected_model_year = arg;
           });

            TakeVahicleImageCommand = new Command(async (obj) =>
            {
                try
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

                    vehicle_pic = ImageSource.FromFile(file.Path);
                }
                catch (Exception ex)
                {
                }
            });

            SegmentSelectionCommand = new Command(async (obj) =>
            {
                try
                {
                    selected_segment = (VehicleSegment)obj;

                    foreach (var item in segment_list.ToList())
                    {
                        if (item.id == selected_segment.id)
                        {
                            item.selected_color = (Color)Application.Current.Resources["theme_color"];
                        }
                        else
                        {
                            item.selected_color = Color.White;
                        }
                    }

                }
                catch (Exception ex)
                {
                }
            });

            SelectSegmentCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);
                    //StaticMethods.last_page = "registration";
                    //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.VehicleBrandPopupPage());
                }
            });

            SelectBrandCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);
                    //StaticMethods.last_page = "registration";
                    if (selected_segment == null)
                    {
                        await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Please select segment.", "Alert", "Ok");
                        return;
                    }
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.VehicleBrandPopupPage(selected_segment.id));
                }
            });

            SelectModelCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);

                    if (selected_brand.oem.name == "Select vehicle brand")
                    {
                        await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Please select vehicle brand.", "Alert", "Ok");
                        return;
                    }

                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.ModelPopupPage(selected_brand.oem.name));
                }
            });

            SelectSubModelCommand = new Command(async (obj) =>
            {

                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);

                    if (selected_model.name == "Select model")
                    {
                        await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Please select vehicle model.", "Alert", "Ok");
                        return;
                    }

                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.SubModelPopupPage(sub_model_list));
                }
            });

            SelectModelYearCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);

                    if (selected_sub_model.name == "Select sub model")
                    {
                        await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Please select vehicle sub model.", "Alert", "Ok");
                        return;
                    }

                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.ModelYearPopupPage(model_year_list));
                }
            });

            SubmitCommand = new Command(async (obj) =>
            {
                try
                {
                    CreateVehicleResponseModel response = new CreateVehicleResponseModel();
                    vehicle_request_model.segment = selected_segment.id;
                    vehicle_request_model.oem = selected_brand.oem.id;
                    vehicle_request_model.vehicle_model = selected_model.id;
                    vehicle_request_model.sub_model = selected_sub_model.id;
                    vehicle_request_model.model_year = selected_model_year.id;
                    vehicle_request_model.registration_id = registration_number;
                    vehicle_request_model.vin = vin_number;
                    vehicle_request_model.user = App.user.user;
                    vehicle_request_model.user_type = App.user.user_type_id;
                    var IsError = await Validation();
                    if (!IsError)
                    {
                        using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                        {
                            await Task.Delay(200);



                            //vehicle_request_model.= user_profile_pic;
                            //user_detail.pin_code = pin_code;
                            //user_detail.rs_agent_id = "WK2397950556"; //workshop_detail.code;
                            if (is_edit)
                            {
                                response = await apiServices.UpdateVehicle(file, Preferences.Get("token", null), vehicle_request_model, selected_vehicle.id);
                            }
                            else
                            {
                                response = await apiServices.CreateVehicle(file, Preferences.Get("token", null), vehicle_request_model);
                            }


                            if (response == null)
                            {
                                await page.DisplayAlert("Failed!", "Please check your internet connection.", "Ok");
                                return;
                            }

                            var api_status_code = StaticMethods.http_status_code(response.status_code);

                            if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                            {
                                await page.DisplayAlert("Success!", "Vehicle added successfully!", "Ok");
                                Preferences.Set("token", "");
                                Preferences.Set("IsUpdate", "true");
                                App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                                //await page.Navigation.PopAsync();
                            }
                            else
                            {
                                await page.DisplayAlert(api_status_code, "service not working", "Ok");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            });
        }

        public async void GetSegmentList(Vehicle vehicle)
        {
            try
            {


                var response = await apiServices.GetSegmentList();

                var api_status_code = StaticMethods.http_status_code(response.status_code);

                if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                {

                    foreach (var item in response.results)
                    {
                        switch (item.segment_name)
                        {
                            case "2W":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_010" });
                                break;

                            case "3W":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_04" });
                                break;

                            case "PV":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_011" });
                                break;

                            case "CV":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_02." });
                                break;

                            case "BUS":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_03.png" });
                                break;

                            case "CE":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_05.png" });
                                break;
                            case "GENSET":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_08.png" });
                                break;
                            case "EV":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_09.png" });
                                break;

                            case "MARINE":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_07.png" });
                                break;

                            case "INDMOB":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_01.png" });
                                break;

                            case "ROB":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_06.png" });
                                break;
                        }

                        if (is_edit)
                        {
                            if (vehicle.segment != null)
                            {
                                if (item.segment_name == vehicle.segment.segment_name)
                                {
                                    segment_list.LastOrDefault().selected_color = (Color)Application.Current.Resources["theme_color"];
                                    selected_segment = item;
                                }
                            }
                        }
                    }
                }
                else
                {
                    await page.DisplayAlert(api_status_code, "Create user Api  not working", "Ok");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> Validation()
        {
            bool IsError = false;

            if (file == null)
            {
                await page.DisplayAlert("Alert", "Click your profile image", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(vehicle_request_model.registration_id))
            {
                await page.DisplayAlert("Alert", "Enter vehicle registration number", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(vehicle_request_model.vin))
            {
                await page.DisplayAlert("Alert", "Enter vehicle VIN / Chassis number", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty($"{vehicle_request_model.segment}"))
            {
                await page.DisplayAlert("Alert", "Select segment", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty($"{vehicle_request_model.oem}"))
            {
                await page.DisplayAlert("Alert", "Select oem", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty($"{vehicle_request_model.vehicle_model}"))
            {
                await page.DisplayAlert("Alert", "Select model", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty($"{vehicle_request_model.sub_model}"))
            {
                await page.DisplayAlert("Alert", "Select sub model", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty($"{vehicle_request_model.model_year}"))
            {
                await page.DisplayAlert("Alert", "Select model year", "Ok");
                IsError = true;
            }

            return IsError;
        }
        #endregion

        #region ICommands
        public ICommand SegmentSelectionCommand { get; set; }
        public ICommand SelectSegmentCommand { get; set; }
        public ICommand TakeVahicleImageCommand { get; set; }
        public ICommand SelectBrandCommand { get; set; }
        public ICommand SelectModelCommand { get; set; }
        public ICommand SelectSubModelCommand { get; set; }
        public ICommand SelectModelYearCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        #endregion
    }
}
