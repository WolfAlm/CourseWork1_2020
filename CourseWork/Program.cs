using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using System.Threading;
using Telegram.Bot.Args;
using System.Drawing;
using System.Collections;
using CourseWork_Library;
using Telegram.Bot.Types.Enums;
using System.IO;
using System.Drawing.Imaging;
using System.Net;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CourseWork
{
    class Program
    {
        // Определяем текущее время для запуска секундомера.
        DateTime date = DateTime.Now;
        // Токен бота.
        private static string tokenBot = "955523636:AAF3THwqIPSRat5q7TZUBow_B8QEvm8zGW8";
        // Создаем бота с нужным токеном бота.
        private static readonly TelegramBotClient bot = new TelegramBotClient(tokenBot);
        //// Сохраняем номер ВЕДУЩЕГО сообщения бота. 
        //static private Telegram.Bot.Types.Message messageLast;
        //// Костыль.
        //static string saveInfo = string.Empty;

        //static List<Bitmap> arrayBitmaps = new List<Bitmap>();
        //static string idGroupMedia = string.Empty;
        //int step = 0;

        //static void Main(string[] args)
        //{
        //    var me = bot.GetMeAsync().Result;
        //    Console.WriteLine(me);
        //    // Подписка на сообщения от пользователя.
        //    bot.OnMessage += Bot_OnMessage;
        //    bot.StartReceiving();
        //    Console.WriteLine("Запустились");

        //    WebRequest request = WebRequest.Create("http://www.contoso.com/PostAccepter.aspx");

        //    Console.ReadLine();
        //    bot.StopReceiving();
        //}



        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

                var res = await bot.SendTextMessageAsync(
                  chatId: e.Message.Chat,
                  text: "Я твое эхо и кидаю к тебе стрелки, ха!\n\n" + $"\"{e.Message.Text}\""
                );

                await bot.DeleteMessageAsync(chatId: e.Message.Chat, messageId: res.MessageId);
            }
        }


        ///// <summary>
        ///// Будет выводиться информация о передвижениях пользователя по меню, также обновляются
        ///// его данные в базе данных. Обрабатывается следующий шаг бота.
        ///// </summary>
        ///// <param name="user">Пользователь, чьи передвижения вносим в базу данных.</param>
        ///// <param name="state">На какой кнопке/этапе меню находится пользователь.</param>
        ///// <param name="text">Какой текст сообщения выводится.</param>
        ///// <param name="keyboard">Какая клавиатура передается.</param>
        ///// <param name="eventUser">Какого пользователя будем обрабатывать.</param>
        //static private async void LogFromCallback(UserBot user, UtilitiesBot.State state, string text,
        //    InlineKeyboardMarkup keyboard, CallbackQueryEventArgs eventUser)
        //{
        //        messageLast = await bot.EditMessageTextAsync(
        //                chatId: eventUser.CallbackQuery.From.Id,
        //                messageId: eventUser.CallbackQuery.Message.MessageId,
        //                parseMode: ParseMode.Markdown,
        //                text: text,
        //                replyMarkup: keyboard
        //            );

        //        user.State = state;
        //        // Вносим изменения в базу данных.
        //        WorkWithBD.UpdateUserAsync(user);
        //    // Если кнопку будут нажимать несколько раз, то мы отлавливаем такие ошибки и просто не
        //    // даем боту падать.
        //}

        ///// <summary>
        ///// Получает от пользователя сообщения вроде изображений, набора текста и так далее, потом
        ///// в зависимости от сообщения, применяет разные подходы.
        ///// </summary>
        ///// <param name="eventUser">Какое сообщение нужно обрабатывать.</param>
        //private static async void Bot_OnMessage(object sender, MessageEventArgs eventUser)
        //{
        //    Telegram.Bot.Types.Message message = eventUser.Message;

        //    // Создаем пользователя на основе события от сообщения, т.е. мы получаем от базы 
        //    // данных информацию.
        //    UserBot user = await WorkWithBD.GetUserAsync(message.Chat.Id, message.From.FirstName);

        //    // Проверка на пустое сообщение и на то, каким типом является сообщение.
        //    if (message == null)
        //    {
        //        return;
        //    }
        //    else if (message.Type == MessageType.Text)
        //    {
        //        Bot_OnMessage_Text(user, message);
        //    }
        //    else if (message.Type == MessageType.Photo || (message.Type == MessageType.Document &&
        //            message.Document.MimeType.Split('/')[0] == "image"))
        //    {
        //        // Проверяем на то, в нужном ли состоянии залил пользователь фотку.
        //        if ((int)user.State == 1)
        //        {
        //            GetPhoto(message, user);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Реагирует на текстовые сообщения пользователя, обрабатывает все некорректные сообщения.
        ///// </summary>
        ///// <param name="user">С каким пользователем взаимодействуем.</param>
        ///// <param name="message">Сообщение, которое нужно обрабатывать.</param>
        //static private async void Bot_OnMessage_Text(UserBot user, Telegram.Bot.Types.Message message)
        //{
        //    switch (message.Text)
        //    {
        //        case "/start":
        //            // Обновляем состояние пользователя.
        //            user.State = UtilitiesBot.State.S_Start;
        //            // Проверяем на то, что такой пользователь есть ли в базе, при отсутствии -- добавляем.
        //            WorkWithBD.AddUserAsync(user);
        //            // Выводим информацию в log о том, что меняется.
        //            // Бот информирует пользователя о текущем разделе.
        //            messageLast = await bot.SendTextMessageAsync(
        //                      chatId: message.Chat,
        //                      parseMode: ParseMode.Markdown,
        //                      text: UtilitiesBot.infoText["start"],
        //                      replyMarkup: UtilitiesBot.keyboards["start"]
        //                    );
        //            break;
        //        default:
        //            break;
        //    }
        //}

        ///// <summary>
        ///// Получает фотку от пользователя в формате Document/Photo, после чего, применяется алгоритм
        ///// на поиск доминирующих цветов, потом получаем результат в виде готовой отрисованной
        ///// картинки и выводим его пользователю.
        ///// </summary>
        ///// <param name="message">Какое событие мы обрабатываем.</param>
        ///// <param name="user">У какого пользователя мы обрабатываем эти события.</param>
        //private static async void GetPhoto(Telegram.Bot.Types.Message message, UserBot user)
        //{
        //    Telegram.Bot.Types.File file;

        //    // Проверяем, какой тип файла, так как в зависимости от этого меняется алгоритм
        //    // обработки таких изображений.
        //    if (message.Type == MessageType.Document)
        //    {
        //        file = await bot.GetFileAsync(message.Document.FileId);
        //    }
        //    else
        //    {
        //        file = await bot.GetFileAsync(message.Photo[message.Photo.Length - 1].FileId);
        //    }

        //    // Мы "скачиваем" файл с сервера телеграмма, сохраняем в Bitmap, после чего, мы 
        //    // обрабатываем эту картинку и возвращаем результат в виде Bitmap.
        //    Bitmap image = WorkWithImage.GetResult(new Bitmap(
        //        new WebClient().OpenRead(@"https://api.telegram.org/file/bot" + tokenBot + "/"
        //        + file.FilePath)), user);

        //    if (idGroupMedia != message.MediaGroupId)
        //    {
        //        arrayBitmaps.Clear();
        //        idGroupMedia = message.MediaGroupId;
        //    }

        //    if (message.MediaGroupId == null)
        //    {
        //        // Сохраним картинку в поток, для того, чтобы бот смог получить изображение из
        //        // потока.
        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            image.Save(memoryStream, ImageFormat.Png);
        //            memoryStream.Position = 0;
        //            await bot.SendPhotoAsync(
        //                        chatId: message.Chat,
        //                        photo: memoryStream,
        //                        caption: "Ваш результат.");

        //            if (messageLast != null)
        //            {
        //                await bot.DeleteMessageAsync(message.Chat.Id, messageLast.MessageId);
        //            }
        //            messageLast = await bot.SendTextMessageAsync(
        //                                chatId: message.Chat,
        //                                text: UtilitiesBot.infoText["upload"],
        //                                replyMarkup: UtilitiesBot.keyboards["back"]);
        //        }
        //    }
        //    else
        //    {
        //        arrayBitmaps.Add(image);

        //        if (arrayBitmaps.Count != 1)
        //        {
        //            // Создаем поток.
        //            MemoryStream[] memoryStream = new MemoryStream[arrayBitmaps.Count];
        //            // Куда будем сохранять изображения.
        //            IAlbumInputMedia[] inputMediasArray = new IAlbumInputMedia[arrayBitmaps.Count];
        //            // Добавляем изображение при каждой итерации.
        //            for (int i = 0; i < inputMediasArray.Length; i++)
        //            {
        //                memoryStream[i] = new MemoryStream();
        //                arrayBitmaps[i].Save(memoryStream[i], ImageFormat.Png);
        //                memoryStream[i].Position = 0;
        //                inputMediasArray[i] = new InputMediaPhoto(new InputMedia(memoryStream[i],
        //                    $"photo{i}.png"));
        //            }

        //            var mA = await bot.SendMediaGroupAsync(
        //                inputMedia: inputMediasArray,
        //                chatId: message.Chat
        //                );

        //            for (int i = 0; i < memoryStream.Length; i++)
        //            {
        //                memoryStream[i].Flush();
        //                memoryStream[i].Close();
        //            }

        //            messageLast = await bot.SendTextMessageAsync(
        //                                chatId: message.Chat,
        //                                text: UtilitiesBot.infoText["upload"],
        //                                replyMarkup: UtilitiesBot.keyboards["back"]);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Метод обрабатывает все реакции нажатия на кнопки inline от пользователя, после чего, в
        ///// зависимости от реакции, выводится необходимая информация и меняется логика поведения бота.
        ///// </summary>
        ///// <param name="eventUser">Пользователь, которого нужно обработать.</param>
        //private static async void BotClient_OnCallbackQuery(object sender, CallbackQueryEventArgs eventUser)
        //{
        //    // Создаем пользователя на основе сообщения, т.е. мы получаем от базы данных информацию.
        //    UserBot user = await WorkWithBD.GetUserAsync(eventUser.CallbackQuery.From.Id,
        //        eventUser.CallbackQuery.From.FirstName);

        //    // Создаем глобальные переменные, для того, чтобы потом можно было в качестве них
        //    // передать в параметры метода LogFromCallback.
        //    string text;
        //    InlineKeyboardMarkup keyboard;

        //    switch (eventUser.CallbackQuery.Data)
        //    {
        //        // Выбор количества пикселей.
        //        case "1":
        //        case "2":
        //        case "3":
        //        case "4":
        //        case "5":
        //        case "6":
        //        case "7":
        //        case "8":
        //        case "9":
        //        case "10":
        //            // Это нужно для корректного отображения текущего количества пикселей.
        //            saveInfo = eventUser.CallbackQuery.Data;

        //            LogFromCallback(user, UtilitiesBot.State.S_PickNumberPix,
        //                UtilitiesBot.infoText["pickNumberPix"] + $"*{saveInfo}*",
        //                UtilitiesBot.keyboards["pickNumberPix"], eventUser);
        //            break;
        //        // Загрузка фотки.
        //        case "upload":
        //            LogFromCallback(user, UtilitiesBot.State.S_Upload, UtilitiesBot.infoText["upload"],
        //                UtilitiesBot.keyboards["back"], eventUser);
        //            break;
        //        // Настройки бота.
        //        case "settings":
        //            LogFromCallback(user, UtilitiesBot.State.S_Settings, UtilitiesBot.infoText["setting"],
        //                UtilitiesBot.keyboards["setting"], eventUser);
        //            break;
        //        // Выбор количества пикселей.
        //        case "numberOfPixels":
        //            LogFromCallback(user, UtilitiesBot.State.S_NumberOfPixels,
        //                UtilitiesBot.infoText["numberOfPixels"] + user.Settings["amount"],
        //                UtilitiesBot.keyboards["numberOfPixels"], eventUser);
        //            break;
        //        // Выбор режима.
        //        case "mode":
        //            LogFromCallback(user, UtilitiesBot.State.S_Mode,
        //                UtilitiesBot.infoText["mode"] + user.Settings["mode"],
        //                UtilitiesBot.keyboards["mode"], eventUser);
        //            break;
        //        // Выбранный режим.
        //        case "artist":
        //        case "colorblind":
        //            user.Settings["mode"] = eventUser.CallbackQuery.Data;

        //            if ((int)user.State == 6)
        //            {
        //                text = UtilitiesBot.infoText["setting"];
        //                keyboard = UtilitiesBot.keyboards["setting"];
        //            }
        //            else
        //            {
        //                text = UtilitiesBot.infoText["info"];
        //                keyboard = UtilitiesBot.keyboards["info"];
        //            }

        //            LogFromCallback(user, UtilitiesBot.State.S_Info, text, keyboard, eventUser);
        //            break;
        //        case "back":
        //            UtilitiesBot.State state;
        //            // В зависимости от нахождения пользователя в меню, будет меняться структура
        //            // вывода информации после нажатия на back.
        //            if ((int)user.State == 3 || (int)user.State == 4 || (int)user.State == 6)
        //            {
        //                text = UtilitiesBot.infoText["setting"];
        //                keyboard = UtilitiesBot.keyboards["setting"];
        //                state = UtilitiesBot.State.S_Settings;

        //                // Если пользователь согласился с тем, что выбрал правильное количество
        //                // пикселей, то это сохранится.
        //                if ((int)user.State == 4)
        //                {
        //                    user.Settings["amount"] = saveInfo;
        //                }
        //            }
        //            else
        //            {
        //                text = UtilitiesBot.infoText["info"];
        //                keyboard = UtilitiesBot.keyboards["info"];
        //                state = UtilitiesBot.State.S_Info;
        //            }

        //            LogFromCallback(user, state, text, keyboard, eventUser);
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}