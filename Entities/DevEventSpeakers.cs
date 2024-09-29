namespace AwesomeDevEvents.API.Entities
{
    public class DevEventSpeaker
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TalkTitle { get; set; }
        public string TalkDescription { get; set; }
        public string LinkedInProfile { get; set; }
        public Guid DevEventId { get; set; } //Sera usado como Foreign Key da tabela/classe DevEvent
        /* public DevEvent Event { get; set; } //Caso fosse HasMany */

        public void Update(string name, string talkTitle, string talkDescription, string linkedInProfile)
        {
            Name = name;
            TalkTitle = talkTitle;
            TalkDescription = talkDescription;
            LinkedInProfile = linkedInProfile;
        }

    }
}