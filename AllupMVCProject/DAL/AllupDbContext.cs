using AllupMVCProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AllupMVCProject.DAL
{
	public class AllupDbContext:DbContext
	{
        public AllupDbContext(DbContextOptions <AllupDbContext> option) : base(option) { }

		public DbSet<Slider> Sliders { get; set; }
    }
}
