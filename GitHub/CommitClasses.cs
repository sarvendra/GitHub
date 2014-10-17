using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub
{
    public class Author
    {
        public string name { get; set; }
        public string email { get; set; }
        public string date { get; set; }
    }

    public class Committer
    {
        public string name { get; set; }
        public string email { get; set; }
        public string date { get; set; }
    }

    public class Tree
    {
        public string sha { get; set; }
        public string url { get; set; }
    }

    public class CommitFull
    {
        public Author author { get; set; }
        public Committer committer { get; set; }
        public string message { get; set; }
        public Tree tree { get; set; }
        public string url { get; set; }
        public int comment_count { get; set; }
    }

    public class Parent
    {
        public string sha { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
    }

    public class CommitRoot
    {
        public string sha { get; set; }
        public CommitFull commit { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string comments_url { get; set; }
        public User author { get; set; }
        public User committer { get; set; }
        public List<Parent> parents { get; set; }
    }
}
