using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Timers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using Server.packetClasses;

namespace Test
{
    public static class Actions
    {
        /// <summary>
        /// register user 
        /// </summary>
        /// <param name="mongoDb"></param>
        /// <param name="registerPacket"></param>
        /// <param name="mqttConnection"></param>
        public static void Register( MongoDb mongoDb , RegisterPacket registerPacket , MqttConnection mqttConnection)
        {
            try
            {

             var result=   mongoDb.UserSignUp(new Users()
                {
                    Username = registerPacket.UserName,
                    Mail = registerPacket.Mail,
                    Password = Hash.CalculateMd5Hash(registerPacket.PassWord+ registerPacket.Mail)
                });
                Console.WriteLine($"User Register Compelet : Username= {registerPacket.UserName} , Mail={registerPacket.Mail}");
                mqttConnection.Respon(registerPacket.ClientId, result);
             //   return () => { };


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                mqttConnection.Respon(registerPacket.ClientId, $"Faild : {e.Message}");

               // return () => { };
            }

        }
      
        /// <summary>
        /// login user and return token to client
        /// </summary>
        /// <param name="mongoDb"></param>
        /// <param name="loginPacket"></param>
        /// <param name="mqttConnection"></param>
        public static void Login(MongoDb mongoDb , LoginPacket loginPacket , MqttConnection mqttConnection)
        {
            try
            {
                   var result = mongoDb.UserFindLogin(loginPacket.UserName, loginPacket.PassWord);
                
                   Console.WriteLine($"User login with this {loginPacket.UserName} username");

                   mqttConnection.LoginRespon(loginPacket.ClientId, result,mongoDb);

     

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                mqttConnection.Respon(loginPacket.ClientId, $"Faild : {e.Message}");

            }

        }

        public static void LogOut(MongoDb mongoDb, LogOutPacket logOutPacket, MqttConnection mqttConnection)
        {
            try
            {
                var result = mongoDb.LogOutUser(logOutPacket.UserName);
                Console.WriteLine($"User with [{logOutPacket.UserName}] username logged out");
                mqttConnection.Respon(logOutPacket.UserName, result);



            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                mqttConnection.Respon(logOutPacket.UserName, $"Faild : {e.Message}");


            }
        }
        /// <summary>
        /// TODO: send message to user and wait for respon if respon/not respon , this mean user is online/ofline : every 10 second check with rest api , query to server to send online user
        /// this method for checking one user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsUserOnline(string userName)
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }
            return true;
        }
        /// <summary>
        /// update playerlist by query to server with RestApi
        /// </summary>
        public static void UpdatePlaylist(MongoDb mongoDb)
        {
            Timer timer = new Timer();
            timer.Interval = 1800;
            timer.Elapsed += (sender, e) => UpdatePlaylistTimer(sender, e, mongoDb);
            timer.Enabled = true;
            timer.Start();

        }

        public static void UpdatePlaylistTimer(object state, ElapsedEventArgs E, MongoDb mongoDb )
        {
            try
            {
                var client =
                    new RestClient("http://178.63.169.234:18083/api/v2/nodes/emq@127.0.0.1/clients")
                    {
                        Authenticator = new HttpBasicAuthenticator("admin", "public")
                    };
                var request = new RestRequest();
                IRestResponse response = client.Execute(request);
                var content = response.Content;

                var jsonObj = JObject.Parse(content);

                var result=  mongoDb.UpdateOnlineUser(jsonObj);
                Console.WriteLine(result);





            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }
    }
}