using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CourseWork_Library
{
    public class WorkWithBD
    {
        // Подключение к серверу для работы с базой данных. 
        static private MongoClient client = new MongoClient("mongodb+srv://sharp:ASDFG12wert@clusterbot-kc2bm." +
            "mongodb.net/test?retryWrites=true&w=majority");
        // Получаем базу по "users". 
        static private IMongoDatabase database = client.GetDatabase("users");

        /// <summary>
        /// При первом взаимодействии с ботом, мы добавляем пользователя в базу. 
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <param name="state">Текущее положение пользователя.</param>
        static public async void AddUserAsync(UserBot user)
        {
            // Получаем все документы из этой базы в формате BsonDocument.
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("user");
            // Устанавливаем фильтр для поиска и проверяем, есть ли такой документ в базе.
            if (await collection.Find(new BsonDocument("ChatId", user.ChatId)).AnyAsync())
            {
                await collection.UpdateOneAsync(new BsonDocument("ChatId", user.ChatId),
                      new BsonDocument("$set", new BsonDocument("State", user.State)));
            }
            else
            {
                await collection.InsertOneAsync(user.ToBsonDocument());
            }
        }

        /// <summary>
        /// Подается на вход идентификатор пользователя, по которому будет производиться поиск в
        /// базе данных, чтобы на ее основе создать пользователя и оперировать им.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Возвращает созданного пользователя.</returns>
        static public async Task<UserBot> GetUserAsync(long id, string name = null)
        {
            // Получаем все документы из этой базы в формате UserBot.
            IMongoCollection<UserBot> collection = database.GetCollection<UserBot>("user");

            // Если пользователя не оказалось в базе каким-то образом, то включаем страховку, а именно:
            // добавляем в базу.
            if (!await collection.Find(new BsonDocument("ChatId", id)).AnyAsync())
            {
                AddUserAsync(new UserBot() { Name = name, ChatId = id });
            }

            // Мы находим конкретного человека по заданному фильтру в качестве id и сохраняем его
            // в список. И возвращаем полученный результат.
            List<UserBot> result = await collection.Find(new BsonDocument("ChatId", id)).ToListAsync();
            return result[0];
        }

        /// <summary>
        /// Вносим изменения перемещения пользователя в базу данных.
        /// </summary>
        /// <param name="user">Чьи изменения нужно будет вносить.</param>
        static public async void UpdateUserAsync(UserBot user)
        {
            // Получаем все документы из этой базы в формате BsonDocument.
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("user");
            // Обновляем данные относительно этого пользователя.
            await collection.UpdateOneAsync(new BsonDocument("ChatId", user.ChatId),
                new BsonDocument("$set", new BsonDocument("State", user.State)));

            await collection.UpdateOneAsync(new BsonDocument("ChatId", user.ChatId),
                new BsonDocument("$set", new BsonDocument("Settings", new BsonDocument 
                { 
                    {"mode", user.Settings["mode"].ToString() },
                    {"amount", user.Settings["amount"].ToString() }
                })));
        }
    }
}
