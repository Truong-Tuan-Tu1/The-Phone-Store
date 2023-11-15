using TStore.Common;

namespace TStore.Models;

public class Menu : BaseModel
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Alias { get; set; }

    public int Position { get; set; }
}