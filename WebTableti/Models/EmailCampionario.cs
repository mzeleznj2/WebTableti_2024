using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTableti.Models
{
    public class EmailCampionario
    {
        public DateTime StopDate { get; set; }
        public string RoomCode { get; set; }
        public string GroupCode { get; set; }
        public int MachCode { get; set; }
        public string UniqueId { get; set; }
        public DateTime TextDate { get; set; }
        public string UserCode { get; set; }
        public string Text { get; set; }
        public int EmailStatus { get; set; }

    }
}