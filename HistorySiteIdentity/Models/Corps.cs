using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HistorySiteIdentity.Models
{
    public class Corps //корпус
    {
        public int CorpsId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public int TotalStrenght { get; set; }
        public virtual ICollection<Commander> Commanders { get; set; }
        public virtual ICollection<Division> Divisions { get; set; }
        public virtual ICollection<Brigade> Brigades { get; set; }
        public virtual ICollection<Regiment> Regiments { get; set; }
        public virtual ICollection<Battalion> Battalions { get; set; }
        public int? BattlefrontId { get; set; }
        public BattleFront BattleFront { get; set; }
        public int? ArmyId { get; set; }
        public Army? Army { get; set; }
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
