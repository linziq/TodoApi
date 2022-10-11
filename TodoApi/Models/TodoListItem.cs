namespace TodoApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    public class TodoListItem
    {
        [Key]
        [JsonIgnore]
        public int PrimaryID { get; set; }

        public DateTime AddDate { get; set; }

        public string? Title { get; set; }

        public bool IsDone { get; set; }

        [JsonIgnore]
        public int? UserID { get; set; }
    }
}
