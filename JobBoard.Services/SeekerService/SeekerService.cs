using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobBoard.Domain.DTO.SeekerDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Shared;
using JobBoard.Repositories.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Tls;

namespace JobBoard.Services.SeekerService
{
	public class SeekerService : ISeekerService
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _env;
		private readonly IConfiguration _configuration;


		public SeekerService(ApplicationDbContext context, IMapper mapper, IConfiguration configuration, IWebHostEnvironment env)
		{
			_context = context;
			_mapper = mapper;
			_env = env;
			_configuration = configuration;
		}

		public async Task<SeekerProfileDto?> GetByUserIdAsync(string userId)
		{
			return await _context.SeekerProfiles
				.Where(s => s.UserId == userId)
				.ProjectTo<SeekerProfileDto>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync();
		}

		public async Task<List<SeekerProfileDto>> GetAllAsync()
		{
			return await _context.SeekerProfiles
				.ProjectTo<SeekerProfileDto>(_mapper.ConfigurationProvider)
				.ToListAsync();
		}

		// ================== GET ==================
		//public async Task<SeekerProfileDto?> GetByUserIdAsync(string userId)
		//{
		//	var seeker = await _context.SeekerProfiles
		//		.Include(s => s.Skills)
		//		.Include(s => s.seekerInterests)
		//		.Include(s => s.seekerCertificates)
		//		.Include(s => s.SeekerTraining)
		//		.Include(s => s.SeekerEducations)
		//		.Include(s => s.SeekerExperiences)
		//		.Include(s => s.User)
		//		.FirstOrDefaultAsync(s => s.UserId == userId);

		//	if (seeker == null) return null;

		//	return _mapper.Map<SeekerProfileDto>(seeker);
		//}

		//public async Task<List<SeekerProfileDto>> GetAllAsync()
		//{
		//	var seekers = await _context.SeekerProfiles
		//		.Include(s => s.Skills)
		//		.Include(s => s.seekerInterests)
		//		.Include(s => s.seekerCertificates)
		//		.Include(s => s.SeekerTraining)
		//		.Include(s => s.SeekerEducations)
		//		.Include(s => s.SeekerExperiences)
		//		.Include(s => s.User)
		//		.AsNoTracking()
		//		.ToListAsync();

		//	return _mapper.Map<List<SeekerProfileDto>>(seekers);
		//}

		// ================== UPDATE ==================
		//public async Task<bool> UpdateAsync(string userID, SeekerProfileUpdateDto dto)
		//{
		//	var seeker = await _context.SeekerProfiles
		//		.Include(s => s.Skills)
		//		.Include(s => s.seekerInterests)
		//		.Include(s => s.seekerCertificates)
		//		.Include(s => s.SeekerTraining)
		//		.Include(s => s.SeekerEducations)
		//		.Include(s => s.SeekerExperiences)
		//		.Include(s => s.User)
		//		.AsNoTracking()
		//		.FirstOrDefaultAsync(s => s.UserId == userID);

		//	if (seeker == null) return false;


		//	// Map basic props (excluding collections)
		//	_mapper.Map(dto, seeker);

		//	// ===== Skills =====
		//	if (dto.Skills != null)
		//	{
		//		var updatedSkills = dto.Skills.Distinct().ToList();

		//		// Clear all current skills first
		//		seeker.Skills.Clear();

		//		// Get all existing skills from database
		//		var existingSkillsInDb = await _context.Skills
		//			.Where(s => updatedSkills.Contains(s.SkillName))
		//			.ToDictionaryAsync(s => s.SkillName, s => s);

		//		// Add skills (existing or new)
		//		foreach (var skillName in updatedSkills)
		//		{
		//			if (existingSkillsInDb.ContainsKey(skillName))
		//			{
		//				// Use existing skill
		//				seeker.Skills.Add(existingSkillsInDb[skillName]);
		//			}
		//			else
		//			{
		//				// Create new skill
		//				var newSkill = new Skill { SkillName = skillName };
		//				seeker.Skills.Add(newSkill);
		//			}
		//		}
		//	}

