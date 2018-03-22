using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hogwarts.Utility
{
    public class AjaxResult
    {
        public Boolean Result { get; set; }

        public string Message { get; set; }

        public dynamic ResultData { get; set; }

        public AjaxResult()
        {
            this.Result = false;
        }

        public JsonResult GetJsonRsult()
        {
            var json = new JsonResult();
            json.Data = new { Result = this.Result, Message = this.Message, ResultData = this.ResultData };
            return json;
        }
    }
}