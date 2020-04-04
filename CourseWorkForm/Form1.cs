using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using CourseWork_Library;


namespace CourseWorkForm
{
    public partial class Form1 : Form
    {
        // Определяем текущее время для запуска секундомера.
        DateTime date = DateTime.Now;
        // Создаем бота с нужным токеном бота.
        private static readonly TelegramBotClient botClient =
            new TelegramBotClient("955523636:AAF3THwqIPSRat5q7TZUBow_B8QEvm8zGW8");

        public Form1()
        {
            InitializeComponent();

            // Запускаем таймер текущей работы.
            stopwatch.Tick += new EventHandler(tickTimer);
            stopwatch.Start();

            // Подписка на сообщения от пользователя.
            botClient.OnMessage += Bot_OnMessage;
            // Подписка на inlin'ы от пользователя.
            botClient.OnCallbackQuery += BotClient_OnCallbackQuery;
            // Запускаем бота.
            botClient.StartReceiving();
        }

        private async void BotClient_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            string buttonText = e.CallbackQuery.Data;
            switch (buttonText)
            {
                case "upload":
                    InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[]
                    {
                                      new []
                                      {
                                           InlineKeyboardButton.WithCallbackData("Назад", "back")
                                      },
                    });

                    await botClient.SendTextMessageAsync(
                                  chatId: e.CallbackQuery.From.Id,
                                  text: DictionaryBot.infoText["upload"],
                                  replyMarkup: keyboard
                                );
                    break;
                case "settings":
                    break;
                case "back":
                    break;
                default:
                    break;
            }

            // Всплывающее уведомление
            //await botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нажали на такую вот кнопку {buttonText}");
        }

        async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            Telegram.Bot.Types.Message message = e.Message;

            if (message == null || message.Type != MessageType.Text)
            {
                return;
            }

            // Выводит информацию о действиях пользователя:
            logBot.BeginInvoke((MethodInvoker)(
                () => logBot.Text += $"{message.From.FirstName} отправил сообщение " +
                $"в чат {message.Chat.Id} с таким текстом: {message.Text} время: {message.Date}.\n"));

            switch (message.Text)
            {
                case "/start":
                    InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[]
                    {
                                      new []
                                      {
                                           InlineKeyboardButton.WithCallbackData("Загрузить фото", "upload"),
                                           InlineKeyboardButton.WithCallbackData("Настройки", "settings")
                                      },
                                      new []
                                      {
                                           InlineKeyboardButton.WithUrl("Контакты", "https://t.me/wolfalm"),
                                      }
                    });

                    await botClient.SendTextMessageAsync(
                              chatId: message.Chat,
                              parseMode: ParseMode.Markdown,
                              text: DictionaryBot.infoText["info"],
                              replyMarkup: keyboard
                            );
                    break;
                default:
                    await botClient.SendTextMessageAsync(
                              chatId: message.Chat,
                              text: DictionaryBot.infoText["error"]
                            );
                    break;
            }
        }

        /// <summary>
        /// Выводит информацию в качестве секундомера о текущей работе.
        /// </summary>
        private void tickTimer(object sender, EventArgs e)
        {
            // Форматируем время под нужный нам формат и обновляем информацию.
            labelStopwatch.Text = "Время работы: " + new DateTime().
                AddTicks(DateTime.Now.Ticks - date.Ticks).ToString("HH:mm:ss:ff");
        }

        /// <summary>
        /// При закрытии формы, бот останавливается насовсем.
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            botClient.StopReceiving();
            Application.Exit();
        }
    }
}
