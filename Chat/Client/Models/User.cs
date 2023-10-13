namespace Client.Models
{
    public class User
    {
        /// <summary>
        /// ID of the user
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of the user
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Phone of the user
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Password of the user
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Indicates whether the user is authorized
        /// </summary>
        public bool IsAuthorized { get; set; } = false;
    }
}
