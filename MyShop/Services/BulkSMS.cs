using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebTestSMSBulk.Services
{
    public class BulkSMS
    {

        public BulkSMS(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        private readonly IHttpClientFactory _clientFactory;
        public string SenderId { get; set; }
        public bool IsUnicode { get; set; }
        public bool IsFlash { get; set; }
        public List<BulkSMSMessage> MessageParameters { get; set; }
        public string ApiKey { get; set; }
        public string ClientId { get; set; }

        public async Task<HttpResponseMessage> SendBulkSMS(string senderId, List<BulkSMSMessage> messageParameter,
            string apiKey, string clientId,
            bool isUnicode = true, bool isFlash=true)
        {

            SenderId = senderId;
            IsUnicode = isUnicode;
            IsFlash = IsFlash;
            MessageParameters = messageParameter;
            ApiKey = apiKey;
            ClientId = clientId;
            
            var client = _clientFactory.CreateClient("AccountClient");
            HttpContent httpcontent
                = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://api.onfonmedia.co.ke/v1/sms/SendBulkSMS", httpcontent);

            return response;
        }
    }


    public class BulkSMSMessage
    {
        public string Number { get; set; }
        public string Text { get; set; }
    }
}
