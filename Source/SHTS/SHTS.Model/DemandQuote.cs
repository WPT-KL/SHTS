//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Witbird.SHTS.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class DemandQuote
    {
        public int QuoteId { get; set; }
        public int WeChatUserId { get; set; }
        public int DemandId { get; set; }
        public string ContactName { get; set; }
        public string ContactPhoneNumber { get; set; }
        public decimal QuotePrice { get; set; }
        public bool HandleStatus { get; set; }
        public string AcceptStatus { get; set; }
        public System.DateTime InsertedTimestamp { get; set; }
        public Nullable<System.DateTime> LastUpdatedTimestamp { get; set; }
        public bool IsActive { get; set; }
    }
}