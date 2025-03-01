﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using OftenColorBotLibrary;
using System.IO;
using System.Net;
using System.Drawing.Imaging;
using Message = Telegram.Bot.Types.Message;

namespace CourseWorkForm
{
    public partial class StartWork : Form
    {
        // Определяем текущее время для запуска секундомера.
        DateTime date = DateTime.Now;
        // Токен бота.
        private const string tokenBot = "955523636:AAF3THwqIPSRat5q7TZUBow_B8QEvm8zGW8";
        // Создаем бота с нужным токеном бота.
        private static readonly TelegramBotClient bot = new TelegramBotClient(tokenBot);

        // Костыль.
        string saveInfo = string.Empty;

        //List<Bitmap> arrayBitmaps = new List<Bitmap>();
        //string idGroupMedia = string.Empty;
        //int step = 0;
        //Message[] mediasLast;

        public StartWork()
        {
            InitializeComponent();

            // Запускаем таймер текущей работы.
            stopwatch.Tick += new EventHandler(TickTimer);
            stopwatch.Start();

            // Подписка на сообщения от пользователя.
            bot.OnMessage += Bot_OnMessage;
            // Подписка на inlin'ы от пользователя.
            bot.OnCallbackQuery += BotClient_OnCallbackQuery;
            // Запускаем бота.
            bot.StartReceiving();

            try
            {
                var res = bot.GetMeAsync().Result;
                startedBot.Text = "БОТ УСПЕШНО ЗАПУЩЕН!";
            }
            catch (AggregateException)
            {
                startedBot.Text = "БОТ, В СВЯЗИ С ОШИБКАМИ, НЕ БЫЛ ЗАПУЩЕН!";
            }
        }

        /// <summary>
        /// Метод обрабатывает все реакции нажатия на кнопки inline от пользователя, после чего, в
        /// зависимости от реакции, выводится необходимая информация и меняется логика поведения бота.
        /// </summary>
        /// <param name="eventUser">Пользователь, которого нужно обработать.</param>
        private async void BotClient_OnCallbackQuery(object sender, CallbackQueryEventArgs eventUser)
        {
            // Создаем пользователя на основе сообщения, т.е. мы получаем от базы данных информацию.
            UserBot user = await WorkWithBD.GetUserAsync(eventUser.CallbackQuery.From.Id,
                eventUser.CallbackQuery.From.FirstName);

            // Создаем глобальные переменные, для того, чтобы потом можно было в качестве них
            // передать в параметры метода LogFromCallback.
            string text;
            InlineKeyboardMarkup keyboard;
            UtilitiesBot.State state;

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
                    // Это нужно для корректного отображения текущего количества пикселей.
                    saveInfo = eventUser.CallbackQuery.Data;

                    LogFromCallback(user, UtilitiesBot.State.S_PickNumberPix,
                        UtilitiesBot.infoText["pickNumberPix"] + $"*{saveInfo}*",
                        UtilitiesBot.keyboards["pickNumberPix"], eventUser);
                    break;
                // Загрузка фотки.
                case "upload":
                    LogFromCallback(user, UtilitiesBot.State.S_Upload, UtilitiesBot.infoText["upload"],
                        UtilitiesBot.keyboards["upload"], eventUser);
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
                        UtilitiesBot.infoText["mode"] + $"*{user.Settings["mode"]}*",
                        UtilitiesBot.keyboards["mode"], eventUser);
                    break;
                // Выбранный режим.
                case "профи":
                case "любитель":
                case "новичок":
                    user.Settings["mode"] = eventUser.CallbackQuery.Data;

                    if ((int)user.State == 6)
                    {
                        text = UtilitiesBot.infoText["setting"];
                        keyboard = UtilitiesBot.keyboards["setting"];
                        state = UtilitiesBot.State.S_Settings;
                    }
                    else
                    {
                        text = UtilitiesBot.infoText["info"];
                        keyboard = UtilitiesBot.keyboards["info"];
                        state = UtilitiesBot.State.S_Info;
                    }

