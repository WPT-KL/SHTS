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
    
    public partial class WeChatUser
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public Nullable<int> Sex { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Photo { get; set; }
        public string AccessToken { get; set; }
        public Nullable<bool> AccessTokenExpired { get; set; }
        public Nullable<System.DateTime> AccessTokenExpireTime { get; set; }
        public Nullable<bool> HasSubscribed { get; set; }
        public Nullable<bool> HasAuthorized { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public Nullable<System.DateTime> LastRequestTimestamp { get; set; }
    }
}
