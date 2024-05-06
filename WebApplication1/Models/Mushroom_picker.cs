using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(idmushroom_picker))]
    public class Mushroom_picker
    {
        public int idmushroom_picker { get; set; }
        public string? name { get; set; }
        public int yearbirth { get; set; }
    }
}