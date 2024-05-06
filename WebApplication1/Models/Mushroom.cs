using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(Idmushroom))]
    public class Mushroom
    {
        public int Idmushroom{ get; set; }
        public string? Name { get; set; }
        public bool Edibility { get; set; }
        public string? Class { get; set; }
        public int? Market_price {get; set; }
        public string? Mushroom_birthplace {get; set;}
        public string? Rarity {get; set; }

    }
}