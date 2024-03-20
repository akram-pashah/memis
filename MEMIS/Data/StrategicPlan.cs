using Microsoft.AspNetCore.Razor.Language.Extensions;
using System.ComponentModel.DataAnnotations;

namespace MEMIS.Data
{
    public class StrategicPlan
    {
        [Key] 
        public int Id { get; set; } 
        [Display(Name ="NDP III")]
        public string ndp { get; set; }
        [Display(Name = "Program")]
        public string program { get; set; }
        [Display(Name = "Program Objectives")]
        public string programObjective { get;set; }
        [Display(Name = "Sub Program")]
        public string subProgram { get; set; }
        [Display(Name = "Focus Area")]
        public string focusArea { get; set; }
        [Display(Name = "NDA Strategic Objective")]
        public string strategicObjective { get; set; }
        [Display(Name = "Strategic Intervention")]
        public string strategicIntervention { get; set; }
        [Display(Name = "Strategic Action")]
        public string StrategicAction { get; set; } 
    }
}
