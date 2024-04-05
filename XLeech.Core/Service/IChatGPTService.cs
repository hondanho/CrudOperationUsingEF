using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLeech.Core.Service
{
    public interface IChatGPTService
    {
        Task<string> GetChatCompletionAsync(string question);
    }
}
