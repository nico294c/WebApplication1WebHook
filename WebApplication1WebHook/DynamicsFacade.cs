using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Web.Configuration;
using WebApplication1WebHook.Controllers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNet.WebHooks.Config;

namespace WebApplication1WebHook
{
    public class DynamicsFacade
    {

        public async Task CreateCustomer(String name, String email, String id)
        {
            System.Diagnostics.Debug.WriteLine("Starting CreateCustomer Task...");

            var _token = $"admin:Password";
            var _tokenBase64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_token));

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Authorization =  new AuthenticationHeaderValue("Basic", "YWRtaW46UGFzc3dvcmQ=");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _tokenBase64);

            parameter p = new parameter { name = name, email = email, id = id };
            String jsonData = JsonConvert.SerializeObject(p);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            System.Diagnostics.Debug.WriteLine("Sending asynchronous Post request...");
            HttpResponseMessage response = await client.PostAsync("http://test:7048/BC/ODataV4/Wordpress_CreateCustomerWs?company=CRONUS%20Danmark%20A%2FS", content);
            System.Diagnostics.Debug.WriteLine("Respone recieved!");


            string data = "";

            if (response.IsSuccessStatusCode)
            {
                data = await response.Content.ReadAsStringAsync();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Error: " + response.StatusCode);
            }

            System.Diagnostics.Debug.WriteLine("Result: " + data);
        }


        public async Task InsertData(String pname, String pemail)
        {
            //l/p
            var _token = $"admin:Password";
            var _tokenBase64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_token));

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _tokenBase64);

            var parameter = new Student { name = pname, lastname = pemail };
            String jsonData = JsonConvert.SerializeObject(parameter);

            var payload = new Payload { student = jsonData };
            var payloadJSON = JsonConvert.SerializeObject(payload);


            var inputData = new StringContent(payloadJSON, Encoding.Unicode, "application/json");

            System.Diagnostics.Debug.WriteLine("json: " + await inputData.ReadAsStringAsync());

            HttpResponseMessage response = await client.PostAsync("http://test:7049/BC/ODataV4/StudentWS_createstudentjs?company=CRONUS%20Danmark%20A%2FS", inputData);

            string data = "";

            if (response.IsSuccessStatusCode)
            {
                data = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine("Result: " + data);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Error: " + response.ReasonPhrase);
            }
        }

    }

    class parameter
    {
        
        public string name { get; set; }
        public string email { get; set; }
        public string id { get; set; }

    }

    

    class Student
    {
        [JsonProperty("name")]
        public String name { get; set; }

        [JsonProperty("lastname")]
        public String lastname { get; set; }
    }

    class Payload
    {
        [JsonProperty("student")]
        public String student { get; set; }
    }
    
}
