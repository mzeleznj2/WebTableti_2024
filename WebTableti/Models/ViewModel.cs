using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTableti.Models
{
    public class ViewModel
    {
        public IEnumerable<PodaciDetalji> podaciDetalji { get; set; }
        public IEnumerable<PodaciDetalji> podacidetalji06 { get; set; }
    }
}