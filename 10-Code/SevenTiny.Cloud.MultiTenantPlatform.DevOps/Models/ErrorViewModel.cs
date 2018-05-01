using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.DevOps.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}