using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HistorySiteIdentity.Models
{
    public class Week
    {
        public int WeekId { get; set; }
        public int WeekNumber { get; set; }
        [Display(Name = "Data Date")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }
        public virtual ICollection<BattleFront> BattleFronts { get; set; }
        public virtual ICollection<Army> Armies { get; set; }
        public virtual ICollection<Corps> Corpss { get; set; }
        public virtual ICollection<Division> Divisions { get; set; }
        public virtual ICollection<Brigade> Brigades { get; set; }
        public virtual ICollection<Regiment> Regiments { get; set; }
        public virtual ICollection<Battalion> Battalions { get; set; }

    }
}
