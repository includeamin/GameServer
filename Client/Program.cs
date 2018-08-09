using System;
using System.Diagnostics;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using MessagePack;
using Server.packetClasses;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


namespace Client
{
    class MqttConnection
    {

        public MqttClient Clinet { get; set; }

        public MqttConnection(string ip)
        {
            Clinet = new MqttClient(IPAddress.Parse(ip));
        }

        public bool Connect(string clientId)
        {
            Clinet.Connect(clientId);
            return Clinet.IsConnected;

        }


    }
    [MessagePackObject]
   public class LogOutPacket
    {
        [Key(0)]
        public string UserName { get; set; }
    }
    [MessagePackObject]
    public class RegisterPacket
    {
        [Key(0)]
        public string UserName { get; set; }

        [Key(1)]
        public string PassWord { get; set; }

        [Key(2)]
        public string Mail { get; set; }
        [Key(3)]
        public string ClientId { get; set; }

    }
    [MessagePackObject]
    public class LoginPacket
    {
        [Key(0)]
        public string UserName { get; set; }
        [Key(1)]
        public string PassWord { get; set; }
        [Key(2)]
        public string ClientId { get; set; }
    }
    class Program
    {
        
        static MqttConnection connection = new MqttConnection("178.63.169.234");

        static void Main(string[] args)
        {
            RegisterPacket mc = new RegisterPacket() { UserName = "includeamin", Mail = "aminjamal10@gmail.com", PassWord = "9518867" ,ClientId = "includeamin"};
            LoginPacket mc2 = new LoginPacket() {PassWord = "9518867", UserName = "includeamin", ClientId = "includeamin" };
            LogOutPacket mc11 = new LogOutPacket(){UserName = "includeamin"};
            connection.Connect("some");
            connection.Clinet.MqttMsgPublished += ClinetOnMqttMsgPublished;
            connection.Clinet.MqttMsgPublishReceived += ClinetOnMqttMsgPublishReceived;
            var bytes = MessagePackSerializer.Serialize(mc);

            connection.Clinet.Subscribe(new string[] { "includeamin" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            connection.Clinet.Publish("Register", bytes, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
           
           
            //Parallel.For(0, 10000, i =>
            //{
            //    RegisterPacket mc = new RegisterPacket() { UserName = i.ToString(), Mail = $"{i.ToString()}@gmail.com", PassWord = "9518867", ClientId = i.ToString() };
            //var bytes = MessagePackSerializer.Serialize(mc);

            //connection.Clinet.Publish("Register", bytes, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
           // });
            //for (int i = 0; i < 10000; i++)
            //{
            //    RegisterPacket mc = new RegisterPacket() { UserName = i.ToString(), Mail = $"{i.ToString()}@gmail.com", PassWord = "9518867", ClientId = i.ToString() };
            //    var bytes = MessagePackSerializer.Serialize(mc);

            //    connection.Clinet.Publish("Register", bytes, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            //}
            stopwatch.Stop();

            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            Console.Read();
        }

        private static void ClinetOnMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs mqttMsgPublishEventArgs)
        {
            var amin = MessagePackSerializer.Deserialize<ResponPacket>(mqttMsgPublishEventArgs.Message);
            Console.WriteLine(amin.Token);
            Console.WriteLine(amin.Message);
        }

        private static void ClinetOnMqttMsgPublished(object sender, MqttMsgPublishedEventArgs mqttMsgPublishedEventArgs)
        {
           Console.WriteLine("Done");
        }
    }
}
