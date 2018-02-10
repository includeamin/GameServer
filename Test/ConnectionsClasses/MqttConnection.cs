using System;
using System.Net;
using System.Text;
using MessagePack;
using Server.packetClasses;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Test
{
    public class MqttConnection
    {
       
        public MqttClient Client { get; set; }

        public MqttConnection(string ip )
        {
            Client = new MqttClient(IPAddress.Parse(ip));
          
        }

        public bool Connect( string clientId)
        {
            Client.Connect(clientId);
            Console.WriteLine($"Server isConnected [{Client.IsConnected}]");
            return Client.IsConnected;


        }

        public bool Respon(string clinetId, string message)
        {
            try
            {
                if (Client.IsConnected)
                {
                   
                    int tempCode = 2;
                    if (message.ToUpper() == "OK")
                    {
                        tempCode = 1;
                    }
                    var respon = new ResponPacket(){ClientId = clinetId,Message = message ,  ResponCode = tempCode };
                   
                    var data = MessagePackSerializer.Serialize<ResponPacket>(respon);
                    ushort msgId = Client.Publish(clinetId, // topic
                        data, // message body
                        MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                        false); // retained
                    Console.WriteLine($"Respon Message Sent to [{clinetId}] : messageId [{msgId}]");
                    return true;
                }
                else
                {
                    return false;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;

            }
          
        }
        /// <summary>
        /// send respone with Token
        /// client id is username
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool LoginRespon(string clientId, string message , MongoDb mongoDb)
        {
            try
            {
                if (Client.IsConnected)
                {
                    string tempToken = "Empty";
                       
                    
                    int tempCode = 2;
                    if (message.ToUpper() == "OK")
                    {
                        tempCode = 1;
                        tempToken = mongoDb.SetUserToken(clientId);
                    }
                    var respon = new ResponPacket() { ClientId = clientId, Message = message, ResponCode = tempCode ,Token = tempToken};

                    var data = MessagePackSerializer.Serialize<ResponPacket>(respon);
                    ushort msgId = Client.Publish(clientId, // topic
                        data, // message body
                        MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                        false); // retained
                    Console.WriteLine($"Respon Message Sent to {clientId} : messageId {msgId}");
                    return true;
                }
                else
                {
                    return false;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;

            }
        }


        

      
       
    }
}