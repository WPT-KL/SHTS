﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Witbird.SHTS.BLL;
using Witbird.SHTS.BLL.Managers;
using Witbird.SHTS.BLL.Services;
using Witbird.SHTS.Common;
using Witbird.SHTS.DAL.New;
using Witbird.SHTS.Web.Models;

namespace Witbird.SHTS.Web.Areas.Wechat.Controllers
{
    public class ResourceController : WechatBaseController
    {
        ResourceManager resourceManager = new ResourceManager();
        ResourceService resourceService = new ResourceService();

        /// <summary>
        /// 我提交的资源
        /// </summary>
        /// <returns></returns>
        [ActionName("my")]
        public ActionResult My(int id = 1)
        {
            if (!IsUserLogin)
            {
                return Redirect("/Wechat/account/login");
            }
            else
            {

                int page = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["page"]))
                {
                    int.TryParse(Request.QueryString["page"], out page);
                }
                page = page < 1 ? 1 : page;

                var result = resourceService.GetResourceByUser(UserInfo.UserId, page - 1, 15);

                return View(result);
            }
        }

        #region 查看资源信息列表

        /// <summary>
        /// 页码从1开始
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("spacelist")]
        public ActionResult SpaceList(int page = 0)
        {
            var query = new Witbird.SHTS.Model.Extensions.QueryResource();
            query.ResourceType = 1;

            query.PageIndex = page < 0 ? 0 : page;
            query.PageIndex = query.PageIndex > 1 ? query.PageIndex - 1 : 0;
            query.PageSize = 15;
            query.State = 2;

            if (Session["CityId"] != null)
            {
                query.CityId = Session["CityId"].ToString();
            }

            var result = resourceService.GetResourceByFilter(query);

            result.Filter = new Witbird.SHTS.Model.Extensions.UserFilter { ActionName = "spacelist", SelectedFilter = new Dictionary<string, string>(), UnselectFilter = new Dictionary<string, string>() };
            result.Paging.SelectedFilters = new Dictionary<string, string>();


            return View("ResourceListView", result);
        }

        /// <summary>
        /// 页码从1开始
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("actorlist")]
        public ActionResult ActorList(int page = 0)
        {
            var query = new Witbird.SHTS.Model.Extensions.QueryResource();
            query.ResourceType = 2;

            query.PageIndex = page < 0 ? 0 : page;
            query.PageIndex = query.PageIndex > 1 ? query.PageIndex - 1 : 0;
            query.PageSize = 15;
            query.State = 2;
            if (Session["CityId"] != null)
            {
                query.CityId = Session["CityId"].ToString();
            }

            var result = resourceService.GetResourceByFilter(query);

            result.Filter = new Witbird.SHTS.Model.Extensions.UserFilter { ActionName = "actorlist", SelectedFilter = new Dictionary<string, string>(), UnselectFilter = new Dictionary<string, string>() };
            result.Paging.SelectedFilters = new Dictionary<string, string>();

            return View("ResourceListView", result);
        }

        /// <summary>
        /// 页码从1开始
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("equipmentlist")]
        public ActionResult EquipmentList(int page = 1)
        {
            var query = new Witbird.SHTS.Model.Extensions.QueryResource();
            query.ResourceType = 3;

            query.PageIndex = page < 1 ? 1 : page;

            query.PageIndex = query.PageIndex > 1 ? query.PageIndex - 1 : 0;
            query.PageSize = 15;
            query.State = 2;
            if (Session["CityId"] != null)
            {
                query.CityId = Session["CityId"].ToString();
            }

            var result = resourceService.GetResourceByFilter(query);

            result.Filter = new Witbird.SHTS.Model.Extensions.UserFilter { ActionName = "equipmentlist", SelectedFilter = new Dictionary<string, string>(), UnselectFilter = new Dictionary<string, string>() };
            result.Paging.SelectedFilters = new Dictionary<string, string>();

            return View("ResourceListView", result);
        }

        /// <summary>
        /// 页码从1开始
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("otherlist")]
        public ActionResult OtherList(int page = 0)
        {
            var query = new Witbird.SHTS.Model.Extensions.QueryResource();
            query.ResourceType = 4;

            query.PageIndex = page < 0 ? 0 : page;
            query.PageIndex = query.PageIndex > 1 ? query.PageIndex - 1 : 0;
            query.PageSize = 15;
            query.State = 2;
            if (Session["CityId"] != null)
            {
                query.CityId = Session["CityId"].ToString();
            }

            var result = resourceService.GetResourceByFilter(query);

            result.Filter = new Witbird.SHTS.Model.Extensions.UserFilter { ActionName = "otherlist", SelectedFilter = new Dictionary<string, string>(), UnselectFilter = new Dictionary<string, string>() };
            result.Paging.SelectedFilters = new Dictionary<string, string>();

            return View("ResourceListView", result);
        }
        #endregion

        #region 详情页
        [ActionName("show")]
        public ActionResult Show(int id)
        {
            var space = resourceManager.GetResourceById(id);

            if (space == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (!IsIdentified)
                {
                    space.Telephone = "认证会员可见";
                    space.Mobile = "认证会员可见";
                    space.QQ = "认证会员可见";
                    space.WeChat = string.Empty;
                    space.Email = "认证会员可见";
                    space.Contract = "认证会员可见";
                    space.DetailAddress = "认证会员可见";
                }

                try
                {
                    CommonService commonService = new CommonService();
                    ViewData["RightModel"] = commonService.GetRight();
                }
                catch (Exception ex)
                {
                    LogService.Log("resource Show GetRight", ex.ToString());
                }
                ViewData["UserInfo"] = this.UserInfo;
                return View(space);
            }
        }
        #endregion
    }
}
