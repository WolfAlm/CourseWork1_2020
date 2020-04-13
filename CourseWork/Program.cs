using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using System.Threading;
using Telegram.Bot.Args;
using System.Drawing;

namespace CourseWork
{
    class WorkWithImage
    {

    }
    class Program
    {
        // Токен бота.
        private static readonly TelegramBotClient botClient = 
            new TelegramBotClient("955523636:AAF3THwqIPSRat5q7TZUBow_B8QEvm8zGW8");
        static string imageFilePath = @"C:\Users\user\Desktop\file.jpg";

        static void Main(string[] args)
        {
            Start();

            Dictionary<string, ulong> arrayPixels = new Dictionary<string, ulong>();
            arrayPixels.Add("ф", 124);
            arrayPixels.Add("фаы", 22222);
            arrayPixels.Add("фа", 2);
            var ar = arrayPixels.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            foreach (var kvp in ar)
                Console.WriteLine("Key: " + kvp.Key + "; Value: " + kvp.Value);


            Console.ReadLine();
            botClient.StopReceiving();
        }

        static void Start()
        {
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
        }
        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

                var res = await botClient.SendTextMessageAsync(
                  chatId: e.Message.Chat,
                  text: "Я твое эхо и кидаю к тебе стрелки, ха!\n\n" + $"\"{e.Message.Text}\""
                );
                await botClient.DeleteMessageAsync(chatId: e.Message.Chat, messageId: res.MessageId);
            }
        }
    }
}
