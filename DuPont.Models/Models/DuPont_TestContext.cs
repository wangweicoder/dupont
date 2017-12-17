using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DuPont.Models.Models.Mapping;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;
using DuPont.Models.Migrations;

namespace DuPont.Models.Models
{
    public partial class DuPont_TestContext : DbContext
    {
        static DuPont_TestContext()
        {
            //Database.SetInitializer<DuPont_TestContext>(new CreateDatabaseIfNotExists<DuPont_TestContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DuPont_TestContext, Configuration>());

        }

        public DuPont_TestContext()
            : base("Name=DoPontEntities")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<T_ADMIN_USER> T_ADMIN_USER { get; set; }
        public DbSet<T_APP_VERSION> T_APP_VERSION { get; set; }
        public DbSet<T_AREA> T_AREA { get; set; }
        public DbSet<T_ARTICLE> T_ARTICLE { get; set; }
        public DbSet<T_ARTICLE_CATEGORY> T_ARTICLE_CATEGORY { get; set; }
        public DbSet<T_BUSINESS_DEMAND_RESPONSE_RELATION> T_BUSINESS_DEMAND_RESPONSE_RELATION { get; set; }
        public DbSet<T_BUSINESS_PUBLISHED_DEMAND> T_BUSINESS_PUBLISHED_DEMAND { get; set; }
        public DbSet<T_BUSINESS_VERIFICATION_INFO> T_BUSINESS_VERIFICATION_INFO { get; set; }
        public DbSet<T_CAROUSEL> T_CAROUSEL { get; set; }
        public DbSet<T_DEMONSTRATION_FARM> T_DEMONSTRATION_FARM { get; set; }
        public DbSet<T_EXPERT> T_EXPERT { get; set; }
        public DbSet<T_FARM_AREA> T_FARM_AREA { get; set; }
        public DbSet<T_FARM_BOOKING> T_FARM_BOOKING { get; set; }
        public DbSet<T_FARMER_DEMAND_RESPONSE_RELATION> T_FARMER_DEMAND_RESPONSE_RELATION { get; set; }
        public DbSet<T_FARMER_PUBLISHED_DEMAND> T_FARMER_PUBLISHED_DEMAND { get; set; }
        public DbSet<T_FARMER_VERIFICATION_INFO> T_FARMER_VERIFICATION_INFO { get; set; }
        public DbSet<T_FileInfo> T_FileInfo { get; set; }
        public DbSet<T_LEARNING_GARDEN_CAROUSEL> T_LEARNING_GARDEN_CAROUSEL { get; set; }
        public DbSet<T_MACHINERY_OPERATOR_VERIFICATION_INFO> T_MACHINERY_OPERATOR_VERIFICATION_INFO { get; set; }
        public DbSet<T_MENU> T_MENU { get; set; }
        public DbSet<T_MENU_ROLE_RELATION> T_MENU_ROLE_RELATION { get; set; }
        public DbSet<T_PIONEERCURRENCYHISTORY> T_PIONEERCURRENCYHISTORY { get; set; }
        public DbSet<T_QUESTION> T_QUESTION { get; set; }
        public DbSet<T_QUESTION_REPLY> T_QUESTION_REPLY { get; set; }
        public DbSet<T_ROLE> T_ROLE { get; set; }
        public DbSet<T_SMS_MESSAGE> T_SMS_MESSAGE { get; set; }
        public DbSet<T_SUPPLIERS_AREA> T_SUPPLIERS_AREA { get; set; }
        public DbSet<T_SYS_ADMIN> T_SYS_ADMIN { get; set; }
        public DbSet<T_SYS_DICTIONARY> T_SYS_DICTIONARY { get; set; }
        public DbSet<T_SYS_LOG> T_SYS_LOG { get; set; }
        public DbSet<T_SYS_SETTING> T_SYS_SETTING { get; set; }
        public DbSet<T_USER> T_USER { get; set; }
        public DbSet<T_USER_PASSWORD_HISTORY> T_USER_PASSWORD_HISTORY { get; set; }
        public DbSet<T_USER_ROLE_DEMANDTYPELEVEL_RELATION> T_USER_ROLE_DEMANDTYPELEVEL_RELATION { get; set; }
        public DbSet<T_USER_ROLE_RELATION> T_USER_ROLE_RELATION { get; set; }
        public DbSet<VM_GET_FARMER_TO_BUSINESS_DEMAND_TYPE> VM_GET_FARMER_TO_BUSINESS_DEMAND_TYPE { get; set; }
        public DbSet<VM_GET_FARMER_TO_GET_MACHINERY_OPERATOR_DEMAND_TYPE> VM_GET_FARMER_TO_GET_MACHINERY_OPERATOR_DEMAND_TYPE { get; set; }
        public DbSet<VM_GET_LARGE_FARMER_DEMAND_TYPE> VM_GET_LARGE_FARMER_DEMAND_TYPE { get; set; }
        public DbSet<VM_GET_PENDING_AUDIT_LIST> VM_GET_PENDING_AUDIT_LIST { get; set; }
        public DbSet<VM_GET_USER_ROLE_INFO_LIST> VM_GET_USER_ROLE_INFO_LIST { get; set; }
        public DbSet<QQUser> QQUsers { get; set; }
        public DbSet<WeChatUser> WeChatUsers { get; set; }
        public DbSet<T_MACHINE_DEMANDTYPE_RELATION> Machine_DemandType_Relations { get; set; }
        /// <summary>
        /// ”√ªß
        /// </summary>
        public DbSet<T_User_Token> UserToken { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new T_ADMIN_USERMap());
            modelBuilder.Configurations.Add(new T_APP_VERSIONMap());
            modelBuilder.Configurations.Add(new T_AREAMap());
            modelBuilder.Configurations.Add(new T_ARTICLEMap());
            modelBuilder.Configurations.Add(new T_ARTICLE_CATEGORYMap());
            modelBuilder.Configurations.Add(new T_BUSINESS_DEMAND_RESPONSE_RELATIONMap());
            modelBuilder.Configurations.Add(new T_BUSINESS_PUBLISHED_DEMANDMap());
            modelBuilder.Configurations.Add(new T_BUSINESS_VERIFICATION_INFOMap());
            modelBuilder.Configurations.Add(new T_CAROUSELMap());
            modelBuilder.Configurations.Add(new T_DEMONSTRATION_FARMMap());
            modelBuilder.Configurations.Add(new T_EXPERTMap());
            modelBuilder.Configurations.Add(new T_FARM_AREAMap());
            modelBuilder.Configurations.Add(new T_FARM_BOOKINGMap());
            modelBuilder.Configurations.Add(new T_FARMER_DEMAND_RESPONSE_RELATIONMap());
            modelBuilder.Configurations.Add(new T_FARMER_PUBLISHED_DEMANDMap());
            modelBuilder.Configurations.Add(new T_FARMER_VERIFICATION_INFOMap());
            modelBuilder.Configurations.Add(new T_FileInfoMap());
            modelBuilder.Configurations.Add(new T_LEARNING_GARDEN_CAROUSELMap());
            modelBuilder.Configurations.Add(new T_MACHINERY_OPERATOR_VERIFICATION_INFOMap());
            modelBuilder.Configurations.Add(new T_MENUMap());
            modelBuilder.Configurations.Add(new T_MENU_ROLE_RELATIONMap());
            modelBuilder.Configurations.Add(new T_PIONEERCURRENCYHISTORYMap());
            modelBuilder.Configurations.Add(new T_QUESTIONMap());
            modelBuilder.Configurations.Add(new T_QUESTION_REPLYMap());
            modelBuilder.Configurations.Add(new T_ROLEMap());
            modelBuilder.Configurations.Add(new T_SMS_MESSAGEMap());
            modelBuilder.Configurations.Add(new T_SUPPLIERS_AREAMap());
            modelBuilder.Configurations.Add(new T_SYS_ADMINMap());
            modelBuilder.Configurations.Add(new T_SYS_DICTIONARYMap());
            modelBuilder.Configurations.Add(new T_SYS_LOGMap());
            modelBuilder.Configurations.Add(new T_SYS_SETTINGMap());
            modelBuilder.Configurations.Add(new T_USERMap());
            modelBuilder.Configurations.Add(new T_USER_PASSWORD_HISTORYMap());
            modelBuilder.Configurations.Add(new T_USER_ROLE_DEMANDTYPELEVEL_RELATIONMap());
            modelBuilder.Configurations.Add(new T_USER_ROLE_RELATIONMap());
            modelBuilder.Configurations.Add(new VM_GET_FARMER_TO_BUSINESS_DEMAND_TYPEMap());
            modelBuilder.Configurations.Add(new VM_GET_FARMER_TO_GET_MACHINERY_OPERATOR_DEMAND_TYPEMap());
            modelBuilder.Configurations.Add(new VM_GET_LARGE_FARMER_DEMAND_TYPEMap());
            modelBuilder.Configurations.Add(new VM_GET_PENDING_AUDIT_LISTMap());
            modelBuilder.Configurations.Add(new VM_GET_USER_ROLE_INFO_LISTMap());
            modelBuilder.Configurations.Add(new T_NOTIFICATIONMap());
            modelBuilder.Configurations.Add(new T_SEND_COMMON_NOTIFICATION_PROGRESSMap());
            modelBuilder.Configurations.Add(new T_SEND_NOTIFICATION_RESULTMap());
            modelBuilder.Configurations.Add(new T_VISITOR_RECEIVED_NOTIFICATIONMap());
            modelBuilder.Configurations.Add(new QQUserMap());
            modelBuilder.Configurations.Add(new WeChatUserMap());
            modelBuilder.Configurations.Add(new T_MACHINE_DEMANDTYPE_RELATIONMap());
            modelBuilder.Configurations.Add(new T_User_TokenMap());
        }

        public List<T_AREA> GET_AREA_CHILDS(string parentId)
        {
            using (var context = new DuPont_TestContext())
            {
                return context.Database.SqlQuery<T_AREA>("select * from [dbo].[GET_AREA_CHILDS](@ParentId)", new SqlParameter("@ParentId", parentId)).ToList();
            }
        }
    }
}
