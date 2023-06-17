using MiBud.Models;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MiBud.Services
{
    public class ApiServices
    {
        string base_url = "https://wikitek.io/api/v1/";//Original Server
        //string base_url = "http://128.199.17.43/api/v1/";//Test Server
        HttpClient client;


        public ApiServices()
        {
            App.is_update = true;
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
        }

        //public async Task<LoginResponse> UserLogin(LoginModel model)
        //{
        //    HttpResponseMessage httpResponse = new HttpResponseMessage();
        //    try
        //    {
        //        //string Data = string.Empty;
        //        //client = new HttpClient();
        //        var json = JsonConvert.SerializeObject(model);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        httpResponse = await client.PostAsync($"{base_url}users/login/", content);
        //        var Data = httpResponse.Content.ReadAsStringAsync().Result;
        //        var user = JsonConvert.DeserializeObject<LoginResponse>(Data);
        //        user.status_code = httpResponse.StatusCode;
        //        return user;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public async Task<UserResponse> UserRegistration(MediaFile file, UserModel model)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            UserResponse userResponse = new UserResponse();
            try
            {
                // MediaFile _image;

                // code here to assign image to _image


                var content = new MultipartFormDataContent();

                StringContent first_name = new StringContent(model.first_name);
                StringContent last_name = new StringContent(model.last_name);
                StringContent email = new StringContent(model.email);
                StringContent mobile = new StringContent(model.mobile);
                StringContent password = new StringContent(model.password);
                StringContent device_type = new StringContent(model.device_type);
                StringContent mac_id = new StringContent(model.mac_id);
                StringContent user_type = new StringContent(model.user_type);
                StringContent role = new StringContent(model.role);
                //StringContent rs_agent_id = new StringContent(model.rs_agent_id);

                content.Add(new StreamContent(file.GetStream()), "\"user_profile_pic\"", $"{file.Path}");

                content.Add(first_name, "first_name");
                content.Add(last_name, "last_name");
                content.Add(email, "email");
                content.Add(mobile, "mobile");
                content.Add(password, "password");
                content.Add(device_type, "device_type");
                content.Add(mac_id, "mac_id");
                content.Add(user_type, "user_type");
                content.Add(role, "role");
                //content.Add(rs_agent_id, "rsagent_id");

                //var httpClient = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"{base_url}users/register/mibud";
                http_response = await client.PostAsync(url, content);

                var Data = http_response.Content.ReadAsStringAsync().Result;
                userResponse = JsonConvert.DeserializeObject<UserResponse>(Data);
                userResponse.status_code = http_response.StatusCode;
                userResponse.error = Data;
                return userResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<LoginResponse> UserLogin(LoginModel model)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            LoginResponse loginResponse = new LoginResponse();
            string Data = string.Empty;
            try
            {
                var is_network = NetworkConnection();
                bool is_online = false;
                if (!string.IsNullOrEmpty(Preferences.Get("token", null)))
                {
                    is_online = true;
                }
                //if (is_network)
                {
                    if (App.is_update)
                    {
                        var json = JsonConvert.SerializeObject(model);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        httpResponse = await client.PostAsync($"{base_url}users/login/mibud/", content);
                        Data = httpResponse.Content.ReadAsStringAsync().Result;

                        Preferences.Set("LoginResponse", Data);
                    }
                    else
                    {
                        Data = Preferences.Get("LoginResponse", null);
                    }
                    loginResponse = JsonConvert.DeserializeObject<LoginResponse>(Data);
                    loginResponse.status_code = httpResponse.StatusCode;
                    Preferences.Set("user_name", model.email);
                    Preferences.Set("password", model.password);
                    Preferences.Set("token", loginResponse.token?.access);
                    Application.Current.Properties["refresh_token"] = App.refresh_token = loginResponse.token?.refresh;
                    Application.Current.Properties["access_token"] = App.access_token = loginResponse.token?.access;
                    Application.Current.Properties["user_mail"] = App.user_mail = loginResponse?.email;
                    App.user_first_name = loginResponse?.first_name;
                    App.user_last_name = loginResponse?.last_name;
                    await Application.Current.SavePropertiesAsync();
                    App.user_id = loginResponse?.user;
                    App.user_type = loginResponse?.user_type;
                    App.country_id = loginResponse?.agent?.workshop?.country;

                    if (is_online)
                    {
                        if (loginResponse.subscriptions != null)
                        {
                            App.is_update = false;
                            Preferences.Set("IsUpdate", "false");
                            if (loginResponse.subscriptions.Count > 0)
                            {
                                App.is_update = false;
                                Preferences.Set("IsUpdate", "false");
                            }
                            else
                            {
                                App.is_update = true;
                                Preferences.Set("IsUpdate", "true");
                            }
                        }
                        else
                        {
                            App.is_update = true;
                            Preferences.Set("IsUpdate", "true");
                        }

                        if (loginResponse.dongles != null)
                        {
                            App.is_update = false;
                            Preferences.Set("IsUpdate", "false");
                            if (loginResponse.dongles.Count > 0)
                            {
                                App.is_update = false;
                                Preferences.Set("IsUpdate", "false");
                            }
                            else
                            {
                                App.is_update = true;
                                Preferences.Set("IsUpdate", "true");
                            }
                        }
                        else
                        {
                            App.is_update = true;
                            Preferences.Set("IsUpdate", "true");
                        }
                    }
                    return loginResponse;

                }
                //else
                //{
                //    loginResponse.error = "No internet connection";
                //    return loginResponse;
                //}
            }
            catch (Exception ex)
            {
                loginResponse.error = Data;
                loginResponse.status_code = httpResponse.StatusCode;
                //if(ex.Message.Contains("Failed to connect"))
                //{
                //    //loginResponse.status_code;
                //    loginResponse.error = "Please check device internet connection.";

                //}
                //else
                //{
                //    loginResponse.status_code = httpResponse.StatusCode;
                //    loginResponse.error = "Error inside login service";
                //}

                //Debug.WriteLine("Error - inside login api");
                return loginResponse;
            }
            #endregion
        }

        public async Task<CountyModel> Countries(string name)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                string url = $"{base_url}locations/get-country/?name={name}";
                //httpResponse = await client.GetAsync(url);
                httpResponse = await client.GetAsync(url);
                Data = httpResponse.Content.ReadAsStringAsync().Result;

                var countries = JsonConvert.DeserializeObject<CountyModel>(Data);
                countries.status_code = httpResponse.StatusCode;
                return countries;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - inside get country api");
                return null;
            }
        }

        public async Task<AgentModel> GetAgentList(string type, int country, string pin_code)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                //client = new HttpClient();
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.GetAsync($"{base_url}workshops/get-agent-list/?name={type}&country={country}&pincode={pin_code}");
                //api/v1/workshops/get-agent-list/?name=rsangelMechanik&country=1&pincode=515843
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var agents = JsonConvert.DeserializeObject<AgentModel>(Data);
                agents.status_code = httpResponse.StatusCode;
                return agents;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<SegmentModel> GetSegmentList()
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                //client = new HttpClient();
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.GetAsync($"{base_url}models/segments/");
                //api/v1/workshops/get-agent-list/?name=rsangelMechanik&country=1&pincode=515843
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var agents = JsonConvert.DeserializeObject<SegmentModel>(Data);
                agents.status_code = httpResponse.StatusCode;
                return agents;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<RsUserTypePincode> DistricAndState(string pin_code, int country_id, string name)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                //client = new HttpClient();
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.GetAsync($"{base_url}locations/get-geography-list/?name={name}&pincode={pin_code}&country={country_id}");
                //http://15.206.157.39:8080/api/v1/locations/get-geography-list/?name=515413&country=4&name=other
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var distric_state = JsonConvert.DeserializeObject<DistricModel>(Data);
                distric_state.status_code = httpResponse.StatusCode;
                var val = distric_state.results.First().rs_user_type_pincode.First();
                return val;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public async Task<CreateRsAgentResponse> CreateRSAgent(CreateRSAgentModel model)
        //{
        //    HttpResponseMessage httpResponse = new HttpResponseMessage();
        //    try
        //    {
        //        string Data = string.Empty;
        //        //client = new HttpClient();
        //        var json = JsonConvert.SerializeObject(model);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
        //        httpResponse = await client.PostAsync($"{base_url}workshops/agent-create/", content);
        //        Data = httpResponse.Content.ReadAsStringAsync().Result;
        //        var countries = JsonConvert.DeserializeObject<CreateRsAgentResponse>(Data);
        //        countries.status_code = httpResponse.StatusCode;
        //        return countries;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public async Task<VehicleModel> GetModel(string token, int id)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                //client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                http_response = await client.GetAsync($"{base_url}models/get-models/");
                Data = http_response.Content.ReadAsStringAsync().Result;
                var vahicle_all_models_list = JsonConvert.DeserializeObject<VehicleModel>(Data);
                return vahicle_all_models_list;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PidModel> GetPid(string token, int id)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                //client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                http_response = await client.GetAsync($"{base_url}datasets/get-pid-datasets/?id={id}");
                Data = http_response.Content.ReadAsStringAsync().Result;
                var pid_list = JsonConvert.DeserializeObject<PidModel>(Data);
                return pid_list;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<OemModel> GetVehicleBrand(string token, int segment_id, bool is_updated)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                if (is_updated)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                    string url = $"{base_url}models/segments-oem/";
                    http_response = await client.GetAsync(url);
                    //http_response = await client.GetAsync($"{base_url}models/segments-oem/?segments_id={segment_id}");
                    Data = http_response.Content.ReadAsStringAsync().Result;
                    Preferences.Set("OemList", Data);
                }
                else
                {
                    Data = Preferences.Get("OemList", null);
                }
                var oems = JsonConvert.DeserializeObject<OemModel>(Data);
                oems.status_code = http_response.StatusCode;
                oems.results = oems.results.Where(x => x.segment_name.id == segment_id).ToList();
                return oems;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<VehicleModel> GetVehicleModel(string token, string oem_name, bool is_update)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                if (is_update)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                    string url = $"{base_url}models/models/";
                    http_response = await client.GetAsync(url);
                    //http_response = await client.GetAsync($"{base_url}models/models/?oem={oem_name}");
                    Data = http_response.Content.ReadAsStringAsync().Result;
                    Preferences.Set("ModelList", Data);
                }
                else
                {
                    Data = Preferences.Get("ModelList", null);
                }
                var vahicle_all_models_list = JsonConvert.DeserializeObject<VehicleModel>(Data);
                vahicle_all_models_list.status_code = http_response.StatusCode;

                vahicle_all_models_list.results = vahicle_all_models_list.results.Where(x => x.oem.name.Contains(oem_name)).ToList();
                return vahicle_all_models_list;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CreateVehicleResponseModel> CreateVehicle(MediaFile file, string token, CreateVehicleRequestModel model)
        {
            CreateVehicleResponseModel createVehicleResponseModel = new CreateVehicleResponseModel();
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                // MediaFile _image;

                // code here to assign image to _image


                var content = new MultipartFormDataContent();

                StringContent registration_id = new StringContent(model.registration_id);
                StringContent vin = new StringContent(model.vin);
                StringContent segment = new StringContent($"{model.segment}");
                StringContent vehicle_model = new StringContent($"{model.vehicle_model}");
                StringContent sub_model = new StringContent($"{model.sub_model}");
                StringContent model_year = new StringContent($"{model.model_year}");
                StringContent user = new StringContent($"{model.user}");
                StringContent oem = new StringContent($"{model.oem}");

                content.Add(new StreamContent(file.GetStream()), "\"picture\"", $"{file.Path}");

                content.Add(registration_id, "registration_id");
                content.Add(vin, "vin");
                content.Add(segment, "segment");
                content.Add(vehicle_model, "vehicle_model");
                content.Add(sub_model, "sub_model");
                content.Add(model_year, "model_year");
                content.Add(user, "user");
                content.Add(oem, "oem");

                //var httpClient = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"{base_url}vehicles/create/";
                http_response = await client.PostAsync(url, content);

                var Data = http_response.Content.ReadAsStringAsync().Result;
                createVehicleResponseModel.status_code = http_response.StatusCode;
                return createVehicleResponseModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CreateVehicleResponseModel> UpdateVehicle(MediaFile file, string token, CreateVehicleRequestModel model, string id)
        {
            CreateVehicleResponseModel createVehicleResponseModel = new CreateVehicleResponseModel();
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                // MediaFile _image;

                // code here to assign image to _image


                var content = new MultipartFormDataContent();

                StringContent registration_id = new StringContent(model.registration_id);
                StringContent vin = new StringContent(model.vin);
                StringContent segment = new StringContent($"{model.segment}");
                StringContent vehicle_model = new StringContent($"{model.vehicle_model}");
                StringContent sub_model = new StringContent($"{model.sub_model}");
                StringContent model_year = new StringContent($"{model.model_year}");
                StringContent user = new StringContent($"{model.user}");
                StringContent oem = new StringContent($"{model.oem}");

                content.Add(new StreamContent(file.GetStream()), "\"picture\"", $"{file.Path}");

                content.Add(registration_id, "registration_id");
                content.Add(vin, "vin");
                content.Add(segment, "segment");
                content.Add(vehicle_model, "vehicle_model");
                content.Add(sub_model, "sub_model");
                content.Add(model_year, "model_year");
                content.Add(user, "user");
                content.Add(oem, "oem");

                //var httpClient = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"{base_url}vehicles/update/{id}/";
                http_response = await client.PutAsync(url, content);

                var Data = http_response.Content.ReadAsStringAsync().Result;
                createVehicleResponseModel.status_code = http_response.StatusCode;
                return createVehicleResponseModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<DtcResult> GetDtc(string token, int id, bool is_updated)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                //http_response = await client.GetAsync($"{base_url}datasets/dtc-dataset/{id}/");
                //Data = http_response.Content.ReadAsStringAsync().Result;

                if (is_updated)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                    string url = $"{base_url}datasets/dtc-dataset/";
                    http_response = await client.GetAsync(url);
                    Data = http_response.Content.ReadAsStringAsync().Result;
                    Preferences.Set("DtcList", Data);
                }
                else
                {
                    Data = Preferences.Get("DtcList", null);
                }

                var dtc_list = JsonConvert.DeserializeObject<DtcModel>(Data);
                return dtc_list.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PidResult> GetPid(string token, int id, bool is_updated)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;

                if (is_updated)
                {
                    //client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                    string url = $"{base_url}datasets/pid-dataset/";
                    http_response = await client.GetAsync(url);
                    Data = http_response.Content.ReadAsStringAsync().Result;
                    Preferences.Set("PidList", Data);
                }
                else
                {
                    Data = Preferences.Get("PidList", null);
                }

                var pid_list = JsonConvert.DeserializeObject<PidModel>(Data);
                return pid_list.results.FirstOrDefault(x => x.id == id);//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<RegDongleRespons> RegisterDongle(RegisterDongleModel registerDongleModel, string token)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                ////client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                var json = JsonConvert.SerializeObject(registerDongleModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                http_response = await client.PostAsync($"{base_url}devices/register/device/mibud/", content);
                var Data = http_response.Content.ReadAsStringAsync().Result;
                var dongle_respons = JsonConvert.DeserializeObject<RegDongleRespons>(Data);
                dongle_respons.status_code = http_response.StatusCode;
                return dongle_respons;
            }
            catch (Exception ex)
            {
                //Application.Current.MainPage.DisplayAlert("Alert", ex.StackTrace, "Ok");
                return null;
            }
        }

        public async Task<ErroMsg> ApproveTransport(ApproveModel model, string token)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                ////client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                http_response = await client.PostAsync($"{base_url}analyze/approved-transport/", content);
                var Data = http_response.Content.ReadAsStringAsync().Result;
                var results = JsonConvert.DeserializeObject<ErroMsg>(Data);
                return results;
            }
            catch (Exception ex)
            {
                //Application.Current.MainPage.DisplayAlert("Alert", ex.StackTrace, "Ok");
                return null;
            }
        }

        public async Task<ErroMsg> ClosedByCustomer(ApproveModel model, string token)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                ////client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                http_response = await client.PostAsync($"{base_url}analyze/closedby-customer/", content);
                var Data = http_response.Content.ReadAsStringAsync().Result;
                var results = JsonConvert.DeserializeObject<ErroMsg>(Data);
                return results;
            }
            catch (Exception ex)
            {
                //Application.Current.MainPage.DisplayAlert("Alert", ex.StackTrace, "Ok");
                return null;
            }
        }

        public async Task<ErroMsg> RejectedTransport(RejectModel model, string token)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                ////client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                http_response = await client.PostAsync($"{base_url}analyze/rejected-transport/", content);
                var Data = http_response.Content.ReadAsStringAsync().Result;
                var results = JsonConvert.DeserializeObject<ErroMsg>(Data);
                return results;
            }
            catch (Exception ex)
            {
                //Application.Current.MainPage.DisplayAlert("Alert", ex.StackTrace, "Ok");
                return null;
            }
        }

        public async Task<ErroMsg> ClosedDisapproved(RejectModel model, string token)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                ////client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                http_response = await client.PostAsync($"{base_url}analyze/closed-disapproved/", content);
                var Data = http_response.Content.ReadAsStringAsync().Result;
                var results = JsonConvert.DeserializeObject<ErroMsg>(Data);
                return results;
            }
            catch (Exception ex)
            {
                //Application.Current.MainPage.DisplayAlert("Alert", ex.StackTrace, "Ok");
                return null;
            }
        }



        public async Task<VehicleModel> PickupApproved(string token, string jobcard_id)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            VehicleModel model = new VehicleModel();
            //client = new HttpClient();
            string Data = string.Empty;
            string json = string.Empty;
            string url = string.Empty;

            try
            {
                //url = $"{base_url}pauthor/get-service-list/?id={id}&sub_model={submodel_id}";
                url = $"{base_url}analyze/jcstatus-approved";
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                json = "{ \"id\":\"" + jobcard_id + "\" }";//JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync(url, content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.success = false;
                    model.error = httpResponse.StatusCode.ToString();
                    return model;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"{Data}", "Relay Command Response");
                model = JsonConvert.DeserializeObject<VehicleModel>(Data);
                model.error = httpResponse.StatusCode.ToString();
                model.success = true;
                return model;
            }
            catch (Exception ex)
            {
                model.success = false;
                //model.status = 1000;
                model.error = ex.Message;
                return model;
            }
        }

        public async Task<VehicleModel> PickupReject(string token, string jobcard_id)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            VehicleModel model = new VehicleModel();
            //client = new HttpClient();
            string Data = string.Empty;
            string json = string.Empty;
            string url = string.Empty;

            try
            {
                //url = $"{base_url}pauthor/get-service-list/?id={id}&sub_model={submodel_id}";
                url = $"{base_url}analyze/reject-pickupdrop";
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                json = "{ \"id\":\"" + jobcard_id + "\" }";//JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync(url, content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.success = false;
                    model.error = httpResponse.StatusCode.ToString();
                    return model;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"{Data}", "Relay Command Response");
                model = JsonConvert.DeserializeObject<VehicleModel>(Data);
                model.error = httpResponse.StatusCode.ToString();
                model.success = true;
                return model;
            }
            catch (Exception ex)
            {
                model.success = false;
                //model.status = 1000;
                model.error = ex.Message;
                return model;
            }
        }



        #region RESET PASSWORD

        public async Task<ResetPasswordModel> SendEmailToServer(string email_id)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            ResetPasswordModel resetPassword = new ResetPasswordModel();
            try
            {
                //string Data = string.Empty;
                //client = new HttpClient();
                var json = "{ \"email\":\"" + email_id + "\" }";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{base_url}users/password/forgot", content);
                var Data = httpResponse.Content.ReadAsStringAsync().Result;
                resetPassword = JsonConvert.DeserializeObject<ResetPasswordModel>(Data);
                resetPassword.status_code = httpResponse.StatusCode;
                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    resetPassword.error = Data;
                }
                return resetPassword;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<VerifyOtpResponseModel> VerifyOtp(VerifyOtpRequestModel model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            VerifyOtpResponseModel verifyOtpResponse = new VerifyOtpResponseModel();
            try
            {
                //string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{base_url}users/password/verify", content);
                var Data = httpResponse.Content.ReadAsStringAsync().Result;
                verifyOtpResponse = JsonConvert.DeserializeObject<VerifyOtpResponseModel>(Data);
                verifyOtpResponse.status_code = httpResponse.StatusCode;
                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    verifyOtpResponse.error = Data;
                }
                return verifyOtpResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<VerifyOtpResponseModel> ResetPassword(string url, string password)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            VerifyOtpResponseModel verifyOtpResponse = new VerifyOtpResponseModel();
            try
            {
                //string Data = string.Empty;
                //client = new HttpClient();
                var json = "{ \"password\":\"" + password + "\" }";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{url}", content);
                var Data = httpResponse.Content.ReadAsStringAsync().Result;
                verifyOtpResponse = JsonConvert.DeserializeObject<VerifyOtpResponseModel>(Data);
                verifyOtpResponse.status_code = httpResponse.StatusCode;
                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    verifyOtpResponse.error = Data;
                }
                return verifyOtpResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        public async Task<SymptomsModel> GetSymp(string token, int submodel_id)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            SymptomsModel model = new SymptomsModel();
            //client = new HttpClient();
            string Data = string.Empty;
            string json = string.Empty;
            string url = string.Empty;

            try
            {

                //url = $"{base_url}symptom/symptom-list/?s_model={submodel_id}";
                url = $"{base_url}symptom/symptom-mapping/";
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpResponse = await client.GetAsync(url);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.success = false;
                    model.status = httpResponse.StatusCode.ToString();
                    return model;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"{Data}", "Relay Command Response");
                model = JsonConvert.DeserializeObject<SymptomsModel>(Data);
                model.status = httpResponse.StatusCode.ToString();
                model.success = true;
                return model;
            }
            catch (Exception ex)
            {
                model.success = false;
                //model.status = 1000;
                model.status = ex.Message;
                return model;
            }
        }

        public async Task<ServicesModel> GetServices(string token, int id, int submodel_id)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            ServicesModel model = new ServicesModel();
            //client = new HttpClient();
            string Data = string.Empty;
            string json = string.Empty;
            string url = string.Empty;

            try
            {
                //url = $"{base_url}pauthor/get-service-list/?id={id}&sub_model={submodel_id}";
                url = $"{base_url}pauthor/get-services-mapping/";
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpResponse = await client.GetAsync(url);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.success = false;
                    model.status = httpResponse.StatusCode.ToString();
                    return model;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"{Data}", "Relay Command Response");
                model = JsonConvert.DeserializeObject<ServicesModel>(Data);
                model.status = httpResponse.StatusCode.ToString();
                model.success = true;
                return model;
            }
            catch (Exception ex)
            {
                model.success = false;
                //model.status = 1000;
                model.status = ex.Message;
                return model;
            }
        }

        public async Task<WorkshopModel> GetWorkshop(string token, string userType)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            WorkshopModel model = new WorkshopModel();
            //client = new HttpClient();
            string Data = string.Empty;
            string json = string.Empty;
            string url = string.Empty;

            try
            {
                url = $"{base_url}workshops/workshops-get/?user_type=" + userType;
                httpResponse = await client.GetAsync(url);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.success = false;
                    model.status = httpResponse.StatusCode.ToString();
                    return model;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"{Data}", "Relay Command Response");
                model = JsonConvert.DeserializeObject<WorkshopModel>(Data);
                model.status = httpResponse.StatusCode.ToString();
                model.success = true;
                return model;
            }
            catch (Exception ex)
            {
                model.success = false;
                //model.status = 1000;
                model.status = ex.Message;
                return model;
            }
        }

        public async Task<JobcardNumber> GenerateJobcardNumber(string token)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            JobcardNumber model = new JobcardNumber();
            //client = new HttpClient();
            string Data = string.Empty;
            string json = string.Empty;
            string url = string.Empty;

            try
            {
                url = $"{base_url}analyze/gen-wikitek-jc";
                httpResponse = await client.GetAsync(url);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.success = false;
                    model.name = httpResponse.StatusCode.ToString();
                    return model;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"{Data}", "Relay Command Response");
                model = JsonConvert.DeserializeObject<JobcardNumber>(Data);
                //model.name = httpResponse.StatusCode.ToString();
                model.success = true;
                return model;
            }
            catch (Exception ex)
            {
                model.success = false;
                //model.status = 1000;
                model.name = ex.Message;
                return model;
            }
        }

        //public async Task<JobcardNumber> CreateJobcart(string token, CreateJobcardModel req_model)
        //{
        //    HttpResponseMessage httpResponse = new HttpResponseMessage();
        //    JobcardNumber model = new JobcardNumber();
        //    //client = new HttpClient();
        //    string Data = string.Empty;
        //    string json = string.Empty;
        //    string url = string.Empty;

        //    try
        //    {
        //        url = $"{base_url}analyze/create-mibud/";
        //         json = JsonConvert.SerializeObject(req_model);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        httpResponse = await client.PostAsync(url, content);

        //        if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
        //        {
        //            model.success = false;
        //            model.name = httpResponse.StatusCode.ToString();
        //            return model;
        //        }

        //        Data = httpResponse.Content.ReadAsStringAsync().Result;
        //        Debug.WriteLine($"{Data}", "Relay Command Response");
        //        model = JsonConvert.DeserializeObject<JobcardNumber>(Data);
        //        model.name = httpResponse.StatusCode.ToString();
        //        model.success = true;
        //        return model;
        //    }
        //    catch (Exception ex)
        //    {
        //        model.success = false;
        //        //model.status = 1000;
        //        model.name = ex.Message;
        //        return model;
        //    }
        //}

        public async Task<CreateJobcardResponse> CreateJobcard(string token, CreateJobcardModel model)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            CreateJobcardResponse loginResponse = new CreateJobcardResponse();
            string Data = string.Empty;


            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync($"{base_url}analyze/create-mibud/", content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    loginResponse.success = false;
                    loginResponse.error = httpResponse.StatusCode.ToString();
                    return loginResponse;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                loginResponse = JsonConvert.DeserializeObject<CreateJobcardResponse>(Data);
                loginResponse.success = true;
                return loginResponse;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                loginResponse.success = false;
                loginResponse.error = "Check internet connectivity";
                return loginResponse;
            }
            catch (Exception ex)
            {
                loginResponse.success = false;
                loginResponse.error = ex.Message;
                return loginResponse;
            }
            #endregion
        }

        public async Task<JobcardModel> GetJobcardList(string token)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            JobcardModel model = new JobcardModel();
            string Data = string.Empty;
            string url = base_url + $"analyze/jobcard-details/?registration_id=" + App.selected_vehicle
                + "&user_type=" + App.selected_vehicle_Service;
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                http_response = await client.GetAsync(url);

                if (http_response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.status = false;
                    model.message = http_response.StatusCode.ToString();
                    return model;
                }

                Data = http_response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<JobcardModel>(Data);
                model.status = true;
                return model;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                model.status = false;
                model.message = ex.Message;
                return model;
            }
        }

        public async Task<JobcardModel> GetJobcardDetail(string token, string id)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            JobcardModel model = new JobcardModel();
            string Data = string.Empty;
            string url = base_url + $"analyze/jobcard-details/?id={id}";
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                http_response = await client.GetAsync(url);

                if (http_response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.status = false;
                    model.message = http_response.StatusCode.ToString();
                    return model;
                }

                Data = http_response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine(Data, "Jobacad-detail");
                model = JsonConvert.DeserializeObject<JobcardModel>(Data);
                model.status = true;
                return model;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                model.status = false;
                model.message = ex.Message;
                return model;
            }
        }

        public string ReadTextFile(string FileName)
        {
            try
            {
                Stream stream = this.GetType().Assembly.GetManifestResourceStream("MiBud.JsonFiles." + FileName);
                string text = "";
                using (var reader = new System.IO.StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
                return text;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public bool NetworkConnection()
        {
            bool is_network = false;
            var current = Connectivity.NetworkAccess;
            var profiles = Connectivity.ConnectionProfiles;
            if (current == NetworkAccess.Internet)
            {
                is_network = true;
            }

            return is_network;
        }


    }
}
