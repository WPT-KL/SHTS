﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Witbird.SHTS.Model;

namespace Witbird.SHTS.Web.Areas.Admin.Models
{
    public class FinanceModel : BaseModel
    {
        public List<FinanceWithdrawRecord> WithdrawRecords { get; set; }
    }
}