using MessagePack;

namespace Test
{
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
}