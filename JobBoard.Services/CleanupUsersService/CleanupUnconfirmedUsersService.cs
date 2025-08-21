using JobBoard.Repositories.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Services.CleanupUsersService
{
    public class CleanupUnconfirmedUsersService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public CleanupUnconfirmedUsersService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var unconfirmedUsers = context.Users
                        .Where(u => !u.EmailConfirmed && u.CreatedAt < DateTime.UtcNow.AddDays(-7));

                    context.Users.RemoveRange(unconfirmedUsers);
                    await context.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }

}