                    LogFromCallback(user, state, text, keyboard, eventUser);
                    break;
                case "modePalette":
                    LogFromCallback(user, UtilitiesBot.State.S_ModePalette,
                            UtilitiesBot.infoText["modePalette"] + $"*{user.Settings["modePalette"]}*",
                            UtilitiesBot.keyboards["modePalette"], eventUser);
                    break;
                case "слева":
                case "справа":
                case "cнизу":
                case "сверху":
                case "без изображения":
                    user.Settings["modePalette"] = eventUser.CallbackQuery.Data;
                    LogFromCallback(user, UtilitiesBot.State.S_Settings,
                        UtilitiesBot.infoText["setting"], UtilitiesBot.keyboards["setting"], eventUser);
                    break;
                case "back":
                    // В зависимости от нахождения пользователя в меню, будет меняться структура
                    // вывода информации после нажатия на back.
                    if ((int)user.State == 3 || (int)user.State == 4 || (int)user.State == 6
                        || (int)user.State == 7)
                    {
                        text = UtilitiesBot.infoText["setting"];
                        keyboard = UtilitiesBot.keyboards["setting"];
                        state = UtilitiesBot.State.S_Settings;

                        // Если пользователь согласился с тем, что выбрал правильное количество
                        // пикселей, то это сохранится.
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
        }

        /// <summary>
        /// Будет выводиться информация о передвижениях пользователя по меню, также обновляются
        /// его данные в базе данных. Обрабатывается следующий шаг бота.
        /// </summary>
        /// <param name="user">Пользователь, чьи передвижения вносим в базу данных.</param>
        /// <param name="state">На какой кнопке/этапе меню находится пользователь.</param>
        /// <param name="text">Какой текст сообщения выводится.</param>
        /// <param name="keyboard">Какая клавиатура передается.</param>
        /// <param name="eventUser">Какого пользователя будем обрабатывать.</param>
        private async void LogFromCallback(UserBot user, UtilitiesBot.State state, string text,
            InlineKeyboardMarkup keyboard, CallbackQueryEventArgs eventUser)
        {
            try
            {
                Message messageL = await bot.EditMessageTextAsync(
                        chatId: eventUser.CallbackQuery.From.Id,
                        messageId: eventUser.CallbackQuery.Message.MessageId,
                        parseMode: ParseMode.Markdown,
                        text: text,
                        replyMarkup: keyboard
                    );

                user.MessageId = messageL.MessageId;
                user.State = state;
                // Выводим информацию в log о том, что изменилось состояние пользователя..
                InfoInLog($"{DateTime.Now} For {user.Name} the state has been updated: {user.State}\n");
                // Вносим изменения в базу данных.
                WorkWithBD.UpdateUserAsync(user);
            }
            // Если кнопку будут нажимать несколько раз, то мы отлавливаем такие ошибки и просто не
            // даем боту падать.
            catch (Telegram.Bot.Exceptions.MessageIsNotModifiedException error)
            {
                InfoInLog(DateTime.Now + " " + error.Message + Environment.NewLine);
            }
        }

        /// <summary>
        /// Выводит информацию о событиях и изменениях в лог винформы.
        /// </summary>
        /// <param name="info">Информация, которую нужно выводить.</param>
        private void InfoInLog(string info)
        {
            logBot.BeginInvoke((MethodInvoker)(() => logBot.AppendText(info)));
        }

