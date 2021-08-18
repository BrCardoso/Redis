using StackExchange.Redis;
using System;

namespace Redis
{
    class Program
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection;
        private static object result;
        static void Main(string[] args)
        {
            var redis = ConnectionMultiplexer.Connect("40.65.216.220");
            IDatabase db = redis.GetDatabase();

            var sub = redis.GetSubscriber();
            sub.Subscribe("perguntas").OnMessage(
                m => {
                    var perguntaId = m.Message.ToString().Split(":")[0];
                    db.HashSet(perguntaId, new[] { new HashEntry("Grupo 4", "Pesquisa no Google") });
                }
                );

            Console.ReadLine();
        }
    }
}
