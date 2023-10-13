namespace ChatServer.Models
{
    public class UserMessage
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
        /// Chat user name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Name of the chat, which consists of nicknames of users who take part in the conversation
        /// </summary>
        public string ChatName { get; set; }
        /// <summary>
        /// ID of the client connected to the chat
        /// </summary>
        public string ClientId { get; set; }
    }
}
