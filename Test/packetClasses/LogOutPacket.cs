using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace Server.packetClasses
{
    [MessagePackObject]
    public class LogOutPacket
   {[Key(0)]
       public string UserName { get; set; }

   }
}
