﻿using System;
using System.Web;
using System.Web.Mvc;
using Witbird.SHTS.BLL.Services;
using Witbird.SHTS.Common;
using Witbird.SHTS.Common.Extensions;
using Witbird.SHTS.Model;
using Witbird.SHTS.Web.Models.User;
using Witbird.SHTS.Web.MvcExtension;

namespace Witbird.SHTS.Web.Areas.M.Controllers
{
    public class UserController : MobileBaseController
    {
        //
        // GET: /M/User/
        OrderService orderService;
        public UserController()
        {
            orderService = new OrderService();
        }
        public ActionResult Index()
        {
            RequireLogin();
            return View(UserInfo ?? new User());
        }

        public ActionResult ViewUser()
        {
            RequireLogin();
            UserViewModel model = new UserViewModel { UserEntity = UserInfo };
            try
            {
                CityService cityService = new CityService();
                model.Provinces = cityService.GetProvinces(true);
                if (!string.IsNullOrEmpty(model.UserEntity.LocationId))
                {
                    model.Cities = cityService.GetCitiesByProvinceId(model.UserEntity.Province, true); //市
                    model.Areas = cityService.GetAreasByCityId(model.UserEntity.City, true); //区
                }
            }
            catch (Exception e)
            {
                LogService.Log("用户中心-index", e.ToString());
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            try
            {
                UserService service = new UserService();
                var oldUser = service.GetUserById(UserInfo.UserId);
                if (oldUser != null)
                {
                    oldUser.Adress = user.Adress;
                    oldUser.QQ = user.QQ;
                    oldUser.LocationId = Request.Form["LocationId[]"];

                    if (!string.IsNullOrEmpty(Request.Form["Photo"]))
                    {
                        oldUser.Photo = Request.Form["Photo"];
                    }
                    if (service.UserUpdate(oldUser))
                    {
                        Session[USERINFO] = oldUser;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Log("会员列表", ex.ToString());
            }
            return RedirectToAction("ViewUser");
        }

        public ActionResult UpdatePassword()
        {
            RequireLogin();
            UserViewModel model = new UserViewModel { UserEntity = UserInfo };
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdatePassword(string oldpassword, string newpassword)
        {
            RequireLogin();
            UserViewModel model = new UserViewModel { UserEntity = UserInfo };
            string message = null;
            try
            {
                if (string.Equals(oldpassword.ToMD5(),
                    model.UserEntity.EncryptedPassword,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    UserService service = new UserService();
                    model.UserEntity.EncryptedPassword = newpassword.ToMD5();
                    service.ResetUserPassword(model.UserEntity);
                }
                else
                {
                    message = "原密码输入错误！";
                }
            }
            catch (Exception e)
            {
                LogService.Log("UpdatePassword 出错了！", e.ToString());
                message = "密码更新失败！";
            }
            model.ErrorMsg = message;
            return View(model);
        }

        #region 会员认证

        /// <summary>
        /// 会员须知
        /// </summary>
        /// <returns></returns>
        public ActionResult ToVip()
        {
            RequireLogin();
            UserViewModel model = new UserViewModel { UserEntity = UserInfo };
            try
            {
                UserService service = new UserService();
                UserVip vipInfo = service.GetUserVipInfoByUserId(UserInfo.UserId);
                if (vipInfo != null && vipInfo.State.HasValue && vipInfo.State.Value == (int)VipState.VIP)
                {
                    return Redirect(GetUrl("/m/user/VipInfo"));
                }

                SinglePageService singlePageService = new SinglePageService();
                model.ToVipNotice = singlePageService.GetSingPageById("95");
            }
            catch (Exception e)
            {
                LogService.Log("ToVip 出错了！", e.ToString());
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ToVip(FormCollection form)
        {
            try
            {
                UserService service = new UserService();
                //service.SetUserToVip(UserInfo);
            }
            catch (Exception e)
            {
                LogService.Log("ToVip 出错了！", e.ToString());
            }
            return View();
        }

        /// <summary>
        /// 上传证件
        /// </summary>
        /// <returns></returns>
        public ActionResult Identify()
        {
            RequireLogin();
            UserViewModel model = new UserViewModel { UserEntity = UserInfo };
            try
            {
                UserService service = new UserService();
                model.VipInfo = service.GetUserVipInfoByUserId(UserInfo.UserId);
            }
            catch (Exception e)
            {
                LogService.Log("Identify 出错了！", e.ToString());
            }
            model.ErrorMsg = GetErrorMessage(model.VipInfo);
            return View(model);
        }

        [HttpPost]
        public ActionResult Identify(FormCollection form)
        {
            RequireLogin();
            UserViewModel model = new UserViewModel { UserEntity = UserInfo };
            UserService service = new UserService();
            try
            {
                UserVip vipInfo = service.GetUserVipInfoByUserId(UserInfo.UserId);

                string errorMsg = string.Empty;
                string imgUrl = string.Empty;
                try
                {
                    HttpPostedFileBase postFile = this.HttpContext.Request.Files["IdentiyImg"];
                    using (System.Drawing.Image img = System.Drawing.Image.FromStream(postFile.InputStream))
                    {
                    }

                }
                catch
                {
                    errorMsg = "图片格式错误，请重新选择";
                }

                if (string.IsNullOrEmpty(errorMsg))
                {
                    errorMsg = FileUploadHelper.SaveFile(this.HttpContext, "IdentiyImg", out imgUrl);
                    if (string.IsNullOrEmpty(errorMsg))
                    {
                        UserInfo.IdentiyImg = imgUrl;
                        UserInfo.Vip = (int)VipState.Normal;
                        if (service.UserUpdate(UserInfo) && service.UpdateUserVipInfo(vipInfo.Id, vipInfo.OrderId, imgUrl,
                            DateTime.Now, DateTime.Now, vipInfo.Duration, vipInfo.Amount, VipState.Normal))
                        {
                            errorMsg = "图片上传成功，等待管理员审核";
                        }
                    }
                }

                // get it again
                model.VipInfo = service.GetUserVipInfoByUserId(UserInfo.UserId);
                if (string.IsNullOrEmpty(errorMsg))
                {
                    errorMsg = GetErrorMessage(model.VipInfo);
                }
                model.ErrorMsg = errorMsg;
            }
            catch (Exception e)
            {
                LogService.Log("Identify 出错了！", e.ToString());
            }

            model.VipInfo = model.VipInfo ?? service.GetUserVipInfoByUserId(UserInfo.UserId);
            return View(model);
        }

        /// <summary>
        /// vip会员费
        /// </summary>
        /// <returns></returns>
        public ActionResult VipOrder()
        {
            RequireLogin();
            UserViewModel model = new UserViewModel { UserEntity = UserInfo ?? new User() };
            UserService service = new UserService();

            try
            {
                model.VipInfo = service.GetUserVipInfoByUserId(UserInfo.UserId);

                if (model.VipInfo != null)
                {
                    if (!model.VipInfo.State.HasValue || model.VipInfo.State.Value == (int)VipState.Normal || model.VipInfo.State.Value == (int)VipState.Invalid)
                    {
                        return Redirect(GetUrl("/m/user/Identify"));
                    }
                    else if (model.VipInfo.State.Value == (int)VipState.VIP)
                    {
                        return Redirect(GetUrl("/m/user/VipInfo"));
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Log("VipOrder 出错了！", e.ToString());
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult VipOrder(string vipDuration)
        {
            string result = "生成VIP会员充值订单失败";

            if (UserInfo != null)
            {
                UserService userService = new UserService();

                try
                {
                    if (UserInfo != null)
                    {
                        int duration = -1;
                        decimal totalAmount = 0;

                        int.TryParse(vipDuration, out duration);

                        if (duration < 1 || duration > 5)
                        {
                            throw new ArgumentException("充值VIP时间不正确，请重新选择");
                        }

                        totalAmount = 100 * duration;

                        UserVip vipInfo = userService.GetUserVipInfoByUserId(UserInfo.UserId);

                        if (vipInfo != null)
                        {
                            if (vipInfo.State == (int)VipState.Normal || vipInfo.State == (int)VipState.Invalid)
                            {
                                throw new ArgumentException("认证资料还没有通过审核，请先上传认证资料");
                            }

                            TradeOrder order = null;
                            string url = GetUrl("/m/user/VipInfo");

                            if (!string.IsNullOrEmpty(vipInfo.OrderId))
                            {
                                order = orderService.GetOrderByOrderId(vipInfo.OrderId);
                            }
                            if (order != null && order.UserName == UserInfo.UserName && order.Amount == totalAmount)
                            {
                                result = string.Format(Constant.PostPayInfoFormatForMobile, order.OrderId, url);
                            }
                            else
                            {
                                // 删掉原来的订单
                                if (order != null)
                                {
                                    orderService.DeleteOrderById(order.OrderId);
                                }
                                string orderId = orderService.GenerateNewOrderNumber();
                                string subject = "活动在线网 | 用户VIP会员充值";
                                string body = "用户" + UserInfo.UserName + "充值VIP会员" + duration + "年";
                                int userId = UserInfo.UserId;
                                string username = UserInfo.UserName;
                                decimal amount = totalAmount;
                                string resourceUrl = url;

                                order = orderService.AddNewOrder(orderId, subject, body, amount, OrderState.New, username, resourceUrl, OrderType.ToVip, userId);
                                bool success = userService.UpdateUserVipInfo(vipInfo.Id, orderId, vipInfo.IdentifyImg, vipInfo.StartTime, vipInfo.EndTime, duration, totalAmount, VipState.Identified);

                                if (success && order != null)
                                {
                                    result = string.Format(Constant.PostPayInfoFormatForMobile, orderId, url);
                                }
                                else
                                {
                                    result = "生成VIP充值支付订单信息失败，请重新尝试";
                                }
                            }
                        }
                    }
                }
                catch (ArgumentException e)
                {
                    result = e.Message;
                }
                catch (Exception e)
                {
                    LogService.Log("生成VIP充值支付订单信息", e.ToString());
                    result = "生成VIP充值支付订单信息，请重新尝试";
                }
            }
            else
            {
                result = "您还未登录或登录超时，请重新登录";
            }

            return Content(result);
        }

        public ActionResult VipInfo()
        {
            RequireLogin();
            UserViewModel model = new UserViewModel { UserEntity = UserInfo ?? new User() };
            UserService service = new UserService();

            try
            {
                model.VipInfo = service.GetUserVipInfoByUserId(UserInfo.UserId);

                if (model.VipInfo != null)
                {
                    if (!model.VipInfo.State.HasValue || model.VipInfo.State.Value != (int)VipState.VIP)
                    {
                        return Redirect(GetUrl("/m/user/ToVip"));
                    }
                    else
                    {
                        if (model.VipInfo.EndTime > DateTime.Now)
                        {
                            model.ErrorMsg = "您的VIP已过期，请重新申请";
                            //service.SetUserToVip(
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Log("VipInfo 出错了！", e.ToString());
            }

            return View(model);
        }

        private string GetErrorMessage(UserVip vipInfo)
        {
            string errorMsg = string.Empty;

            if (vipInfo != null)
            {

                if (!string.IsNullOrEmpty(vipInfo.IdentifyImg))
                {
                    if (vipInfo.State.Value == (int)Witbird.SHTS.Model.VipState.Normal)
                    {
                        errorMsg = "认证资料已上传，管理员正在审核中";
                    }
                    else if (vipInfo.State.Value == (int)Witbird.SHTS.Model.VipState.Invalid)
                    {
                        errorMsg = "认证资料审核未通过，请重新上传";
                    }
                    else if (vipInfo.State.Value == (int)Witbird.SHTS.Model.VipState.Identified)
                    {
                        errorMsg = "认证资料已通过审核，您现在是认证会员，还可以升级为VIP会员。&nbsp;<a href='/m/user/tovip' title='立即升级'>立即升级</a>";
                    }
                    else if (vipInfo.State.Value == (int)Witbird.SHTS.Model.VipState.VIP)
                    {
                        errorMsg = "认证资料已通过审核，您是VIP会员";
                    }
                    else
                    {
                        errorMsg = "数据错误，请联系网站管理员";
                    }
                }
            }

            return errorMsg;
        }

        #endregion
    }
}
