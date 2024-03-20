using MEMIS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Models
{
    public class SDTAssessmentVerifyDto
    {
        public SDTAssessment sDTAssessment { get; set; }
        public int Status { get; set; }
        public string? Comments { get; set; }
    }
}
