using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    [PrimaryKey(nameof(IdGather))]
    public class Gather
    {
        public int IdGather { get; set; }

        [ForeignKey("Mushroom")]
        public int Idmushroom { get; set; }

        [ForeignKey("Mushroom_picker")]
        public int idmushroom_picker {get; set; }
    }
}