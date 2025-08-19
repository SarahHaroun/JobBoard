using JobBoard.Domain.Shared.SortingOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Shared
{
    public class EmployerJobFilterParams
    {
		public EmployerJobStatus? Status { get; set; }
		public EmployerJobSortingOptions SortingOption { get; set; } = EmployerJobSortingOptions.PostedDateDesc;
		public string? SearchValue { get; set; }
	}

	public enum EmployerJobStatus
	{
		Active, Filled, Expired
	}
}
