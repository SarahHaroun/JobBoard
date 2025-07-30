using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.DTO.JobDto;

namespace JobBoard.Domain.DTO.AIEmbeddingDto
{
    public class SemanticSearchResultDto
    {
        public JobBoard.Domain.DTO.JobDto.JobDto Job { get; set; }
        public double Similarity { get; set; }
    }
}
