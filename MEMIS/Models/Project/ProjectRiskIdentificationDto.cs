using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEMIS.Models
{
  public class ProjectRiskIdentificationDto
  {
    public int Id { get; set; }
    public string? Stage { get; set; }
    [Required]
    public string Risk { get; set; }

    private int _rank;

    [Display(Name = "Rank")]
    [Required]
    public int Rank
    {
      get { return _rank; }
      private set { _rank = value; }
    }

    private int _likelihood;
    public int Likelihood
    {
      get { return _likelihood; }
      set
      {
        _likelihood = value;
        CalculateRank();
      }
    }

    private int _severity;
    public int Severity
    {
      get { return _severity; }
      set
      {
        _severity = value;
        CalculateRank();
      }
    }
    public string? Consequence { get; set; }
    public string? Mitigation { get; set; }
    [Display(Name = "Cost Of Implementing The Risk")]
    public double RiskImplementationCost { get; set; }
    public string? Ownership { get; set; }
    private void CalculateRank()
    {
      Rank = Likelihood * Severity;
    }

    [Required]
    public virtual int? ProjectInitiationId { get; set; }
  }
}
