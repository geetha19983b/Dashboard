using DashBoard.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace DashBoard.Controllers
{
    [Authorize]
    public class SASController : Controller
    {
        //
        // GET: /SAS/
        private DashboardDBEntities dashDB = new DashboardDBEntities();

        public ActionResult Index()
        {
            //var CSLMartixVMs = new CSLMatrixVM
            //{
            //    CSLSummary = dashDB.CSL_Summary.ToList(),
            //    CriticalSLM = dashDB.CriticalServicelevelMatrices.ToList()

            //};
            //return View(CSLMartixVMs);

            var CSLSummary = dashDB.CSL_Summary.ToList();
            return View(CSLSummary);
           
        }
        
        public ActionResult _CSLMatrix()
        {
            var CSLMartixVMs = new CSLMatrixVM
            {
                CSLSummary = dashDB.CSL_Summary.ToList(),
                CriticalSLM = dashDB.CriticalServicelevelMatrices.ToList()

            };
            return PartialView(CSLMartixVMs);
        }
        public ActionResult _CSLSummary()
        {
            var CSLSummary = dashDB.CSL_Summary.ToList();
            return PartialView(CSLSummary);
        }
        public ActionResult _CSLTowerLevelDetailsforMonth()
        {
            var CSLTowerLevelDetailsforMonth = dashDB.CSLTowerLevelDetailsforMonths.ToList();
            return PartialView(CSLTowerLevelDetailsforMonth);
        }
        public ActionResult _IncidentSummary()
        {
            var IncidentSumVM = new IncidentSumVM
            {
                IncidentSummary = dashDB.Incident_summary.ToList(),
                UnResolvIncident = dashDB.UnResolvedIncidents.ToList()
            };
            return PartialView(IncidentSumVM);
        }
        public ActionResult _KPI()
        {
            var KPI = dashDB.KeyPerformanceIndicators.ToList();
            return PartialView(KPI);
        }
        public ActionResult _PrblmManagement()
        {
            var PrblmMangVM = new PrblmMangVM
            {
                ProblmSummaries = dashDB.tblProblemSummaries.ToList(),
                WeeklyPrblmManag = dashDB.tblWeeklyProblemmanagements
                                    .Where(x => x.StatusandETA == "Open")
                                    .ToList()

            };
            return PartialView(PrblmMangVM);
        }
        public ActionResult _ActionItems()
        {
            var ActionItem = dashDB.tblActionItems.ToList();
            return PartialView(ActionItem);
        }
        public ActionResult SummaryInfer(string catgry, string month, int? page)
        {
            DateTime dt = DateTime.ParseExact(month.Trim(), "MMM", CultureInfo.InvariantCulture);
            int mon = dt.Month;

            int pageNumber = (page ?? 1);
            ViewBag.Catgry = catgry;
            ViewBag.Month = month;
                       
            if(catgry.Trim() == "Application Availability")
            {
                List<AppAvailabilityVM> lstAppAvail = (from availrow in dashDB.App_Availability
                                                       where availrow.AsOfMonth == mon && availrow.Metal == "Gold"
                                                      select new AppAvailabilityVM
                                                      {
                                                          ServiceOffering = availrow.ServiceOffering,
                                                          Average = availrow.Average,
                                                          Metal = availrow.Metal,
                                                          Tower = availrow.Tower
                                                      }).ToList();
             

                //return PartialView("InferAppAvail", lstAppAvail.ToPagedList(pageNumber, pageSize));
                return PartialView("InferAppAvail", lstAppAvail);
            }
            else if (catgry.Trim() == "Root Cause Analysis")
            {
                List<RCAVM> lstRCAVM = (from rcarow in dashDB.RCAs
                                        where rcarow.AsOfMonth == mon
                                        select new RCAVM
                                        {
                                            ParentIncident = rcarow.ParentIncident,
                                            Parent_IncidentPriority = rcarow.Parent_IncidentPriority,
                                            Number = rcarow.Number,
                                            RCA_State = rcarow.RCA_State,
                                            ProblemTicket = rcarow.ProblemTicket,
                                            ProblemTicketOpenDate = rcarow.ProblemTicketOpenDate,
                                            SLAMet = rcarow.SLAMet
                                        }).ToList();

                //return PartialView("InferRCA", lstRCAVM.ToPagedList(pageNumber, pageSize));
                return PartialView("InferRCA", lstRCAVM);
            }
            else if (catgry.Trim() == "Synthetic Transaction Response Time")
            {
                List<Synthetic_Trans> lstSynthTran= dashDB.Synthetic_Trans.Where(x => x.AsOfMonth == mon)
                    .ToList();

                //return PartialView("InferSynthRespn", lstSynthTran.ToPagedList(pageNumber, pageSize));
                return PartialView("InferSynthRespn", lstSynthTran);
            }
            else if(catgry.Trim() == "Resolution SLA")
            {
                List<FeedSnapShotVM> lstFeedSnap = (from feedsnaprow in dashDB.Feed_Snapshot
                                                    where feedsnaprow.AsOfMonth == mon
                                                    select new FeedSnapShotVM
                                                    {
                                                        ParentIncident = feedsnaprow.ParentIncident,
                                                        IncidentPriority = feedsnaprow.IncidentPriority,
                                                        Opened = feedsnaprow.Opened,
                                                        State = feedsnaprow.State,
                                                        Tower = feedsnaprow.Tower,
                                                        ResolutionSLA=feedsnaprow.ResolutionSLA
                                                    }).ToList();
                //return PartialView("InferReslSLA", lstFeedSnap.ToPagedList(pageNumber, pageSize));
                return PartialView("InferReslSLA", lstFeedSnap);
            }
            else
            {
                return View();
            }
            
            

        }


        public ActionResult SummaryDetails(string catgry, string tower, int? page)
        {
            //DateTime dt = DateTime.ParseExact(month.Trim(), "MMM", CultureInfo.InvariantCulture);
            DateTime dt = DateTime.Now.AddMonths(-1);
            int mon = dt.Month;

            int pageNumber = (page ?? 1);
            ViewBag.Catgry = catgry;
            ViewBag.Month = Convert.ToString(mon);

            if (catgry.Trim() == "Application Availability")
            {
                List<AppAvailabilityVM> lstAppAvail = (from availrow in dashDB.App_Availability
                                                       where availrow.AsOfMonth == mon && availrow.Metal == "Gold" && availrow.Tower == tower.Trim()
                                                       select new AppAvailabilityVM
                                                       {
                                                           ServiceOffering = availrow.ServiceOffering,
                                                           Average = availrow.Average,
                                                           Metal = availrow.Metal,
                                                           Tower = availrow.Tower
                                                       }).ToList();


                //return PartialView("InferAppAvail", lstAppAvail.ToPagedList(pageNumber, pageSize));
                return PartialView("InferAppAvail", lstAppAvail);
            }
            else if (catgry.Trim() == "Root Cause Analysis")
            {
                List<RCAVM> lstRCAVM = (from rcarow in dashDB.RCAs
                                        where rcarow.AsOfMonth == mon && rcarow.tower == tower
                                        select new RCAVM
                                        {
                                            ParentIncident = rcarow.ParentIncident,
                                            Parent_IncidentPriority = rcarow.Parent_IncidentPriority,
                                            Number = rcarow.Number,
                                            RCA_State = rcarow.RCA_State,
                                            ProblemTicket = rcarow.ProblemTicket,
                                            ProblemTicketOpenDate = rcarow.ProblemTicketOpenDate,
                                            SLAMet = rcarow.SLAMet
                                        }).ToList();

                //return PartialView("InferRCA", lstRCAVM.ToPagedList(pageNumber, pageSize));
                return PartialView("InferRCA", lstRCAVM);
            }
            else if (catgry.Trim() == "Synthetic Transaction Response Time")
            {
                List<Synthetic_Trans> lstSynthTran = dashDB.Synthetic_Trans.Where(x => x.AsOfMonth == mon && x.tower == tower)
                    .ToList();

                //return PartialView("InferSynthRespn", lstSynthTran.ToPagedList(pageNumber, pageSize));
                return PartialView("InferSynthRespn", lstSynthTran);
            }
            else if (catgry.Trim() == "Resolution SLA")
            {
                List<FeedSnapShotVM> lstFeedSnap = (from feedsnaprow in dashDB.Feed_Snapshot
                                                    where feedsnaprow.AsOfMonth == mon
                                                    && feedsnaprow.Tower == tower
                                                    select new FeedSnapShotVM
                                                    {
                                                        ParentIncident = feedsnaprow.ParentIncident,
                                                        IncidentPriority = feedsnaprow.IncidentPriority,
                                                        Opened = feedsnaprow.Opened,
                                                        State = feedsnaprow.State,
                                                        Tower = feedsnaprow.Tower,
                                                        ResolutionSLA = feedsnaprow.ResolutionSLA
                                                    }).ToList();
                //return PartialView("InferReslSLA", lstFeedSnap.ToPagedList(pageNumber, pageSize));
                return PartialView("InferReslSLA", lstFeedSnap);
            }
            else
            {
                return View();
            }



        }
             
      
    }
}
