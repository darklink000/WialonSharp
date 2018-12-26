using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static WialonSharp.Models;

namespace WialonSharp
{
    /// <summary>
    /// This is the primary Class 
    /// </summary>
    public class Wi_api
    {
        /// <summary>
        /// You need to provide you app token, you can generate one on the skd documentacion: https://sdk.wialon.com/playground/demo/token_simple_form
        /// </summary>
        public Wi_Login LoginWialon()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://hst-api.wialon.com/wialon/ajax.html?svc=token/login");

            //Here you need to replace with you Login Token
            var postData = "&params={\"token\":\"\"}";

            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


            Wi_Login login = JsonConvert.DeserializeObject<Wi_Login>(responseString);

            return login;
        }

        /// <summary>
        /// Get all the avaible units on your Wialon Account: https://sdk.wialon.com/playground/demo/get_units
        /// </summary>
        public List<Wi_vehicles> GetUnits(Wi_Login login)
        {

            var request = (HttpWebRequest)WebRequest.Create("https://hst-api.wialon.com/wialon/ajax.html?svc=core/update_data_flags&sid=" + login.eid);

            var postData = "&params={\"spec\":[{\"type\":\"type\",\"data\":\"avl_unit\",\"flags\":1,\"mode\":0}]}";

            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


            List<Wi_vehicles> unidades = JsonConvert.DeserializeObject<List<Wi_vehicles>>(responseString);

            return unidades;
        }
        /// <summary>
        /// Get the message result form a report on your wialon dashboard https://sdk.wialon.com/playground/demo/execute_report
        /// </summary>
        int Get_kms(DateTime FromDate, DateTime ToDate, int wid, Wi_Login login, int template)
        {

            int resultado = 0;

            try
            {
                if (execReport(FromDate, ToDate, wid, login, template,0))
                {
                    var request = (HttpWebRequest)WebRequest.Create("https://hst-api.wialon.com/wialon/ajax.html?svc=report/get_result_rows&sid=" + login.eid.ToString());

                    var postData = "&params={\"tableIndex\":0,\"indexFrom\":0,\"indexTo\":1}";


                    var data = Encoding.UTF8.GetBytes(postData);

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }

                    var response = (HttpWebResponse)request.GetResponse();

                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    Regex regex = new Regex("\\d{3,4} ");
                    string valor = "";
                    try
                    {
                        valor = regex.Match(responseString).Value;
                    }
                    catch (Exception)
                    {
                        valor = "0";
                        //throw;
                    }

                    try
                    {
                        resultado = int.Parse(valor);
                    }
                    catch (Exception)
                    {
                        resultado = 0;
                        // throw;
                    }

                }
            }
            catch (Exception)
            {
                resultado = 0;
                //throw;
            }




            return resultado;
        }

        bool execReport(DateTime FromDate, DateTime ToDate, int wid, Wi_Login login, int template,int resourceID)
        {

            var request = (HttpWebRequest)WebRequest.Create("https://hst-api.wialon.com/wialon/ajax.html?svc=report/exec_report&sid=" + login.eid.ToString());

            var postData = "&params={\"reportResourceId\":"+ resourceID .ToString()+ ",";
            postData += "\"reportTemplateId\":" + template.ToString() + ",";
            postData += "\"reportTemplate\":null,";
            postData += "\"reportObjectId\":" + wid.ToString() + ",";
            postData += "\"reportObjectSecId\":0,";
            postData += "\"interval\":";
            postData += "{ \"from\":" + ConvertToTimestamp(FromDate).ToString() + ",\"to\":" + ConvertToTimestamp(ToDate).ToString() + ",\"flags\":0}";
            postData += "}\";";

            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            Wi_Report responseReport = JsonConvert.DeserializeObject<Wi_Report>(responseString);

            if (responseReport.reportResult.tables.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private long ConvertToTimestamp(DateTime value)
        {
            long epoch = (value.Ticks - 621355968000000000) / 10000000;
            return epoch;
        }
    }

   
}
