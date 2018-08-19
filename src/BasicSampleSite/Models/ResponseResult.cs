using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicSampleSite.Models
{
    public class ResponseResult
    {
        public Person[] Results { get; set; }
        public Info Info { get; set; }
    }

    public class Info
    {
        public string Seed { get; set; }
        public int Results { get; set; }
        public int Page { get; set; }
        public string Version { get; set; }
    }
}
