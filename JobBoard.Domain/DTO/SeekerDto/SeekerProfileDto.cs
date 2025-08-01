﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.SeekerDto
{
    public class SeekerProfileDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Address { get; set; }
        public string CV_Url { get; set; }

        public string? UserId { get; set; }
    }
}
