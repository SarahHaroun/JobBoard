using JobBoard.Domain.Shared.SortingOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Shared
{
	public class SavedJobFilterParams
	{
		public string? SearchValue { get; set; }
		public SortingDateOptions SortingOption { get; set; } = SortingDateOptions.DateDesc;
		public int? SeekerId { get; set; }
	}
}
