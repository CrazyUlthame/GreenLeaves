using System.Collections.Generic;

namespace WebAPI.Models.Response
{
    public class STRResponseList
    {
        public bool IsError { get; set; }
        public string Message { get; set; }
        public List<object> List { get; set; }
    }
}
