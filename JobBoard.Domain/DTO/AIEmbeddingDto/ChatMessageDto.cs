using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.AIEmbeddingDto
{
    public class ChatMessageDto
    {
        public string Role { get; set; } = ""; // "user" / "assistant" / "system"
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
