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
    
    public partial class FinanceBalance
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal FrozenBalance { get; set; }
        public System.DateTime InsertedTimestamp { get; set; }
        public System.DateTime LastUpdatedTimestamp { get; set; }
    }
}
