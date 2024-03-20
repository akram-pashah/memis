using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Models
{
    public class PreinspectionDto
    {
        public int Id { get; set; }
        [Display(Name ="Inspection Date")]
        public DateTime InspectionDate { get; set; }
        public string Applicant { get; set; }
        public string Business { get; set; }
        public string Road { get; set; }
        public string Zone { get; set; }
        public string Village { get; set; }
        public string Country { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }

		[Display(Name = "Product Classification")]
        public int ProductClassification { get; set; }
        [Display(Name = "Nearest Pharma Name")]
        public string NearestPharmaName { get; set; }

        [Display(Name = "Nearest Pharma Road")]
        public string NearestPharmaRoad { get; set; }
        [Display(Name = "Nearest Pharma Distance")]
        public string NearestPharmaDistance { get; set; }
        [Display(Name = "Nearest Drug Shop Name")]
        public string NearestDrugShopName { get; set; }
        [Display(Name = "Nearest Drug Shop Road")]
        public string NearestDrugShopRoad { get; set; }
        [Display(Name = "Nearest Drug Shop Distance")]
        public string NearestDrugShopDistance { get; set; }
        [Display(Name = "Comments")]
        public string comments { get; set; }
        [Display(Name = "Approval Status")]
        public int ApprovalStatus { get; set; }
		[Display(Name = "District")]
		public virtual int DistrictId { get; set; }
        public virtual string? DistrictName { get; set; }
        [Display(Name ="Inspector Name")]
        public string? InspectorId { get; set; }
    }
}
