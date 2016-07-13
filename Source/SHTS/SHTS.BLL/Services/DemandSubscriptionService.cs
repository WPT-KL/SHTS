﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Witbird.SHTS.Model;
using Witbird.SHTS.Common;
using System.Transactions;
using Witbird.SHTS.DAL;
using Witbird.SHTS.DAL.Daos;
using System.Data.SqlClient;

namespace Witbird.SHTS.BLL.Service
{
    /// <summary>
    /// 微信订阅业务逻辑处理类
    /// </summary>
    public class DemandSubscriptionService
    {
        #region Constants

        DemandSubscriptionDao subscriptionDao = new DemandSubscriptionDao();

        #endregion Constants

        #region Public methods

        /// <summary>
        /// 获取已订阅需求推送的用户详细订阅信息
        /// </summary>
        /// <returns></returns>
        public List<DemandSubscription> GetSubscriptionsOnlySubscribed()
        {
            var subscriptions = new List<DemandSubscription>();
            var conn = DBHelper.GetSqlConnection();

            try
            {
                conn.Open();
                subscriptions = subscriptionDao.SelectAllSubscriedSubscriptionsWithDetails(conn);
            }
            catch (Exception ex)
            {
                LogService.Log("获取已订阅需求推送的用户详细订阅信息", ex.ToString());
            }
            finally
            {
                conn.Close();
            }

            return subscriptions;
        }

        /// <summary>
        /// 根据用户ID获取用户需求订阅信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DemandSubscription GetSubscription(int userId)
        {
            var subscription = new DemandSubscription();
            var conn = DBHelper.GetSqlConnection();

            try
            {
                conn.Open();
                subscription = subscriptionDao.SelectSubscriptionByUserId(conn, userId);
            }
            catch (Exception ex)
            {
                LogService.Log("根据用户ID获取用户需求订阅信息", ex.ToString());
            }
            finally
            {
                conn.Close();
            }

            return subscription;
        }

        /// <summary>
        /// 更新用户需求订阅信息
        /// </summary>
        /// <param name="subcription"></param>
        /// <returns>True: 更新成功，否则失败</returns>
        public bool UpdateSubscription(DemandSubscription subscription)
        {
            ParameterChecker.Check(subscription, "DemandSubscription");
            var isSuccessful = false;
            var conn = DBHelper.GetSqlConnection();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    conn.Open();
                    subscription.IsSubscribed = subscription.IsSubscribed;
                    subscription.LastUpdatedTimestamp = DateTime.Now;
                    subscriptionDao.InsertOrUpdateSubscription(conn, subscription);

                    // Deletes old subscription details first
                    subscriptionDao.DeleteSubscriptionDetails(conn, subscription.SubscriptionId);
                    isSuccessful = true;

                    if (subscription.SubscriptionDetails.HasItem())
                    {
                        foreach (var item in subscription.SubscriptionDetails)
                        {
                            if (item.IsNotNull())
                            {
                                item.SubscriptionId = subscription.SubscriptionId;
                                item.InsertedTimestamp = DateTime.Now;
                                
                                isSuccessful = subscriptionDao.InsertSubscriptionDetail(conn, item);
                                if (!isSuccessful)
                                {
                                    throw new Exception("InsertSubscriptionDetail failed.");
                                }
                            }
                        }
                    }

                    if (isSuccessful)
                    {
                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Log("更新用户需求订阅信息并返回更新后的结果", ex.ToString());
            }
            finally
            {
                conn.Close();
            }

            return isSuccessful;
        }

        /// <summary>
        /// 更新最后推送时间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateLastPushTimestamp(int userId)
        {
            var isSuccessful = false;
            var conn = DBHelper.GetSqlConnection();

            try
            {
                conn.Open();
                isSuccessful = subscriptionDao.UpdateDemandSubscriptionDateTimeField(conn, userId, "LastPushTimestamp");
            }
            catch (Exception ex)
            {
                LogService.Log("更新最后推送时间", ex.ToString());
            }
            finally
            {
                conn.Close();
            }

            return isSuccessful;
        }

        /// <summary>
        /// 更新最后推送时间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateLastPushTimestamp(List<int> userIds)
        {
            ParameterChecker.Check(userIds, "UserIds");
            var isSuccessful = false;
            var conn = DBHelper.GetSqlConnection();

            try
            {
                using (var scope = new TransactionScope())
                {
                    conn.Open();
                    foreach (var userId in userIds)
                    {
                        subscriptionDao.UpdateDemandSubscriptionDateTimeField(conn, userId, "LastPushTimestamp");
                    }

                    isSuccessful = true;
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                LogService.Log("更新最后推送时间", ex.ToString());
            }
            finally
            {
                conn.Close();
            }

            return isSuccessful;
        }

        /// <summary>
        /// 更新最后主动请求交互时间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateLastRequestTimestamp(int userId)
        {
            var isSuccessful = false;
            var conn = DBHelper.GetSqlConnection();

            try
            {
                conn.Open();
                isSuccessful = subscriptionDao.UpdateDemandSubscriptionDateTimeField(conn, userId, "LastRequestTimestamp");
            }
            catch (Exception ex)
            {
                LogService.Log("更新最后主动请求交互时间", ex.ToString());
            }
            finally
            {
                conn.Close();
            }

            return isSuccessful;
        }

        /// <summary>
        /// 添加默认的需求订阅信息当新用户关注时
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool AddDefautlSubcription(int userId)
        {
            var isSuccessful = false;
            var conn = DBHelper.GetSqlConnection();

            try
            {
                var demandCatogoryRepository = new DemandCategoryRepository();
                var catogroies = demandCatogoryRepository.FindAll();

                using (TransactionScope scope = new TransactionScope())
                {
                    conn.Open();

                    var currentTime = DateTime.Now;
                    var subscription = new DemandSubscription()
                    {
                        InsertedTimestamp = currentTime,
                        IsSubscribed = true,
                        LastPushTimestamp = currentTime,
                        LastRequestTimestamp = currentTime,
                        LastUpdatedTimestamp = currentTime,
                        WeChatUserId = userId
                    };

                    subscription = subscriptionDao.InsertOrUpdateSubscription(conn, subscription);

                    if (subscription.IsNotNull() && catogroies.HasItem())
                    {
                        foreach (var item in catogroies)
                        {
                            var subscriptionDetail = new DemandSubscriptionDetail()
                            {
                                InsertedTimestamp = currentTime,
                                SubscriptionId = subscription.SubscriptionId,
                                SubscriptionType = DemandSubscriptionType.Category.ToString(),
                                SubscriptionValue = item.Id.ToString()
                            };

                            isSuccessful = isSuccessful && subscriptionDao.InsertSubscriptionDetail(conn, subscriptionDetail);
                        }
                    }

                    if (isSuccessful)
                    {
                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Log("添加默认的需求订阅信息当新用户关注时", ex.ToString());
            }
            finally
            {
                conn.Close();
            }

            return isSuccessful;
        }

        #endregion Public methods


        #region Private methods


        #endregion Private methods

    }
}
