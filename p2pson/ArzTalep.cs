namespace p2pson
{
        public class Result
        {
            public double bid { get; set; }
            public double ask { get; set; }
            public double open { get; set; }
            public double high { get; set; }
            public double low { get; set; }
            public double last { get; set; }
            public double volume { get; set; }
            public double deal { get; set; }
            public double change { get; set; }
        }

        public class ArzTalep
        {
            public bool success { get; set; }
            public string errorCode { get; set; }
            public string message { get; set; }
            public Result result { get; set; }
            public double cache_time { get; set; }
            public double current_time { get; set; }
        }

}
