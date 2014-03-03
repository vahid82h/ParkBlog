namespace ParkBlog.Domain
{
    public class Profile : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Website { get; set; }

        public virtual User User { get; set; }
    }
}