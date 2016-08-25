using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                DropDownList1SelectSource.Items.Add("Weather Underground");
                DropDownList1SelectSource.Items.Add("site3");
                DropDownList1SelectSource.SelectedValue = "Global Weather";
            }
            CountryDropList1(null, new EventArgs());  //Load list of Countries and leter list of cities
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
            if (DropDownList1SelectSource.SelectedValue == "Global Weather")
            {
                GetInfoGlobalWeather();
            }
            if (DropDownList1SelectSource.SelectedValue == "Weather Underground")
            {
                GetInfoWeatherUnderground();
            }
        }

        protected void GetInfoGlobalWeather()
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
            LabelTest.Text = "";
        }

        protected void ButtonBestCity_Click(object sender, EventArgs e) //At the moment just with GlobalWeather
        {
            string parameterToComp = null;
            string location = null;
            string locationBest = null;
            string parameter = null; // temperature - wind - visibility - dewPoint - relativeHumidity - pressure
            string parameterBest = null;
            decimal parameterBestDecimal = -1000; //reference
            //get the list of cities of selected country
            List<string> cityList = new List<string>();
            int i = 0;
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
                            if (node.Name == DropDownListParameterToCompa.SelectedValue)
                            {
                                switch (DropDownListParameterToCompa.SelectedValue)
                                {
                                    case "Temperature":
                                        parameterToComp = LabelTemperature.Text;

                                        parameter = node.InnerText; //format -->  <Temperature> 59 F (15 C)</Temperature>
                                        decimal temperatureDecimal = Convert.ToDecimal(parameter.Substring(parameter.IndexOf("(") + 1, parameter.IndexOf(")") - parameter.IndexOf("(") - 3));
                                        if (temperatureDecimal >= parameterBestDecimal)
                                        {
                                            parameterBestDecimal = temperatureDecimal;
                                            parameterBest = parameter;
                                            locationBest = location;
                                        }
                                        break;
                                    case "Wind":
                                        parameterToComp = LabelWind.Text;

                                        parameter = node.InnerText; //format --> <Wind> from the E (100 degrees) at 6 MPH (5 KT):0</Wind>
                                        parameter = parameter.Substring(parameter.Length - 12);
                                        decimal windDecimal = Convert.ToDecimal(parameter.Substring(parameter.IndexOf("(") + 1, parameter.IndexOf(")") - parameter.IndexOf("(") - 3));
                                        if (windDecimal >= parameterBestDecimal)
                                        {
                                            parameterBestDecimal = windDecimal;
                                            parameterBest = parameter;
                                            locationBest = location;
                                        }
                                        break;
                                    case "Visibility":
                                        parameterToComp = LabelVisibility.Text;

                                        parameter = node.InnerText; //format --> <Visibility> greater than 7 mile(s):0</Visibility>
                                        decimal visibilityDecimal = Convert.ToDecimal(Regex.Match(parameter, @"\d+").Value);
                                        if (visibilityDecimal >= parameterBestDecimal)
                                        {
                                            parameterBestDecimal = visibilityDecimal;
                                            parameterBest = parameter;
                                            locationBest = location;
                                        }
                                        break;
                                    case "DewPoint":
                                        parameterToComp = LabelDewPoint.Text;

                                        parameter = node.InnerText; //format -->   <DewPoint> 55 F (13 C)</DewPoint>
                                        decimal dewPointDecimal = Convert.ToDecimal(parameter.Substring(parameter.IndexOf("(") + 1, parameter.IndexOf(")") - parameter.IndexOf("(") - 3));
                                        if (dewPointDecimal >= parameterBestDecimal)
                                        {
                                            parameterBestDecimal = dewPointDecimal;
                                            parameterBest = parameter;
                                            locationBest = location;
                                        }
                                        break;
                                    case "RelativeHumidity":
                                        parameterToComp = LabelRelativeHumidity.Text;

                                        parameter = node.InnerText; //format -->  <RelativeHumidity> 82%</RelativeHumidity>
                                        decimal relativeHumidityDecimal = Convert.ToDecimal(Regex.Match(parameter, @"\d+").Value);
                                        if (relativeHumidityDecimal >= parameterBestDecimal)
                                        {
                                            parameterBestDecimal = relativeHumidityDecimal;
                                            parameterBest = parameter;
                                            locationBest = location;
                                        }
                                        break;
                                    case "Pressure":
                                        parameterToComp = LabelPressure.Text;

                                        parameter = node.InnerText; //format -->  <Pressure> 29.38 in. Hg (0995 hPa)</Pressure>
                                        decimal pressureDecimal = Convert.ToDecimal(parameter.Substring(parameter.IndexOf("(") + 1, parameter.IndexOf(")") - parameter.IndexOf("(") - 4));
                                        if (pressureDecimal >= parameterBestDecimal)
                                        {
                                            parameterBestDecimal = pressureDecimal;
                                            parameterBest = parameter;
                                            locationBest = location;
                                        }
                                        break;
                                    default:
                                        LabelTest.Text = "wrong parameterToComp";
                                        break;
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
            //LabelBestCityTemp.Text = "Best Temperature City: <span style='font-weight: bold;'>"+ temperatureBest +"</span>"  + " in: <span style='font-weight: bold;'>" + locationBest + " </span>";
            //LabelBestCityTemp.Text = "Most Windy City: <span style='font-weight: bold;'>" + windbest + "</span>" + " in: <span style='font-weight: bold;'>" + locationBest + " </span>";
            //LabelBestCityTemp.Text = "Most Dew Point City: <span style='font-weight: bold;'>" + dewPointBest + "</span>" + " in: <span style='font-weight: bold;'>" + locationBest + " </span>";
            //LabelBestCityTemp.Text = "Best Visibility City: <span style='font-weight: bold;'>" + visibilityBest + "</span>" + " in: <span style='font-weight: bold;'>" + locationBest + " </span>";
            //LabelBestCityTemp.Text = "Most Relative Humidity City: <span style='font-weight: bold;'>" + relativeHumidityBest + "</span>" + " in: <span style='font-weight: bold;'>" + locationBest + " </span>";
            //LabelBestCityTemp.Text = "Most Pressure City: <span style='font-weight: bold;'>" + pressureBest + "</span>" + " in: <span style='font-weight: bold;'>" + locationBest + " </span>";
            LabelBestCityTemp.Text = "Most " + DropDownListParameterToCompa.SelectedValue + " City: <span style='font-weight: bold;'>" + parameterBest + "</span>" + " in: <span style='font-weight: bold;'>" + locationBest + " </span>";
        }
        protected void GetInfoWeatherUnderground()
        {
            string key = "";
            string info = ("http://api.wunderground.com/api/" + key + "/conditions/q/" + DropDownList1SelectCountry.SelectedValue + "/" + DropDownList2SelectCity.SelectedValue) + ".json";
            var client = new WebClient();
            string jsonWeatherData = client.DownloadString(info);
            JObject infoP = JObject.Parse(jsonWeatherData);
            LabelLocation.Text = "Location: " + infoP["current_observation"]["display_location"]["full"];
            LabelTime.Text = "Time: " + infoP["current_observation"]["local_time_rfc822"];
            LabelWind.Text = "Wind: " + infoP["current_observation"]["wind_string"];
            LabelVisibility.Text = "Visibility: " + infoP["current_observation"]["visibility_km"];
            LabelSkyConditions.Text = "SkyConditions: " + infoP["current_observation"]["weather"];
            LabelTemperature.Text = "Temperature: " + infoP["current_observation"]["temperature_string"];
            LabelDewPoint.Text = "DewPoint: " + infoP["current_observation"]["dewpoint_string"];
            LabelRelativeHumidity.Text = "RelativeHumidity: " + infoP["current_observation"]["relative_humidity"];
            LabelPressure.Text = "Pressure: " + infoP["current_observation"]["pressure_mb"];

        }
    }
}