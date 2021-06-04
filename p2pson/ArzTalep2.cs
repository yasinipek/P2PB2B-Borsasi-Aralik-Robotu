using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2pson
{
    public class Result1
    {
        public List<List<string>> asks { get; set; }
        public List<List<string>> bids { get; set; }
    }

    public class ArzTalep2
    {
        public bool success { get; set; }
        public string errorCode { get; set; }
        public string message { get; set; }
        public Result1 result { get; set; }
        public double cache_time { get; set; }
        public double current_time { get; set; }
    }
}