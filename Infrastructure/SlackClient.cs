using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Altidude.Infrastructure
{
    public sealed class SlackClient
    {
        public static readonly Uri DefaultWebHookUri = new Uri("https://hooks.slack.com/services/.../.../...");

        private readonly Uri _webHookUri;

        public SlackClient(Uri webHookUri)
        {
            this._webHookUri = webHookUri;
        }

        public void SendSlackMessage(SlackMessage message)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                byte[] request = System.Text.Encoding.UTF8.GetBytes("payload=" + JsonConvert.SerializeObject(message));
                byte[] response = webClient.UploadData(this._webHookUri, "POST", request);

                // ...handle response...
            }
        }

        public sealed class SlackMessage
        {
            [JsonProperty("channel")]
            public string Channel { get; set; }

            [JsonProperty("username")]
            public string UserName { get; set; }

            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("icon_emoji")]
            public string Icon
            {
                get { return ":computer:"; }
            }
        }
    }
}
