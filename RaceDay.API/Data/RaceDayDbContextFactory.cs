using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RaceDay.API.Data
{
    public class RaceDayDbContextFactory : IDesignTimeDbContextFactory<RaceDayDbContext>
    {
        public RaceDayDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RaceDayDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=LUBISDEVICE\\SQLEXPRESS;Database=RaceDay;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

            return new RaceDayDbContext(optionsBuilder.Options);
        }
    }
}