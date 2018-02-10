namespace Test
{
    class Packet
    {
        public Packet(string topic, string body)
        {
            Topic = topic;
            Body = body;
        }
        public string Topic { get; set; }
        public string Body { get; set; }
    }
}