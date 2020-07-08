using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistorySiteIdentity.Models;
using Microsoft.AspNetCore.Http;

namespace HistorySiteIdentity.ViewModels
{
    public class CombatFormationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public int TotalStrenght { get; set; }
        public virtual ICollection<Commander>? Commanders { get; set; }
        public int? WeekId { get; set; }
        public Week? Week { get; set; }
        public int? OldWeekId { get; set; }
        public int? BattleFrontId { get; set; }
        public BattleFront? BattleFront { get; set; }
        public int? ArmyId { get; set; }
        public Army Army {get;set;} 
        public int? CorpsId { get; set; }
        public Corps? Corpus { get; set; }
        public int? DivisionId { get; set; }
        public Division? Division { get; set; }
        public int? BrigadeId { get; set; }
        public Brigade Brigade { get; set; }
        public int? RegimentId { get; set; }
        public Regiment Regiment { get; set; }
        public int? BattalionId { get; set; }
        public Battalion Battalion { get; set; }
        public virtual ICollection<Army>? Armies { get; set; }
        public virtual ICollection<Corps>? Corps { get; set; }
        public virtual ICollection<Division>? Divisions { get; set; }
        public virtual ICollection<Brigade>? Brigades {get;set;}
        public virtual ICollection<Regiment>? Regiments {get;set;}
        public virtual ICollection<Battalion>? Battalions {get;set;}
        public IFormFile File { get; set; }
        public byte[]? Image { get; set; }
        public string? AdditionalInformation { get; set; }
        public string Coordinates { get; set; }
        public string CoordX { get; set; }
        public string CoordY { get; set; }
        public string Adress { get; set; }

    }
}
