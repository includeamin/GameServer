using MongoDB.Bson;

namespace Test
{
    public class Users
    {
        public ObjectId _id { set; get; }
        public string Username { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }

        public bool isLogin { set; get; }

        public string Token { set; get; }
        
        public string CustomJson { set; get; }

    }
}
