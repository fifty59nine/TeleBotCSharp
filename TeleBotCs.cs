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

        /// <summary>
        /// CsTeleBot object constructor
        /// </summary>
        /// <param name="Token">Telegram bot api token</param>
        /// <param name="ParseMode">Default parse mode. Details on: https://core.telegram.org/bots/api#formatting-options </param>
        public TeleBotCs(string Token, string ParseMode = null)
        {
            this.Token = Token;
            this.ParseMode = ParseMode;
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

        private async Task<GetUpdatesResp> GetUpdates()
        {
            Task<string> taskResult = MakeRequset("getUpdates");
            string result = taskResult.Result;
            return JsonConvert.DeserializeObject<GetUpdatesResp>(result);
        }

        /// <summary>
        /// Start check messages
        /// </summary>
        /// <param name="timeout">Checking timeout in millisec</param>
        public async Task Polling(int timeout = 2000)
        {
            bool DoesFirstTime = true;
            List<int> MessagesIds = new List<int>();
            while (true)
            {
                GetUpdatesResp resp = await GetUpdates();
                foreach (Result result in resp.result)
                {
                    if (DoesFirstTime)
                    {
                        try
                        {
                            MessagesIds.Add(result.message.message_id);
                        } catch (NullReferenceException)
                        {
                            MessagesIds.Add(result.message.message_id);
                        }
                    }
                    try
                    {
                        if (MessagesIds.Contains(result.message.message_id)) { }
                        else
                        {
                            Type messageType = null;
                            if (result.message.photo != null)
                            {
                                messageType = typeof(PhotoHandler);
                            }
                            else if (result.message.text.StartsWith('/'))
                            {
                                messageType = typeof(CommandHandler);
                            }
                            else if (result.message.text != null)
                            {
                                messageType = typeof(MessageHandler);
                            }
                            var methods = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(x => x.GetTypes())
                                .Where(x => x.IsClass)
                                .SelectMany(x => x.GetMethods())
                                .Where(x => x.GetCustomAttributes(messageType, false).FirstOrDefault() != null);
                            if (messageType == typeof(CommandHandler))
                            {
                                foreach (var x in methods)
                                {
                                    foreach (var command in x.GetCustomAttributesData().ToArray().Last().ConstructorArguments)
                                    {
                                        if (command.ToString().Replace("\"", "") == result.message.text.Replace("/", ""))
                                        {
                                            var temp = Activator.CreateInstance(x.DeclaringType);
                                            x.Invoke(temp, new object[1] { result });
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (var x in methods)
                                {
                                    var obj = Activator.CreateInstance(x.DeclaringType);
                                    x.Invoke(obj, new object[1] { result });
                                }
                            }
                        }
                        MessagesIds.Add(result.message.message_id);
                    } catch (NullReferenceException)
                    {
                        if (MessagesIds.Contains(result.callback_query.message.message_id)) { }
                        else
                        {
                            var messageType = typeof(CallbackDataHandler);
                            var methods = AppDomain.CurrentDomain.GetAssemblies()
                                    .SelectMany(x => x.GetTypes())
                                    .Where(x => x.IsClass)
                                    .SelectMany(x => x.GetMethods())
                                    .Where(x => x.GetCustomAttributes(messageType, false).FirstOrDefault() != null);
                            foreach (var x in methods)
                            {
                                var obj = Activator.CreateInstance(x.DeclaringType);
                                x.Invoke(obj, new object[1] { result });
                            }
                            MessagesIds.Add(result.callback_query.message.message_id);
                        }                        
                    }
                }
                Thread.Sleep(timeout);
                DoesFirstTime = false;
            }
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