        /// <summary>
        /// Получает от пользователя сообщения вроде изображений, набора текста и так далее, потом
        /// в зависимости от сообщения, применяет разные подходы.
        /// </summary>
        /// <param name="eventUser">Какое сообщение нужно обрабатывать.</param>
        private async void Bot_OnMessage(object sender, MessageEventArgs eventUser)
        {
            Message message = eventUser.Message;

            // Создаем пользователя на основе события от сообщения, т.е. мы получаем от базы 
            // данных информацию.
            UserBot user = await WorkWithBD.GetUserAsync(message.Chat.Id, message.From.FirstName);

            // Проверка на пустое сообщение и на то, каким типом является сообщение.
            if (message == null)
            {
                return;
            }
            else if (message.Type == MessageType.Text)
            {
                // Выводит информацию о действиях пользователя:
                InfoInLog($"{DateTime.Now} {user.Name} sent a message to " +
                    $"with such text:\n{message.Text}\n");
                Bot_OnMessage_Text(user, message);
            }
            else if (message.Type == MessageType.Photo || (message.Type == MessageType.Document &&
                    message.Document.MimeType.Split('/')[0] == "image"))
            {
                // Выводит информацию о действиях пользователя:
                InfoInLog($"{DateTime.Now} {user.Name} sent a photo/document\n");
                // Проверяем на то, в нужном ли состоянии залил пользователь фотку.
                if ((int)user.State == 1)
                {
                    GetPhoto(message, user);
                }
                else
                {
                    BotErrorMessage(message, user);
                }
            }
            else
            {
                BotErrorMessage(message, user);
            }
        }

        /// <summary>
        /// Какое сообщение нужно вернуть для переотправки сообщения в случае удаления.
        /// Для этого мы смотрим на положение относительно меню пользователя и получаем всю
        /// необходимую информацию.
        /// </summary>
        /// <param name="user">У какго пользвователя смотрим переписку.</param>
        /// <param name="inlineKeyboard">Какие кнопки были.</param>
        /// <param name="text">Какое текстовое содержание было.</param>
        private void ReturnMessage(UserBot user, out InlineKeyboardMarkup inlineKeyboard,
            out string text)
        {
            switch ((int)user.State)
            {
                case 0:
                    inlineKeyboard = UtilitiesBot.keyboards["info"];
                    text = UtilitiesBot.infoText["info"];
                    break;
                case 1:
                    inlineKeyboard = UtilitiesBot.keyboards["upload"];
                    text = UtilitiesBot.infoText["upload"];
                    break;
                case 2:
                    inlineKeyboard = UtilitiesBot.keyboards["setting"];
                    text = UtilitiesBot.infoText["setting"];
                    break;
                case 3:
                    inlineKeyboard = UtilitiesBot.keyboards["numberOfPixels"];
                    text = UtilitiesBot.infoText["numberOfPixels"];
                    break;
                case 4:
                    inlineKeyboard = UtilitiesBot.keyboards["pickNumberPix"];
                    text = UtilitiesBot.infoText["pickNumberPix"];
                    break;
                case 5:
                    inlineKeyboard = UtilitiesBot.keyboards["start"];
                    text = UtilitiesBot.infoText["start"];
                    break;
                case 6:
                    inlineKeyboard = UtilitiesBot.keyboards["mode"];
                    text = UtilitiesBot.infoText["mode"];
                    break;
                case 7:
                    inlineKeyboard = UtilitiesBot.keyboards["modePalette"];
                    text = UtilitiesBot.infoText["modePalette"];
                    break;
                default:
                    // Это просто случайное, чтобы не ругалось, так как сюда невозможно зайти.
                    inlineKeyboard = UtilitiesBot.keyboards["start"];
                    text = "";
                    break;
            }
        }

        /// <summary>
        /// Выводятся сообщения о ошибках работы.
        /// </summary>
        /// <param name="message">Какое событие мы обрабатываем.</param>
        private async void BotErrorMessage(Message message, UserBot user)
        {
            try
            {
                // Этот раздел нужен для того, чтобы сказать, что команда неверна и будет
                // выведена соотвествующая информация.
                await bot.SendTextMessageAsync(
                          chatId: message.Chat,
                          text: UtilitiesBot.infoText["error"]
                        );

                // Мы удаляем сообщение, где было связанное с меню во избежание неудобства и 
                // некорректности.
                await bot.DeleteMessageAsync(message.Chat.Id, user.MessageId);
                // Получаем удаленное сообщение.
                ReturnMessage(user, out InlineKeyboardMarkup inlineKeyboard, out string text);

                Message messageL = await bot.SendTextMessageAsync(
                                    chatId: message.Chat,
                                    parseMode: ParseMode.Markdown,
                                    text: text,
                                    replyMarkup: inlineKeyboard
                                );

                user.MessageId = messageL.MessageId;
                WorkWithBD.UpdateUserAsync(user);
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException error)
            {
                InfoInLog(DateTime.Now + " " + error.Message + Environment.NewLine);

                await bot.SendTextMessageAsync(
                          chatId: message.Chat,
                          text: "*Мне стало плохо... Даже железяки могут подводить. Напишите, " +
                          "пожалуйста, /start, для того, чтобы я заработал корректно на 100%.*",
                          parseMode: ParseMode.Markdown
                        );
            }
        }

