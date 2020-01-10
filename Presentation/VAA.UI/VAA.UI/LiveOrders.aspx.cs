using System;
using System.Linq;
using System.Web.UI;
using Telerik.Web.UI;
using VAA.BusinessComponents;
using VAA.DataAccess;

namespace VAA.UI
{
    public partial class LiveOrders : Page
    {
        readonly OrderManagement _orderManagement = new OrderManagement();
        readonly CycleManagement _cycleManagement = new CycleManagement();

        MenuProcessor _menuProcessor = new MenuProcessor();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var liveOrder = _orderManagement.GetLiveOrder();

                lblLiveOrderIDValue.Text = Convert.ToString(liveOrder.LiveOrderId);
                lblLotNumberValue.Text = Convert.ToString(liveOrder.LotNo);
                lblCycleNameValue.Text = _cycleManagement.GetCycle(Convert.ToInt64(liveOrder.CycleId)).CycleName;
            }
        }

        public void UpdateQuantiesBtnClicked(object sender, EventArgs e)
        {
            if (RadAsyncUploadSchedule.UploadedFiles.Count == 0)
                return;

            UploadedFile attachment = RadAsyncUploadSchedule.UploadedFiles[0];

            _menuProcessor.UploadFlightSchedule(attachment.InputStream, chkClearFlightSchedule.Checked);
        }

        public void OrderNowBtnClicked(object sender, EventArgs e)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

    }
}