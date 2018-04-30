using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hogwarts.Models
{
    public class Ask
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public int ParentAskId { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        public string Content { get; set; }

        public string CreateUserId { get; set; }

        public bool IsClosed { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }
    }

    public class AskViewModel
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public bool IsMyAsk { get; set; }

        public int ParentAskId { get; set; }

        public int AnswerCount { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        public string Content { get; set; }

        public string CreateUserId { get; set; }

        public string CreateUserName { get; set; }

        public bool IsClosed { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }
    }
}