        /// <summary>
        /// Получает фотку от пользователя в формате Document/Photo, после чего, применяется алгоритм
        /// на поиск доминирующих цветов, потом получаем результат в виде готовой отрисованной
        /// картинки и выводим его пользователю.
        /// </summary>
        /// <param name="message">Какое событие мы обрабатываем.</param>
        /// <param name="user">У какого пользователя мы обрабатываем эти события.</param>
        private async void GetPhoto(Message message, UserBot user)
        {
            try
            {
                Message temporaryM = await bot.SendTextMessageAsync(chatId: message.Chat,
                                        text: "Ожидайте, пожалуйста, результат...");

                Telegram.Bot.Types.File file;

                // Проверяем, какой тип файла, так как в зависимости от этого меняется алгоритм
                // обработки таких изображений.
                if (message.Type == MessageType.Document)
                {
                    file = await bot.GetFileAsync(message.Document.FileId);
                }
                else
                {
                    file = await bot.GetFileAsync(message.Photo[message.Photo.Length - 1].FileId);
                }

                // Мы "скачиваем" файл с сервера телеграмма, сохраняем в Bitmap, после чего, мы 
                // обрабатываем эту картинку и возвращаем результат в виде Bitmap.
                Bitmap image = WorkWithImage.GetResult(new Bitmap(
                    new WebClient().OpenRead(@"https://api.telegram.org/file/bot" + tokenBot + "/"
                    + file.FilePath)), user);

                //if (idGroupMedia != message.MediaGroupId)
                //{
                //    arrayBitmaps.Clear();
                //    idGroupMedia = message.MediaGroupId;
                //}

                //if (message.MediaGroupId == null)
                //{
                // Сохраним картинку в поток, для того, чтобы бот смог получить изображение из
                // потока.
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await bot.DeleteMessageAsync(message.Chat.Id, temporaryM.MessageId);

                    image.Save(memoryStream, ImageFormat.Png);
                    memoryStream.Position = 0;
                    try
                    {
                        await bot.SendPhotoAsync(
                                    chatId: message.Chat,
                                    photo: memoryStream,
                                    caption: "Ваш результат.");
                    }
                    catch (Telegram.Bot.Exceptions.ApiRequestException error)
                    {
                        await bot.SendTextMessageAsync(
                                        chatId: message.Chat,
                                        text: "Произошла непредвиденная ошибка, возможно, Ваш" +
                                        " файл был огромным и я не смог Вам его отправить :(");

                        InfoInLog($"{DateTime.Now} {error.Message}\n");
                    }

                    try
                    {
                        await bot.DeleteMessageAsync(message.Chat.Id, user.MessageId);
                    }
                    catch (Telegram.Bot.Exceptions.ApiRequestException error)
                    {
                        InfoInLog($"{DateTime.Now} {error.Message}\n");
                    }

                    Message messageL = await bot.SendTextMessageAsync(
                                        chatId: message.Chat,
                                        text: UtilitiesBot.infoText["upload"],
                                        replyMarkup: UtilitiesBot.keyboards["upload"]);

                    user.MessageId = messageL.MessageId;
                    WorkWithBD.UpdateUserAsync(user);
                }
                //}
                //else
                //{
                //arrayBitmaps.Add(image);

                //if (arrayBitmaps.Count != 1)
                //{
                //    // Создаем поток.
                //    MemoryStream[] memoryStream = new MemoryStream[arrayBitmaps.Count];
                //    // Куда будем сохранять изображения.
                //    IAlbumInputMedia[] inputMediasArray = new IAlbumInputMedia[arrayBitmaps.Count];
                //    // Добавляем изображение при каждой итерации.
                //    for (int i = 0; i < inputMediasArray.Length; i++)
                //    {
                //        memoryStream[i] = new MemoryStream();
                //        arrayBitmaps[i].Save(memoryStream[i], ImageFormat.Png);
                //        memoryStream[i].Position = 0;
                //        inputMediasArray[i] = new InputMediaPhoto(new InputMedia(memoryStream[i],
                //            $"photo{step}.png"));
                //        step++;
                //    }

                //    //if (mediasLast != null)
                //    //{
                //    //    await bot.DeleteMessageAsync(message.Chat.Id, mediasLast[0].MessageId);
                //    //}

                //    mediasLast = await bot.SendMediaGroupAsync(
                //        inputMedia: inputMediasArray,
                //        chatId: message.Chat
                //        );

                //    for (int i = 0; i < memoryStream.Length; i++)
                //    {
                //        memoryStream[i].Close();
                //    }

                //    if (messageLast != null)
                //    {
                //        await bot.DeleteMessageAsync(message.Chat.Id, messageLast.MessageId);
                //    }

                //    messageLast = await bot.SendTextMessageAsync(
                //                        chatId: message.Chat,
                //                        text: UtilitiesBot.infoText["upload"],
                //                        replyMarkup: UtilitiesBot.keyboards["back"]);
                //}
                //}
            }
            catch (IOException)
            {
                InfoInLog($"{DateTime.Now} Ошибка ввода-вывода.\n");
            }
            catch (Exception error)
            {
                InfoInLog($"{DateTime.Now} Произошел конец света, упс." +
                    $"\n{error.Message}");
            }
        }

