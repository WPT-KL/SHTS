﻿using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Witbird.SHTS.BLL.Managers;
using Witbird.SHTS.BLL.Services;
using Witbird.SHTS.Common;
using Witbird.SHTS.Model;
using Witbird.SHTS.Web.Areas.Wechat.Models;
using Witbird.SHTS.Web.Public;
using WitBird.Common;

namespace Witbird.SHTS.Web.Areas.Wechat.Controllers
{
    public class DemandController : WechatBaseController
    {
        private static TenPayV3Info _tenPayV3Info;

        /// <summary>
        /// 微信支付相关配置信息
        /// </summary>
        public static TenPayV3Info TenPayV3Info
        {
            get
            {
                if (_tenPayV3Info == null)
                {
                    _tenPayV3Info =
                        TenPayV3InfoCollection.Data[System.Configuration.ConfigurationManager.AppSettings["TenPayV3_MchId"]];
                }
                return _tenPayV3Info;
            }
        }

        DemandManager demandManager = new DemandManager();
        DemandService demandService = new DemandService();
        OrderService orderService = new OrderService();
        CityService cityService = new CityService();
        DemandQuoteService quoteService = new DemandQuoteService();

        public ActionResult Index(string page, string LastResourceType, string ResourceType, string ResourceTypeId, string area, string starttime, string endtime, string startbudget, string endbudget)
        {
            DemandModel model = new DemandModel();

            string city = string.Empty;
            if (!string.IsNullOrEmpty(CurrentCityId))
            {
                city = CurrentCityId;
            }
            int tempPage = 1;
            if (!string.IsNullOrEmpty(page))
            {
                Int32.TryParse(page, out tempPage);
            }

            // 每次更换需求列别需要重置需求类型选中的值
            if (string.IsNullOrEmpty(ResourceType))
            {
                ResourceTypeId = string.Empty;
            }
            if (!(LastResourceType ?? string.Empty).Equals(ResourceType, StringComparison.CurrentCultureIgnoreCase))
            {
                ResourceTypeId = string.Empty;
            }
            LastResourceType = ResourceType;

            //-------------------------------初始化页面参数（不含分页）-----------------------
            model.PageIndex = tempPage;
            model.LastResourceType = LastResourceType ?? string.Empty;
            model.ResourceType = ResourceType ?? string.Empty;
            model.ResourceTypeId = ResourceTypeId ?? string.Empty;
            model.City = city;
            model.Area = area ?? string.Empty;
            model.StartBudget = startbudget ?? string.Empty;
            model.EndBudget = endbudget ?? string.Empty;
            model.StartTime = starttime ?? string.Empty;
            model.EndTime = endtime ?? string.Empty;


            //-------------------------------初始化SQL查询参数-----------------------
            DemandParameters parameters = new DemandParameters();
            parameters.PageCount = 10;//每页显示10条 (与下面保持一致)
            parameters.PageIndex = tempPage;
            parameters.ResourceType = model.ResourceType;
            parameters.ResourceTypeId = model.ResourceTypeId;
            parameters.City = model.City;
            parameters.Area = model.Area;
            parameters.StartBudget = model.StartBudget;
            parameters.EndBudget = model.EndBudget;
            parameters.StartTime = model.StartTime;
            parameters.EndTime = model.EndTime;
            
            if (!string.IsNullOrEmpty(city))
            {
                model.Areas = cityService.GetAreasByCityId(city, true);
            }


            int allCount = 0;
            //需求分页列表,每页10条
            model.Demands = demandService.GetDemandsByParameters(parameters, out allCount);
            //分页
            if (model.Demands != null && model.Demands.Count > 0)
            {
                model.PageIndex = tempPage;//当前页数
                model.PageSize = 10;//每页显示多少条
                model.PageStep = 5;//每页显示多少页码
                model.AllCount = allCount;//总条数
                if (model.AllCount % model.PageSize == 0)
                {
                    model.PageCount = model.AllCount / model.PageSize;
                }
                else
                {
                    model.PageCount = model.AllCount / model.PageSize + 1;
                }
            }

            return View(model);
        }

