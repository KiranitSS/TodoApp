namespace TodoAppWeb.Models.ViewModels
{
    public class ReminderViewModel
    {
        public long Id { get; set; }

        public long TodoId { get; set; }

        public long ListId { get; set; }

        public DateTime RemindTime { get; set; }
    }
}
