using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Models.Navigation;

public class SerializedNavigation : AuditableObject
{
    public string Name { get; set; } = string.Empty;
    public string SerializedData { get; set; } = string.Empty;

    [NotMapped]
    public Navigation? Navigation
    {
        get => string.IsNullOrEmpty(SerializedData) ? null : JsonSerializer.Deserialize<Navigation>(SerializedData);
        set => SerializedData = value == null ? string.Empty : JsonSerializer.Serialize(value);
    }
}
