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
    
    public partial class Activity
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public string ActivityType { get; set; }
        public string LocationId { get; set; }
        public string Adress { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string HoldBy { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Jingdu { get; set; }
        public string Weidu { get; set; }
        public string Description { get; set; }
        public string ContentStyle { get; set; }
        public string ContentText { get; set; }
        public string ImageUrl { get; set; }
        public string Link { get; set; }
        public Nullable<int> State { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedTime { get; set; }
        public Nullable<bool> IsFromMobile { get; set; }
    }
}
