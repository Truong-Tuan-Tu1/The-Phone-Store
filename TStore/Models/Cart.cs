using TStore.Common;

namespace TStore.Models
{
    public class Cart : BaseModel
    {
        public string UserId { get; set; }

        public AppUser User { get; set; } = null!;

    }
}
