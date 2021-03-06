using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TeleBotCSharp
{
    /// <summary>
    /// Avalible methods:
    ///     GetMe
    ///     SendMessage
    ///     Polling
    ///     ForwardMessage
    ///     SendAudio
    ///     SendPhoto
    ///     AnswerCallbackQuery
    /// </summary>
    public class TeleBotCs
    {
        private const string BaseApiUrl = "https://api.telegram.org/bot";
        public string Token { private get; set; }
        public string ParseMode { private get; set; }
        public int Timeout { get; set; }

        /// <summary>
        /// TeleBotCs object constructor
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

        public async Task<GetUpdatesResp> GetUpdates(int offset = 0)
        {
            var param = new AllAvalibleParams { offset = offset };
            Task<string> taskResult = MakeRequset("getUpdates", param: param);
            string result = taskResult.Result;
            return JsonConvert.DeserializeObject<GetUpdatesResp>(result);
        }

        #region ForwardMessage overloadings
        public async Task ForwardMessage(int chatId, int fromChatId, int messageId,
            bool disableNotification = false)
        {
            var param = new ForwardMessageParamsV1
            {
                chat_id = chatId,
                from_chat_id = fromChatId,
                message_id = messageId,
                disable_notification = disableNotification
            };
            await MakeRequset("forwardMessage", param: param);
        }
        public async Task ForwardMessage(int chatId, string fromChatId, int messageId,
            bool disableNotification = false)
        {
            var param = new ForwardMessageParamsV2
            {
                chat_id = chatId,
                from_chat_id = fromChatId,
                message_id = messageId,
                disable_notification = disableNotification
            };
            await MakeRequset("forwardMessage", param: param);
        }
        public async Task ForwardMessage(string chatId, int fromChatId, int messageId,
            bool disableNotification = false)
        {
            var param = new ForwardMessageParamsV3
            {
                chat_id = chatId,
                from_chat_id = fromChatId,
                message_id = messageId,
                disable_notification = disableNotification
            };
            await MakeRequset("forwardMessage", param: param);
        }
        public async Task ForwardMessage(string chatId, string fromChatId, int messageId,
            bool disableNotification = false)
        {
            var param = new ForwardMessageParamsV4
            {
                chat_id = chatId,
                from_chat_id = fromChatId,
                message_id = messageId,
                disable_notification = disableNotification
            };
            await MakeRequset("forwardMessage", param: param);
        }
        #endregion

        #region SendPhoto overloadings
        public async Task SendPhoto(int chatId, string photo,
            string caption = null, string parseMode = null, List<MessageEntity> captionEntites = null,
            bool disableNotification = false, int replyToMessageId = 0,
            bool allowSendingWithoutReply = false, Markup replyMarkup = null)
        {
            var param = new SendPhotoParamsV1
            {
                chat_id = chatId,
                photo = photo,
                caption = caption,
                parse_mode = parseMode,
                caption_entities = captionEntites,
                disable_notification = disableNotification,
                allow_sending_without_reply = allowSendingWithoutReply,
                reply_markup = replyMarkup
            };
            if (replyToMessageId != 0) { param.reply_to_message_id = replyToMessageId; }
        }
        public async Task SendPhoto(string chatId, string photo,
            string caption = null, string parseMode = null, List<MessageEntity> captionEntites = null,
            bool disableNotification = false, int replyToMessageId = 0,
            bool allowSendingWithoutReply = false, Markup replyMarkup = null)
        {
            var param = new SendPhotoParamsV2
            {
                chat_id = chatId,
                photo = photo,
                caption = caption,
                parse_mode = parseMode,
                caption_entities = captionEntites,
                disable_notification = disableNotification,
                allow_sending_without_reply = allowSendingWithoutReply,
                reply_markup = replyMarkup
            };
            if (replyToMessageId != 0) { param.reply_to_message_id = replyToMessageId; }
        }
        #endregion

        #region SendAudio overloadings
        public async Task SendAudio(int chatId, string audio, string caption = null, string parseMode = null,
            List<MessageEntity> captionEntities = null, int duration = 0, string performer = null, string title = null, string thumb = null)
        {
            var param = new SendAudioParamsV1
            {
                chat_id = chatId,
                audio = audio,
                caption = caption,
                parse_mode = parseMode,
                caption_entities = captionEntities,
                duration = duration,
                performer = performer,
                title = title,
                thumb = thumb
            };
            await MakeRequset("sendAudio", param: param);
        }

        public async Task SendAudio(string chatId, string audio, string caption = null, string parseMode = null,
    List<MessageEntity> captionEntities = null, int duration = 0, string performer = null, string title = null, string thumb = null)
        {
            var param = new SendAudioParamsV2
            {
                chat_id = chatId,
                audio = audio,
                caption = caption,
                parse_mode = parseMode,
                caption_entities = captionEntities,
                duration = duration,
                performer = performer,
                title = title,
                thumb = thumb
            };
            await MakeRequset("sendAudio", param: param);
        }
        #endregion

        #region AnswerCallbackQuery

        public async Task AnswerCallbackQuery(string callbackQueryId, string text = null, bool showAlert = false,
            string url = null, int cacheTime = 0)
        {
            var param = new AnswerCallbackQueryParams
            {
                callback_query_id = callbackQueryId,
                text = text,
                show_alert = showAlert,
                url = url,
                cache_time = cacheTime
            };
            MakeRequset("answerCallbackQuery", param: param);
        }

        #endregion

        #region private methods
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

        private async Task<string> MakeRequset(string methodName, string method = "GET", Params param = null)
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
        #endregion
    }
}
