using System.Collections.Generic;

namespace Services.Contracts.Response
{
    public abstract class ResponseBase
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
