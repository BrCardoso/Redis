using StackExchange.Redis;
using System;
using System.Data;
using System.Text.RegularExpressions;

namespace Redis
{
    class Program
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection;
        private static object result;
        static void Main(string[] args)
        {
            var redis = ConnectionMultiplexer.Connect("40.65.216.220");
            DataTable dt = new DataTable();
            IDatabase db = redis.GetDatabase();

            var sub = redis.GetSubscriber();
            sub.Subscribe("perguntas").OnMessage(
                m => {
                    var message = m.Message.ToString().Split(":");

                    var calc = Regex.Replace(message[1], "[A-Z|a-z|é]", "").Trim().Replace("?", "");
                    var result = dt.Compute(calc, "");

                    db.HashSet(message[0], new[] { new HashEntry("Grupo 4", result.ToString()) });
                }
                );

            Console.ReadLine();
        }
    }
}
