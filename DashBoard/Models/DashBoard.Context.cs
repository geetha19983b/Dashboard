﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DashBoard.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class DashboardDBEntities : DbContext
    {
        public DashboardDBEntities()
            : base("name=DashboardDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CSL_Summary> CSL_Summary { get; set; }
        public virtual DbSet<CriticalServicelevelMatrix> CriticalServicelevelMatrices { get; set; }
        public virtual DbSet<Metal> Metals { get; set; }
        public virtual DbSet<feed> feeds { get; set; }
        public virtual DbSet<Priority> Priorities { get; set; }
        public virtual DbSet<SummaryPeriod> SummaryPeriods { get; set; }
        public virtual DbSet<CSLTowerLevelDetailsforMonth> CSLTowerLevelDetailsforMonths { get; set; }
        public virtual DbSet<Incident_summary> Incident_summary { get; set; }
        public virtual DbSet<UnResolvedIncident> UnResolvedIncidents { get; set; }
        public virtual DbSet<KeyPerformanceIndicator> KeyPerformanceIndicators { get; set; }
        public virtual DbSet<tblProblemSummary> tblProblemSummaries { get; set; }
        public virtual DbSet<tblWeeklyProblemmanagement> tblWeeklyProblemmanagements { get; set; }
        public virtual DbSet<tblActionItem> tblActionItems { get; set; }
        public virtual DbSet<IncidentFeedSummary> IncidentFeedSummaries { get; set; }
        public virtual DbSet<Tower2> Tower2 { get; set; }
        public virtual DbSet<CSLInference> CSLInferences { get; set; }
        public virtual DbSet<CSL_table> CSL_table { get; set; }
        public virtual DbSet<CSLField> CSLFields { get; set; }
        public virtual DbSet<App_Availability> App_Availability { get; set; }
        public virtual DbSet<RCA> RCAs { get; set; }
        public virtual DbSet<Synthetic_Trans> Synthetic_Trans { get; set; }
        public virtual DbSet<Feed_Snapshot> Feed_Snapshot { get; set; }
        public virtual DbSet<Onboarding> Onboardings { get; set; }
        public virtual DbSet<UserTower> UserTowers { get; set; }
    
        public virtual ObjectResult<sp_GetFilterConditions_Result> sp_GetFilterConditions(string filterCondn)
        {
            var filterCondnParameter = filterCondn != null ?
                new ObjectParameter("FilterCondn", filterCondn) :
                new ObjectParameter("FilterCondn", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetFilterConditions_Result>("sp_GetFilterConditions", filterCondnParameter);
        }
    
        public virtual ObjectResult<GetIncidentsList_Result> GetIncidentsList(string orderByClause)
        {
            var orderByClauseParameter = orderByClause != null ?
                new ObjectParameter("OrderByClause", orderByClause) :
                new ObjectParameter("OrderByClause", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetIncidentsList_Result>("GetIncidentsList", orderByClauseParameter);
        }
    
        public virtual ObjectResult<GetIncidentListByCondn_Result1> GetIncidentListByCondn(string orderByClause, string whereClause)
        {
            var orderByClauseParameter = orderByClause != null ?
                new ObjectParameter("OrderByClause", orderByClause) :
                new ObjectParameter("OrderByClause", typeof(string));
    
            var whereClauseParameter = whereClause != null ?
                new ObjectParameter("WhereClause", whereClause) :
                new ObjectParameter("WhereClause", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetIncidentListByCondn_Result1>("GetIncidentListByCondn", orderByClauseParameter, whereClauseParameter);
        }
    
        public virtual ObjectResult<GetIncidentListByWhereCondn_Result> GetIncidentListByWhereCondn(string orderByClause, string whereClause)
        {
            var orderByClauseParameter = orderByClause != null ?
                new ObjectParameter("OrderByClause", orderByClause) :
                new ObjectParameter("OrderByClause", typeof(string));
    
            var whereClauseParameter = whereClause != null ?
                new ObjectParameter("WhereClause", whereClause) :
                new ObjectParameter("WhereClause", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetIncidentListByWhereCondn_Result>("GetIncidentListByWhereCondn", orderByClauseParameter, whereClauseParameter);
        }
    
        public virtual ObjectResult<GetTasksList_Result1> GetTasksList(string incID)
        {
            var incIDParameter = incID != null ?
                new ObjectParameter("incID", incID) :
                new ObjectParameter("incID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetTasksList_Result1>("GetTasksList", incIDParameter);
        }
    
        public virtual int UpdateIncident(Nullable<int> feedID, string comments)
        {
            var feedIDParameter = feedID.HasValue ?
                new ObjectParameter("FeedID", feedID) :
                new ObjectParameter("FeedID", typeof(int));
    
            var commentsParameter = comments != null ?
                new ObjectParameter("Comments", comments) :
                new ObjectParameter("Comments", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateIncident", feedIDParameter, commentsParameter);
        }
    
        public virtual ObjectResult<sp_GetChartSummaryData_Result> sp_GetChartSummaryData()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetChartSummaryData_Result>("sp_GetChartSummaryData");
        }
    
        public virtual ObjectResult<string> sp_GetChartFeedSummaryData()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_GetChartFeedSummaryData");
        }
    
        public virtual int UpdateTask(string number, string comments)
        {
            var numberParameter = number != null ?
                new ObjectParameter("Number", number) :
                new ObjectParameter("Number", typeof(string));
    
            var commentsParameter = comments != null ?
                new ObjectParameter("Comments", comments) :
                new ObjectParameter("Comments", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateTask", numberParameter, commentsParameter);
        }
    
        public virtual ObjectResult<GETIncidentsBySplConditions_Result> GETIncidentsBySplConditions(string cndn, string tower, string month, string sort)
        {
            var cndnParameter = cndn != null ?
                new ObjectParameter("Cndn", cndn) :
                new ObjectParameter("Cndn", typeof(string));
    
            var towerParameter = tower != null ?
                new ObjectParameter("tower", tower) :
                new ObjectParameter("tower", typeof(string));
    
            var monthParameter = month != null ?
                new ObjectParameter("month", month) :
                new ObjectParameter("month", typeof(string));
    
            var sortParameter = sort != null ?
                new ObjectParameter("sort", sort) :
                new ObjectParameter("sort", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GETIncidentsBySplConditions_Result>("GETIncidentsBySplConditions", cndnParameter, towerParameter, monthParameter, sortParameter);
        }
    
        public virtual ObjectResult<string> sp_GetAgingData(string tower)
        {
            var towerParameter = tower != null ?
                new ObjectParameter("Tower", tower) :
                new ObjectParameter("Tower", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_GetAgingData", towerParameter);
        }
    
        public virtual ObjectResult<string> sp_GetBacklogData(string tower)
        {
            var towerParameter = tower != null ?
                new ObjectParameter("Tower", tower) :
                new ObjectParameter("Tower", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_GetBacklogData", towerParameter);
        }
    
        public virtual ObjectResult<Sp_GetChrtsDataTest_Result> Sp_GetChrtsDataTest()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_GetChrtsDataTest_Result>("Sp_GetChrtsDataTest");
        }
    
        public virtual int MapTowerToUser(string userName, string tower)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var towerParameter = tower != null ?
                new ObjectParameter("Tower", tower) :
                new ObjectParameter("Tower", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MapTowerToUser", userNameParameter, towerParameter);
        }
    }
}