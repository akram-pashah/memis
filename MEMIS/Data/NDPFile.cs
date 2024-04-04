using System.ComponentModel.DataAnnotations;

namespace MEMIS.Data
{
  public class NDPFile
  {
    public int Id { get; set; }

    [Display(Name = "File Upload")]
    public byte[] FileContent { get; set; }

    [Display(Name = "File Name")]
    public string FileName { get; set; }

    public int? FinancialYear { get; set; }

    public DateTime? CreatedDate { get; set; }
  }
}
