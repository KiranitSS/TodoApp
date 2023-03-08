namespace TodoApp_DomainEntities
{
    public class Reminder : EntityBase
    {
        public long TodoId { get; set; }

        public DateTime RemindTime { get; set; }

        public string UserId { get; set; } = string.Empty;
    }
}