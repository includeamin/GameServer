using MessagePack;

namespace Server.Matchmake
{
    [MessagePackObject()]
    /// <summary>
    /// Todo : send all data that need to exhcange between two clients
    /// </summary>
    /// 
    public class MatchPacket
    {
        public int X { get; set; }
        public int Y { get; set; }
       
    }
}