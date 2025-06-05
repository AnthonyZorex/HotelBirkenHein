using Microsoft.EntityFrameworkCore;
using Motel_birkenhein.Data;

namespace Motel_birkenhein.Send_Data
{
    public class SendData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MotelBirkenheinData(
            serviceProvider.GetRequiredService<
            DbContextOptions<MotelBirkenheinData>>()))
            {
                context.Database.Migrate();
                context.SaveChanges();
            }
        }
    }
}
