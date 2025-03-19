namespace MEMIS.Models
{
  public class EditUserViewModel
  {
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public Guid intDept { get; set; }
    public Guid intDir { get; set; }
    public Guid? intRegion { get; set; }
    public string? Password { get; set; }
  }
}
