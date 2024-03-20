namespace BloomersGeneralIntegrations.Mobsim.Models
{
    public class MobsimObject
    {
        public string groupId { get; set; }
        public string groupMsg { get; set; }
        public List<Message> messages { get; set; }
    }

    public class Message
    {
        public string id { get; set; }
        public string to { get; set; }
        public string msg { get; set; }
    }
}
