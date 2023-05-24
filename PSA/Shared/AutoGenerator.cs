using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Shared
{
    public class AutoGenerator
    {
        [Required]
        public int Material { get; set; }
        [Required]
        public int Fighting_Style { get; set; }
        [Required]
        public int Budget { get; set; }
        [Required]
        public int Zodiac { get; set; }
    }
}
