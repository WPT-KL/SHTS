﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Witbird.SHTS.Model;
using Witbird.SHTS.Web.Models;

namespace Witbird.SHTS.Web.Areas.Wechat.Models
{
    public class WeChatResourceViewModel : ResourceViewModel
    {
        public WeChatUser CurrentWechatUser { get; set; }
        public User CurrentUser { get; set; }
    }
}