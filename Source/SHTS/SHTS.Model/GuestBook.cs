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
    
    public partial class GuestBook
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedTime { get; set; }
        public Nullable<int> State { get; set; }
    }
}
