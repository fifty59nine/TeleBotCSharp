using CsTeleBot;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TeleBotCSharp;

namespace TeleBotCSharp
{
    public class TeleBotService : BackgroundService
    {
        TeleBotCs bot { get; set; }
        private int LastUpdateId { get; set; } = 0;

        public TeleBotService(TeleBotCs bot)
        {
            this.bot = bot;
        }

        public async Task StartBotAsync()
        {
            await ExecuteAsync(new CancellationToken());
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Polling();
            }
        }

        private async Task Polling()
        {
            bool DoesFirstTime = true;
            List<int> MessagesIds = new List<int>();
            while (true)
            {
                GetUpdatesResp resp = await bot.GetUpdates(offset: LastUpdateId);
                foreach (Result result in resp.result)
                {
                    LastUpdateId = result.update_id;
                    if (DoesFirstTime)
                    {
                        try
                        {
                            MessagesIds.Add(result.message.message_id);
                        }
                        catch (NullReferenceException)
                        {
                            MessagesIds.Add(result.callback_query.message.message_id);
                        }
                        continue;
                    }
                    try
                    {
                        if (MessagesIds.Contains(result.message.message_id)) {  }
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
                    }
                    catch (NullReferenceException)
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
                Thread.Sleep(this.bot.Timeout);
                DoesFirstTime = false;
            }
        }
    }
}
