using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using WebShop.Models;

namespace WebShop.Data
{
    public enum UserRole
    {
        Chief,
        Accountant,
        Performer
    }

    public class ApplicationUser : IdentityUser
    {
        // Store a single role per user (Chief, Accountant, Performer).
        // This is also mirrored into AspNetRoles if you prefer Identity Roles;
        // For simplicity and to follow the request "Update User model/table to store these roles properly"
        // we're storing the role directly on the user as an enum mapped to string.
        [Required]
        public UserRole Role { get; set; } = UserRole.Performer;

        // Navigation properties (optional)
        // Tasks assigned to this user
        public virtual ICollection<TaskItem> AssignedTasks { get; set; }

        // Tasks created by this user
        public virtual ICollection<TaskItem> CreatedTasks { get; set; }
    }
}