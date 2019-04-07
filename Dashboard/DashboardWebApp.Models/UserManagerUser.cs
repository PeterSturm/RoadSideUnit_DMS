namespace DashboardWebApp.Models
{
    public class UserManagerUser
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int ManagerUserManagerId { get; set; }
        public string ManagerUserName { get; set; }
        public ManagerUser ManagerUser { get; set; }
    }
}
