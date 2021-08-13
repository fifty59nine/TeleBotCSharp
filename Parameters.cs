using System.Collections.Generic;
using CsTeleBot;

namespace TeleBotCSharp
{
    public abstract class Params { }

    public class AllAvalibleParams : Params
    {
        public int chat_id { get; set; }
        public string text { get; set; }
        public string parse_mode { get; set; }
        public List<MessageEntity> entities { get; set; }
        public bool disable_web_page_preview { get; set; }
        public bool disable_notification { get; set; }
        public int reply_to_message_id { get; set; }
        public bool allow_sending_without_reply { get; set; }
        public Markup reply_markup { get; set; }
        public int offset { get; set; }
    }

    public class ForwardMessageParamsV1 : Params
    {
        public int chat_id { get; set; }
        public int from_chat_id { get; set; }
        public bool disable_notification { get; set; }
        public int message_id { get; set; }
    }
    public class ForwardMessageParamsV2 : Params
    {
        public int chat_id { get; set; }
        public string from_chat_id { get; set; }
        public bool disable_notification { get; set; }
        public int message_id { get; set; }
    }
    public class ForwardMessageParamsV3 : Params
    {
        public string chat_id { get; set; }
        public int from_chat_id { get; set; }
        public bool disable_notification { get; set; }
        public int message_id { get; set; }
    }
    public class ForwardMessageParamsV4 : Params
    {
        public string chat_id { get; set; }
        public string from_chat_id { get; set; }
        public bool disable_notification { get; set; }
        public int message_id { get; set; }
    }

    public class SendPhotoParamsV1 : Params
    {
        public int chat_id { get; set; }
        public string photo { get; set; }
        public string caption { get; set; }
        public string parse_mode { get; set; }
        public List<MessageEntity> caption_entities { get; set; }
        public bool disable_notification { get; set; }
        public int reply_to_message_id { get; set; }
        public bool allow_sending_without_reply { get; set; }
        public Markup reply_markup { get; set; }
    }
    public class SendPhotoParamsV2 : Params
    {
        public string chat_id { get; set; }
        public string photo { get; set; }
        public string caption { get; set; }
        public string parse_mode { get; set; }
        public List<MessageEntity> caption_entities { get; set; }
        public bool disable_notification { get; set; }
        public int reply_to_message_id { get; set; }
        public bool allow_sending_without_reply { get; set; }
        public Markup reply_markup { get; set; }
    }

    public class SendAudioParamsV1 : Params
    {
        public int chat_id { get; set; }
        public string audio { get; set; }
        public string caption { get; set; }
        public string parse_mode { get; set; }
        public List<MessageEntity> caption_entities { get; set; }
        public int duration { get; set; }
        public string performer { get; set; }
        public string title { get; set; }
        public string thumb { get; set; }
        public bool disable_notification { get; set; }
        public int reply_to_message_id { get; set; }
        public bool allow_sending_without_reply { get; set; }
        public Markup reply_markup { get; set; }
    }
    public class SendAudioParamsV2 : Params
    {
        public string chat_id { get; set; }
        public string audio { get; set; }
        public string caption { get; set; }
        public string parse_mode { get; set; }
        public List<MessageEntity> caption_entities { get; set; }
        public int duration { get; set; }
        public string performer { get; set; }
        public string title { get; set; }
        public string thumb { get; set; }
        public bool disable_notification { get; set; }
        public int reply_to_message_id { get; set; }
        public bool allow_sending_without_reply { get; set; }
        public Markup reply_markup { get; set; }
    }
}
