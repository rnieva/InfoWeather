using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InfoWeather
{
    public partial class MainInfoWeather : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DropDownList1SelectSource.Items.Add("Global Weather");
            DropDownList1SelectSource.Items.Add("Site2");
            DropDownList1SelectSource.SelectedValue = "Global Weather";


        }
    }
}