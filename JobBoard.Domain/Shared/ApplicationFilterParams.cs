using JobBoard.Domain.Entities;
using JobBoard.Domain.Entities.Enums;
using JobBoard.Domain.Shared.SortingOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Shared
{
    public class ApplicationFilterParams
    {
		public int? JobId { get; set; }
		public int? ApplicantId { get; set; }
		public ApplicationStatus? Status { get; set; }
		public DateTime? AppliedDateFrom { get; set; }
		public DateTime? AppliedDateTo { get; set; }
		public string? SearchValue { get; set; }
		public SortingDateOptions SortingOption { get; set; } = SortingDateOptions.DateDesc;
	}
}
