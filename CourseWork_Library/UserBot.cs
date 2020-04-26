using System.Collections.Generic;
using MongoDB.Bson;

namespace OftenColorBotLibrary
{
    public class UserBot
    {
        /// <summary>
        /// Индивидуальный ID для корректного отображения документа на сервере.
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Индивидуальный ID пользователя, по которому бот отслеживает все переписки и коммунцирует
        /// с ними.
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// В каком этапе меню находится пользователь. 
        /// </summary>
        public UtilitiesBot.State State { get; set; }

        /// <summary>
        /// Имя пользователя, т.е. первое имя, без фамилии.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Сначала идет режим игрока, а потом количество цвета.
        /// </summary>
        public Dictionary<string, object> Settings { get; set; } =
            new Dictionary<string, object>() 
            { 
                ["mode"] = null, 
                ["amount"] = "5" 
            };
    }
}
