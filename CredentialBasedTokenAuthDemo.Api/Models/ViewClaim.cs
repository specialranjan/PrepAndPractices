using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredentialBasedTokenAuthDemo.Api.Models
{
    public class ViewClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}