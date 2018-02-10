using System;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using MessagePack;

namespace Test
{
    class Program
    {

        static MqttConnection connection = new MqttConnection("178.63.169.234");
      
       static MongoDb MongoDb = new MongoDb("mongodb://localhost:27017","Game");
     

       
        static void Main(string[] args)
        {
           
            Server mainServer = new Server("178.63.169.234", "mongodb://localhost:27017","MainServer","Game",new string[]{"Register","Login"});
            mainServer.StartServer();
         

            Console.Read();
        }

      
    }

   
}
