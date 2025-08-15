using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.AIEmbeddingDto
{
    public class AskQuestionDto
    {

        /* receive the user Id */
        public string UserId { get; set; } = string.Empty;
        /* receive the user Qusetion*/
        public string Question { get; set; }
    }
}
