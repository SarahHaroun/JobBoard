using JobBoard.Domain.Repositories.Contract;
using JobBoard.Domain.Services.Contract;
using JobBoard.Repositories;
using JobBoard.Services;
using JobBoard.Services._ِAuthService;
using JobBoard.Services.AdminService;
using JobBoard.Services.EmailService;
using JobBoard.Services.EmployerService;
using JobBoard.Services.SeekerService;

namespace JobBoard.API.Extensions
{
	public static class BusinessServices
	{
		public static IServiceCollection AddBusinessServices(this IServiceCollection services)
		{
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IEmployerService, EmployerService>();
			services.AddScoped<ISeekerService, SeekerService>();
			services.AddScoped<IJobService, JobService>();
			services.AddScoped<IEmailService, EmailService>();
			services.AddScoped<IApplicationService, ApplicationService>();
			services.AddScoped<ISavedJobService, SavedJobService>();
			services.AddScoped<IAdminService, AdminService>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			return services;
		}
	}
}