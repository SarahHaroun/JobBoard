using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JobBoard.Domain.DTO.AIEmbeddingDto
{
    public class SemanticSearchRequestDto
    {
        public string Query { get; set; }
        public int TopK { get; set; } = 5;
    }
}
