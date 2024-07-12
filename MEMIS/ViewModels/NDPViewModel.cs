using MEMIS.Data;

namespace MEMIS.ViewModels
{
  public class NDPViewModel
  {
    public NDPFile NDPFile { get; set; }
    public NDAFile NDAFile { get; set; }
    public cloudscribe.Pagination.Models.PagedResult<NDP> NDP { get; set; }
  }
}
