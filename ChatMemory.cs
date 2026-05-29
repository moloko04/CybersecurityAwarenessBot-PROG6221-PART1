using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ChatMemory.cs - Stores conversation data for the Cybersecurity Bot.
// Used for remembering user name and tracking last topic discussed.

namespace CybersecurityBot.GUI
{
    internal class ChatMemory
    {
        public Dictionary<string, string> UserMemory = new Dictionary<string, string>();
        public string LastTopic { get; set; } 
    }
}
