using MiBud.Models;
using MiBud.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MiBud.ViewModels
{
    public class RSManagerSectionViewModel : ViewModelBase
    {
        ApiServices apiServices;
        public RSManagerSectionViewModel(Page page) : base(page)
        {
            try
            {
                InitializeCommands();
                apiServices = new ApiServices();
                var selected_jobcard = App.selected_jobcard;
                id = selected_jobcard.id;
                if (selected_jobcard.jobcard_pickupdrop != null)
                {
                    var res = selected_jobcard.jobcard_pickupdrop.FirstOrDefault(d => d.status == "Pickup_Closed");
                    if (res != null)
                    {
                        status = "Pickup Closed";
                    }
                }
                if (selected_jobcard.status == "QuotedForTransport")
                {
                    status = "QuotedForTransport";
                }
            }
            catch { }
            { }
        }
        private string mstatus;
        public string status
        {
            get { return mstatus; }
            set { mstatus = value; OnPropertyChanged(nameof(status)); }
        }

        private string mid;
        public string id
        {
            get { return mid; }
            set { mid = value; OnPropertyChanged(nameof(id)); }
        }

        public void InitializeCommands()
        {
            ApproveCommand = new Command(async (obj) =>
            {
                try
                {
                    if (status == "Pickup Closed")
                    {
                        ApproveModel model = new ApproveModel();
                        model.id = id;
                        var msg = await apiServices.ApproveTransport(model, App.access_token);
                        await page.DisplayAlert(msg.message, msg.status, "OK");
                    }
                    else if (status == "QuotedForTransport")
                    {
                        ApproveModel model = new ApproveModel();
                        model.id = id;
                        var msg = await apiServices.ClosedByCustomer(model, App.access_token);
                        await page.DisplayAlert(msg.message, msg.status, "OK");
                    }
                }
                catch (Exception ex)
                {
                }
            });

            RejectCommand = new Command(async (obj) =>
            {
                try
                {
                    if (status == "Pickup Closed")
                    {
                        RejectModel model = new RejectModel();
                        model.id = id;
                        var msg = await apiServices.RejectedTransport(model, App.access_token);
                        await page.DisplayAlert(msg.message, msg.status, "OK");
                    }
                    else if (status == "QuotedForTransport")
                    {
                        RejectModel model = new RejectModel();
                        model.id = id;
                        var msg = await apiServices.ClosedDisapproved(model, App.access_token);
                        await page.DisplayAlert(msg.message, msg.status, "OK");
                    }
                }
                catch (Exception ex)
                {
                }
            });
        }
        #region ICommands
        public ICommand ApproveCommand { get; set; }
        public ICommand RejectCommand { get; set; }
        #endregion
    }
}
