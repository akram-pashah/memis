using System.ComponentModel.DataAnnotations;

namespace MEMIS.Data
{
    public class NDP
    {
        public int id { get; set; }
        [Display(Name="Programme")]
        public string Programme { get; set; }
        [Display(Name = "Programme Objective")]
        public string ProgrammeObjective { get; set; }
        [Display(Name = "Sub Program")]
        public string SubProgramme { get; set; }
        [Display(Name = "Sub Programme Objective")]
        public string SubProgrammeObjective { get; set; }
        [Display(Name = "Programme Intervention")]
        public string ProgrammeIntervention { get; set; }

    }
}