		//	// ===== Interests =====
		//	if (dto.Interests != null)
		//	{
		//		var current = seeker.seekerInterests.Select(i => i.InterestName).ToList();
		//		var updated = dto.Interests.Distinct().ToList();

		//		var toRemove = seeker.seekerInterests.Where(i => !updated.Contains(i.InterestName)).ToList();
		//		foreach (var interest in toRemove)
		//			seeker.seekerInterests.Remove(interest);

		//		foreach (var interestName in updated)
		//		{
		//			if (!current.Contains(interestName))
		//			{
		//				seeker.seekerInterests.Add(new SeekerInterest
		//				{
		//					InterestName = interestName,
		//				});
		//			}
		//		}
		//	}

		//	// ===== Certificates =====
		//	if (dto.Certificates != null)
		//	{
		//		var current = seeker.seekerCertificates.Select(c => c.CertificateName).ToList();
		//		var updated = dto.Certificates.Distinct().ToList();

		//		var toRemove = seeker.seekerCertificates.Where(c => !updated.Contains(c.CertificateName)).ToList();
		//		foreach (var cert in toRemove)
		//			seeker.seekerCertificates.Remove(cert);

		//		foreach (var certName in updated)
		//		{
		//			if (!current.Contains(certName))
		//			{
		//				seeker.seekerCertificates.Add(new SeekerCertificate { CertificateName = certName });
		//			}
		//		}
		//	}

		//	// ===== Trainings =====
		//	if (dto.Trainings != null)
		//	{
		//		var current = seeker.SeekerTraining.Select(t => t.TrainingName).ToList();
		//		var updated = dto.Trainings.Distinct().ToList();

		//		var toRemove = seeker.SeekerTraining.Where(t => !updated.Contains(t.TrainingName)).ToList();
		//		foreach (var training in toRemove)
		//			seeker.SeekerTraining.Remove(training);

		//		foreach (var trainingName in updated)
		//		{
		//			if (!current.Contains(trainingName))
		//			{
		//				seeker.SeekerTraining.Add(new SeekerTraining { TrainingName = trainingName });
		//			}
		//		}
		//	}



		//	// ===== Experiences =====
		//	if (dto.SeekerExperiences != null)
		//	{

		//		var currentExperiences = seeker.SeekerExperiences.ToList();


		//		var dtoIds = dto.SeekerExperiences
		//			.Where(e => e.Id != null)
		//			.Select(e => e.Id)
		//			.ToList();

		//		var toRemove = currentExperiences
		//			.Where(e => !dtoIds.Contains(e.Id))
		//			.ToList();

		//		foreach (var exp in toRemove)
		//			seeker.SeekerExperiences.Remove(exp);

		//		foreach (var expDto in dto.SeekerExperiences)
		//		{
		//			if (expDto.Id == null)
		//			{
		//				var newExp = _mapper.Map<SeekerExperience>(expDto);
		//				seeker.SeekerExperiences.Add(newExp);
		//			}
		//			else
		//			{
		//				var existingExp = currentExperiences.FirstOrDefault(e => e.Id == expDto.Id);
		//				if (existingExp != null)
		//				{
		//					_mapper.Map(expDto, existingExp);
		//				}
		//			}
		//		}
		//	}

		//	// ===== Educations =====
		//	if (dto.SeekerEducations != null)
		//	{
		//		var currentEducations = seeker.SeekerEducations.ToList();

		//		var dtoIds = dto.SeekerEducations
		//			.Where(e => e.Id != null)
		//			.Select(e => e.Id)
		//			.ToList();

		//		var toRemove = currentEducations
		//			.Where(e => !dtoIds.Contains(e.Id))
		//			.ToList();

		//		foreach (var edu in toRemove)
		//			seeker.SeekerEducations.Remove(edu);

		//		foreach (var eduDto in dto.SeekerEducations)
		//		{
		//			if (eduDto.Id == null)
		//			{
		//				var newEdu = _mapper.Map<SeekerEducation>(eduDto);
		//				seeker.SeekerEducations.Add(newEdu);
		//			}
		//			else
		//			{
		//				var existingEdu = currentEducations.FirstOrDefault(e => e.Id == eduDto.Id);
		//				if (existingEdu != null)
		//				{
		//					_mapper.Map(eduDto, existingEdu);
		//				}
		//			}
		//		}
		//	}


