namespace TodoApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    public class TodoListItem
    {
        [JsonIgnore]
        [Key]
        public int OrdersId { get; set; }

        public DateTime AddDate { get; set; }

        public string? Title { get; set; }

        public bool IsDone { get; set; }

        [JsonIgnore]
        public int? UserID { get; set; }
    }
}
