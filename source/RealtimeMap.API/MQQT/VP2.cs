using System;

public class VP
    {
        public string desi { get; set; }
        public string dir { get; set; }
        public int? oper { get; set; }
        public int? veh { get; set; }
        public DateTime? tst { get; set; }
        public int? tsi { get; set; }
        public double? spd { get; set; }
        public int? hdg { get; set; }
        public double? lat { get; set; }
        public double? @long { get; set; }
        public double? acc { get; set; }
        public int? dl { get; set; }
        public int? odo { get; set; }
        public int? drst { get; set; }
        public string oday { get; set; }
        public int? jrn { get; set; }
        public int? line { get; set; }
        public string start { get; set; }
        public string loc { get; set; }
        public object stop { get; set; }
        public string route { get; set; }
        public int? occu { get; set; }
    }

    public class Root2
    {
        public VP VP { get; set; }
    }