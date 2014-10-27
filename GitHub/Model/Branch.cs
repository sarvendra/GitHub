﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.Model
{
    public class Commit
    {
        public string sha { get; set; }
        public string url { get; set; }
    }

    public class Branch
    {
        public string name { get; set; }
        public Commit commit { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
        public string git { get; set; }
        public string html { get; set; }
    }

    public class BranchContent
    {
        public string name { get; set; }
        public string path { get; set; }
        public string sha { get; set; }
        public int size { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string git_url { get; set; }
        public string type { get; set; }
        public Links _links { get; set; }
    }

}
