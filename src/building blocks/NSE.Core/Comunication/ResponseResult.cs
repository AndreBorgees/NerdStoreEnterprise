using System.Collections.Generic;

namespace NSE.Core.Comunication
{
    public class ResponseResult
    {
        public ResponseResult()
        {
            Errors = new ResponseErrorMessage();
        }
        public string Title { get; set; }
        public int Status { get; set; }
        public ResponseErrorMessage Errors { get; set; }
    }

    public class ResponseErrorMessage
    {
        public ResponseErrorMessage()
        {
            Messages = new List<string>();
        }
        public List<string> Messages { get; set; }
    }
}
