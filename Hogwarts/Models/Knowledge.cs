using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hogwarts.Models
{
    public class KnowledgeCategory
    {
        public int Id { get; set; }

        public string Role { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }

    public class Knowledge
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        [DataType(DataType.Html)]
        public string Content { get; set; }
        public bool IsPublic { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }

    public class KnowledgeViewModel
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public int CategoryId { get; set; }

        public string Title { get; set; }
        [AllowHtml]
        [DataType(DataType.Html)]
        public string Content { get; set; }

        public bool IsPublic { get; set; }

        public string CreateUserId { get; set; }

        public string UserName { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }
    }
}