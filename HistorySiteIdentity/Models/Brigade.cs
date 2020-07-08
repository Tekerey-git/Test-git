using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HistorySiteIdentity.Models
{
    public class Brigade //бригада
    {
        public int BrigadeId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public int TotalStrenght { get; set; }
        public virtual ICollection<Commander> Commanders { get; set; }
        public virtual ICollection<Regiment> Regiments { get; set; }
        public virtual ICollection<Battalion> Battalions { get; set; }
        public int? DivisionId { get; set; }
        public Division Division { get; set; }
        public int? CorpsId { get; set; }
        public Corps Corps { get; set; }
        public int? ArmyId { get; set; }
        public Army Army { get; set; }
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
