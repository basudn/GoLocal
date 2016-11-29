using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace GoLocal.Util
{
    public class AppUtil
    {
        public static Dictionary<string, string> GetProps()
        {
            Dictionary<string, string> prop = new Dictionary<string, string>();
            foreach (var row in System.IO.File.ReadAllLines(HostingEnvironment.MapPath(@"~/App_Data/CustomProps.properties")))
                prop.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));
            return prop;
        }

        public static async Task<string> GetMapData(string latLng)
        {
            HttpClient client = new HttpClient();
            string zip = string.Empty;
            string country = string.Empty;
            HttpResponseMessage message = await client.GetAsync(new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + latLng + "&key=" + AppUtil.GetProps()["gKey"]));
            if (message.IsSuccessStatusCode)
            {
                string response = await message.Content.ReadAsStringAsync();
                GMapResponse res = JsonConvert.DeserializeObject<GMapResponse>(response);
                foreach (AddressComp comp in res.results[0].address_components)
                {
                    if (comp.types.Contains("postal_code"))
                    {
                        zip = comp.short_name;
                    }
                    if (comp.types.Contains("country"))
                    {
                        country = comp.short_name;
                    }
                }
            }
            return country + "-" + zip;
        }

        //Satrt Send Email Function
        public static string SendMail(string toList, string subject, string body)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            string msg = "";
            try
            {
                var prop = GetProps();
                MailAddress fromAddress = new MailAddress(prop["mailFrom"]);
                message.From = fromAddress;
                message.To.Add(toList);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                // We use gmail as our smtp client
                smtpClient.Host = prop["mailHost"];
                smtpClient.Port = int.Parse(prop["mailPort"]);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new System.Net.NetworkCredential(
                    prop["mailFrom"], prop["pwd"]);

                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
        //End Send Email Function
    }
}