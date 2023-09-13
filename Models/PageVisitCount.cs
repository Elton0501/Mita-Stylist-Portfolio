using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PageVisitCount
    {
        public int Id { get; set; }
        public string VisitPage { get; set; }
        public string SessionID { get; set; }
        public DateTime VisitDateTime { get; set; }
        [NotMapped]
        public Album Album { get; set; }
    }
}
