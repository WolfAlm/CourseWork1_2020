using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace CourseWork_Library
{
    public class UtilitiesBot
    {
        /// <summary>
        /// Словарь бота. Выбирается текст относительно действий пользователя.
        /// </summary>
        static public readonly Dictionary<string, string> infoText = new Dictionary<string, string>()
        {
            ["info"] = "Вы будете взаимодействовать со мной в пределах одного сообщения, " +
            "Вы можете отсюда перейти к загрузке фотографии, также настраивать меня под ваши " +
            "удобства. Дерзайте. *smirk*",
            ["error"] = "Простите, я не такой умный, как ты думаешь, не понимаю я это " +
            "ваше сообщение... Следуйте, пожалуйста, инструкциям следующего сообщения: ",
            ["upload"] = "Отправьте мне Ваше фото с помощью загрузки изображения, в " +
            "котором вы хотите узнать, какой цвет самый частый и преобладает в целом.",
            ["setting"] = "Вы можете настроить МОИ параметры(ну или вспороть мои организмы, переделать их, " +
            "а потом всунуть обратно, но не суть).\n\n1. *Количество цвета.* Здесь вы отправляете " +
            $"целочисленное число от 1 до 10, для того, чтобы задать количество нужных вам пикселей.\n" +
            "2. *Выбор режима*. Вы можете выбрать в зависимости от ваших целей режим. При *художнике* " +
            "выводится палитра с полным названием RGB, а при *дальтоник* выводится палитр с названиями" +
            "цвета.",
            ["numberOfPixels"] = "Выберите количество пикселей. Ваше текущее количество: ",
            ["mode"] = "Выберите Ваш режим. Ваш текущий режим: ",
            ["pickNumberPix"] = "*Вы выбрали такое количество: *",
            ["start"] = "*Привет!* Я бот, который был написан на коленях(ой).\nДля убедительности, " +
            "Вы можете даже посмотреть на *аватарОчку*. Так вот. \n\nВсе что я умею - реагировать" +
            "на определенные команды и кнопки, которые я буду выводить Вам. А весь мой " +
            "функционал заключается в том, чтобы обрабатывать ваши фотки на основные " +
            "цвета, Вы можете отдельно настраивать какие-то пункты. Ой, этот текст все равно " +
            "перепишется, так что забудьте о том, что читали :D\n\nВажный вопрос в другом." +
            "Для того, чтобы определить режим бота, нам нужно понять, кто вы: дальтоник или художник?",
        };

        /// <summary>
        /// Здесь хранится словарь всех клавиатур, которые будут в зависимости от ситуации выводить
        /// пользователю нужные кнопки.
        /// </summary>
        static public readonly Dictionary<string, InlineKeyboardMarkup> keyboards = new Dictionary<string, InlineKeyboardMarkup>()
        {
            ["info"] = new InlineKeyboardMarkup(new[]
                    {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("Загрузить фото", "upload"),
                                InlineKeyboardButton.WithCallbackData("Настройки", "settings")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithUrl("Контакты", 
                                    "https://t.me/wolfalm"),
                            }
                    }),
            ["setting"] = new InlineKeyboardMarkup(new[]
                    {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("Количество цвета",
                                    "numberOfPixels")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("Выбор режима", "mode")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "back")
                            }
                    }),
            ["numberOfPixels"] = new InlineKeyboardMarkup(new[]
                    {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("1"),
                                InlineKeyboardButton.WithCallbackData("2"),
                                InlineKeyboardButton.WithCallbackData("3")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("4"),
                                InlineKeyboardButton.WithCallbackData("5"),
                                InlineKeyboardButton.WithCallbackData("6")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("7"),
                                InlineKeyboardButton.WithCallbackData("8"),
                                InlineKeyboardButton.WithCallbackData("9")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("10"),
                                InlineKeyboardButton.WithCallbackData("Назад", "back")
                            }
                    }),
            ["pickNumberPix"] = new InlineKeyboardMarkup(new[]
                    {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("Другое число", "numberOfPixels"),
                                InlineKeyboardButton.WithCallbackData("Подтвердить", "back")
                            }
                    }),
            ["back"] = new InlineKeyboardMarkup(new[]
                    {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "back")
                            }
                    }),
            ["start"] = new InlineKeyboardMarkup(new[]
                    {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("Дальтоник", "colorblind"),
                                InlineKeyboardButton.WithCallbackData("Художник", "artist")
                            }
                    }),
            ["mode"] = new InlineKeyboardMarkup(new[]
                    {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("Дальтоник", "colorblind"),
                                InlineKeyboardButton.WithCallbackData("Художник", "artist")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", "back")
                            }
                    }),
        };

        /// <summary>
        /// Приведены все состояния пользователя, т.е. здесь описано каждое взаимодействие с кнопкой
        /// и текущий этап.
        /// </summary>
        public enum State
        {
            S_Info,
            S_Upload,
            S_Settings,
            S_NumberOfPixels,
            S_PickNumberPix,
            S_Start,
            S_Mode,
        }
    }
}
