﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Witbird.SHTS.BLL.Managers;
using Witbird.SHTS.BLL.Services;
using Witbird.SHTS.Common;
using Witbird.SHTS.Model;
using Witbird.SHTS.Web.Controllers;
using Witbird.SHTS.Web.Models;

namespace Witbird.SHTS.Web.Areas.Wechat.Controllers
{
    public class ResourceController : BaseController
    {
        ResourceManager resourceManager = new ResourceManager();
        ResourceService resourceService = new ResourceService();

        #region 查看资源信息列表

        /// <summary>
        /// 页码从1开始
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("spacelist")]
        public ActionResult SpaceList()
        {
            var query = new Witbird.SHTS.Model.Extensions.QueryResource();
            query.ResourceType = 1;

            var filters = ProcessSpaceFilters();
            query.CityId = GetCityFilter();
            query.AreaId = GetFilterValue(filters, "ara");
            query.PageIndex = GetIntFilterValue(filters, "page");
            query.SpaceType = GetIntFilterValue(filters, "sptp");
            query.SpaceFeature = GetIntFilterValue(filters, "spft");
            query.SpaceFacility = (int)Math.Pow(2, GetIntFilterValue(filters, "spfc") - 1);
            query.SpaceSizeId = GetIntFilterValue(filters, "spsz");
            query.SpacePeopleId = GetIntFilterValue(filters, "sppc");
            query.SpaceTreat = GetIntFilterValue(filters, "sptt");
            query.PageIndex = query.PageIndex > 1 ? query.PageIndex - 1 : 0;
            query.PageSize = 15;
            query.State = 2;

            var result = resourceService.GetResourceByFilter(query);

            result.Filter = new Witbird.SHTS.Model.Extensions.UserFilter { ActionName = "spacelist", SelectedFilter = new Dictionary<string, string>(), UnselectFilter = new Dictionary<string, string>() };
            result.Paging.SelectedFilters = new Dictionary<string, string>();
            foreach (var item in filters)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    result.Filter.UnselectFilter.Add(item.Key, item.Value);
                }
                else
                {
                    result.Filter.SelectedFilter.Add(item.Key, item.Value);
                    result.Paging.SelectedFilters.Add(item.Key, item.Value);
                }
            }
            if (Session["CityId"] != null)
            {
                if (result.Filter.SelectedFilter.ContainsKey("city"))
                {
                    result.Filter.SelectedFilter.Remove("city");
                }
                result.Filter.SelectedFilter.Add("city", Session["CityId"].ToString());
            }


            return View("ResourceListView", result);
        }

        /// <summary>
        /// 页码从1开始
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("actorlist")]
        public ActionResult ActorList()
        {
            var query = new Witbird.SHTS.Model.Extensions.QueryResource();
            query.ResourceType = 2;

            var filters = ProcessActorFilters();
            query.CityId = GetCityFilter();
            query.AreaId = GetFilterValue(filters, "ara");
            query.ActorFromId = GetIntFilterValue(filters, "acfm");
            query.ActorSex = GetIntFilterValue(filters, "acsx");
            query.ActorTypeId = GetIntFilterValue(filters, "actp");
            query.PageIndex = GetIntFilterValue(filters, "page");
            query.PageIndex = query.PageIndex > 1 ? query.PageIndex - 1 : 0;
            query.PageSize = 15;
            query.State = 2;

            var result = resourceService.GetResourceByFilter(query);

            result.Filter = new Witbird.SHTS.Model.Extensions.UserFilter { ActionName = "actorlist", SelectedFilter = new Dictionary<string, string>(), UnselectFilter = new Dictionary<string, string>() };
            result.Paging.SelectedFilters = new Dictionary<string, string>();
            foreach (var item in filters)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    result.Filter.UnselectFilter.Add(item.Key, item.Value);
                }
                else
                {
                    result.Filter.SelectedFilter.Add(item.Key, item.Value);
                    result.Paging.SelectedFilters.Add(item.Key, item.Value);
                }
            }
            if (Session["CityId"] != null)
            {
                if (result.Filter.SelectedFilter.ContainsKey("city"))
                {
                    result.Filter.SelectedFilter.Remove("city");
                }
                result.Filter.SelectedFilter.Add("city", Session["CityId"].ToString());
            }

            return View("ResourceListView", result);
        }

        /// <summary>
        /// 页码从1开始
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("equipmentlist")]
        public ActionResult EquipmentList()
        {
            var query = new Witbird.SHTS.Model.Extensions.QueryResource();
            query.ResourceType = 3;

            var filters = ProcessEquipmentFilters();
            query.CityId = GetCityFilter();
            query.AreaId = GetFilterValue(filters, "ara");
            query.PageIndex = GetIntFilterValue(filters, "page");
            query.EquipTypeId = GetIntFilterValue(filters, "eqtp");

            query.PageIndex = query.PageIndex > 1 ? query.PageIndex - 1 : 0;

            query.PageSize = 15;
            query.State = 2;

            var result = resourceService.GetResourceByFilter(query);

            result.Filter = new Witbird.SHTS.Model.Extensions.UserFilter { ActionName = "equipmentlist", SelectedFilter = new Dictionary<string, string>(), UnselectFilter = new Dictionary<string, string>() };
            result.Paging.SelectedFilters = new Dictionary<string, string>();
            foreach (var item in filters)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    result.Filter.UnselectFilter.Add(item.Key, item.Value);
                }
                else
                {
                    result.Filter.SelectedFilter.Add(item.Key, item.Value);
                    result.Paging.SelectedFilters.Add(item.Key, item.Value);
                }
            }
            if (Session["CityId"] != null)
            {
                if (result.Filter.SelectedFilter.ContainsKey("city"))
                {
                    result.Filter.SelectedFilter.Remove("city");
                }
                result.Filter.SelectedFilter.Add("city", Session["CityId"].ToString());
            }

            return View("ResourceListView", result);
        }

        /// <summary>
        /// 页码从1开始
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("otherlist")]
        public ActionResult OtherList()
        {
            var query = new Witbird.SHTS.Model.Extensions.QueryResource();
            query.ResourceType = 4;

            var filters = ProcessOtherFilters();
            query.CityId = GetCityFilter();
            query.AreaId = GetFilterValue(filters, "ara");
            query.PageIndex = GetIntFilterValue(filters, "page");
            query.OtherTypeId = GetIntFilterValue(filters, "ottp");
            query.PageIndex = query.PageIndex > 1 ? query.PageIndex - 1 : 0;
            query.PageSize = 15;
            query.State = 2;

            var result = resourceService.GetResourceByFilter(query);

            result.Filter = new Witbird.SHTS.Model.Extensions.UserFilter { ActionName = "otherlist", SelectedFilter = new Dictionary<string, string>(), UnselectFilter = new Dictionary<string, string>() };
            result.Paging.SelectedFilters = new Dictionary<string, string>();
            foreach (var item in filters)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    result.Filter.UnselectFilter.Add(item.Key, item.Value);
                }
                else
                {
                    result.Filter.SelectedFilter.Add(item.Key, item.Value);
                    result.Paging.SelectedFilters.Add(item.Key, item.Value);
                }
            }
            if (Session["CityId"] != null)
            {
                if (result.Filter.SelectedFilter.ContainsKey("city"))
                {
                    result.Filter.SelectedFilter.Remove("city");
                }
                result.Filter.SelectedFilter.Add("city", Session["CityId"].ToString());
            }

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
                return RedirectToAction("Index", "User");
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
                //ViewData["UserInfo"] = CurrentUser;
                return View(space);
            }
        }


        #endregion


        #region Process Filters
        private string GetFilterValue(Dictionary<string, string> filterDict, string filterName)
        {
            string value = string.Empty;
            if (filterDict != null && !string.IsNullOrEmpty(filterName))
            {
                if (filterDict.Keys.Contains(filterName))
                {
                    value = filterDict[filterName];
                }
            }
            return value;
        }

        private int GetIntFilterValue(Dictionary<string, string> filterDict, string filterName)
        {
            int value = 0;
            if (filterDict != null && !string.IsNullOrEmpty(filterName)
                && filterDict.Keys.Contains(filterName) && int.TryParse(filterDict[filterName], out value))
            {
            }
            return value;
        }

        private Dictionary<string, string> ProcessSpaceFilters()
        {
            string[] filters = new[] { "ara", "city", "page", "sptp", "spft", "spfc", "spsz", "sppc", "sptt", "rsst", "rsed" };//城市,分页,场地类型,场地特点,场地设施,场地大小,容纳人数,提供酒宴,开始时间,结束时间

            return ProcessFiltersCore(filters);
        }

        private Dictionary<string, string> ProcessActorFilters()
        {
            string[] filters = new[] { "ara", "city", "page", "actp", "rsst", "rsed", "acfm", "acsx" };//城市,分页,演员工作,开始时间,结束时间,来源,性别

            return ProcessFiltersCore(filters);
        }

        private Dictionary<string, string> ProcessEquipmentFilters()
        {
            string[] filters = new[] { "ara", "city", "page", "eqtp", "rsst", "rsed" };//城市,分页,开始时间,结束时间

            return ProcessFiltersCore(filters);
        }

        private Dictionary<string, string> ProcessOtherFilters()
        {
            string[] filters = new[] { "ara", "city", "page", "ottp", "rsst", "rsed" };//城市,分页,开始时间,结束时间

            return ProcessFiltersCore(filters);
        }

        private Dictionary<string, string> ProcessFiltersCore(params string[] filters)
        {
            Dictionary<string, string> filterDict = new Dictionary<string, string>();

            foreach (var filter in filters)
            {
                var value = ProcessFilter(filter);
                filterDict.Add(filter, value);
            }

            return filterDict;
        }

        private string GetCityFilter()
        {
            string city = null;
            if (Session["CityId"] != null)
            {
                city = Session["CityId"].ToString();
            }
            return city;
        }

        private string ProcessFilter(string filterName)
        {
            if (string.IsNullOrEmpty(filterName) || string.IsNullOrEmpty(Request.QueryString[filterName]))
            {
                return string.Empty;
            }
            else
            {
                return Request.QueryString[filterName];
            }
        }

        #endregion

        #region 中介交易成功后点评资源
        /// <summary>
        /// 点评资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("comment")]
        public ActionResult Comment(int id)
        {
            if (!IsUserLogin)
            {
                return Redirect("/account/login");
            }
            else
            {

                CommentViewModel model = new CommentViewModel();

                if (id > 0)
                {
                    var resource = resourceManager.GetResourceById(id);
                    if (resource != null)
                    {
                        model.ResourceId = resource.Id.ToString();
                        model.Title = resource.Title;
                    }
                }
                else
                {
                    return Redirect("/");
                }

                return View(model);
            }
        }

        [HttpPost]
        [ActionName("comment")]
        [ValidateInput(false)]
        public ActionResult Comment(string rid, string content)
        {
            string message = ValidateCommentParameters(rid, content);

            if (string.IsNullOrWhiteSpace(message))
            {
                int resourceid = int.Parse(rid);
                var resource = resourceManager.GetResourceById(resourceid);

                if (resource != null)
                {
                    resourceManager.CommentOnResource(resource.Id, UserInfo.UserId, content);
                    message = "success";
                }
            }
            return Content(message);
        }

        private string ValidateCommentParameters(string rid, string content)
        {
            string errorMessage = string.Empty;

            if (!IsUserLogin)
            {
                errorMessage = "对不起，您还没有登录";
            }
            else if (!new UserService().ValidateUserResoureCommentPermission(UserInfo.UserId, int.Parse(rid)))
            {
                errorMessage = "对不起，只有对该资源进行中介申请的买家才能点评资源";
            }
            else if (string.IsNullOrWhiteSpace(content))
            {
                errorMessage = "点评内容不能为空";
            }

            return errorMessage;
        }
        #endregion 中介交易成功后点评资源
    }
}
