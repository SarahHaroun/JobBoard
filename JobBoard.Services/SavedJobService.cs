using AutoMapper;
using JobBoard.Domain.DTO.SavedJobsDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Repositories.Contract;
using JobBoard.Domain.Services.Contract;
using JobBoard.Domain.Shared;
using JobBoard.Repositories.Specifications;

namespace JobBoard.Services
{
	public class SavedJobService : ISavedJobService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public SavedJobService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<IEnumerable<SavedJobDto>> GetSavedJobsAsync(SavedJobFilterParams filterParams)
		{
			var spec = new SavedJobWithFilterSpecification(filterParams);
			var savedJob = await _unitOfWork.Repository<SavedJob>().GetAllAsync(spec);
			var mappedSavedJobs = _mapper.Map<IEnumerable<SavedJobDto>>(savedJob);
			return mappedSavedJobs;

		}
		public async Task<SavedJobDto> GetSavedJobByIdAsync(int id)
		{
			var spec = new SavedJobWithFilterSpecification(id);
			var savedJob = await _unitOfWork.Repository<SavedJob>().GetByIdAsync(spec);

			if (savedJob == null)
				return null;

			var mappedSavedJob = _mapper.Map<SavedJobDto>(savedJob);
			return mappedSavedJob;
		}

		public async Task<SavedJobDto> SaveJobAsync(int seekerId, CreateSavedJobDto savedJobDto)
		{
			var alreadySaved = await _unitOfWork.Repository<SavedJob>()
				.AnyAsync(s => s.SeekerId == seekerId && s.JobId == savedJobDto.JobId);
			if (alreadySaved)
				return null;

			var jobExists = await _unitOfWork.Repository<Job>()
				.AnyAsync(j => j.Id == savedJobDto.JobId);
			
			if (!jobExists)
				return null;

			var savedJob = _mapper.Map<SavedJob>(savedJobDto);
			savedJob.SeekerId = seekerId;

			await _unitOfWork.Repository<SavedJob>().AddAsync(savedJob);
			await _unitOfWork.CompleteAsync();

			var spec = new SavedJobWithFilterSpecification(savedJob.Id);
			savedJob = await _unitOfWork.Repository<SavedJob>().GetByIdAsync(spec);
			var mappedSavedJob = _mapper.Map<SavedJobDto>(savedJob);
			return mappedSavedJob;
		}

		public async Task<bool> IsJobSavedAsync(int seekerId, int jobId)
		{
			var isSaved = await _unitOfWork.Repository<SavedJob>()
				.AnyAsync(s => s.SeekerId == seekerId && s.JobId == jobId);
			return isSaved;
		}

		public async Task<bool> UnsaveJobAsync(int seekerId, int jobId)
		{
			var savedJob = await _unitOfWork.Repository<SavedJob>()
		   .FindAsync(s => s.SeekerId == seekerId && s.JobId == jobId);

			if (savedJob == null)
				return false;

			_unitOfWork.Repository<SavedJob>().Delete(savedJob);
			await _unitOfWork.CompleteAsync();

			return true;
		}
	}
}
