namespace ISP_Projektas_2022.Shared
{
    public class CurrentUser
    {
        public int Id { get; set; }
        public bool LoggedIn { get; set; }
        public AccessLevelType UserLevel { get; set; }
        public string? Email { get; set; }

        public string? Username { get; set; }

    }
}