        public ActionResult Show(string id)
        {
            DemandModel model = new DemandModel();

            int tempId = 0;
            if (!string.IsNullOrEmpty(id))
            {
                Int32.TryParse(id, out tempId);
            }

            if (tempId != 0)
            {
                bool hasWeChatUserBoughtForDemand = false;
                var demand = demandService.GetDemandById(tempId);

                if (demand != null)
                {
                    demand.ContentText = FilterHelper.Filter(FilterLevel.PhoneAndEmail, demand.ContentText, CommonService.ReplacementForContactInfo);
                    demand.ContentStyle = FilterHelper.Filter(FilterLevel.PhoneAndEmail, demand.ContentStyle, CommonService.ReplacementForContactInfo);
                    demand.Description = FilterHelper.Filter(FilterLevel.PhoneAndEmail, demand.Description, CommonService.ReplacementForContactInfo);
                    demand.Title = FilterHelper.Filter(FilterLevel.PhoneAndEmail, demand.Title, CommonService.ReplacementForContactInfo);

                    // 如果是自己发布的需求，无需购买即可查看
                    var isPostedByMyself = (demand.UserId == CurrentWeChatUser.UserId);
                    hasWeChatUserBoughtForDemand = isPostedByMyself || demandService.HasWeChatUserBoughtForDemand(CurrentWeChatUser.OpenId, demand.Id);

                    // 查询报价记录
                    if (isPostedByMyself)
                    {
                        var quotes = quoteService.GetAllDemandQuotesForOneDemand(demand.Id);
                        if (quotes.HasItem())
                        {
                            demand.QuoteEntities.AddRange(quotes);
                        }
                    }
                    
                    if (!hasWeChatUserBoughtForDemand)
                    {
                        demand.Phone = "未购买查看权限";
                        demand.QQWeixin = "未购买查看权限";
                        demand.Email = "未购买查看权限";
                        demand.Address = "未购买查看权限";
                    }
                }

                model.HasCurrentWeChatUserBought = hasWeChatUserBoughtForDemand;
                model.Demand = demand;
            }

            if (model.Demand == null)
            {
                model.Demand = new Demand();
                model.Demand.Title = "该需求不存在或已被删除";
                model.Demand.StartTime = DateTime.Now;
                model.Demand.EndTime = DateTime.Now;
            }

            ViewData["BuyDemandFee"] = BuyDemandFee.ToString();

            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            if (!CurrentWeChatUser.IsUserLoggedIn)
            {
                return Redirect("/wechat/account/login");
            }
            DemandModel model = new DemandModel();
            model.Provinces = cityService.GetProvinces(true);
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(int ResourceType, int? SpaceTypeId, int? ActorTypeId, int? EquipTypeId, int? OtherTypeId,
            string title, string contentText,
            string province, string city, string area, string address, string phone, string qqweixin, string email,
            string startTime, string endTime, string peopleNumber, string budget)
        {
            string result = string.Empty;
            if (!CurrentWeChatUser.IsUserLoggedIn)
            {
                result = "未登录或登录超时！请在新窗口进行登录后返回继续操作！";
            }
            else
            {
                int resourceSubTypeId = -1;
                switch (ResourceType)
                {
                    case 1:
                        resourceSubTypeId = SpaceTypeId ?? -1;
                        break;
                    case 2:
                        resourceSubTypeId = ActorTypeId ?? -1;
                        break;
                    case 3:
                        resourceSubTypeId = EquipTypeId ?? -1;
                        break;
                    case 4:
                        resourceSubTypeId = OtherTypeId ?? -1;
                        break;
                    default:
                        break;
                }

                int demandBudget = 0;
                int.TryParse(budget, out demandBudget);
                int demandId = -1;

                if (demandManager.AddDemand(CurrentUser.UserId, ResourceType, resourceSubTypeId, title, contentText, province,
                    city, area, address, phone, qqweixin, email, startTime, endTime, string.Empty, peopleNumber,
                    demandBudget, (int)BuyDemandFee, out result, out demandId))
                {
                    Subscription.WorkingThread.Instance.SendDemandByEmail(demandId);
                }
            }

            return Content(result);
        }

        /// <summary>
        /// 购买需求查看权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Buy(int id)
        {
            bool isSuccessFul = false;
            string message = "";

            string appId = string.Empty;
            string timeStamp = string.Empty;
            string nonceStr = string.Empty;
            string package = string.Empty;
            string paySign = string.Empty;

            try
            {
                Demand demand = demandService.GetDemandById(id);

                if (demand == null)
                {
                    message = "该需求不存在或已被删除，请刷新页面后重试。";
                }
                else
                {
                    bool hasWeChatUserBoughtForDemand = demandService.HasWeChatUserBoughtForDemand(CurrentWeChatUser.OpenId, id);

                    if (hasWeChatUserBoughtForDemand)
                    {
                        message = "您已购买该需求联系方式及详细地址的永久查看权限，无需重复购买。请刷新页面后查看。";
                    }
                    else
                    {
                        isSuccessFul = orderService.DeleteOrderByOpenIdAndDemandIdForWeChatClient(CurrentWeChatUser.OpenId, id);

                        if (isSuccessFul)
                        {
                            string orderId = orderService.GenerateNewOrderNumber();
                            string subject = string.IsNullOrWhiteSpace(demand.Title) ? "微信用户购买需求联系方式永久权限" : demand.Title;
                            string body = "微信用户" + CurrentWeChatUser.NickName + "购买需求（" + demand.Title + "）联系方式的永久查看权限";
                            string username = CurrentWeChatUser.OpenId;
                            
                            string resourceUrl = "http://" + StaticUtility.Config.Domain + "/wechat/demand/show/" + id;

                            TradeOrder order = orderService.AddNewOrder(
                                orderId, subject, body, demand.WeixinBuyFee ?? BuyDemandFee, OrderState.New, username, resourceUrl, OrderType.WeChatDemand, id);

                            if (order != null)
                            {
                                isSuccessFul = PreparePaySign(order, out appId, out timeStamp, out nonceStr, out package, out paySign, out message);
                            }
                            else
                            {
                                message = "支付订单创建失败，请刷新页面后重新尝试。";
                                isSuccessFul = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.LogWexin("购买需求联系方式", ex.ToString());
                isSuccessFul = false;
                message = "购买失败，请稍后再试！";
            }

            var jsonResult = new
            {
                IsSuccessFul = isSuccessFul,
                Message = message,
                AppId = appId,
                TimeStamp = timeStamp,
                NonceStr = nonceStr,
                Package = package,
                PaySign = paySign
            };

            LogService.LogWexin("购买需求联系方式", "isSuccessFul: " + isSuccessFul.ToString() + "  message:"
                + message + "  appId: " + appId + "  timeStamp:" + timeStamp + "  nonceStr:" + nonceStr + "  package:" + package + "  paySign:" + paySign);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 我的购买记录
        /// </summary>
        /// <returns></returns>
        public ActionResult MyPaidDemand(int page = 1)
        {
            DemandModel model = new DemandModel();

            model.PageIndex = page;//当前页数
            model.PageSize = 10;//每页显示多少条
            model.PageStep = 5;//每页显示多少页码
            model.ActionName = "MyPaidDemand";

            int allCount = 0;
            model.PaidDemandOrders = orderService.GetWeChatUserPaidDemands(CurrentWeChatUser.OpenId, model.PageSize, model.PageIndex, out allCount);

            //分页
            if (model.PaidDemandOrders != null && model.PaidDemandOrders.Count > 0)
            {
                model.AllCount = allCount;//总条数
                if (model.AllCount % model.PageSize == 0)
                {
                    model.PageCount = model.AllCount / model.PageSize;
                }
                else
                {
                    model.PageCount = model.AllCount / model.PageSize + 1;
                }
            }

            return View(model);
        }

        /// <summary>
        /// 我发布的需求记录
        /// </summary>
        /// <returns></returns>
        public ActionResult MyDemands(string page)
        {
            if (!CurrentWeChatUser.IsUserLoggedIn)
            {
                return Redirect("/wechat/account/login");
            }

            DemandModel model = new DemandModel();
            model.ActionName = "MyDemands";

            //页码，总数重置
            int pageIndex = 1;
            if (!string.IsNullOrEmpty(page))
            {
                Int32.TryParse(page, out pageIndex);
            }
            int allCount = 0;
            model.Demands = demandService.GetDemandsByUserId(CurrentWeChatUser.UserId.Value, 20, pageIndex, out allCount);//每页显示20条
            //分页
            if (model.Demands != null && model.Demands.Count > 0)
            {
                model.PageIndex = pageIndex;//当前页数
                model.PageSize = 20;//每页显示多少条
                model.PageStep = 10;//每页显示多少页码
                model.AllCount = allCount;//总条数
                if (model.AllCount % model.PageSize == 0)
                {
                    model.PageCount = model.AllCount / model.PageSize;
                }
                else
                {
                    model.PageCount = model.AllCount / model.PageSize + 1;
                }
            }

            return View(model);
        }

        public ActionResult UpdateDemandStatusAsComplete(int id)
        {
            if (id > -1)
            {
                demandService.UpdateDemandStatus(id, DemandStatus.Complete);
            }

            return Redirect(Request.UrlReferrer.OriginalString);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!CurrentWeChatUser.IsUserLoggedIn)
            {
                return Redirect("/wechat/account/login");
            }
            DemandModel model = new DemandModel();

            model.Provinces = cityService.GetProvinces(true);//省

            if (id > 0)
            {
                model.Demand = demandService.GetDemandById(id);
                if (model.Demand != null)
                {
                    if (CurrentUser.UserId == model.Demand.UserId)
                    {
                        if (!string.IsNullOrEmpty(model.Demand.Province))
                        {
                            model.Cities = cityService.GetCitiesByProvinceId(model.Demand.Province, true);//市
                        }
                        if (!string.IsNullOrEmpty(model.Demand.City))
                        {
                            model.Areas = cityService.GetAreasByCityId(model.Demand.City, true);//区
                        }
                    }
                }
            }
            if (model.Demand == null)
            {
                model.Demand = new Demand();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, int ResourceType, int? SpaceTypeId, int? ActorTypeId, int? EquipTypeId, int? OtherTypeId,
            string title, string contentText,
            string province, string city, string area, string address, string phone, string qqweixin, string email,
            string startTime, string endTime, string peopleNumber, string budget)
        {
            string result = "更新需求失败";
            if (!CurrentWeChatUser.IsUserLoggedIn)
            {
                result = "长时间为操作，请重新登录";
            }
            else
            {
                if (ResourceType < 1)
                {
                    result = "请选择需求类别";
                }
                else if (id > 0 &&
                    !string.IsNullOrEmpty(title) &&
                    !string.IsNullOrEmpty(contentText) &&
                    !string.IsNullOrEmpty(startTime) &&
                    !string.IsNullOrEmpty(endTime) &&
                    !string.IsNullOrEmpty(budget) &&
                    !string.IsNullOrEmpty(phone))
                {
                    Demand demand = demandManager.GetDemandById(id);

                    if (demand != null)
                    {
                        if (demand.UserId != CurrentUser.UserId)
                        {
                            result = "只能对自己发布的需求进行编辑";
                        }
                        else if (demand.IsCompleted)
                        {
                            result = "该需求已完成，不能进行编辑";
                        }
                        else
                        {
                            demand.ResourceType = ResourceType;

                            switch (demand.ResourceType)
                            {
                                case 1:
                                    demand.ResourceTypeId = SpaceTypeId;
                                    break;
                                case 2:
                                    demand.ResourceTypeId = ActorTypeId;
                                    break;
                                case 3:
                                    demand.ResourceTypeId = EquipTypeId;
                                    break;
                                case 4:
                                    demand.ResourceTypeId = OtherTypeId;
                                    break;
                                default:
                                    break;
                            }

                            demand.Title = title;
                            demand.ContentText = contentText;
                            demand.ContentStyle = demand.ContentText;

                            if (demand.ContentText.Length > 291)
                            {
                                demand.Description = demand.ContentText.Substring(0, 290);
                            }
                            else
                            {
                                demand.Description = demand.ContentText;
                            }

                            demand.Province = string.IsNullOrEmpty(province) ? string.Empty : province;
                            demand.City = string.IsNullOrEmpty(city) ? string.Empty : city;
                            demand.Area = string.IsNullOrEmpty(area) ? string.Empty : area;
                            demand.Address = string.IsNullOrEmpty(address) ? string.Empty : address;
                            demand.Phone = phone;
                            demand.QQWeixin = string.IsNullOrEmpty(qqweixin) ? string.Empty : qqweixin;
                            demand.Email = string.IsNullOrEmpty(email) ? string.Empty : email;
                            demand.StartTime = DateTime.Parse(startTime);
                            demand.EndTime = DateTime.Parse(endTime);
                            demand.TimeLength = string.Empty;
                            demand.PeopleNumber = string.IsNullOrEmpty(peopleNumber) ? string.Empty : peopleNumber;
                            int tempBudget = 0;
                            if (!string.IsNullOrEmpty(budget))
                            {
                                Int32.TryParse(budget, out tempBudget);
                            }
                            demand.Budget = tempBudget;
                            demand.IsActive = true;
                            demand.ViewCount = 0;
                            demand.InsertTime = DateTime.Now;
                            demand.Status = (int)DemandStatus.InProgress;

                            if (demand.EndTime < demand.StartTime)
                            {
                                result = "需求结束日期应不小于开始日期";
                            }
                            else if (demandService.EditDemand(demand))
                            {
                                result = "success";
                            }
                            else
                            {
                                result = "需求更新失败";
                            }
                        }
                    }
                    else
                    {
                        result = "需求不存在或已被删除";
                    }
                }
                else
                {
                    result = "请将需求信息补充完整并检查其正确性";
                }
            }
            return Content(result);
        }

        private bool PreparePaySign(TradeOrder order, out string appId, out string timeStamp,
            out string nonceStr, out string package, out string paySign, out string message)
        {
            bool isSuccessFul = false;

            appId = string.Empty;
            timeStamp = string.Empty;
            nonceStr = string.Empty;
            package = string.Empty;
            paySign = string.Empty;

            message = string.Empty;

            try
            {

                //创建支付应答对象
                RequestHandler packageReqHandler = new RequestHandler(null);
                //初始化
                packageReqHandler.Init();

                appId = TenPayV3Info.AppId;
                timeStamp = TenPayV3Util.GetTimestamp();
                nonceStr = TenPayV3Util.GetNoncestr();

                string body = "活动在线网-购买需求联系方式";

                //设置package订单参数
                packageReqHandler.SetParameter("appid", TenPayV3Info.AppId);		  //公众账号ID
                packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);		  //商户号
                packageReqHandler.SetParameter("nonce_str", nonceStr);                    //随机字符串
                packageReqHandler.SetParameter("body", body);    //商品信息
                packageReqHandler.SetParameter("out_trade_no", order.OrderId);		//商家订单号
                packageReqHandler.SetParameter("total_fee", (Convert.ToInt32(order.Amount * 100).ToString()));			        //商品金额,以分为单位(money * 100).ToString()
                packageReqHandler.SetParameter("spbill_create_ip", Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
                packageReqHandler.SetParameter("notify_url", TenPayV3Info.TenPayV3Notify);		    //接收财付通通知的URL
                packageReqHandler.SetParameter("trade_type", TenPayV3Type.JSAPI.ToString());	                    //交易类型
                packageReqHandler.SetParameter("openid", CurrentWeChatUser.OpenId);	                    //用户的openId

                string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
                packageReqHandler.SetParameter("sign", sign);	                    //签名

                string data = packageReqHandler.ParseXML();

                var result = TenPayV3.Unifiedorder(data);
                var res = XDocument.Parse(result);

                LogService.LogWexin("微信支付参数", res.ToString());
                string prepayId = res.Element("xml").Element("prepay_id").Value;

                //设置支付参数
                RequestHandler paySignReqHandler = new RequestHandler(null);
                paySignReqHandler.SetParameter("appId", TenPayV3Info.AppId);
                paySignReqHandler.SetParameter("timeStamp", timeStamp);
                paySignReqHandler.SetParameter("nonceStr", nonceStr);
                paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
                paySignReqHandler.SetParameter("signType", "MD5");

                paySign = paySignReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
                package = string.Format("prepay_id={0}", prepayId);
            }
            catch (Exception ex)
            {
                message = "支付过程发生错误";
                LogService.LogWexin("构建支付参数出错", ex.ToString());
            }

            if (!string.IsNullOrEmpty(appId) &&
                !string.IsNullOrEmpty(timeStamp) &&
                !string.IsNullOrEmpty(nonceStr) &&
                !string.IsNullOrEmpty(package) &&
                !string.IsNullOrEmpty(paySign))
            {
                isSuccessFul = true;
            }

            return isSuccessFul;
        }

    }
}
