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
using System.IO;
using Telegram.Bot.Types.InputFiles;
using System.Net;
using System.Drawing.Imaging;

namespace CourseWorkForm
{
    public partial class StartWork : Form
    {
        // Определяем текущее время для запуска секундомера.
        DateTime date = DateTime.Now;
        // Токен бота.
        private const string tokenBot = "955523636:AAF3THwqIPSRat5q7TZUBow_B8QEvm8zGW8";
        // Создаем бота с нужным токеном бота.
        private static readonly TelegramBotClient botClient = new TelegramBotClient(tokenBot);
        // Сохраняем номер ВЕДУЩЕГО сообщения бота. 
        Telegram.Bot.Types.Message messageLast;

        string saveInfo = string.Empty;

        public StartWork()
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

        /// <summary>
        /// Метод обрабатывает все реакции нажатия на кнопки inline от пользователя, после чего, в
        /// зависимости от реакции, выводится необходимая информация и меняется логика.
        /// </summary>
        /// <param name="eventUser">Пользователь, которого нужно обработать.</param>
        private async void BotClient_OnCallbackQuery(object sender, CallbackQueryEventArgs eventUser)
        {
            // Создаем пользователя на основе сообщения, т.е. мы получаем от базы данных информацию.
            UserBot user = await WorkWithBD.GetUserAsync(eventUser.CallbackQuery.From.Id, eventUser.CallbackQuery.From.FirstName);
            //catch (TimeoutException)
            //{
            //    logBot.BeginInvoke((MethodInvoker)(
            //        () => logBot.Text += $"У {user.Name} обновилось состояние: {user.State}\n"));
            //    return;
            //}

            string text;
            InlineKeyboardMarkup keyboard;

            switch (eventUser.CallbackQuery.Data)
            {
                // Выбор количества пикселей.
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "10":
                    saveInfo = eventUser.CallbackQuery.Data;

                    LogFromCallback(user, UtilitiesBot.State.S_PickNumberPix,
                        UtilitiesBot.infoText["pickNumberPix"] + $"*{saveInfo}*",
                        UtilitiesBot.keyboards["pickNumberPix"], eventUser);
                    break;
                // Загрузка фотки.
                case "upload":
                    LogFromCallback(user, UtilitiesBot.State.S_Upload, UtilitiesBot.infoText["upload"],
                        UtilitiesBot.keyboards["back"], eventUser);
                    break;
                // Настройки бота.
                case "settings":
                    LogFromCallback(user, UtilitiesBot.State.S_Settings, UtilitiesBot.infoText["setting"],
                        UtilitiesBot.keyboards["setting"], eventUser);
                    break;
                // Выбор количества пикселей.
                case "numberOfPixels":
                    LogFromCallback(user, UtilitiesBot.State.S_NumberOfPixels,
                        UtilitiesBot.infoText["numberOfPixels"] + user.Settings["amount"],
                        UtilitiesBot.keyboards["numberOfPixels"], eventUser);
                    break;
                // Выбор режима.
                case "mode":
                    LogFromCallback(user, UtilitiesBot.State.S_Mode,
                        UtilitiesBot.infoText["mode"] + user.Settings["mode"],
                        UtilitiesBot.keyboards["mode"], eventUser);
                    break;
                // Выбранный режим.
                case "artist":
                case "colorblind":
                    user.Settings["mode"] = eventUser.CallbackQuery.Data;

                    if ((int)user.State == 6)
                    {
                        text = UtilitiesBot.infoText["setting"];
                        keyboard = UtilitiesBot.keyboards["setting"];
                    }
                    else
                    {
                        text = UtilitiesBot.infoText["info"];
                        keyboard = UtilitiesBot.keyboards["info"];
                    }

                    LogFromCallback(user, UtilitiesBot.State.S_Info, text, keyboard, eventUser);
                    break;
                case "back":
                    UtilitiesBot.State state;

                    if ((int)user.State == 3 || (int)user.State == 4 || (int)user.State == 6)
                    {
                        text = UtilitiesBot.infoText["setting"];
                        keyboard = UtilitiesBot.keyboards["setting"];
                        state = UtilitiesBot.State.S_Settings;

                        if ((int)user.State == 4)
                        {
                            user.Settings["amount"] = saveInfo;
                        }
                    }
                    else
                    {
                        text = UtilitiesBot.infoText["info"];
                        keyboard = UtilitiesBot.keyboards["info"];
                        state = UtilitiesBot.State.S_Info;
                    }

                    LogFromCallback(user, state, text, keyboard, eventUser);
                    break;
                default:
                    break;
            }

            // Всплывающее уведомление
            //await botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нажали на такую вот кнопку {buttonText}");
        }

