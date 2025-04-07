using MEMIS.Data;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
