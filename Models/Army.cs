using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using HistorySiteIdentity.Models;
using HistorySiteIdentity.ViewModels;

namespace HistorySiteIdentity.Models
{
    public class Army // армия
    {
        
        public int ArmyId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public int TotalStrenght { get; set; }
        public virtual ICollection<Commander> Commanders { get; set; }
        public virtual ICollection<Corps> Corps { get; set; }
        public virtual ICollection<Division> Divisions { get; set; }
        public virtual ICollection<Brigade> Brigades { get; set; }
        [ForeignKey("BattleFront")]
        public int? BattleFrontId { get; set; }
        public BattleFront BattleFront { get; set; }
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