        /// <summary>
        /// Реагирует на текстовые сообщения пользователя, обрабатывает все некорректные сообщения.
        /// </summary>
        /// <param name="user">С каким пользователем взаимодействуем.</param>
        /// <param name="message">Сообщение, которое нужно обрабатывать.</param>
        private async void Bot_OnMessage_Text(UserBot user, Message message)
        {
            switch (message.Text)
            {
                case "/start":
                    // Обновляем состояние пользователя.
                    user.State = UtilitiesBot.State.S_Start;
                    // Проверяем на то, что такой пользователь есть ли в базе, при отсутствии -- добавляем.
                    WorkWithBD.AddUserAsync(user);
                    // Выводим информацию в log о том, что меняется.
                    InfoInLog($"{DateTime.Now} For {user.Name} the state " +
                            $"has been updated: {user.State}\n");
                    // Бот информирует пользователя о текущем разделе.
                    Message messageL = await bot.SendTextMessageAsync(
                              chatId: message.Chat,
                              parseMode: ParseMode.Markdown,
                              text: UtilitiesBot.infoText["start"],
                              replyMarkup: UtilitiesBot.keyboards["start"]
                            );
                    user.MessageId = messageL.MessageId;
                    WorkWithBD.UpdateUserAsync(user);
                    break;
                default:
                    BotErrorMessage(message, user);
                    break;
            }
        }

        /// <summary>
        /// Выводит информацию в качестве секундомера о текущей работе.
        /// </summary>
        private void TickTimer(object sender, EventArgs e)
        {
            // Форматируем время под нужный нам формат и обновляем информацию.
            labelStopwatch.Text = "Время работы: " + new DateTime().
                AddTicks(DateTime.Now.Ticks - date.Ticks).ToString("HH:mm:ss:ff");
        }

        /// <summary>
        /// При закрытии формы, бот прекращает работу, форма окончательно закрывается.
        /// </summary>
        private void BotForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bot.StopReceiving();
            Application.Exit();
        }
    }
}