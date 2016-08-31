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

        public string key = ""; //key for WU
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropDownList1SelectSource.Items.Add("Global Weather");
                DropDownList1SelectSource.Items.Add("Weather Underground");
                DropDownList1SelectSource.SelectedValue = "Global Weather";
            }
            CleanLabels(null, new EventArgs());  // Clean labels and imagenes after click any button
            LabelTest.ForeColor = System.Drawing.Color.Red;
            ImgSkyConditions.Visible = false;
            ImgMap.Visible = false;
            ChangeImgLogo(null, new EventArgs());     // Load the Data Source Logo
            CountryDropList1(null, new EventArgs());  // Load list of Countries and leter list of cities
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

        protected void ChangeImgLogo(object sender, EventArgs e)
        {
            if (DropDownList1SelectSource.SelectedValue == "Global Weather")
            {
                ImageLogoUW.ImageUrl = ("/imgs/infoWeatherLogo_1.png");
            }
            if (DropDownList1SelectSource.SelectedValue == "Weather Underground")
            {
                ImageLogoUW.ImageUrl = "https://icons.wxug.com/logos/PNG/wundergroundLogo_4c.png";
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
                        if (node.Name == "Location")
                        {
                            location = node.InnerText;
                            LabelLocation.Text = "<span style='font-weight: bold;'>Location:</span> " + location;
                        }
                        if (node.Name == "Time")
                        {
                            time = node.InnerText;
                            LabelTime.Text = "<span style='font-weight: bold;'>Time:</span> " + time;
                        }
                        if (node.Name == "Wind")
                        {
                            wind = node.InnerText;
                            LabelWind.Text = "<span style='font-weight: bold;'>Wind:</span> " + wind;
                        }
                        if (node.Name == "Visibility")
                        {
                            visibility = node.InnerText;
                            LabelVisibility.Text = "<span style='font-weight: bold;'>Visibility:</span> " + visibility;
                        }
                        if (node.Name == "SkyConditions")
                        {
                            skyconditions = node.InnerText;
                            LabelSkyConditions.Text = "<span style='font-weight: bold;'>SkyConditions:</span> " + skyconditions;
                        }
                        if (node.Name == "Temperature")
                        {
                            temperature = node.InnerText;
                            LabelTemperature.Text = "<span style='font-weight: bold;'>Temperature:</span> " + temperature;
                        }
                        if (node.Name == "DewPoint")
                        {
                            dewpoint = node.InnerText;
                            LabelDewPoint.Text = "DewPoint: " + dewpoint;
                        }
                        if (node.Name == "RelativeHumidity")
                        {
                            RelativeHumidity = node.InnerText;
                            LabelRelativeHumidity.Text = "<span style='font-weight: bold;'>RelativeHumidity:</span> " + RelativeHumidity;
                        }
                        if (node.Name == "Pressure")
                        {
                            Pressure = node.InnerText;
                            LabelPressure.Text = "<span style='font-weight: bold;'>Pressure:</span> " + Pressure;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Data Not Found",ex);
                LabelTest.Text = "Data Not Found";
            }
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
            ImgSkyConditions.Visible = false;
            ImgMap.Visible = false;
            ImgForescast1.Visible = false;
            ImgForescast12.Visible = false;
            ImgForescast2.Visible = false;
            ImgForescast21.Visible = false;
            ImgForescast3.Visible = false;
            ImgForescast31.Visible = false;
            LabelForecast1.Text = "";
            LabelForecast12.Text = "";
            LabelForecast2.Text = "";
            LabelForecast21.Text = "";
            LabelForecast3.Text = "";
            LabelForecast31.Text = "";
            LabelLocationForecast.Text = "";
            LabelBestCityTemp.Text = "";
        }

        protected void ButtonBestCity_Click(object sender, EventArgs e)
        {
            List<string> citiesNoData = new List<string>();
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
            if (DropDownList1SelectSource.SelectedValue == "Global Weather")
            {
                for (i = 0; i < cityList.Count; i++)
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
                        citiesNoData.Add(cityList[i] + " - ");
                        System.Diagnostics.Debug.WriteLine("Data Not Found Best city GW", ex);
                        LabelTest.Text = "Data Not Found";
                    }
                }
                if (parameterBest != null)
                {
                    LabelBestCityTemp.Text = "<br>Most " + DropDownListParameterToCompa.SelectedValue + " City: <span style='font-weight: bold;'>" + parameterBest + "</span>" + " in: <span style='font-weight: bold;'>" + locationBest + " </span>";
                    if (citiesNoData[1] != null)
                    {
                        string citiesNoDat = string.Join("", citiesNoData.ToArray());
                        LabelBestCityTemp.Text = LabelBestCityTemp.Text + "<br> No data of: " + citiesNoDat;
                    }
                }
            } //end if (DropDownList1SelectSource.SelectedValue == "Global Weather)
            if (DropDownList1SelectSource.SelectedValue == "Weather Underground")
            {
                for (i = 0; i < cityList.Count; i++)
                {
                    string info = ("http://api.wunderground.com/api/" + key + "/conditions/q/" + DropDownList1SelectCountry.SelectedValue + "/" + cityList[i]) + ".json";
                    var client = new WebClient();
                    string jsonWeatherData = client.DownloadString(info);
                    JObject infoP = JObject.Parse(jsonWeatherData);
                    switch (DropDownListParameterToCompa.SelectedValue)
                    {
                        case "Temperature":
                            parameterToComp = LabelTemperature.Text;
                            try
                            {
                                parameter = (string)infoP["current_observation"]["temperature_string"]; // format --> "72 F (22 C)"
                                decimal temperatureDecimal = Convert.ToDecimal(parameter.Substring(parameter.IndexOf("(") + 1, parameter.IndexOf(")") - parameter.IndexOf("(") - 3));
                                if (temperatureDecimal >= parameterBestDecimal)
                                {
                                    parameterBestDecimal = temperatureDecimal;
                                    parameterBest = parameter;
                                    locationBest = (string)infoP["current_observation"]["display_location"]["full"];
                                }
                            }
                            catch (Exception ex)
                            {
                                citiesNoData.Add(cityList[i] + " - ");
                                System.Diagnostics.Debug.WriteLine("Temperature Not Found Best city UW", ex);
                            }
                            break;
                        case "Wind":
                            parameterToComp = LabelWind.Text;
                            try
                            {
                                parameter = (string)infoP["current_observation"]["wind_string"]; //format --> From the West at 8 MPH
                                decimal windDecimal = Convert.ToDecimal(Regex.Match(parameter, @"\d+").Value);
                                if (windDecimal >= parameterBestDecimal)
                                {
                                    parameterBestDecimal = windDecimal;
                                    parameterBest = parameter;
                                    locationBest = (string)infoP["current_observation"]["display_location"]["full"];
                                }
                            }
                            catch (Exception ex)
                            {
                                citiesNoData.Add(cityList[i] + " - ");
                                System.Diagnostics.Debug.WriteLine("Wind Not Found Best city UW", ex);
                            }
                            break;
                        case "Visibility":
                            parameterToComp = LabelVisibility.Text;
                            try
                            {
                                parameter = (string)infoP["current_observation"]["visibility_km"]; ; //format --> "visibility_km":"N/A"
                                decimal visibilityDecimal = Convert.ToDecimal(Regex.Match(parameter, @"\d+").Value);
                                if (visibilityDecimal >= parameterBestDecimal)
                                {
                                    parameterBestDecimal = visibilityDecimal;
                                    parameterBest = parameter;
                                    locationBest = (string)infoP["current_observation"]["display_location"]["full"];
                                }
                            }
                            catch (Exception ex)
                            {
                                citiesNoData.Add(cityList[i] + " - ");
                                System.Diagnostics.Debug.WriteLine("Visibility Not Found Best city UW", ex);
                            }
                            break;
                        case "DewPoint":
                            parameterToComp = LabelDewPoint.Text;
                            try
                            {
                                parameter = (string)infoP["current_observation"]["dewpoint_string"]; ; //format -->   "dewpoint_c":20,
                                decimal dewPointDecimal = Convert.ToDecimal(Regex.Match(parameter, @"\d+").Value);
                                if (dewPointDecimal >= parameterBestDecimal)
                                {
                                    parameterBestDecimal = dewPointDecimal;
                                    parameterBest = parameter;
                                    locationBest = (string)infoP["current_observation"]["display_location"]["full"];
                                }
                            }
                            catch (Exception ex)
                            {
                                citiesNoData.Add(cityList[i] + " - ");
                                System.Diagnostics.Debug.WriteLine("DewPoint Not Found Best city UW", ex);
                            }
                            break;
                        case "RelativeHumidity":
                            parameterToComp = LabelRelativeHumidity.Text;
                            try
                            {
                                parameter = (string)infoP["current_observation"]["relative_humidity"]; //format -->  "relative_humidity":"88%",
                                decimal relativeHumidityDecimal = Convert.ToDecimal(Regex.Match(parameter, @"\d+").Value);
                                if (relativeHumidityDecimal >= parameterBestDecimal)
                                {
                                    parameterBestDecimal = relativeHumidityDecimal;
                                    parameterBest = parameter;
                                    locationBest = (string)infoP["current_observation"]["display_location"]["full"];
                                }
                            }
                            catch (Exception ex)
                            {
                                citiesNoData.Add(cityList[i] + " - ");
                                System.Diagnostics.Debug.WriteLine("RelativeHumidity Not Found Best city WU", ex);
                            }
                            break;
                        case "Pressure":
                            parameterToComp = LabelPressure.Text;
                            try
                            {
                                parameter = (string)infoP["current_observation"]["pressure_mb"]; //format --> pressure_mb
                                decimal pressureDecimal = Convert.ToDecimal(Regex.Match(parameter, @"\d+").Value);
                                if (pressureDecimal >= parameterBestDecimal)
                                {
                                    parameterBestDecimal = pressureDecimal;
                                    parameterBest = parameter;
                                    locationBest = (string)infoP["current_observation"]["display_location"]["full"];
                                }
                            }
                            catch (Exception ex)
                            {
                                citiesNoData.Add(cityList[i] + " - ");
                                System.Diagnostics.Debug.WriteLine("Pressure Not Found Best city WU", ex);
                            }
                            break;
                        default:
                            LabelTest.Text = "wrong parameterToComp";
                            break;
                    }
                }
                if (parameterBest != null)
                {
                    LabelBestCityTemp.Text = "<br>Most " + DropDownListParameterToCompa.SelectedValue + " City: <span style='font-weight: bold;'>" + parameterBest + "</span>" + " in: <span style='font-weight: bold;'>" + locationBest + " </span>";
                    if (citiesNoData[1] != null)
                    {
                        string citiesNoDat = string.Join("", citiesNoData.ToArray());
                        LabelBestCityTemp.Text = LabelBestCityTemp.Text + "<br> No data of: " + citiesNoDat;
                    }
                }
           } //end if (DropDownList1SelectSource.SelectedValue == "Weather Underground") 
        }

        protected void GetInfoWeatherUnderground()
        {
            string info = ("http://api.wunderground.com/api/" + key + "/conditions/q/" + DropDownList1SelectCountry.SelectedValue + "/" + DropDownList2SelectCity.SelectedValue) + ".json";
            var client = new WebClient();
            string jsonWeatherData = client.DownloadString(info);
            JObject infoP = JObject.Parse(jsonWeatherData);
            try
            {
                LabelLocation.Text = "<span style='font-weight: bold;'>Location:</span> " + infoP["current_observation"]["display_location"]["full"];
                LabelTime.Text = "<span style='font-weight: bold;'>Time:</span> " + infoP["current_observation"]["local_time_rfc822"];
                LabelWind.Text = "<span style='font-weight: bold;'>Wind:</span> " + infoP["current_observation"]["wind_string"];
                LabelVisibility.Text = "<span style='font-weight: bold;'>Visibility:</span> " + infoP["current_observation"]["visibility_km"];
                LabelTemperature.Text = "<span style='font-weight: bold;'>Temperature:</span> " + infoP["current_observation"]["temperature_string"];
                LabelDewPoint.Text = "<span style='font-weight: bold;'>DewPoint:</span> " + infoP["current_observation"]["dewpoint_string"];
                LabelRelativeHumidity.Text = "<span style='font-weight: bold;'>RelativeHumidity:</span> " + infoP["current_observation"]["relative_humidity"];
                LabelPressure.Text = "<span style='font-weight: bold;'>Pressure:</span> " + infoP["current_observation"]["pressure_mb"];
                LabelSkyConditions.Text = "<span style='font-weight: bold;'>SkyConditions:</span> " + infoP["current_observation"]["weather"];
                if (((string)infoP["current_observation"]["weather"]) != null)
                    ImgSkyConditions.Visible = true;
                if (((string)infoP["current_observation"]["display_location"]["full"]) != null)
                {
                    ImgMap.Visible = true;
                    ImgMap.ImageUrl = "http://api.wunderground.com/api/" + key + "/animatedradar/animatedsatellite/q/" + DropDownList1SelectCountry.SelectedValue + "/" + DropDownList2SelectCity.SelectedValue + ".gif?num=6&delay=50&interval=30";
                }
                switch ((string)infoP["current_observation"]["weather"])
                {
                    case "Clear":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/j/clear.gif";
                        break;
                    case "Sunny":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/j/clear.gif";
                        break;
                    case "Drizzle":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/j/chancesleet.gif";
                        break;
                    case "Mostly Cloudy":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/j/mostlycloudy.gif";
                        break;
                    case "Partly Cloudy":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/j/partlycloudy.gif";
                        break;
                    case "Mostly Sunny":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/j/mostlysunny.gif";
                        break;
                    case "Flurries":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/j/flurries.gif";
                        break;
                    case "Chance Sleet":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/j/chancesleet.gif";
                        break;
                    case "Cahnce Rain":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/j/chancerain.gif";
                        break;
                    case "Rain":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/j/rain.gif";
                        break;
                    case "Sleet":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/j/sleet.gif";
                        break;
                    case "Tstorm":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/j/tstorms.gif";
                        break;
                    case "Chance Tstorms":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/j/chancetstorms.gif";
                        break;
                    case "Nt Clear":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/j/nt_clear.gif";
                        break;
                    case "Nt Sunny":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/j/nt_sunny.gif";
                        break;
                    case "Overcast":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/i/cloudy.gif";
                        break;
                    case "Cloudy":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/i/cloudy.gif";
                        break; 
                    case "Scattered Clouds":
                        ImgSkyConditions.ImageUrl = "http://icons.wxug.com/i/c/i/cloudy.gif";
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Not img");
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                LabelTest.Text = "Data Not Found";
            }
        }
        protected void ButtonForecast_Click(object sender, EventArgs e)
        {
            if (DropDownList1SelectSource.SelectedValue == "Weather Underground")
            {
                GetInfoWeather(null, new EventArgs()); //Also it show info weather of selected place, could be added a flag to know if it clicked and dont clean parameters´,this way to avoid to execute this void two times
                string info = ("http://api.wunderground.com/api/" + key + "/forecast/q/" + DropDownList1SelectCountry.SelectedValue + "/" + DropDownList2SelectCity.SelectedValue) + ".json";
                var client = new WebClient();
                string jsonWeatherData = client.DownloadString(info);
                JObject infoP = JObject.Parse(jsonWeatherData);
                try
                {
                    LabelForecast1.Text = (string)infoP["forecast"]["txt_forecast"]["forecastday"][0]["title"] + "<br>High: " + (string)infoP["forecast"]["simpleforecast"]["forecastday"][1]["high"]["celsius"] + "C" + "<br>Low: " + (string)infoP["forecast"]["simpleforecast"]["forecastday"][1]["low"]["celsius"] + "C";
                    LabelForecast12.Text = (string)infoP["forecast"]["txt_forecast"]["forecastday"][1]["title"];
                    LabelForecast2.Text = (string)infoP["forecast"]["txt_forecast"]["forecastday"][2]["title"] + "<br>High: " + (string)infoP["forecast"]["simpleforecast"]["forecastday"][1]["high"]["celsius"] + "C" + "<br>Low: " + (string)infoP["forecast"]["simpleforecast"]["forecastday"][2]["low"]["celsius"] + "C";
                    LabelForecast21.Text = (string)infoP["forecast"]["txt_forecast"]["forecastday"][3]["title"];
                    LabelForecast3.Text = (string)infoP["forecast"]["txt_forecast"]["forecastday"][4]["title"] + "<br>High: " + (string)infoP["forecast"]["simpleforecast"]["forecastday"][1]["high"]["celsius"] + "C" + "<br>Low: " + (string)infoP["forecast"]["simpleforecast"]["forecastday"][3]["low"]["celsius"] + "C";
                    LabelForecast31.Text = (string)infoP["forecast"]["txt_forecast"]["forecastday"][5]["title"];
                    string link1 = (string)infoP["forecast"]["txt_forecast"]["forecastday"][0]["icon_url"];
                    ImgForescast1.ImageUrl = link1;
                    string link12 = (string)infoP["forecast"]["txt_forecast"]["forecastday"][1]["icon_url"];
                    ImgForescast12.ImageUrl = link12;
                    string link2 = (string)infoP["forecast"]["txt_forecast"]["forecastday"][2]["icon_url"];
                    ImgForescast2.ImageUrl = link2;
                    string link21 = (string)infoP["forecast"]["txt_forecast"]["forecastday"][3]["icon_url"];
                    ImgForescast21.ImageUrl = link21;
                    string link3 = (string)infoP["forecast"]["txt_forecast"]["forecastday"][4]["icon_url"];
                    ImgForescast3.ImageUrl = link3;
                    string link31 = (string)infoP["forecast"]["txt_forecast"]["forecastday"][5]["icon_url"];
                    ImgForescast31.ImageUrl = link31;
                    LabelLocationForecast.Text = LabelLocation.Text;
                    ImgForescast1.Visible = true;
                    ImgForescast12.Visible = true;
                    ImgForescast2.Visible = true;
                    ImgForescast21.Visible = true;
                    ImgForescast3.Visible = true;
                    ImgForescast31.Visible = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    LabelTest.Text = "Data Not Found";
                }  
            }
            else
            {
                LabelTest.Text = "This source don´t have this service";
            }
        }
    }
}