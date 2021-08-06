using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.Linq;

using System.Net.Http;

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TeleBotCSharp;

namespace CsTeleBot
{
    /// <summary>
    /// Avalible methods:
    ///     GetMe
    ///     SendMessage
    ///     Polling
    /// </summary>
    public class TeleBotCs
    {
        private const string BaseApiUrl = "https://api.telegram.org/bot";
        public string Token { private get; set; }
        public string ParseMode { private get; set; }
        public int Timeout { get; set; }

        /// <summary>
        /// CsTeleBot object constructor
        /// </summary>
        /// <param name="Token">Telegram bot api token</param>
        /// <param name="ParseMode">Default parse mode. Details on: https://core.telegram.org/bots/api#formatting-options </param>
        public TeleBotCs(string Token, string ParseMode = null, int Timeout = 500)
        {
            this.Token = Token;
            this.ParseMode = ParseMode;
            this.Timeout = Timeout;
        }
        /// <summary>
        /// A simple method for testing your bot's auth token. Requires no parameters.
        /// </summary>
        /// <returns>Basic information about the bot in form of a User object.</returns>
        public async Task<User> GetMe()
        {
            Task<string> taskResult = MakeRequset("getMe");
            string result = taskResult.Result;
            return JsonConvert.DeserializeObject<GetMeResp>(result).result;
        }

        public async Task<SendMessageResp> SendMessage(int chatId, string text, string parseMode = null, List<MessageEntity> entities = null, 
            bool disableWebPagePreview = false, bool disableNotification = false, int replyToMessageId = 0,
            bool allowSendingWithoutReply = false, Markup replyMarkup = null)
        {
            var param = new AllAvalibleParams();
            param.chat_id = chatId;
            param.text = text;
            param.parse_mode = parseMode;
            param.disable_web_page_preview = disableWebPagePreview;
            param.disable_notification = disableNotification;
            param.reply_to_message_id = replyToMessageId;
            param.allow_sending_without_reply = allowSendingWithoutReply;
            param.reply_markup = replyMarkup;
            Task<string> taskResult = MakeRequset("sendMessage", param: param);
            string result = taskResult.Result;
            return JsonConvert.DeserializeObject<SendMessageResp>(result);
        }

        public async Task<GetUpdatesResp> GetUpdates()
        {
            Task<string> taskResult = MakeRequset("getUpdates");
            string result = taskResult.Result;
            return JsonConvert.DeserializeObject<GetUpdatesResp>(result);
        }

        private string httpBuildQuery(Dictionary<string, string> keyValuePairs)
        {
            string result = "";
            foreach (var obj in keyValuePairs)
            {
                var key = obj.Key.Replace("[", "%5B");
                key = key.Replace("]", "%5D");
                key = key.Replace(",", "%2C");
                var val = obj.Value.Replace("[", "%5B");
                val = val.Replace(' ', '+');
                val = val.Replace("]", "%5D");
                val = val.Replace(",", "%2C");
                val = val.Replace(":", "%3A");
                val = val.Replace("/", "%2F");
                result += key + "=" + val + "&";
            }
            return result.Remove(result.Length - 1, 1);
        }

        private async Task<string> MakeRequset(string methodName, string method = "GET", AllAvalibleParams param = null)
        {
            string requestUrl = $"{BaseApiUrl}{this.Token}/{methodName}";
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUrl),
                Content = new StringContent(JsonConvert.SerializeObject(param, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                Encoding.UTF8, "application/json"),
            };
            //var x = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
            //Console.WriteLine(x);
            var response = await client.SendAsync(request).ConfigureAwait(false);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            //Console.WriteLine(result);
            response.EnsureSuccessStatusCode();
            return result;
        }
    }
}
