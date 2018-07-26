namespace ReadySetResource.Models
{

    public partial class BusinessUserType
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public int BusinessId { get; set; }
        public virtual Business Business { get; set; }

    }
}
