﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Witbird.SHTS.Model
{
    public partial class Demand
    {
        private List<DemandQuote> quoteEntities = new List<DemandQuote>();

        public string ResourceTypeName
        {
            get
            {
                var name = "不确定";

                switch (ResourceType)
                {
                    case 1:
                        name = "活动场地";
                        break;
                    case 2:
                        name = "演艺人员";
                        break;
                    case 3:
                        name = "活动设备";
                        break;
                    case 4:
                        name = "其他资源";
                        break;
                    default:
                        break;
                }

                return name;
            }
        }

        public Int64 RowNumber { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// Gets the demand status dispaly value
        /// </summary>
        public string StatusValueString
        {
            get
            {
                if (!Status.HasValue || Status.Value == (int)DemandStatus.InProgress)
                {
                    return "寻找中";
                }
                else if (Status.Value == (int)DemandStatus.Complete)
                {
                    return "寻找完成";
                }

                return "正在进行";
            }
        }

        public bool InProgress
        {
            get
            {
                return !Status.HasValue || Status.Value == (int)DemandStatus.InProgress;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return Status.HasValue && Status.Value == (int)DemandStatus.Complete;
            }
        }

        /// <summary>
        /// 当前需求所有报价记录
        /// </summary>
        public List<DemandQuote> QuoteEntities
        {
            get { return this.quoteEntities; }
        }

        /// <summary>
        /// 是否处理了该需求的所有报价，如果为false，提示该需求具有为处理的报价。
        /// </summary>
        public bool IsAllQuoteHandled
        {
            get
            {
                return this.quoteEntities.Any(x => x != null && !x.HandleStatus);
            }
        }

        /// <summary>
        /// 所有报价总数
        /// </summary>
        public int TotalQuoteCount
        {
            get
            {
                return this.quoteEntities.Count;
            }
        }

        /// <summary>
        /// 未处理报价总数
        /// </summary>
        public int TotalNotHandledQuoteCount
        {
            get
            {
                return this.quoteEntities.Count(x => x != null && !x.HandleStatus);
            }
        }

        /// <summary>
        /// 是否已发放需求奖励金
        /// </summary>
        public bool IsGotDemandBonus
        {
            get
            {
                return DemandBonus > 0;
            }
        }

        /// <summary>
        /// 需求鼓励金
        /// </summary>
        public decimal DemandBonus
        {
            get;
            set;
        }
    }
}
