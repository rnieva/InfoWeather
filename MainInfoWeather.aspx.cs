﻿using System;
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
            CleanLabels(null, new EventArgs());
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
            CleanLabels(null, new EventArgs());
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
            string location = null;
            string time = null;
            string wind = null;
            string visibility = null;
            string skyconditions = null;
            string temperature = null;
            string dewpoint = null;
            string RelativeHumidity = null;
            string Pressure = null;
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
                        if (node.Name == "Location")
                        {
                            location = node.InnerText;
                        }
                        if (node.Name == "Time")
                        {
                            time = node.InnerText;
                        }
                        if (node.Name == "Wind")
                        {
                            wind = node.InnerText;
                        }
                        if (node.Name == "Visibility")
                        {
                            visibility = node.InnerText;
                        }
                        if (node.Name == "SkyConditions")
                        {
                            skyconditions = node.InnerText;
                        }
                        if (node.Name == "Temperature")
                        {
                            temperature = node.InnerText;
                        }
                        if (node.Name == "DewPoint")
                        {
                            dewpoint = node.InnerText;
                        }
                        if (node.Name == "RelativeHumidity")
                        {
                            RelativeHumidity = node.InnerText;
                        }
                        if (node.Name == "Pressure")
                        {
                            Pressure = node.InnerText;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                info1.Append("Data Not Found");
                LabelTest.Text = "Data Not Found";
            }
            LabelLocation.Text = "Location: " + location;
            LabelTime.Text = "Time: " + time;
            LabelWind.Text = "Wind: " + wind;
            LabelVisibility.Text = "Visibility: " + visibility;
            LabelSkyConditions.Text = "SkyConditions: " + skyconditions;
            LabelTemperature.Text = "Temperature: " + temperature;
            LabelDewPoint.Text = "DewPoint: " + dewpoint;
            LabelRelativeHumidity.Text = "RelativeHumidity: " + RelativeHumidity;
            LabelPressure.Text = "Pressure: " + Pressure;
        }

        protected void CleanLabels(object sender, EventArgs e)
        {
            LabelLocation.Text = "";
            LabelTime.Text = "";
            LabelWind.Text = "";
            LabelVisibility.Text = "";
            LabelSkyConditions.Text = "";
            LabelTemperature.Text = "";
            LabelDewPoint.Text = "";
            LabelRelativeHumidity.Text = "";
            LabelPressure.Text = "";
        }

        protected void ButtonBestCity_Click(object sender, EventArgs e)
        {
            string temperatureToComp = LabelTemperature.Text;
            string location = null;
            string temperature = null;
            string locationBest = null;
            string temperatureBest = null;
            decimal temperatureBesttemp = -100; //reference
            List<string> cityList = new List<string>();
            int i = 0;
            //get the list of cities of selected country
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
            //store and compare the temperature with the rest of cities
            for (i=0;i<cityList.Count;i++)
            {
                string xmlInfo = soapService.GetWeather(cityList[i], DropDownList1SelectCountry.SelectedValue);
                bool success = false;
                try
                {
                    XmlDocument xmlDoc2 = new XmlDocument();
                    xmlDoc2.LoadXml(xmlInfo);
                    XmlNode xmlNode = xmlDoc2.DocumentElement;
                    foreach (XmlNode node in xmlNode.ChildNodes)
                    {
                        if (node.Name == "Status")
                        {
                            success = node.InnerText == "Success";
                        }
                        else
                        {
                            if (node.Name == "Location")
                            {
                                location = node.InnerText;
                            }
                            if (node.Name == "Temperature")
                            {
                                temperature = node.InnerText;
                                decimal temperatured = Convert.ToDecimal(temperature.Substring(temperature.IndexOf("(") + 1, temperature.IndexOf(")") - temperature.IndexOf("(") - 3));
                                if (temperatured >= temperatureBesttemp)
                                {
                                    temperatureBesttemp = temperatured;
                                    temperatureBest = temperature;
                                    locationBest = location;
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    info1.Append("Data Not Found");
                    LabelTest.Text = "Data Not Found";
                }
            }
            LabelBestCityTemp.Text = "Best Temperature City: <span style='font-weight: bold;'>"+ temperatureBest +"</span>"  + " in: <span style='font-weight: bold;'>" + locationBest + " </span>";
        }
    }
}