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
    
    public partial class UserBankInfo
    {
        public int BankId { get; set; }
        public int UserId { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BankUserName { get; set; }
        public string BankAddress { get; set; }
        public bool IsDefault { get; set; }
        public System.DateTime CreatedTime { get; set; }
        public System.DateTime LastUpdatedTime { get; set; }
    }
}
