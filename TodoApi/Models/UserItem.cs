namespace TodoApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    public class UserItem
    {
        [Key]
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? PassWord { get; set; }

        public string? Permission { get; set; }
    }
}
