using System;
using System.Text;
using System.Threading.Tasks;
using MessagePack;
using Server.packetClasses;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Test
{
    public class Server
    {
        private readonly MqttConnection _mqttConnection;
        protected string MqttAddress;
        protected string MongoDbAddress;
        private readonly MongoDb _mongoDb;
        private readonly string _serverId;
        private readonly string[] _serverTopics;

        public Server(string mqttAddress,string mongoDbAddress,string serverId,string dataBaseName ,string[] serverTopics)
        {
            _mongoDb = new MongoDb(mongoDbAddress,dataBaseName);
            _mqttConnection = new MqttConnection(mqttAddress);
            _serverId = serverId;
            _serverTopics = serverTopics;
        }

        public void StartServer()
        {
            try
            {
                Console.WriteLine("Server has been started");

                _mqttConnection.Connect(_serverId);
                
                _mqttConnection.Client.ConnectionClosed += ClinetOnConnectionClosed;
                _mqttConnection.Client.MqttMsgPublishReceived += ClinetOnMqttMsgPublishReceived;
                _mqttConnection.Client.MqttMsgPublished += ClinetOnMqttMsgPublished;
                _mqttConnection.Client.MqttMsgSubscribed += ClinetOnMqttMsgSubscribed;
                _mqttConnection.Client.MqttMsgUnsubscribed += ClinetOnMqttMsgUnsubscribed;


                
                _mqttConnection.Client.Subscribe(new string[]{ "Register" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                _mqttConnection.Client.Subscribe(new string[] { "Login" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
                _mqttConnection.Client.Subscribe(new string[] { "Logout" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
                //when user connect to server send message to this topic to say i am online
                _mqttConnection.Client.Subscribe(new string[] { "OpenApp" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
                //when user close app this send to server to say client is offline
                _mqttConnection.Client.Subscribe(new string[] { "CloseApp" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
                //when user wanto matchmake send this message to this topic to move user from playerlist to lobie 
                _mqttConnection.Client.Subscribe(new string[] { "GotoLobies" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
                //match make Todo : subscribe in topic "Matchmake" when user want to match make



              


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               
            }

          



        }

        private void ClinetOnMqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            
        }

        private void ClinetOnMqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            
        }

        private void ClinetOnMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var topic = e.Topic;

            switch (topic)
            {
                case "Register":
                    Actions.Register(_mongoDb, MessagePackSerializer.Deserialize<RegisterPacket>(e.Message),
                        _mqttConnection);
                    break;
                case "Login":
                    
                    Actions.Login(_mongoDb, MessagePackSerializer.Deserialize<LoginPacket>(e.Message),_mqttConnection);
                    
                    break;
                case "Logout":
                    
                    Actions.LogOut(_mongoDb, MessagePackSerializer.Deserialize<LogOutPacket>(e.Message),_mqttConnection);

                    break;
                case "OpenApp":
                    //Todo: query to data base with username and add the user to playerlist

                    break;
                case "CloseApp":
                    //Todo: remove user from playerlist (query by username)
                    break;
            }

        }

        private void ClinetOnMqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
        }

        private void ClinetOnConnectionClosed(object sender, EventArgs e)
        {
        }
    }
}