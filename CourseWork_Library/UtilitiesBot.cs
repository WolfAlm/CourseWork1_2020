using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace OftenColorBotLibrary
{
    public class UtilitiesBot
    {
        /// <summary>
        /// Словарь бота. Выбирается текст относительно действий пользователя.
        /// </summary>
        static public readonly Dictionary<string, string> infoText = new Dictionary<string, string>()
        {
            ["info"] = "Вам придется взаимодействовать со мной в пределах лишь одного сообщения. " +
            "Печалька. Отсюда Вы можете перейти к загрузке фотографии. " +
            "Ах да, еще можете настроить меня так, как вам будет комфортнее, для полной информации" +
            " Вы также можете перейти к настройкам. Дерзайте, котики.",
            ["error"] = "Простите, я не такой умный, как ты думаешь, не понимаю текстовые сообщения " +
            "и отправленные изображения в неправильных местах. Следуйте, пожалуйста, инструкциям" +
            " следующего сообщения: ",
            ["upload"] = "Отправьте мне Ваше фото с помощью загрузки изображения, в " +
            "котором вы хотите узнать, какие цвета преобладают в изображении.",
            ["setting"] = "*НАСТРОЙ МЕНЯ, НАСТРОЙ МЕНЯ ПОЛНОСТЬЮ!!!*\n\n Извиняюсь. Немного отвлекся." +
            " В общем, Вы можете настроить мои параметры, переделав их под себя. Я, конечно, не так хорош," +
            " как сатисфаер, но тоже кое — что могу!\n\n1. *Количество цвета.* Здесь вы отправляете " +
            "целое число от 1 до 10 для того, чтобы задать количество нужных вам пикселей.\n\n" +
            "2. *Выбор режима.* Вы можете выбрать режим в зависимости от ваших целей. " +
            "Выбрав режим *профи*, вам выведется палитра цветов с HEX кодами, при режиме " +
            "*любителя* — обычная палитра, и наконец, режим *дальтоник* выводит палитру с названиями" +
            "цветов.\n\n3. *Выбор расположения палитры.* Здесь Вы можете выбирать, в каком виде " +
            "должна выводиться палитра. Она может идти вместе с вашим фото с выбранным расположением, " +
            "как и идти без изображения, только палитра.",
            ["numberOfPixels"] = "Выберите количество пикселей. Ваше текущее количество: ",
            ["mode"] = "Выберите Ваш режим. *Ваш текущий режим:* ",
            ["pickNumberPix"] = "*Вы выбрали такое количество: *",
            ["start"] = "*Привет!* Я бот, который был написан на коленках, потому что моя создательница ленивая жопа." +
            " Для убедительности, Вы можете даже посмотреть на аватарОчку.\n\nВ общем, все, что я умею" +
            " — это реагировать на команды, отраженные на кнопках под текстом. Тыкать в них нужно," +
            " но только аккуратно, пожалуйста, я ранимый. \n\nСобственно, весь мой функционал " +
            "заключается в том, чтобы обрабатывать ваши фотокарточки и определять на них " +
            "превалирующие цвета. Но прямо сейчас больше волнует меня другое.\n\n" +
            "Перед нами стоит важный вопрос! Чтобы определить режим, в котором я буду с вами работать, " +
            " нам нужно понять, кто вы: профи или любитель? *Профи* — палитра цветов с HEX кодами, " +
            "*любитель* — обычная палитра цветов.",
            ["modePalette"] = "Выберите Ваше предпочтение относительно расположения палитры. Сбоку " +
            "к изображению *слева* или *справа*? Или, может, *сверху* над изображением, как и *снизу*? " +
            "Также как вариант — оригинальная палитра *без изображения*. *Ваш текущий режим:* "
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
                                InlineKeyboardButton.WithCallbackData("📥 Загрузить фото", "upload"),
                                InlineKeyboardButton.WithCallbackData("⚙ Настройки", "settings")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithUrl("📲 Связаться с разработчиком", "https://t.me/wolfalm"),
                            }
                    }),
            ["setting"] = new InlineKeyboardMarkup(new[]
                    {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("🔢 Количество цвета",
                                    "numberOfPixels")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("🛠 Выбор режима", "mode")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("🌅 Выбор вида палитры", 
                                    "modePalette")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("🔙 Назад", "back"),
                                InlineKeyboardButton.WithCallbackData("📥 Перейти к загрузке фото", 
                                    "upload"),
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
                                InlineKeyboardButton.WithCallbackData("🔙 Назад", "back")
                            }
                    }),
            ["pickNumberPix"] = new InlineKeyboardMarkup(new[]
                    {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("✖️ Другое число", "numberOfPixels"),
                                InlineKeyboardButton.WithCallbackData("✔️ Подтвердить", "back")
                            }
                    }),
            ["upload"] = new InlineKeyboardMarkup(new[]
                    {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("🔙 Назад", "back"),
                                InlineKeyboardButton.WithCallbackData("⚙ Настройки", "settings")
                            }
                    }),
            ["start"] = new InlineKeyboardMarkup(new[]
                    {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("👩🏼‍🎨 Профи", "профи"),
                                InlineKeyboardButton.WithCallbackData("👨🏼‍💼 Любитель", "любитель")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("🦹🏼‍♀️ Дальтоник", "дальтоник")
                            }
                    }),
            ["mode"] = new InlineKeyboardMarkup(new[]
                    {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("👩🏼‍🎨 Профи", "профи"),
                                InlineKeyboardButton.WithCallbackData("👨🏼‍💼 Любитель", "любитель")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("🦹🏼‍♀️ Дальтоник", "дальтоник"),
                                InlineKeyboardButton.WithCallbackData("🔙 Назад", "back")
                            }
                    }),
            ["modePalette"] = new InlineKeyboardMarkup(new[]
                    {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("⏹ Без изображения", "без изображения")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("⬅ Слева", "слева"),
                                InlineKeyboardButton.WithCallbackData("➡ Справа", "справа")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("⬆ Сверху", "сверху"),
                                InlineKeyboardButton.WithCallbackData("⬇ Cнизу", "cнизу")
                            },
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("🔙 Назад", "back")
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
            S_ModePalette,
        }
    }
}
