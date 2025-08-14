using System.Text.Json;

namespace MEMIS.Helpers
{
  public static class Json
  {
    public static string Serialize(object data)
    {
      return JsonSerializer.Serialize(data, new JsonSerializerOptions
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles // Avoid circular reference issues
      });
    }
  }
}
