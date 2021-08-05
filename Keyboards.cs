using System;
using System.Collections.Generic;
using System.Text;

namespace TeleBotCSharp
{
    public class Markup { }

    public class ReplyKeyboardMarkup : Markup
    {
        public List<List<KeyboardButton>> keyboard { get; set; }
        public bool resize_keyboard { get; set; }
        public bool one_time_keyboard { get; set; }
        public string input_field_placeholder { get; set; }
        public bool selective { get; set; }

        /// <summary>
        /// This object represents a custom keyboard with reply options 
        /// </summary>
        /// <param name="resizeKeyboard">Requests clients to resize the keyboard vertically for optimal fit </param>
        /// <param name="oneTimeKeyboard">Requests clients to hide the keyboard as soon as it's been used</param>
        /// <param name="inputFieldPlaceHolder">The placeholder to be shown in the input field when the keyboard is active</param>
        /// <param name="selective">Use this parameter if you want to show the keyboard to specific users only</param>
        public ReplyKeyboardMarkup(bool resizeKeyboard = false, bool oneTimeKeyboard = false, 
            string inputFieldPlaceHolder = null, bool selective = false)
        {
            this.keyboard = new List<List<KeyboardButton>>();
            this.resize_keyboard = resizeKeyboard; this.one_time_keyboard = oneTimeKeyboard;
            this.input_field_placeholder = inputFieldPlaceHolder; this.selective = selective;
        }

        public void Add(KeyboardButton button)
        {
            this.keyboard.Add(new List<KeyboardButton> { button });
        }
    }
    public class KeyboardButton
    {
        public string text { get; set; }
        public bool request_contact { get; set; }
        public bool request_location { get; set; }
        public KeyboardButtonPollType request_poll { get; set; }

        public KeyboardButton(string text, bool requestContact = false, 
            bool requestLocation = false, KeyboardButtonPollType requestPoll = null)
        {
            this.text = text; this.request_contact = requestContact; this.request_location = requestLocation;
            this.request_poll = requestPoll;
        }
    }

    public class KeyboardButtonPollType
    {
        public string type { get; set; }

        /// <summary>
        /// This object represents type of a poll, which is allowed to be created and sent when the corresponding button is pressed.
        /// </summary>
        /// <param name="type">If quiz is passed, the user will be allowed to create only polls in the quiz mode. If regular is passed, only regular polls will be allowed. Otherwise, the user will be allowed to create a poll of any type</param>
        public KeyboardButtonPollType(string type)
        {
            this.type = type;
        }
    }
}
