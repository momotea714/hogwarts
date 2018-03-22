using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hogwarts.Models
{
    public class KnowledgeCategory
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public string Categoryname { get; set; }
    }

    public class Knowledge
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public string IncludeCategoryName { get; set; }

        public string Content { get; set; }

        public bool IsPublic { get; set; }

        public string CreateUserId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }
    }
}