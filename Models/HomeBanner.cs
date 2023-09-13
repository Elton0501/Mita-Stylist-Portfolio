using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class HomeBanner : Base
    {
        public int Id { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string SubTitle { get; set; }
        public string ButtonText { get; set; }
        public string ButtonUrl { get; set; }
    }
}
