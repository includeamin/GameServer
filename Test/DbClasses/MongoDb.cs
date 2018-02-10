using System;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using Server.Matchmake;

namespace Test
{
    public class MongoDb
    {
       
        private readonly MongoClient _client;
        private readonly string _dbName;

        public MongoDb(string url , string database)
        {
            _client = new MongoClient(url);
            _dbName = database;
        }
        /// <summary>
        /// signup users in server hash user password
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public string Insert<T>(T obj)
        {

            var database = _client.GetDatabase(_dbName);
            var data = database.GetCollection<T>(typeof(T).Name);
            if (data.FindSync(new ObjectFilterDefinition<T>(obj)) != null)
            {
                data.InsertOne(obj);
                return "OK";
            }
            else
            {
                return "same object is exist";
            }
           
            
            
        }

        public string UserSignUp(Users user)
        {
            var database = _client.GetDatabase(_dbName);
            var data = database.GetCollection<Users>(typeof(Users).Name);
            var usertemp = data.Find(x => x.Username == user.Username);
            Console.WriteLine(usertemp.Count());

            if (usertemp.Count() > 0)
            {
                
                    return "User with this username is exist";
                
            }

            var temp = data.Find(x => x.Mail == user.Mail);
            if ( temp.Count()>0)
            {
                return "user with this mail address are exist";
            }


           data.InsertOne(user);
           return "OK";
                
            

            
        }
        /// <summary>
        /// Search for user with this featurs and return true if user exist and password is ok
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public string UserFindLogin(string userName , string passWord)
        {
             //TODO:when login user return hash code to clinet for Autologin or other Request, all of request should have a hash token

            var database = _client.GetDatabase(_dbName );
            var data = database.GetCollection<Users>(typeof(Users).Name);
           
            var userObj = data.Find(x => x.Username == userName).First();
            if (userObj == null)
            {
                return "User not Found";
            }

            if (userObj.isLogin)
            {
                return "User is currently logged in ";
                
            }
            if (userObj.Password == Hash.CalculateMd5Hash(passWord + userObj.Mail))
            {
                userObj.isLogin = true;
                data.ReplaceOne(x => x._id == userObj._id, userObj);
                return "OK";
                
            }
            else
            {
                return "wrong password";
            }
            
           
  

        }

        public string SetUserToken(string username)
        {
            try
            {
                var database = _client.GetDatabase(_dbName);
                var data = database.GetCollection<Users>(typeof(Users).Name);
                var userObj = data.Find(X => X.Username == username).First();
                var hashTemp = Hash.CalculateMd5Hash(username + DateTime.Now.ToLongDateString() +
                                                     DateTime.Now.ToLongTimeString());
                userObj.Token = hashTemp;
                data.ReplaceOne(x => x._id == userObj._id, userObj);
                return hashTemp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// chnage islogin to false and token to null 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string LogOutUser(string username)
        {
            try
            {
                var database = _client.GetDatabase(_dbName);
                var data = database.GetCollection<Users>(typeof(Users).Name);
                var userObj = data.Find(X => X.Username == username).First();
                if (userObj != null)
                {
                    userObj.Token = null;
                    userObj.isLogin = false;
                    data.ReplaceOne(x => x._id == userObj._id, userObj);
                    return "OK";
       }
                
                    return "User not found";
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return $"User logout is failed \n {e.Message}";

            }
        }
        ///// <summary>
        ///// when user is Online user's username come in playerlist
        ///// </summary>
        ///// <param name="username"></param>
        ///// <returns></returns>
        //public string AddUserToPlayerList(string username)
        //{
        //    try
        //    {
        //        var database = _client.GetDatabase(_dbName);
        //        var data = database.GetCollection<Playlists>(typeof(Playlists).Name);
        //        var playerlisObj = data.Find(x => x.UserName == username);
        //        if (playerlisObj.Count() > 0)
        //        {
        //            return $"This username corrently in online player list [{username}]";
        //        }
              
        //        data.InsertOne(new Playlists(){UserName = username});
        //        return "OK";


        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        return $"add to playerlistFaild [{e.Message}]";

        //    }
        //}
        ///// <summary>
        ///// when user is Offline , user's username is delete from playlist
        ///// </summary>
        ///// <param name="username"></param>
        ///// <returns></returns>
        //public string DeleteFromPlayerList(string username)
        //{
        //    try
        //    {
        //        var database = _client.GetDatabase(_dbName);
        //        var data = database.GetCollection<Playlists>(typeof(Playlists).Name);
        //        var result = data.FindOneAndDelete(x => x.UserName == username);
        //        if (result == null)
        //        {
        //            return $"user with this username is offline (not is playerlst) [{username}].";
        //        }

        //        return $"User with [{username}] delete from playerlist ( user is offline )";
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        return $"Delete from playerlistFaild [{e.Message}]";
        //    }
        //}

        public string UpdateOnlineUser(JObject jObject)
        {
            try
            {
                var database = _client.GetDatabase(_dbName);
                var data = database.GetCollection<JObject>(typeof(JObject).Name);
                data.InsertOne(jObject);
                return $"Online UserList update [{jObject["result"]["client_num"]}] users are online";

            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                return $"Online UserList update";
            }

        }

      

        
        

    }
}