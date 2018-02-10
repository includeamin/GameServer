using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace Server.packetClasses
{
    [MessagePackObject]
   public class ResponPacket
    {
        [Key(0)]
        public int ResponCode { get; set; }
        [Key(1)]
        public string Message { get; set; }
        [Key(2)]
        public string ClientId { get; set; }
        [Key(3)]
        public string Token { get; set; }




    }
}
