using System;
using System.Collections.Generic;
using System.Text;
using TeleBotCSharp;

namespace CsTeleBot
{
    public class GetMeResponse
    {
        public bool Ok { get; private set; }
        public User Result { get; private set; }
    }
    public class User
    {
        public int id { get; set; }
        public bool is_bot { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string language_code { get; set; }
        public bool can_join_groups { get; set; }
        public bool can_read_all_group_messages { get; set; }
        public bool supports_inline_queries { get; set; }
    }

    public class GetMeResp
    {
        public bool ok { get; set; }
        public User result { get; set; }
    }

    public class From
    {
        public int id { get; set; }
        public bool is_bot { get; set; }
        public string first_name { get; set; }
        public string username { get; set; }
        public string language_code { get; set; }
    }

    public class Chat
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string username { get; set; }
        public string type { get; set; }
    }

    public class Message
    {
        public int message_id { get; set; }
        public From from { get; set; }
        public Chat chat { get; set; }
        public int date { get; set; }
        public string text { get; set; }
        public List<Photo> photo { get; set; }
        public string caption { get; set; }
    }

    public class SendMessageResp
    {
        public bool ok { get; set; }
        public Message result { get; set; }
    }

    public class MessageEntity
    {
        public string type { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
        public string url { get; set; }
        public User user { get; set; }
        public string language { get; set; }
    }

    public class Result
    {
        public int update_id { get; set; }
        public Message message { get; set; }
        public CallbackQuery callback_query { get; set; }
    }

    public class GetUpdatesResp
    {
        public bool ok { get; set; }
        public List<Result> result { get; set; }
    }

    public class Photo
    {
        public string file_id { get; set; }
        public string file_unique_id { get; set; }
        public int file_size { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class CallbackQuery
    {
        public string id { get; set; }
        public From from { get; set; }
        public Message message { get; set; }
        public string chat_instance { get; set; }
        public string data { get; set; }
    }
}
