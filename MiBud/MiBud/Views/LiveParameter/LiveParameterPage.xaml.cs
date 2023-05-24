using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiBud.Models;
using MiBud.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.LiveParameter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LiveParameterPage : ContentView
    {
        LiveParameterViewModel viewModel;
        //ObservableCollection<LiveParameterModel> SelectedParameterList;
        public LiveParameterPage()
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new LiveParameterViewModel(this);
            }
            catch (Exception ex)
            {
            }
        }

        
        //private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(txtSearch.Text))
        //        {
        //            imgClose.IsVisible = false;
        //            ems_List.ItemsSource = viewModel.EMSParameterList;
        //            //iceu_List.ItemsSource = IecuList;
        //            //acm_List.ItemsSource = ACMList;
        //            //tgw_List.ItemsSource = TGW2List;
        //        }
        //        else
        //        {
        //            imgClose.IsVisible = true;
        //            if (EmsView.IsVisible)
        //            {
        //                ems_List.ItemsSource = viewModel.EMSParameterList.Where(x => x.Description.ToLower().Contains(e.NewTextValue.ToLower()) || x.Description.ToLower().Contains(e.NewTextValue.ToLower()));// ;

        //            }
        //            //else if (IecuView.IsVisible)
        //            //{
        //            //    iceu_List.ItemsSource = IecuList.Where(x => x.Description.ToLower().Contains(e.NewTextValue.ToLower()) || x.ParametersID.ToLower().Contains(e.NewTextValue.ToLower()));

        //            //}
        //            //else if (AcmView.IsVisible)
        //            //{
        //            //    acm_List.ItemsSource = ACMList.Where(x => x.Description.ToLower().Contains(e.NewTextValue.ToLower()) || x.ParametersID.ToLower().Contains(e.NewTextValue.ToLower()));

        //            //}
        //            //else if (TgwView.IsVisible)
        //            //{
        //            //    tgw_List.ItemsSource = TGW2List.Where(x => x.Description.ToLower().Contains(e.NewTextValue.ToLower()) || x.ParametersID.ToLower().Contains(e.NewTextValue.ToLower()));

        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private void CloseClick(object sender, EventArgs e)
        //{
        //    txtSearch.Text = string.Empty;
        //    imgClose.IsVisible = false;
        //}

        //private void Selection_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (EmsView.IsVisible)
        //        {
        //            foreach (var EmsItem in viewModel.EMSParameterList)
        //            {
        //                EmsItem.Selected = true;
        //            }
        //            ems_List.ItemsSource = null;
        //            ems_List.ItemsSource = viewModel.EMSParameterList;
        //        }
        //        //else if (IecuView.IsVisible)
        //        //{
        //        //    foreach (var IecuItem in IecuList)
        //        //    {
        //        //        IecuItem.IsSelected = e.Value;
        //        //    }
        //        //    iceu_List.ItemsSource = null;
        //        //    iceu_List.ItemsSource = IecuList;
        //        //}
        //        //else if (AcmView.IsVisible)
        //        //{
        //        //    foreach (var AcmItem in ACMList)
        //        //    {
        //        //        AcmItem.IsSelected = e.Value;
        //        //    }
        //        //    acm_List.ItemsSource = null;
        //        //    acm_List.ItemsSource = ACMList;
        //        //}
        //        //else if (TgwView.IsVisible)
        //        //{
        //        //    foreach (var Tgw2Item in TGW2List)
        //        //    {
        //        //        Tgw2Item.IsSelected = e.Value;
        //        //    }
        //        //    tgw_List.ItemsSource = null;
        //        //    tgw_List.ItemsSource = TGW2List;
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //}

        //private void btnContinue_Clicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var EmsSelectedParameters = viewModel.EMSParameterList.Where(x => x.Selected);
        //        //var IecuSelectedParameters = IecuList.Where(x => x.IsSelected);
        //        //var AcmSelectedParameters = ACMList.Where(x => x.IsSelected);
        //        //var TGW2SelectedParameters = TGW2List.Where(x => x.IsSelected);

        //        if (EmsSelectedParameters != null)
        //        {
        //            foreach (var EmsParameter in EmsSelectedParameters)
        //            {
        //                SelectedParameterList.Add(
        //                    new LiveParameterModel
        //                    {
        //                        Description = EmsParameter.Description,
        //                        //NodeType = EmsParameter.ECUType,
        //                        //ECUType = EmsParameter.ECUType,
        //                        PCode = EmsParameter.PCode,
        //                        Selected = EmsParameter.Selected,
        //                        Unit = EmsParameter.Unit,
        //                        Value = EmsParameter.Value,
        //                        //KeyName = EmsParameter.KeyName,

        //                    });
        //            }
        //        }
        //        //if (IecuSelectedParameters != null)
        //        //{
        //        //    foreach (var IecuParameter in IecuSelectedParameters)
        //        //    {
        //        //        SelectedParameterList.Add(
        //        //            new ParaModel
        //        //            {
        //        //                Description = IecuParameter.Description,
        //        //                //NodeType = IecuParameter .ECUType,
        //        //                ECUType = IecuParameter.ECUType,
        //        //                ParametersID = IecuParameter.ParametersID,
        //        //                Selected = IecuParameter.IsSelected,
        //        //                Unit = IecuParameter.Unit,
        //        //                KeyName = IecuParameter.KeyName,
        //        //            });
        //        //    }
        //        //}
        //        //if (AcmSelectedParameters != null)
        //        //{
        //        //    foreach (var AcmParameter in AcmSelectedParameters)
        //        //    {
        //        //        SelectedParameterList.Add(
        //        //            new ParaModel
        //        //            {
        //        //                Description = AcmParameter.Description,
        //        //                //NodeType = AcmParameter .ECUType,
        //        //                ECUType = AcmParameter.ECUType,
        //        //                ParametersID = AcmParameter.ParametersID,
        //        //                Selected = AcmParameter.IsSelected,
        //        //                Unit = AcmParameter.Unit,
        //        //                KeyName = AcmParameter.KeyName,
        //        //            });
        //        //    }
        //        //}
        //        //if (TGW2SelectedParameters != null)
        //        //{
        //        //    foreach (var Tgw2Parameter in TGW2SelectedParameters)
        //        //    {
        //        //        SelectedParameterList.Add(
        //        //            new ParaModel
        //        //            {
        //        //                Description = Tgw2Parameter.Description,
        //        //                //NodeType = EmsParameter.ECUType,
        //        //                ECUType = Tgw2Parameter.ECUType,
        //        //                ParametersID = Tgw2Parameter.ParametersID,
        //        //                Selected = Tgw2Parameter.IsSelected,
        //        //                Unit = Tgw2Parameter.Unit,
        //        //                KeyName = Tgw2Parameter.KeyName,
        //        //            });
        //        //    }
        //        //}

        //        if (SelectedParameterList.Count < 1)
        //        {
        //            //DisplayAlert("Alert", "Please Selected a parameter", "Ok");
        //            return;
        //        }

        //        Navigation.PushAsync(new LiveParameterSelectedPage(SelectedParameterList));
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
    }
}