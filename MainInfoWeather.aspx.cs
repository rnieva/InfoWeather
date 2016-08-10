using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace InfoWeather
{
    public partial class MainInfoWeather : System.Web.UI.Page
    {
        public StringBuilder info1 = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropDownList1SelectSource.Items.Add("Global Weather");
                DropDownList1SelectSource.Items.Add("Site2");
                DropDownList1SelectSource.SelectedValue = "Global Weather";
            }
            if (DropDownList1SelectSource.SelectedValue == "Global Weather")
            {
                CountryDropList1(null, new EventArgs());
            }
        }
        protected void CountryDropList1(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {   // added maxReceivedMessageSize="20000000" in Web.config
                // set to True the property AutoPostBack for refresh the DropDownList2SelectCity and Event CityDropList1
                ServiceReference1.GlobalWeatherSoapClient soapService = new ServiceReference1.GlobalWeatherSoapClient("GlobalWeatherSoap");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(soapService.GetCitiesByCountry(""));
                XmlNodeList xmlNodes = xmlDoc.GetElementsByTagName("Country");
                List<string> countryList = new List<string>();
                foreach (XmlNode node in xmlNodes)
                {
                    if (!countryList.Contains(node.InnerText))
                    {
                        countryList.Add(node.InnerText);
                    }
                }
                countryList.Sort();
                DropDownList1SelectCountry.DataSource = countryList;
                DropDownList1SelectCountry.DataBind();
                CityDropList1(null, new EventArgs());
            }
        }

        protected void CityDropList1(object sender, System.EventArgs e)
        {
            //CleanLabels(null, new EventArgs());
            if (this.DropDownList1SelectCountry.SelectedItem != null)
            {
                List<string> cityList = new List<string>();
                ServiceReference1.GlobalWeatherSoapClient soapService = new ServiceReference1.GlobalWeatherSoapClient("GlobalWeatherSoap");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(soapService.GetCitiesByCountry(DropDownList1SelectCountry.SelectedValue));
                XmlNodeList xmlNodes = xmlDoc.GetElementsByTagName("City");
                foreach (XmlNode node in xmlNodes)
                {
                    if (!cityList.Contains(node.InnerText))
                    {
                        cityList.Add(node.InnerText);
                    }
                }
                cityList.Sort();
                DropDownList2SelectCity.DataSource = cityList;
                DropDownList2SelectCity.DataBind();
            }
            else
            {
                DropDownList2SelectCity.DataSource = null;
                DropDownList2SelectCity.DataBind();
            }
        }

        protected void GetInfoWeather(object sender, EventArgs e)
        {
            string temperature = null;
            ServiceReference1.GlobalWeatherSoapClient soapService = new ServiceReference1.GlobalWeatherSoapClient("GlobalWeatherSoap");
            string xmlInfo = soapService.GetWeather(DropDownList2SelectCity.SelectedValue, DropDownList1SelectCountry.SelectedValue);
            bool success = false;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlInfo);
                XmlNode xmlNode = xmlDoc.DocumentElement;
                foreach (XmlNode node in xmlNode.ChildNodes)
                {
                    if (node.Name == "Status")
                    {
                        success = node.InnerText == "Success";
                    }
                    else
                    {
                        info1.Append("<b>" + node.Name + ":</b> " + node.InnerText + "<br/>");
                        if (node.Name == "Temperature")
                        {
                            temperature = node.InnerText;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                info1.Append("Data Not Found");
                LabelTempTest.Text = "Data Not Found";
            }
            LabelTempTest.Text = temperature;
        }

    }
}