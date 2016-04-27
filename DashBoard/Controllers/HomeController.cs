using DashBoard.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Core.Objects;
using System.Text;
using DataAccessLayer.Model;
using PagedList;
using System.Xml;
using Newtonsoft.Json;

namespace DashBoard.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private static string filterCndn = null;
        private static string towerCndn = null;
        private static string periodCndn = null;

        private LoginDBEntities logindb = new LoginDBEntities();
        private DashboardDBEntities dashDB = new DashboardDBEntities();
        public ActionResult Index()
        {

         

            List<string> towerAccessLst = AuthorizeUser("Admin");

            ViewBag.towerSel = string.Join(",", towerAccessLst);
            ViewBag.FilterText = "Displaying Data For Towers: " + ViewBag.towerSel;

           //string chrtsData = dashDB.sp_GetChartFeedSummaryData().AsQueryable().First().ToString();
           
            //ViewBag.chartsData = chrtsData;

            
                var strquerychrtsdt = (from chrtsdtrow in dashDB.Sp_GetChrtsDataTest()
                                     group chrtsdtrow by new
                                     {
                                         chrtsdtrow.AsOfYear,
                                         chrtsdtrow.AsOfMonth
                                     } into chrtsgrup
                                     select new
                                     {
                                         period = chrtsgrup.Key.AsOfYear + "-" + chrtsgrup.Key.AsOfMonth,
                                         values = chrtsgrup.ToList()
                                     }).AsQueryable();
            var json = JsonConvert.SerializeObject(strquerychrtsdt);
            ViewBag.chartsData = json;
            
            filterCndn = Request.QueryString["type"];

            var towerlist = (from towrrow in dashDB.UserTowers
                             where towrrow.Username == User.Identity.Name
                             select new TowerSel {Tower=towrrow.Tower,TowerValue= towrrow.TowerValue }).ToList();

            ViewBag.towerSel = towerlist;

            //if (string.IsNullOrEmpty(towerCndn))
            //{
            //    towerCndn = ViewBag.towerSel;
            //}
            if (string.IsNullOrEmpty(periodCndn))
            {
                periodCndn = ViewBag.periodSel;
            }


            var grupditems = (from cslrow in dashDB.CSL_table
                              group cslrow by cslrow.Category into cslrowgroup
                              select new ProgramCSLVM
                              {
                                  Category = cslrowgroup.Key,
                                  Categoryavg = cslrowgroup.Average(x => x.Value),
                                  TowerList = dashDB.CSL_table.Where(e => e.Category.Equals(cslrowgroup.Key)).ToList()


                              }).ToList();



            return View(grupditems);
        }
        public JsonResult AgingData(string Towers)
        {
            string[] towers = Towers.Split(',');

            StringBuilder commtowers = new StringBuilder();

            foreach (string tower in towers)
            {
                commtowers.Append("'");
                commtowers.Append(tower);
                commtowers.Append("'");
                commtowers.Append(",");
            }

            commtowers.Remove(commtowers.Length - 1, 1);

            var agingData = dashDB.sp_GetAgingData(commtowers.ToString()).AsQueryable().First().ToString();

            return Json(agingData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BackLogData(string Towers)
        {

            string[] towers = Towers.Split(',');

            StringBuilder commtowers = new StringBuilder();

            foreach (string tower in towers)
            {
                commtowers.Append("'");
                commtowers.Append(tower);
                commtowers.Append("'");
                commtowers.Append(",");
            }

            commtowers.Remove(commtowers.Length - 1, 1);

            var bcklogdata = dashDB.sp_GetBacklogData(commtowers.ToString()).AsQueryable().First().ToString();
            return Json(bcklogdata, JsonRequestBehavior.AllowGet);
        }
        private List<string> AuthorizeUser(string userNm)
        {

            return logindb.sp_AuthorizeUser(userNm).ToList();

        }
        public ActionResult BarChartInfer(string chart, string Tower, string Aging, int? page)
        {
            ViewBag.Chart = chart;
            ViewBag.Aging = Aging;
            ViewBag.Tower = Tower;
            List<BarChartVM> lstFeed;
            if (chart == "Aging")
            {
                lstFeed = (from row in dashDB.feeds
                           join twrrow in dashDB.Tower2 on row.Tower equals
                           twrrow.Tower
                           where twrrow.tower_alias == Tower &&
                           row.Aging == Aging
                           select new BarChartVM
                           {
                               IncidentNo = row.ParentIncident,
                               Priority = row.IncidentPriority,
                               ShortDesc = row.ShortDescription,
                               State = row.State,
                               Opened = row.Opened,
                               Metal = row.Metal,
                               Tower = row.Tower
                           }).ToList();
            }
            else
            {
                lstFeed = (from row in dashDB.feeds
                           join twrrow in dashDB.Tower2 on row.Tower equals
                           twrrow.Tower
                           where twrrow.tower_alias == Tower &&
                           row.State == Aging
                           select new BarChartVM
                           {
                               IncidentNo = row.ParentIncident,
                               Priority = row.IncidentPriority,
                               ShortDesc = row.ShortDescription,
                               State = row.State,
                               Opened = row.Opened,
                               Metal = row.Metal,
                               Tower = row.Tower
                           }).ToList();
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return PartialView("BarChartInfer", lstFeed.ToPagedList(pageNumber, pageSize));

            //return PartialView(lstFeed);




        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult IncidentPartialSrch()
        {
            PopulatePeriodFilter();
            PopulateMetal();
            PopulateTower();
            PopulateStateFilter();
            var incidentVM = new IncidentInterVM
            {
                AvailablePriority = dashDB.Priorities.ToList()
            };
            return PartialView("_IncidentSrch", incidentVM);
        }

        public ActionResult ConvertThisPageToPdfTest2()
        {
            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
            converter.Options.MaxPageLoadTime = 10000;

            String thisPageUrl = this.ControllerContext.HttpContext.Request.Url.AbsoluteUri;
            String baseUrl = thisPageUrl.Substring(0, thisPageUrl.Length - "Home/ConvertThisPageToPdf".Length);

            SelectPdf.PdfDocument doc = converter.ConvertUrl(baseUrl);
            doc.Save("D:\\HtmlasPdfDoc.pdf");
            doc.Close();

            EmptyResult rr = new EmptyResult();
            return rr;
        }


        [HttpPost]
        public JsonResult IncidentsByFilter(string IncId, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null, string cndn = null)
        {
            if (cndn == null)
            {
                cndn = "'" + IncId + "'";
            }
            if (string.IsNullOrEmpty(towerCndn))
            {
                towerCndn = ViewBag.towerSel;
            }
            if (string.IsNullOrEmpty(periodCndn))
            {
                periodCndn = ViewBag.periodSel;
            }



            string[] towers = { "Core" };//= towerCndn.Split(',');
            if ((towerCndn != null) && (towerCndn != ""))
                towers = towerCndn.Split(',');


            if ((periodCndn == null) || (periodCndn == ""))
                periodCndn = "2016-3";

            StringBuilder commtowers = new StringBuilder();

            foreach (string tower in towers)
            {
                commtowers.Append("'");
                commtowers.Append(tower);
                commtowers.Append("'");
                commtowers.Append(",");
            }

            commtowers.Remove(commtowers.Length - 1, 1);

            try
            {
                string period = "'" + periodCndn + "'";
                List<Incidents> incdentlst = dashDB.GETIncidentsBySplConditions(cndn, commtowers.ToString(), period, jtSorting).Select(x => new Incidents
                {
                    FeedID = x.FeedID,
                    ParentIncident = x.ParentIncident,
                    IncidentPriority = x.IncidentPriority,
                    IncServiceOffering = x.IncServiceOffering,
                    IncAssignmentGroup = x.IncAssignmentGroup,
                    //Opened = row.Field<string>("Opened"),
                    State = x.State,
                    ResponseSLA = x.ResponseSLA,
                    ResolutionSLA = x.ResolutionSLA,
                    TimeRemaining = x.TimeRemaining,
                    Tower = x.Tower,
                    AssignedTo = x.AssignedTo,
                    Metal = x.Metal,
                    Comments = x.Comments,
                }).ToList();


                int icCnt = incdentlst.Count;


                int pgCnt = icCnt - jtStartIndex < jtPageSize ? icCnt - jtStartIndex : jtPageSize;

                List<Incidents> curPageIncidents = incdentlst.GetRange(jtStartIndex, pgCnt);

                //Return result to jTable
                return Json(new { Result = "OK", Records = curPageIncidents, TotalRecordCount = icCnt });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }



        }


        [HttpPost]
        public JsonResult UpdateIncident(Incidents incidentrecord)
        {

            try
            {
                dashDB.UpdateIncident(incidentrecord.FeedID, incidentrecord.Comments);
                return Json(new { Result = "OK", Message = "Data Saved Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult TaskList(string ITaskId)
        {

            try
            {

                List<Tasks> lstTasks = dashDB.GetTasksList(ITaskId).Select(row =>
                                                                       new Tasks
                                                                       {
                                                                           Number = row.Number,
                                                                           Priority = row.Priority,
                                                                           ServiceOffering = row.serviceOffering,
                                                                           AssignedTo = row.AssignedTo,
                                                                           ItState = row.ItState,
                                                                           Metal = row.Metal,
                                                                           ResponseSLA = row.ResponseSLA,
                                                                           ResolutionSLA = row.ResolutionSLA,
                                                                           Comments = row.Comments

                                                                       }).ToList();



                return Json(new { Result = "OK", Records = lstTasks });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        [HttpPost]

        public JsonResult UpdateTask(Tasks record)
        {
            try
            {
                dashDB.UpdateTask(record.Number, record.Comments);
                return Json(new { Result = "OK", Message = "Data Saved Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        private void PopulatePeriodFilter(object selectedFilter = null)
        {

            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Yearly", Value = "Yearly" });

            items.Add(new SelectListItem { Text = "Monthly", Value = "Monthly" });

            ViewBag.PeriodFilter = new SelectList(items, "Text", "Value", selectedFilter);


        }
        private void PopulateStateFilter(object selectedFilter = null)
        {
            List<SelectListItem> lstStateItems = new List<SelectListItem>();

            lstStateItems.Add(new SelectListItem { Text = "Work in Progress", Value = "Work in Progress" });

            lstStateItems.Add(new SelectListItem { Text = "Pending User", Value = "Pending User/Appointment" });
            lstStateItems.Add(new SelectListItem { Text = "Resolved", Value = "Resolved" });
            lstStateItems.Add(new SelectListItem { Text = "Pending Vendor", Value = "Pending Vendor" });
            lstStateItems.Add(new SelectListItem { Text = "Closed", Value = "Closed" });
            lstStateItems.Add(new SelectListItem { Text = "Pending Change", Value = "Pending Change" });
            lstStateItems.Add(new SelectListItem { Text = "Reassigned", Value = "Reassigned" });
            lstStateItems.Add(new SelectListItem { Text = "Pending Validation", Value = "Pending Validation" });

            ViewBag.StateFilter = new SelectList(lstStateItems, "Text", "Value", selectedFilter);
        }
        private void PopulateMetal()
        {
            List<SelectListItem> listMetalListItems = new List<SelectListItem>();

            foreach (Metal metal in dashDB.Metals)
            {
                SelectListItem lstboxMetal = new SelectListItem()
                {
                    Text = metal.Metal1,
                    Value = metal.Metal1

                };
                listMetalListItems.Add(lstboxMetal);
            }

            ViewBag.MetalList = listMetalListItems;

        }
        private void PopulateTower()
        {
            List<SelectListItem> listTower = new List<SelectListItem>();
            var lstTowerValues = dashDB.sp_GetFilterConditions("Tower").ToList();
            foreach (var TowerVal in lstTowerValues)
            {
                SelectListItem lstboxTower = new SelectListItem()
                {
                    Text = TowerVal.Value,
                    Value = TowerVal.Value

                };
                listTower.Add(lstboxTower);
            }

            ViewBag.TowerList = listTower;
        }
       

    }
}