using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hogwarts.Models
{
    public class Record
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public int LectureId { get; set; }

        public string UserId { get; set; }

        public int Point { get; set; }
    }
}