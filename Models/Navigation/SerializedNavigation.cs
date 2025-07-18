using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Models.Navigation;

public class SerializedNavigation : AuditableObject
{
    public string? Name { get; set; }
    public string SerializedData { get; set; } = string.Empty;

    private static JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
        WriteIndented = true
    };

    [NotMapped]
    public Navigation? Navigation
    {
        get => string.IsNullOrEmpty(SerializedData) ? null : JsonSerializer.Deserialize<Navigation>(SerializedData, _serializerOptions);
        set => SerializedData = value == null ? string.Empty : JsonSerializer.Serialize(value, _serializerOptions);
    }
}
