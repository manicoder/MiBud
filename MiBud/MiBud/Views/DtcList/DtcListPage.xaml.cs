using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiBud.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.DtcList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DtcListPage : ContentView
    {
       DtcViewModel viewModel;
        public DtcListPage()
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new DtcViewModel(null);
            }
            catch (Exception ex)
            {
            }
        }

     
        private async void GD_Clicked(object sender, EventArgs e)
        { }

        ////private async void GD_Clicked(object sender, EventArgs e)
        ////{
        ////    try
        ////    {

        ////        using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
        ////        {
        ////            await Task.Delay(100);
        ////            string model = string.Empty;
        ////            var selectedItem = (DtcModel)((Button)sender).BindingContext;
        ////            //var ecuName = GetECU(selectedItem.NodeId.Id);
        ////            if (selectedItem.PCode == "P0190-13")
        ////            {
        ////                //model = $"{App.SelectedModel}_NA_NA_EMS";
        ////                model = "Pro6000 MDE5 And 8 BSVI_NA_NA_EMS";
        ////            }
        ////            else
        ////            {
        ////                //model = $"{App.SelectedModel}_NA_NA_EMS";
        ////                model = "Pro2000 E366 DELPHI BSVI_NA_NA_EMS";
        ////            }
        ////            var json = DependencyService.Get<IGdLocalFile>().GetGdData().Result;
        ////            //Debug.WriteLine("CatchGDData   "+ json);
        ////            var result = GetGdauthor(App.JwtToken, selectedItem.PCode, model, json);
        ////            //var result = apiServices.GetGdauthor(App.JwtToken, selectedItem.Identifier.FailureName, model, "GdJson.txt");
        ////            if (result != null)
        ////            {
        ////                await this.Navigation.PushAsync(new InfoPage(result.Result, selectedItem.Description, selectedItem.PCode));
        ////            }
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////    }

        ////}
        ////public async Task<GdMainModel> GetGdauthor(string token, string PCode, string model, string json)
        ////{
        ////    try
        ////    {


        ////        //DataModel dataModel = new DataModel();

        ////        GdMainModel gdMainModel = new GdMainModel();
        ////        TreeClass treeClass1 = new TreeClass();



        ////        //TreeClass treeClass = new TreeClass();
        ////        //string json = ReadTextFile(FileName);
        ////        //client = new HttpClient();
        ////        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
        ////        //var response = client.GetAsync(BaseUrl + "oem/get-gdauthor/").Result;
        ////        //var json = response.Content.ReadAsStringAsync().Result;
        ////        dynamic list = JsonConvert.DeserializeObject(json);
        ////        var info = list["gauther"][model][PCode];

        ////        if (info == null)
        ////        {
        ////            DisplayAlert("Alert", "This P Code not available", "OK");
        ////            return null;
        ////        }
        ////        else
        ////        {
        ////            foreach (var pair in info)
        ////            {
        ////                var InfoKey = pair.Name;
        ////                var InfoValue = pair.Value;
        ////                if (pair.Name == "gd_id")
        ////                {
        ////                    gdMainModel.gd_id = pair.Value.Value;
        ////                }
        ////                else if (pair.Name == "info")
        ////                {
        ////                    foreach (var infoPair in pair.Value)
        ////                    {
        ////                        if (infoPair.Name == "causes")
        ////                        {
        ////                            gdMainModel.info.causes = infoPair.Value.Value;
        ////                        }
        ////                        else if (infoPair.Name == "effects_on_vehicle")
        ////                        {
        ////                            var re = infoPair.Value;
        ////                            gdMainModel.info.effects_on_vehicle = infoPair.Value.Value;
        ////                        }
        ////                        else if (infoPair.Name == "remedial_actions")
        ////                        {
        ////                            gdMainModel.info.remedial_actions = infoPair.Value.Value;
        ////                        }
        ////                    }
        ////                }
        ////                else if (pair.Name == "tree")
        ////                {
        ////                    foreach (var treePair in pair.Value)
        ////                    {
        ////                        foreach (var pairr in treePair)
        ////                        {
        ////                            ser serr = new ser();
        ////                            treeClass1.TreeName = pairr.Name;

        ////                            var Name = pairr.Name;
        ////                            DecisionData decision = new DecisionData();
        ////                            foreach (var valueItem in pairr.Value)
        ////                            {
        ////                                ReapeterClass reapeterClass = new ReapeterClass();
        ////                                foreach (var valueIt in valueItem)
        ////                                {
        ////                                    if (valueIt.Name == "id")
        ////                                    {
        ////                                        serr.id = valueIt.Value.Value;
        ////                                    }
        ////                                    else if (valueIt.Name == "name")
        ////                                    {
        ////                                        serr.name = valueIt.Value.Value;
        ////                                    }
        ////                                    else if (valueIt.Name == "parent")
        ////                                    {
        ////                                        serr.parent = valueIt.Value.Value;
        ////                                    }
        ////                                    else if (valueIt.Name == "description")
        ////                                    {
        ////                                        serr.description = valueIt.Value.Value;
        ////                                    }
        ////                                    else if (valueIt.Name == "data")
        ////                                    {
        ////                                        foreach (var data in valueIt)
        ////                                        {

        ////                                            foreach (var ReData in data)
        ////                                            {
        ////                                                if (ReData.Name == "decisions")
        ////                                                {
        ////                                                    serr.description = valueIt.Value.Value;
        ////                                                    foreach (var deci in ReData)
        ////                                                    {
        ////                                                        foreach (var deci1 in deci)
        ////                                                        {

        ////                                                            if (deci1.Name == "type")
        ////                                                            {
        ////                                                                serr.data.decisions.type = deci1.Value.Value;
        ////                                                            }
        ////                                                            if (deci1.Name == "data")
        ////                                                            {

        ////                                                                foreach (var deci2 in deci1)
        ////                                                                {

        ////                                                                    foreach (var deci3 in deci2)
        ////                                                                    {
        ////                                                                        DecissionListModel decissionListModel = new DecissionListModel();
        ////                                                                        foreach (var deci4 in deci3)
        ////                                                                        {
        ////                                                                            if (deci4.Name == "node")
        ////                                                                            {
        ////                                                                                decision.node = deci4.Value.Value;
        ////                                                                                decissionListModel.node = deci4.Value.Value;
        ////                                                                            }
        ////                                                                            if (deci4.Name == "text_val")
        ////                                                                            {
        ////                                                                                decision.text_val = deci4.Value.Value;
        ////                                                                                decissionListModel.text_val = deci4.Value.Value;
        ////                                                                            }
        ////                                                                            if (deci4.Name == "type")
        ////                                                                            {
        ////                                                                                decision.type = deci4.Value.Value;
        ////                                                                                decissionListModel.type = deci4.Value.Value;
        ////                                                                            }
        ////                                                                        }

        ////                                                                        reapeterClass.data.decisions.data.Add(decissionListModel);
        ////                                                                    }

        ////                                                                }

        ////                                                            }
        ////                                                        }
        ////                                                        reapeterClass.data.decisions.type = serr.data.decisions.type;
        ////                                                        //reapeterClass.data.decisions.data = serr.data.decisions.data;
        ////                                                    }
        ////                                                }
        ////                                                else if (ReData.Name == "type_form")
        ////                                                {
        ////                                                    foreach (var TypeFormData in ReData)
        ////                                                    {
        ////                                                        foreach (var ReTypeForm in TypeFormData)
        ////                                                        {
        ////                                                            if (ReTypeForm.Name == "topic")
        ////                                                            {
        ////                                                                serr.data.type_form.topic = ReTypeForm.Value.Value;
        ////                                                            }
        ////                                                            else if (ReTypeForm.Name == "description")
        ////                                                            {
        ////                                                                serr.data.type_form.description = ReTypeForm.Value.Value;
        ////                                                            }
        ////                                                            else if (ReTypeForm.Name == "groups")
        ////                                                            {
        ////                                                                foreach (var group in ReTypeForm)
        ////                                                                {
        ////                                                                    foreach (var group1 in group)
        ////                                                                    {
        ////                                                                        GroupModel groupModel = new GroupModel();
        ////                                                                        Group gr = new Group();
        ////                                                                        foreach (var group2 in group1)
        ////                                                                        {
        ////                                                                            if (group2.Name == "upper_limit")
        ////                                                                            {
        ////                                                                                gr.upper_limit = group2.Value.Value;
        ////                                                                            }
        ////                                                                            else if (group2.Name == "lower_limit")
        ////                                                                            {
        ////                                                                                gr.lower_limit = group2.Value.Value;
        ////                                                                            }
        ////                                                                            else if (group2.Name == "unit")
        ////                                                                            {
        ////                                                                                gr.unit = group2.Value.Value;
        ////                                                                            }
        ////                                                                            else if (group2.Name == "entry_description")
        ////                                                                            {
        ////                                                                                gr.entry_description = group2.Value.Value;
        ////                                                                            }
        ////                                                                            else if (group2.Name == "group_name")
        ////                                                                            {
        ////                                                                                gr.group_name = group2.Value.Value;
        ////                                                                            }
        ////                                                                        }
        ////                                                                        serr.data.type_form.groups.Add(gr);
        ////                                                                        groupModel.entry_description = gr.entry_description;
        ////                                                                        groupModel.lower_limit = gr.lower_limit;
        ////                                                                        groupModel.unit = gr.unit;
        ////                                                                        groupModel.upper_limit = gr.upper_limit;
        ////                                                                        groupModel.group_name = gr.group_name;
        ////                                                                        reapeterClass.data.type_form.groups.Add(groupModel);
        ////                                                                    }
        ////                                                                }
        ////                                                            }
        ////                                                            //else if (ReTypeForm.Name == "entry_group_names")
        ////                                                            //{
        ////                                                            //    foreach (var groupName in ReTypeForm)
        ////                                                            //    {
        ////                                                            //        foreach (var groupName1 in groupName)
        ////                                                            //        {
        ////                                                            //            var ite = groupName1[1].Value;
        ////                                                            //            foreach (var groupName2 in groupName1)
        ////                                                            //            {
        ////                                                            //                serr.data.type_form.entry_group_names = groupName2.Value.Value;
        ////                                                            //            }
        ////                                                            //        }
        ////                                                            //        //foreach (var groupName1 in groupName)
        ////                                                            //        //{
        ////                                                            //        //    foreach (var groupName2 in ReTypeForm)
        ////                                                            //        //    {
        ////                                                            //        //        serr.data.type_form.entry_group_names = groupName2.Value.Value;
        ////                                                            //        //    }
        ////                                                            //        //}
        ////                                                            //    }
        ////                                                            //}
        ////                                                        }
        ////                                                        reapeterClass.data.type_form.topic = serr.data.type_form.topic;
        ////                                                        reapeterClass.data.type_form.description = serr.data.type_form.description;
        ////                                                        reapeterClass.data.type_form.entry_group_names = serr.data.type_form.entry_group_names;


        ////                                                    }
        ////                                                }
        ////                                                else if (ReData.Name == "entry_script")
        ////                                                {
        ////                                                    serr.data.entry_script = ReData.Value.Value;
        ////                                                }
        ////                                                else if (ReData.Name == "id")
        ////                                                {
        ////                                                    serr.data.id = ReData.Value.Value;
        ////                                                }
        ////                                                else if (ReData.Name == "description")
        ////                                                {
        ////                                                    serr.data.description = ReData.Value.Value;
        ////                                                }
        ////                                                else if (ReData.Name == "topic")
        ////                                                {
        ////                                                    serr.data.topic = ReData.Value.Value;
        ////                                                }

        ////                                            }

        ////                                            //reapeterClass.data.type_form = serr.data.type_form;
        ////                                            reapeterClass.data.description = serr.data.description;
        ////                                            //reapeterClass.data.decisions = serr.data.decisions;
        ////                                            reapeterClass.data.is_active = serr.data.is_active;
        ////                                            reapeterClass.data.exit_script = serr.data.exit_script;
        ////                                            reapeterClass.data.topic = serr.data.topic;
        ////                                            reapeterClass.data.entry_script = serr.data.entry_script;
        ////                                            reapeterClass.data.globals = serr.data.globals;
        ////                                            reapeterClass.data.images = serr.data.images;
        ////                                            reapeterClass.data.type = serr.data.type;
        ////                                            reapeterClass.data.id = serr.data.id;

        ////                                        }
        ////                                    }

        ////                                }

        ////                                //reapeterClass.data = serr.data;
        ////                                reapeterClass.name = serr.name;
        ////                                reapeterClass.parent = serr.parent;
        ////                                reapeterClass.description = serr.description;
        ////                                reapeterClass.id = serr.id;



        ////                                treeClass1.TreeList.Add(reapeterClass);
        ////                            }
        ////                            //treeClass1.TreeList.Add(serr);
        ////                            gdMainModel.tree.Add(treeClass1);
        ////                            serr.data.decisions.data.Add(decision);
        ////                        }
        ////                    }

        ////                }
        ////                else if (pair.Name == "gd_image")
        ////                {
        ////                    foreach (var imageList in pair)
        ////                    {
        ////                        foreach (var image in imageList)
        ////                        {
        ////                            GdImage gdImage = new GdImage();
        ////                            foreach (var image1 in image)
        ////                            {
        ////                                if (image1.Name == "image name")
        ////                                {
        ////                                    gdImage.gdname = image1.Value.Value;
        ////                                }
        ////                                else if (image1.Name == "gd image")
        ////                                {
        ////                                    gdImage.gdimage = $"https://vecvdaliteplus.vecv.net/media/{image1.Value.Value}";
        ////                                }
        ////                            }
        ////                            gdMainModel.gd_image.Add(gdImage);
        ////                        }
        ////                    }
        ////                }
        ////            }

        ////            var AdminPackageList = list["gauther"][model][PCode]["tree"][0].ToString();

        ////            JObject obj = JObject.Parse(AdminPackageList);
        ////            foreach (var pair in obj)
        ////            {
        ////                var tree_data = pair;

        ////            }
        ////            return gdMainModel;
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Debug.WriteLine("CatchGDData   " + json);
        ////        await DisplayAlert("Alert", ex.Message + ex.StackTrace, "Ok");
        ////        //Working = false;
        ////        return null;
        ////    }
        ////}
    }
}