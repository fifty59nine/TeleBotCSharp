using System;
using System.Collections.Generic;
using System.Text;

namespace TeleBotCSharp
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MessageHandler : System.Attribute { }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PhotoHandler : System.Attribute { }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandHandler : System.Attribute 
    { 
        public string Command { get; set; }
        public CommandHandler(string command)
        {
            this.Command = command;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CallbackDataHandler : System.Attribute { }
}
