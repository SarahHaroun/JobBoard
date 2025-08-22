using JobBoard.Domain.Mapping;
using JobBoard.Domain.Mapping.Resolvers;

namespace JobBoard.API.Extensions
{
	public static class AutoMapperExtensions
	{
		public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
		{
			services.AddAutoMapper(cfg =>
			{
				cfg.AddProfile<JobProfile>();
				cfg.AddProfile<EmployerProfileMapping>();
				cfg.AddProfile<UserProfileMapping>();
				cfg.AddProfile<SkillAndCategoryProfile>();
				cfg.AddProfile<ApplicationProfile>();
				cfg.AddProfile<SeekerProfileMapping>();
				cfg.AddProfile<SavedJobProfile>();
                cfg.AddProfile<NotificationProfile>();
            });

			services.AddScoped<CompanyImageUrlResolver>();
			services.AddScoped<SeekerCvUrlResolver>();
			services.AddScoped<ProfileImageUrlResolver>();
			services.AddScoped<ApplicationUrlResolver>();

			return services;
		}
	}
}