namespace NZWalks.Api.Models.Domian
{
    public class Role
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        //Navigation property
        public List<User_Role> User_Roles { get; set; }
    }
}
