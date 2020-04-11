using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using System.Threading;
using Telegram.Bot.Args;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

// Добавляется новый элемент:
//BsonElement bel = new BsonElement("name", "Bill");
//BsonDocument doc = new BsonDocument();
//doc.Add(bel);
namespace CourseWork
{
    class Person
    {
        //[BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public Company Company { get; set; }
        public List<string> Languages { get; set; }
    }
    class Company
    {
        public string Name { get; set; }
    }
    class Program
    {
        // Токен бота.
        private static readonly TelegramBotClient botClient = 
            new TelegramBotClient("955523636:AAF3THwqIPSRat5q7TZUBow_B8QEvm8zGW8");

        static void Main(string[] args)
        {
            Person[] pe = new Person[2] { new Person { Name = "Bill", Surname = "Gates", Age = 48 },
                new Person { Name = "Lol", Surname = "Ololo", Age = 48 } };
            //BsonDocument doc = pe[0].ToBsonDocument();
            //Console.ReadLine();


            //MongoClient client = new MongoClient("mongodb+srv://sharp:ASDFG12wert@clusterbot-kc2bm.mongodb.net/test?retryWrites=true&w=majority");
            //SaveDocs(client);
            //FindDocs(client);
            //UpdatePerson(client);
            //Console.ReadLine();
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Console.ReadLine();
            botClient.StopReceiving();
        }

        private static async Task UpdatePerson(MongoClient client)
        {
            var database = client.GetDatabase("house");
            var collection = database.GetCollection<BsonDocument>("people");
            var result = new BsonDocument("Name", "Nataliya");
            bool exists = await collection.Find(result).AnyAsync();
            Console.WriteLine(exists);
            //Console.WriteLine("Найдено по соответствию: {0}; обновлено: {1}",
            //    result.MatchedCount, result.ModifiedCount);
            var people = await collection.Find(new BsonDocument()).ToListAsync();
            foreach (var p in people)
                Console.WriteLine(p);
        }

        //private static async Task FindDocs(MongoClient client)
        //{
        //    var database = client.GetDatabase("house");
        //    var collection = database.GetCollection<BsonDocument>("people");
        //    var filter = new BsonDocument("Name", "Nataliy");

        //    /* Альтернативное решение. Плохо на память влияет(?)
        //    var people = await collection.Find(filter).ToListAsync();
        //    foreach (var doc in people)
        //    {
        //        Console.WriteLine(doc);
        //    }*/

        //    using (var cursor = await collection.FindAsync(filter))
        //    {
        //        while (await cursor.MoveNextAsync())
        //        {
        //            var people = cursor.Current;
        //            foreach (var doc in people)
        //            {
        //                Console.WriteLine(doc);
        //            }
        //        }
        //    }
        //}

        //private static async Task SaveDocs(MongoClient client)
        //{

        //    var database = client.GetDatabase("house");
        //    var collection = database.GetCollection<BsonDocument>("people");

        //    BsonDocument person1 = new BsonDocument
        //        {
        //            {"Name", "Bill"},
        //            {"Age", 32},
        //            {"Languages", new BsonArray{"english", "german"}}
        //        };
        //    BsonDocument person2 = new BsonDocument
        //        {
        //            {"Name", "Nataliya"},
        //            {"Age", 19},
        //            {"Languages", new BsonArray{"english", "russian"}}
        //        };
        //    await collection.InsertOneAsync(person1);
        //    await collection.InsertOneAsync(person2);
        //}

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
