using Microsoft.AspNetCore.Identity;

namespace CRUDPersonsAPI.models.IdentityModels
{
    public class ApplicationUser: IdentityUser<int>
    {
        public string PersonName { get; set; }
    }
}
