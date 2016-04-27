using DashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using DataAccessLayer.Model;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace DashBoard.Controllers
{
    [Authorize]
    public class DashBoardController : Controller
    {
        //
        // GET: /DashBoard/
        private DashboardDBEntities dashDB = new DashboardDBEntities();


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
        public ActionResult IncidentInterface()
        {
            PopulatePeriodFilter();
            PopulateMetal();
            PopulateTower();
            PopulateStateFilter();
            var incidentVM = new IncidentInterVM
            {
                AvailablePriority = dashDB.Priorities.ToList()
            };
            return View(incidentVM);
        }
        public JsonResult PeriodSubFilter(string Id)
        {

            if (Id == "Yearly")
            {
                var filterSubCond = dashDB.SummaryPeriods.GroupBy(p => p.Year).Select(g => g.FirstOrDefault()).ToList();
                return Json(new SelectList(filterSubCond.ToArray(), "Year", "Year"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var filterSubCond = dashDB.SummaryPeriods.GroupBy(p => p.MM).Select(g => g.FirstOrDefault()).ToList();
                return Json(new SelectList(filterSubCond.ToArray(), "MM", "Mon"), JsonRequestBehavior.AllowGet);
            }



        }


        [HttpPost]
        public JsonResult IncidentsByFilter(string incidentNo = null, string TaskNo = null, string selmetalList = null, string seltowerList = null, string selpriorities = null, string perfilter = null, int persubfilter = 0, string statefilter = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {


            try
            {
                List<Incidents> incdentlst =
                    GetFilteredIncidents(incidentNo, TaskNo, selmetalList, seltowerList, selpriorities, perfilter, persubfilter, statefilter, jtStartIndex, jtPageSize, jtSorting);

                TempData["lstFull"] = incdentlst;
                TempData.Keep("lstFull");
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
        public void GridExporttoExcel(string incidentNo = null, string TaskNo = null, string selmetalList = null, string seltowerList = null, string selpriorities = null, string perfilter = null, int persubfilter = 0, string statefilter = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {

            try
            {
                List<Incidents> incdentlst = (List<Incidents>)TempData["lstFull"];
                TempData.Keep("lstFull");
                var grid = new GridView();
                grid.DataSource = incdentlst;
                grid.DataBind();

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=IncidentList.xls");

                Response.ContentType = "application/excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Write(sw.ToString());
                Response.End();

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }

        }
        public List<Incidents> GetFilteredIncidents(string incidentNo = null, string TaskNo = null, string selmetalList = null, string seltowerList = null, string selpriorities = null, string perfilter = null, int persubfilter = 0, string statefilter = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            List<Incidents> incdentlst = dashDB.GetIncidentsList(jtSorting).Select(x => new Incidents
            {
                FeedID = x.FeedID,
                ParentIncident = x.ParentIncident,
                IncidentPriority = x.IncidentPriority,
                IncServiceOffering = x.IncServiceOffering,
                IncAssignmentGroup = x.IncAssignmentGroup,
                Opened = x.Opened,
                State = x.State,
                ResponseSLA = x.ResponseSLA,
                ResolutionSLA = x.ResolutionSLA,
                TimeRemaining = x.TimeRemaining,
                Tower = x.Tower,
                AssignedTo = x.AssignedTo,
                Metal = x.Metal,
                Comments = x.Comments
            }).ToList();

            //filter on incident no
            if (!string.IsNullOrEmpty(incidentNo))
            {
                incdentlst = incdentlst.Where(x => x.ParentIncident == incidentNo).ToList();
            }
            //filter on task list

            //get incident number from Feed table for the task list
            if (!string.IsNullOrEmpty(TaskNo))
            {
                List<string> strTaskIncidentNo = dashDB.feeds
                                                 .Where(t => t.ITaskNumber.Contains(TaskNo))
                                                 .Select(t => t.ParentIncident).ToList();

                //filter the incidents based on the task list
                incdentlst = (from irow in incdentlst
                              where strTaskIncidentNo.Any(t => t.Contains(irow.ParentIncident))
                              select irow).ToList();


            }

            //metal list filter
            if (!string.IsNullOrEmpty(selmetalList))
            {
                List<string> strmetalList = selmetalList.Split(',').ToList<string>();
                incdentlst = (from x in incdentlst
                              where strmetalList.Any(c => c.Contains(x.Metal))
                              select x).ToList();
            }
            //tower list filter
            if (!string.IsNullOrEmpty(seltowerList))
            {
                List<string> strseltowerList = seltowerList.Split(',').ToList<string>();
                incdentlst = (from x in incdentlst
                              where strseltowerList.Any(c => c.Contains(x.Tower))
                              select x).ToList();
            }
            //priorities filter
            if (!string.IsNullOrEmpty(selpriorities))
            {
                List<string> strselpriorities = selpriorities.Split(',').ToList<string>();
                incdentlst = (from x in incdentlst
                              where strselpriorities.Any(c => c.Contains(x.IncidentPriority))
                              select x).ToList();
            }
            //filter on period
            if (perfilter == "Yearly")
            {
                incdentlst = incdentlst.Where(x => x.Opened.Value.Year == persubfilter).ToList();
            }
            if (perfilter == "Monthly")
            {
                incdentlst = incdentlst.Where(x => x.Opened.Value.Month == persubfilter).ToList();
            }


            //filter on state
            if (!string.IsNullOrEmpty(statefilter))
            {
                incdentlst = incdentlst.Where(x => x.State == statefilter).ToList();
            }

            return incdentlst;
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
        public ActionResult Wizard()
        {
            return View();
        }
        public JsonResult GetCSLInferences(string Status)
        {

            try
            {

                if (Status == "All")
                {


                    var groupedCommentsList = dashDB.CSLInferences.GroupBy(u => u.Category).Select(g => new CSLInferencesVM
                    {
                        Category = g.Key,
                        CLSList = g.Select(i => new CLSList
                        {
                            CommentID = i.CommentID,
                            Comments = i.Comments,
                            Approved = i.Approved

                        }).ToList(),
                        Categories = dashDB.CSLFields.Select(x => x.Category).ToList()

                    }).ToList();





                    return Json(groupedCommentsList, JsonRequestBehavior.AllowGet);


                }
                else
                {
                    var groupedCommentsList = dashDB.CSLInferences.Where(x => x.Approved == true).GroupBy(u => u.Category).Select(g => new CSLInferencesVM
                    {
                        Category = g.Key,
                        CLSList = g.Select(i => new CLSList
                        {
                            CommentID = i.CommentID,
                            Comments = i.Comments,
                            Approved = i.Approved

                        }).ToList(),
                        Categories = dashDB.CSLFields.Select(x => x.Category).ToList()
                    }).ToList();

                    return Json(groupedCommentsList, JsonRequestBehavior.AllowGet);
                }



            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }



        }

        public string UpdateCSLInferComment(CLSList clslist)
        {
            try
            {
                var clsCommentsList = dashDB.CSLInferences.Where(x => x.CommentID == clslist.CommentID).FirstOrDefault();
                clsCommentsList.Comments = clslist.Comments;
                clsCommentsList.Approved = clslist.Approved;

                dashDB.SaveChanges();
                return "Comments Updated";
            }
            catch (Exception ex)
            {
                return ex.Message + ex.InnerException;
            }
        }
        public string AddCSLInferComment(string Category, string Comment, string Approved)
        {
            string Status = Approved;
            try
            {
                CSLInference cslinf = new CSLInference
                {
                    Category = Category,
                    Comments = Comment,
                    Approved = Approved == "true" ? true : false,
                    //EnteredBy = "Admin",
                    ApprovedOn = DateTime.Now

                };
                dashDB.CSLInferences.Add(cslinf);
                dashDB.SaveChanges();
                return "Comments Added";

            }
            catch (Exception ex)
            {
                return ex.Message + ex.InnerException;
            }
        }
        public string DeleteCSLInferenceComment(CLSList clslist)
        {
            try
            {
                int no = Convert.ToInt32(clslist.CommentID);
                var commentsList = dashDB.CSLInferences.Where(x => x.CommentID == no).FirstOrDefault();
                dashDB.CSLInferences.Remove(commentsList);
                dashDB.SaveChanges();
                return "Comment Deleted";
            }
            catch (Exception ex)
            {
                return ex.Message + ex.InnerException;
            }

        }
        

    }


}
