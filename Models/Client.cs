using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Client : Base
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public int PreferNo { get; set; }
    }
}
