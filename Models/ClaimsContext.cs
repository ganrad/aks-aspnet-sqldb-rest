using Microsoft.EntityFrameworkCore;
namespace ClaimsApi.Models {
    public class ClaimsContext : DbContext {
        public ClaimsContext(DbContextOptions<ClaimsContext> options) : base(options) {

        }
        public DbSet<ClaimItem> ClaimItems { get; set; }
    }
}