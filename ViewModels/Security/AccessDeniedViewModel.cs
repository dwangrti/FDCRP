using System;

namespace ASJ.Models
{
    public class AccessDeniedViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}