using AutoMapper;
using JobBoard.Domain.DTO.SeekerDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Shared;
using JobBoard.Repositories.Persistence;
using Microsoft.AspNetCore.Hosting;
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

		public SeekerService(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
			_env = env;
			_configuration = configuration;
		}

        // ================== GET ==================
        public async Task<SeekerProfileDto?> GetByUserIdAsync(string userId)
        {
            var seeker = await _context.SeekerProfiles
                .Include(s => s.Skills)
                .Include(s => s.seekerInterests)
                .Include(s => s.seekerCertificates)
                .Include(s => s.SeekerTraining)
                .Include(s => s.SeekerEducations)
                .Include(s => s.SeekerExperiences)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (seeker == null) return null;

            return _mapper.Map<SeekerProfileDto>(seeker);
        }

        public async Task<List<SeekerProfileDto>> GetAllAsync()
        {
            var seekers = await _context.SeekerProfiles
                .Include(s => s.Skills)
                .Include(s => s.seekerInterests)
                .Include(s => s.seekerCertificates)
                .Include(s => s.SeekerTraining)
                .Include(s => s.SeekerEducations)
                .Include(s => s.SeekerExperiences)
                .Include(s => s.User)
                .ToListAsync();

            return _mapper.Map<List<SeekerProfileDto>>(seekers);
        }

        // ================== UPDATE ==================
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

			if (dto.CV_Url != null && dto.CV_Url.Length > 0)
			{
				// Delete old file
				DocumentSettings.DeleteFile(seeker.CV_Url, "cv", _env);

				// Upload New file
				seeker.CV_Url = await DocumentSettings.UploadFileAsync(dto.CV_Url, "cv", _env, _configuration);
			}


			// Map basic props (excluding collections)
			_mapper.Map(dto, seeker);

            // ===== Skills =====
            if (dto.Skills != null)
            {
                var updatedSkills = dto.Skills.Distinct().ToList();

                // Clear all current skills first
                seeker.Skills.Clear();

                // Get all existing skills from database
                var existingSkillsInDb = await _context.Skills
                    .Where(s => updatedSkills.Contains(s.SkillName))
                    .ToDictionaryAsync(s => s.SkillName, s => s);

                // Add skills (existing or new)
                foreach (var skillName in updatedSkills)
                {
                    if (existingSkillsInDb.ContainsKey(skillName))
                    {
                        // Use existing skill
                        seeker.Skills.Add(existingSkillsInDb[skillName]);
                    }
                    else
                    {
                        // Create new skill
                        var newSkill = new Skill { SkillName = skillName };
                        seeker.Skills.Add(newSkill);
                    }
                }
            }

            // ===== Interests =====
            if (dto.Interests != null)
            {
                var current = seeker.seekerInterests.Select(i => i.InterestName).ToList();
                var updated = dto.Interests.Distinct().ToList();

                var toRemove = seeker.seekerInterests.Where(i => !updated.Contains(i.InterestName)).ToList();
                foreach (var interest in toRemove)
                    seeker.seekerInterests.Remove(interest);

                foreach (var interestName in updated)
                {
                    if (!current.Contains(interestName))
                    {
                        seeker.seekerInterests.Add(new SeekerInterest
                        {
                            InterestName = interestName,
                            SeekerProfileId = seeker.Id
                        });
                    }
                }
            }

            // ===== Certificates =====
            if (dto.Certificates != null)
            {
                var current = seeker.seekerCertificates.Select(c => c.CertificateName).ToList();
                var updated = dto.Certificates.Distinct().ToList();

                var toRemove = seeker.seekerCertificates.Where(c => !updated.Contains(c.CertificateName)).ToList();
                foreach (var cert in toRemove)
                    seeker.seekerCertificates.Remove(cert);

                foreach (var certName in updated)
                {
                    if (!current.Contains(certName))
                    {
                        seeker.seekerCertificates.Add(new SeekerCertificate { CertificateName = certName });
                    }
                }
            }

            // ===== Trainings =====
            if (dto.Trainings != null)
            {
                var current = seeker.SeekerTraining.Select(t => t.TrainingName).ToList();
                var updated = dto.Trainings.Distinct().ToList();

                var toRemove = seeker.SeekerTraining.Where(t => !updated.Contains(t.TrainingName)).ToList();
                foreach (var training in toRemove)
                    seeker.SeekerTraining.Remove(training);

                foreach (var trainingName in updated)
                {
                    if (!current.Contains(trainingName))
                    {
                        seeker.SeekerTraining.Add(new SeekerTraining { TrainingName = trainingName });
                    }
                }
            }

            // ===== Educations =====
            if (dto.SeekerEducations != null)
            {
                seeker.SeekerEducations.Clear();
                foreach (var edu in dto.SeekerEducations)
                {
                    seeker.SeekerEducations.Add(_mapper.Map<SeekerEducation>(edu));
                }
            }

            // ===== Experiences =====
            if (dto.SeekerExperiences != null)
            {
                seeker.SeekerExperiences.Clear();
                foreach (var exp in dto.SeekerExperiences)
                {
                    seeker.SeekerExperiences.Add(_mapper.Map<SeekerExperience>(exp));
                }
            }

            // ===== Update phone =====
            if (!string.IsNullOrEmpty(dto.PhoneNumber))
                seeker.User.PhoneNumber = dto.PhoneNumber;

            await _context.SaveChangesAsync();
            return true;
        }

        // ================== DELETE ==================
        public async Task<bool> DeleteAsync(int Id)
        {
            var seeker = await _context.SeekerProfiles.FirstOrDefaultAsync(s => s.Id == Id);
            if (seeker == null) return false;

            _context.SeekerProfiles.Remove(seeker);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
