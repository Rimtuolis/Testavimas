namespace ISP_Projektas_2022.Shared
{
    public class MailOptions
    {
        public bool UseSmtp4Dev { get; set; }
        public string? SenderEmail { get; set; }
        public string? SenderName { get; set; }
        public string? Host { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int Port { get; set; }
    }
}
