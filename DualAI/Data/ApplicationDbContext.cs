using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DualAI.Models;

namespace DualAI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DualAI.Models.Mainpage> Mainpage { get; set; } = default!;
        public DbSet<DualAI.Models.RedirectViewModel> RedirectViewModel { get; set; } = default!;
    }
}
