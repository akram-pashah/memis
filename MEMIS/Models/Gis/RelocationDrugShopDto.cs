using MEMIS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MEMIS.Models
{
    public class RelocationDrugShopDto
    {
        public int Id { get; set; }
        [Display(Name = "Inspection Date")]
        public DateTime InspectionDate { get; set; }
        public string Applicant { get; set; }
        public string Business { get; set; }
        public string CRoad { get; set; }
        public string CZone { get; set; }
        public string CVillage { get; set; }
        public string CCountry { get; set; }
        public string CTelephone { get; set; }
        public string CEmail { get; set; }
        public string CGPS { get; set; }
        public string PRoad { get; set; }
        public string PZone { get; set; }
        public string PVillage { get; set; }
        public string PCountry { get; set; }
        public string PTelephone { get; set; }
        public string PEmail { get; set; }
        public string PGPS { get; set; }
        [Display(Name = "Product Classification")]
        public int ProductClassification { get; set; }

        [Display(Name = "Category of  Premises")]
        public int CategoryOfpremises { get; set; }
         

        [Display(Name = "Comments")]
        public string comments { get; set; }
        [Display(Name = "Approval Status Inspector")]



        public int ApprovalStatusInspector { get; set; }
        [Display(Name = "District")]


        public string? InspectorId { get; set; }
        [Display(Name = "Inspector Name")]

        public string? InspectorName { get; set; }
        [Display(Name = "Inspector comments")]

        public string? Inspectorcomments { get; set; }


        public string? HeadInspectorId { get; set; }
        [Display(Name = "Head Name")]
        public string? HeadInspectorName { get; set; }
        [Display(Name = "Head comments")]
        public string? HeadInspectorcomments { get; set; }


        public string? DirectorInspectorId { get; set; }
        [Display(Name = "Director Name")]

        public string? DirectorInspectorName { get; set; }
        [Display(Name = "Director Comments")]

        public string? DirectorInspectorcomments { get; set; }

        public virtual int DistrictId { get; set; }

        public int ApprovalStatusHead { get; set; }
        public int ApprovalStatusDir { get; set; }
    }
}
