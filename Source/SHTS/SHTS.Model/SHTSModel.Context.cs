﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class shtsEntities : DbContext
    {
        public shtsEntities()
            : base("name=shtsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AccessAnalytics> AccessAnalytics { get; set; }
        public DbSet<Activity> Activity { get; set; }
        public DbSet<ActivityType> ActivityType { get; set; }
        public DbSet<Actor> Actor { get; set; }
        public DbSet<ActorType> ActorType { get; set; }
        public DbSet<AdminRole> AdminRole { get; set; }
        public DbSet<AdminUser> AdminUser { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Config> Config { get; set; }
        public DbSet<Demand> Demand { get; set; }
        public DbSet<DemandCategory> DemandCategory { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipType> EquipType { get; set; }
        public DbSet<Facility> Facility { get; set; }
        public DbSet<GuestBook> GuestBook { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Other> Other { get; set; }
        public DbSet<OtherType> OtherType { get; set; }
        public DbSet<PublicConfig> PublicConfig { get; set; }
        public DbSet<Resource> Resource { get; set; }
        public DbSet<ShortMessage> ShortMessage { get; set; }
        public DbSet<SinglePage> SinglePage { get; set; }
        public DbSet<Space> Space { get; set; }
        public DbSet<SpaceFacility> SpaceFacility { get; set; }
        public DbSet<SpaceFeature> SpaceFeature { get; set; }
        public DbSet<SpacePeople> SpacePeople { get; set; }
        public DbSet<SpaceSize> SpaceSize { get; set; }
        public DbSet<SpaceType> SpaceType { get; set; }
        public DbSet<Trade> Trade { get; set; }
        public DbSet<TradeHistory> TradeHistory { get; set; }
        public DbSet<TradeOrder> TradeOrder { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<UserVip> UserVip { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<UserBankInfo> UserBankInfo { get; set; }
        public DbSet<ActorFrom> ActorFrom { get; set; }
        public DbSet<ActorSex> ActorSex { get; set; }
        public DbSet<WeChatUser> WeChatUser { get; set; }
        public DbSet<DemandQuoteHistory> DemandQuoteHistory { get; set; }
        public DbSet<C__RefactorLog> C__RefactorLog { get; set; }
        public DbSet<DemandQuote> DemandQuote { get; set; }
        public DbSet<DemandSubscription> DemandSubscription { get; set; }
        public DbSet<DemandSubscriptionDetail> DemandSubscriptionDetail { get; set; }
        public DbSet<SmsRecord> SmsRecord { get; set; }
        public DbSet<ActivityVote> ActivityVote { get; set; }
        public DbSet<FinanceBalance> FinanceBalance { get; set; }
        public DbSet<FinanceOrder> FinanceOrder { get; set; }
        public DbSet<FinanceRecord> FinanceRecord { get; set; }
        public DbSet<FinanceWithdrawRecord> FinanceWithdrawRecord { get; set; }
    }
}
