using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEIP.API.Dtos
{
    public class PlanForReferenceToCreationDto
    {
        public string Reference { get; set; }
        public int Amount { get; set; }
        public int Position { get; set; }
    }
}
