using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiBud.Views.SelfDiagnose
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelfDiagnosePage : ContentPage
    {
        public SelfDiagnosePage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
            }
        }

        private void TabClicked(object sender, EventArgs e)
        {
            try
            {
                var sen = sender as Grid;
                if (sen == null) { return; }
                DtcTab.Opacity = 0.5;
                LiveParametersTab.Opacity = 0.5;

                DtcView.IsVisible = false;
                LiveParametersView.IsVisible = false;

                switch (sen.ClassId)
                {
                    case "dtc":
                        DtcTab.Opacity = 1;
                        DtcView.IsVisible = true;
                        //btnContinue.IsVisible = true;
                        //SelectedECU = "(1.1.0)";
                        break;

                    case "lp":
                        LiveParametersTab.Opacity = 1;
                        LiveParametersView.IsVisible = true;
                        //btnContinue.IsVisible = false;
                        //SelectedECU = "(6.2.0)";
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}