		//	// ===== Update phone =====
		//	if (!string.IsNullOrEmpty(dto.PhoneNumber))
		//		seeker.User.PhoneNumber = dto.PhoneNumber;

		//	// ===== Update user name =====
		//	if (!string.IsNullOrEmpty(dto.Name))
		//		seeker.User.UserName = dto.Name;


		//	await _context.SaveChangesAsync();
		//	return true;
		//}


		// =================== Handle File Upload ===================

		public async Task<bool> UpdateAsync(string userID, SeekerProfileUpdateDto dto)
		{
			var seeker = await _context.SeekerProfiles
				.Include(s => s.Skills)
				.Include(s => s.seekerInterests)
				.Include(s => s.seekerCertificates)
				.Include(s => s.SeekerTraining)
				.Include(s => s.SeekerEducations)
				.Include(s => s.SeekerExperiences)
				.Include(s => s.User)
				.FirstOrDefaultAsync(s => s.UserId == userID);

			if (seeker == null) return false;

			MapBasicInfo(dto, seeker);
			await UpdateSkills(dto.Skills, seeker);
			UpdateInterests(dto.Interests, seeker);
			UpdateCertificates(dto.Certificates, seeker);
			UpdateTrainings(dto.Trainings, seeker);
			UpdateExperiences(dto.SeekerExperiences, seeker);
			UpdateEducations(dto.SeekerEducations, seeker);

			await _context.SaveChangesAsync();
			return true;
		}

		private void MapBasicInfo(SeekerProfileUpdateDto dto, SeekerProfile seeker)
		{
			_mapper.Map(dto, seeker);

			if (!string.IsNullOrEmpty(dto.PhoneNumber))
				seeker.User.PhoneNumber = dto.PhoneNumber;

			if (!string.IsNullOrEmpty(dto.Name))
				seeker.User.UserName = dto.Name;
		}

		private async Task UpdateSkills(List<string>? skills, SeekerProfile seeker)
		{
			if (skills == null) return;

			var updatedSkills = skills.Distinct().ToList();
			seeker.Skills.Clear();

			var existingSkillsInDb = await _context.Skills
				.Where(s => updatedSkills.Contains(s.SkillName))
				.ToDictionaryAsync(s => s.SkillName, s => s);

			foreach (var skillName in updatedSkills)
			{
				if (existingSkillsInDb.ContainsKey(skillName))
					seeker.Skills.Add(existingSkillsInDb[skillName]);
				else
					seeker.Skills.Add(new Skill { SkillName = skillName });
			}
		}

		private void UpdateInterests(List<string>? interests, SeekerProfile seeker)
		{
			if (interests == null) return;

			var current = seeker.seekerInterests.Select(i => i.InterestName).ToList();
			var updated = interests.Distinct().ToList();

			var toRemove = seeker.seekerInterests.Where(i => !updated.Contains(i.InterestName)).ToList();
			foreach (var interest in toRemove)
				seeker.seekerInterests.Remove(interest);

			foreach (var interestName in updated)
			{
				if (!current.Contains(interestName))
					seeker.seekerInterests.Add(new SeekerInterest { InterestName = interestName });
			}
		}

		private void UpdateCertificates(List<string>? certificates, SeekerProfile seeker)
		{
			if (certificates == null) return;

			var current = seeker.seekerCertificates.Select(c => c.CertificateName).ToList();
			var updated = certificates.Distinct().ToList();

			var toRemove = seeker.seekerCertificates.Where(c => !updated.Contains(c.CertificateName)).ToList();
			foreach (var cert in toRemove)
				seeker.seekerCertificates.Remove(cert);

			foreach (var certName in updated)
			{
				if (!current.Contains(certName))
					seeker.seekerCertificates.Add(new SeekerCertificate { CertificateName = certName });
			}
		}

