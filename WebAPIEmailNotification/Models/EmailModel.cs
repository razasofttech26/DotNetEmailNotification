namespace WebAPIEmailNotification.Models
{
    public class EmailModel
    {
        public EmailModel() { }

        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public long CustomerPhone { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
    }
}
