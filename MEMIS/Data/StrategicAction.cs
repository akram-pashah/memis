using MEMIS.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Data
{
  public class StrategicAction
  {
    [Key]
    [Required]
    public int intAction { get; set; }
    [Required]
    [Display(Name = "Code")]
    [MaxLength(10)]
    public string actionCode { get; set; }
    [Required]
    [Display(Name = "Strategic Action")]
    [MaxLength(1000)]
    public string actionName { get; set; }
    [Display(Name = "Strategic Intervention")]
    public virtual int? intIntervention { get; set; }
    [ForeignKey("intIntervention")]
    public virtual StrategicIntervention? StrategicIntervention { get; set; }
    public virtual ICollection<Activity> Activities { get; set; }

  }
}