        /// <summary>
        /// Будет выводиться информация о передвижениях пользователя по меню, также обновляются
        /// его данные в базе данных. Обрабатывается следующий шаг бота.
        /// </summary>
        /// <param name="user">Пользователь, чьи передвижения вносим в базу данных.</param>
        /// <param name="state">На какой кнопке находится пользователь.</param>
        /// <param name="text">Какой текст сообщения выводится.</param>
        /// <param name="keyboard">Какая клавиатура передается.</param>
        async void LogFromCallback(UserBot user, UtilitiesBot.State state, string text,
            InlineKeyboardMarkup keyboard, CallbackQueryEventArgs eventUser)
        {
            try
            {
                messageLast = await botClient.EditMessageTextAsync(
                        chatId: eventUser.CallbackQuery.From.Id,
                        messageId: eventUser.CallbackQuery.Message.MessageId,
                        parseMode: ParseMode.Markdown,
                        text: text,
                        replyMarkup: keyboard
                    );

                user.State = state;
                // Выводим информацию в log о том, что меняется.
                logBot.BeginInvoke((MethodInvoker)(
                        () => logBot.Text += $"For {user.Name} the state has been updated: {user.State}\n"));
                WorkWithBD.UpdateUserAsync(user);
            }
            catch (Telegram.Bot.Exceptions.MessageIsNotModifiedException error)
            {
                logBot.BeginInvoke((MethodInvoker)(
                        () => logBot.Text += error.Message + Environment.NewLine));
            }
        }

        /// <summary>
        /// Получает от пользователя сообщения вроде изображений, набора текста и так далее, потом
        /// в зависимости от сообщения, применяет разные подходы.
        /// </summary>
        /// <param name="eventUser">Какое сообщение нужно обрабатывать.</param>
        async void Bot_OnMessage(object sender, MessageEventArgs eventUser)
        {
            Telegram.Bot.Types.Message message = eventUser.Message;
            // Создаем пользователя на основе сообщения, т.е. мы получаем от базы данных информацию.
            UserBot user = await WorkWithBD.GetUserAsync(message.Chat.Id, message.From.FirstName);

            // Проверка на пустое сообщение и на то, что сообщение является набором букв.
            if (message == null)
            {
                return;
            }
            else if (message.Type == MessageType.Text)
            {
                // Выводит информацию о действиях пользователя:
                logBot.BeginInvoke((MethodInvoker)(
                    () => logBot.Text += $"{user.Name} sent a message at: {message.Date} to " +
                    $"{user.ChatId} with such text:\n{message.Text}\n"));

                Bot_OnMessage_Text(user, message);
            }
            else if (message.Type == MessageType.Photo)
            {
                Bot_OnMessage_Photo(user, message);
            }
        }

        async void Bot_OnMessage_Photo(UserBot user, Telegram.Bot.Types.Message message)
        {
            if ((int)user.State == 1)
            {
                GetPhoto(message, user);
            }
            else
            {
                BotErrorMessage(message);
            }
        }

