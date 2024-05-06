using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(Id))]
    internal class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public int Type { get; set; }
        public string? Password { get; set; }
    }
}