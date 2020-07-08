using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HistorySiteIdentity.ViewModels;
using System.Threading.Tasks;

namespace HistorySiteIdentity.Models
{
    public class Commander
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Paronymic { get; set; }
        [Display(Name = "Data Date")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public string Position { get; set; }
        public string AdditionalInformation { get; set; }
        public int WeekId { get; set; }  // ???
        public Week Week { get; set; }
        public byte[]? Image { get; set; }
        public int? BattleFrontId { get; set; }
        public BattleFront BattleFront { get; set; }
        public int? ArmyId { get; set; }
        public Army? Army { get; set; }
        public int? CorpsId { get; set; }
        public Corps? Corps { get; set; }
        public int? DivisionId { get; set; }
        public Division? Division { get; set; }
        public int? BrigadeId { get; set; }
        public Brigade? Brigade { get; set; }
        public int? RegimentId { get; set; }
        public Regiment? Regiment { get; set; }
        public int? BattalionId { get; set; }
        public Battalion? Battalion { get; set; }

        //public Commander(CommanderViewModel CommanderVM)
        //{
        //    Name = CommanderVM.Name;
        //    Surname = CommanderVM.Surname;
        //    Paronymic = CommanderVM.Paronymic;
        //    DateOfBirth = CommanderVM.DateOfBirth;
        //    Position = CommanderVM.Position;
        //    AdditionalInformation = CommanderVM.AdditionalInformation;
        //    WeekId = CommanderVM.WeekId;
        //    Image = CommanderVM.Image;
        //}
}
}
