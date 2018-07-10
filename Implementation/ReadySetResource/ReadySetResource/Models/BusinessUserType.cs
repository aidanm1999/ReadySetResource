namespace ReadySetResource.Models
{

    public partial class BusinessUserType
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int BusinessId { get; set; }
        public string Administrator { get; set; }
        public string Updates { get; set; }
        public string Store { get; set; }
        public string Messenger { get; set; }
        public string Meetings { get; set; }
        public string Calendar { get; set; }
        public string Holidays { get; set; }

        public virtual Business Business { get; set; }

    }
}