		private void UpdateTrainings(List<string>? trainings, SeekerProfile seeker)
		{
			if (trainings == null) return;

			var current = seeker.SeekerTraining.Select(t => t.TrainingName).ToList();
			var updated = trainings.Distinct().ToList();

			var toRemove = seeker.SeekerTraining.Where(t => !updated.Contains(t.TrainingName)).ToList();
			foreach (var training in toRemove)
				seeker.SeekerTraining.Remove(training);

			foreach (var trainingName in updated)
			{
				if (!current.Contains(trainingName))
					seeker.SeekerTraining.Add(new SeekerTraining { TrainingName = trainingName });
			}
		}

		private void UpdateExperiences(List<SeekerExperienceUpdateDto>? experiences, SeekerProfile seeker)
		{
			if (experiences == null) return;

			var currentExperiences = seeker.SeekerExperiences.ToList();
			var dtoIds = experiences.Where(e => e.Id != null).Select(e => e.Id).ToList();

			var toRemove = currentExperiences.Where(e => !dtoIds.Contains(e.Id)).ToList();
			foreach (var exp in toRemove)
				seeker.SeekerExperiences.Remove(exp);

			foreach (var expDto in experiences)
			{
				if (expDto.Id == null)
					seeker.SeekerExperiences.Add(_mapper.Map<SeekerExperience>(expDto));
				else
				{
					var existingExp = currentExperiences.FirstOrDefault(e => e.Id == expDto.Id);
					if (existingExp != null)
						_mapper.Map(expDto, existingExp);
				}
			}
		}

		private void UpdateEducations(List<SeekerEducationUpdateDto>? educations, SeekerProfile seeker)
		{
			if (educations == null) return;

			var currentEducations = seeker.SeekerEducations.ToList();
			var dtoIds = educations.Where(e => e.Id != null).Select(e => e.Id).ToList();

			var toRemove = currentEducations.Where(e => !dtoIds.Contains(e.Id)).ToList();
			foreach (var edu in toRemove)
				seeker.SeekerEducations.Remove(edu);

			foreach (var eduDto in educations)
			{
				if (eduDto.Id == null)
					seeker.SeekerEducations.Add(_mapper.Map<SeekerEducation>(eduDto));
				else
				{
					var existingEdu = currentEducations.FirstOrDefault(e => e.Id == eduDto.Id);
					if (existingEdu != null)
						_mapper.Map(eduDto, existingEdu);
				}
			}
		}

		// =================== Upload Files ===================
		public async Task<(string? CvUrl, string? ProfileImageUrl)> UploadFilesAsync(string userId, SeekerFileUploadDto dto)
		{
			var seeker = await _context.SeekerProfiles
				.FirstOrDefaultAsync(s => s.UserId == userId);
			if (seeker == null)
				return (null, null);

			// Handle CV upload/update 
			var newCvUrl = await FileUploadHelper.HandleFileUploadAsync(
				dto.CV_Url, seeker.CV_Url, "cv", _env, _configuration, dto.RemoveCV,null);

			// Handle Profile Image upload/removal with default image
			var defaultProfileImage = $"{_configuration["ApiBaseUrl"]}/images/profilepic/user.jpg";
			var newProfileImageUrl = await FileUploadHelper.HandleFileUploadAsync(
				dto.ProfileImageUrl, seeker.ProfileImageUrl, "images/profilepic",
				_env, _configuration, dto.RemoveProfileImage, defaultProfileImage);

			seeker.CV_Url = newCvUrl;
			seeker.ProfileImageUrl = newProfileImageUrl;

			await _context.SaveChangesAsync();

			return (newCvUrl, newProfileImageUrl);
		}

		// ================== DELETE ==================
		public async Task<bool> DeleteAsync(int Id)
		{
			var seeker = await _context.SeekerProfiles.FirstOrDefaultAsync(s => s.Id == Id);
			if (seeker == null) return false;

			// Delete profile image only if it is not the default
			var defaultProfileImage = $"{_configuration["ApiBaseUrl"]}/images/profilepic/user.jpg";
			if (!string.IsNullOrEmpty(seeker.ProfileImageUrl) && !FileUploadHelper.IsDefaultImage(seeker.ProfileImageUrl, defaultProfileImage))
			{
				DocumentSettings.DeleteFile(seeker.ProfileImageUrl, "images/profilepic", _env);
			}

			_context.SeekerProfiles.Remove(seeker);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
   
