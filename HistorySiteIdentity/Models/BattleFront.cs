using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.ComponentModel.DataAnnotations.Schema;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HistorySiteIdentity.Models
{
    public class BattleFront // фронт
    {
        public int BattleFrontId { get; set; }
        public string Name { get; set; }
        public int TotalStrenght { get; set; }
        public virtual ICollection<Commander> Commanders { get; set; }
        public virtual ICollection<Army> Armies { get; set; }
        public byte[]? Image { get; set; }
        public int? WeekId { get; set; }
        public Week? Week { get; set; }
        public string CoordinatesXY { get; set; }
        public string CoordX { get; set; }
        public string CoordY { get; set; }
        public string Adress { get; set; }
    }
}
