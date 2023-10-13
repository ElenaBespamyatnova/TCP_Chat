namespace Client.Models
{
    public class Message
    {
        /// <summary>
        /// User message ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Time the user sent the message
        /// </summary>
        public DateTime DateTimeMessage { get; set; } = DateTime.Now;
        /// <summary>
        /// User message text
        /// </summary>
        public string MessageBody { get; set; }
        /// <summary>
        /// ID of the user
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// User class instance
        /// </summary>
        public User User { get; set; }
    }
}
