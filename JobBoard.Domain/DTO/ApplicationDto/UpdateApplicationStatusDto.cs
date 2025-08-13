using JobBoard.Domain.Entities;
using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.ApplicationDto
{
    public class UpdateApplicationStatusDto
    {
		[Required]
		[EnumDataType(typeof(ApplicationStatus))]
		public ApplicationStatus Status { get; set; }
	}

}
