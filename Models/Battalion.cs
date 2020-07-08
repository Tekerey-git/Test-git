using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HistorySiteIdentity.Models
{
    public class Battalion // батальон
    {
        public int BattalionId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public int TotalStrenght { get; set; }
        public virtual ICollection<Commander> Commanders { get; set; }
        public int? RegimentId { get; set; }
        public Regiment Regiment { get; set; }
        public int? BrigadeId { get; set; }
        public Brigade Brigade { get; set; }
        public int? WeekId { get; set; }
        public Week? Week { get; set; }
        public byte[]? Image { get; set; }
        public string? AdditionalInformation { get; set; }
        public string CoordinatesXY { get; set; }
        public string CoordX { get; set; }
        public string CoordY { get; set; }
        public string Adress { get; set; }
    }
}
