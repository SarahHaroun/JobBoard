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
		public SortingOptions SortingOption { get; set; } = SortingOptions.DateDesc;
	}
}