        async void BotErrorMessage(Telegram.Bot.Types.Message message)
        {
            try
            {
                await botClient.DeleteMessageAsync(message.Chat.Id, messageLast.MessageId);
                // Этот раздел нужен для того, чтобы сказать, что команда неверна и будет
                // выведена соотвествующая информация.
                await botClient.SendTextMessageAsync(
                          chatId: message.Chat,
                          text: UtilitiesBot.infoText["error"]
                        );

                messageLast = await botClient.SendTextMessageAsync(
                            chatId: message.Chat,
                            parseMode: ParseMode.Markdown,
                            text: messageLast.Text,
                            replyMarkup: messageLast.ReplyMarkup
                        );
            }
            catch (NullReferenceException error)
            {
                logBot.BeginInvoke((MethodInvoker)(
                        () => logBot.Text += error.Message + Environment.NewLine));
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException error)
            {
                logBot.BeginInvoke((MethodInvoker)(
                    () => logBot.Text += error.Message + Environment.NewLine));
                await botClient.SendTextMessageAsync(
                          chatId: message.Chat,
                          text: "*Мне стало плохо... Даже железяки могут подводить. Напишите, " +
                          "пожалуйста, /start, для того, чтобы я заработал.*",
                          parseMode: ParseMode.Markdown
                        );
            }
        }

        private async void GetPhoto(Telegram.Bot.Types.Message message, UserBot user)
        {
            try
            {
                var file = await botClient.GetFileAsync(message.Photo[message.Photo.Length - 1].FileId);

                Bitmap image = WorkWithImage.GetResult(new Bitmap(
                    new WebClient().OpenRead(@"https://api.telegram.org/file/bot" + tokenBot + "/" 
                    + file.FilePath)), int.Parse(user.Settings["amount"].ToString()));

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, ImageFormat.Png);
                    memoryStream.Position = 0;
                    await botClient.SendPhotoAsync(
                                chatId: message.Chat, 
                                photo: memoryStream,
                                caption: "Ваш результат.");
                }
            }
            catch (IOException)
            {
                logBot.BeginInvoke((MethodInvoker)(() => logBot.Text += $"Ошибка ввода-вывода."));
            }
            catch (System.Security.SecurityException)
            {
                logBot.BeginInvoke((MethodInvoker)(() => logBot.Text += $"Ошибка безопасности."));
            }
            catch (UnauthorizedAccessException)
            {
                logBot.BeginInvoke((MethodInvoker)(() => logBot.Text += $"Ошибка доступа."));
            }
            catch (Exception)
            {
                logBot.BeginInvoke((MethodInvoker)(
                    () => logBot.Text += $"Произошел конец света, упс."));
            }
        }

        async void Bot_OnMessage_Text(UserBot user, Telegram.Bot.Types.Message message)
        {
            switch (message.Text)
            {
                case "/start":
                    // Обновляем состояние пользователя.
                    user.State = UtilitiesBot.State.S_Start;
                    // Проверяем на то, что такой пользователь есть ли в базе, при отсутствии -- добавляем.
                    WorkWithBD.AddUserAsync(user);
                    // Выводим информацию в log о том, что меняется.
                    logBot.BeginInvoke((MethodInvoker)(
                            () => logBot.Text += $"For {user.Name} the state has been updated: {user.State}\n"));
                    // Бот информирует пользователя о текущем разделе.
                    messageLast = await botClient.SendTextMessageAsync(
                              chatId: message.Chat,
                              parseMode: ParseMode.Markdown,
                              text: UtilitiesBot.infoText["start"],
                              replyMarkup: UtilitiesBot.keyboards["start"]
                            );
                    break;
                //case "Петушара":
                //    await botClient.SendPhotoAsync(
                //          chatId: message.Chat,
                //          photo: "https://d2hhj3gz5jljkm.cloudfront.net/assets2/159/662/390/528/normal/file.jpg"
                //        );
                //    break;
                default:
                    BotErrorMessage(message);
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
        /// При закрытии формы, бот прекращает работу, форма окончательно закрывается.
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            botClient.StopReceiving();
            Application.Exit();
        }
    }
}
