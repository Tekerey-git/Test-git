using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HistorySiteIdentity.Models;
using Microsoft.AspNetCore.Http;

namespace HistorySiteIdentity.ViewModels
{
    public class CommanderViewModel
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
        public IFormFile File { get; set; }
        public int WeekId { get; set; }
        public byte[] Image { get; set; }
        public int? BattleFrontId { get; set; }
        public int? ArmyId { get; set; }
        public int? CorpsId { get; set; }
        public int? DivisionId { get; set; }
        public int? BrigadeId { get; set; }
        public int? RegimentId { get; set; }
        public int? BattalionId { get; set; }

        //public CommanderViewModel(Commander commander)
        //{
        //    Name = commander.Name;
        //    Surname = commander.Surname;
        //    Paronymic = commander.Paronymic;
        //    DateOfBirth = commander.DateOfBirth;
        //    Position = commander.Position;
        //    AdditionalInformation = commander.AdditionalInformation;
        //    WeekId = commander.WeekId;
        //    Image = commander.Image;

        //}
    }
}
