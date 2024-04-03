using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLeech.Core.Model
{
    public class CrawlerResult
    {
        public bool IsSavePost { get; set; }
        public bool IsSaveCategory { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }
    }
}
