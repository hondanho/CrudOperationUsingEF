using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLeech.Core.Service
{
    interface ICrawlerService
    {
        Task CrawlerAsync();
    }
}
