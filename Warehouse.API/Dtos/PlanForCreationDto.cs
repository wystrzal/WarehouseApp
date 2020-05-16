using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEIP.API.Dtos
{
    public class PlanForCreationDto
    {
        public string Line { get; set; }
        public int Position { get; set; }
        public string TypeOfCasing { get; set; }
        public string Casing { get; set; }
        public string Cover { get; set; }
        public int Amount { get; set; }
        public string Chamber { get; set; }
        public string Reference { get; set; }
        public string Saucepan { get; set; }
        public string Status { get; set; }
    }
}
