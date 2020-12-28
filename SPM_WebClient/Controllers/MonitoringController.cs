using Newtonsoft.Json;
using SPM_WebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SPM_WebClient.Controllers
{
    // [Authorize]
    public class MonitoringController : Controller
    {        
        

        // GET: Monitoring
        
        public ActionResult Index(string group_filter = "All Hosts")
        {
            MonitoringViewModel viewModel;
            if (group_filter != "All Hosts")
            { viewModel = new MonitoringViewModel(group_filter); }
            else { viewModel = new MonitoringViewModel(); }         
            return GetView(viewModel);
        }

        public ActionResult Search(string search_filter)
        {
            if (search_filter != "")
            {
                MonitoringViewModel viewModel = new MonitoringViewModel(search_filter, true);
                return GetView(viewModel);
            }else
            {
                MonitoringViewModel viewModel = new MonitoringViewModel();
                return GetView(viewModel);
            }
        }


        private ActionResult GetView(MonitoringViewModel viewModel)
        {

            if (viewModel.Hosts.Count > 0)
            {
                switch (viewModel.Hosts.FirstOrDefault().HostVisualType)
                {
                    case "SmallMonitor":
                        return View("SmallMonitorView", viewModel);
                    case "String":
                        return View("StringView", viewModel);

                    default:
                        return View("Index",viewModel);
                }
            }
            else { return View("Index", viewModel); }
        }


        public ActionResult SmallMonitorView(string group_filter = "All Hosts")
        {
            MonitoringViewModel viewModel;
            if (group_filter != "All Hosts")
            {viewModel = new MonitoringViewModel(group_filter);}
            else
            { viewModel = new MonitoringViewModel(); }

            return View("SmallMonitorView", viewModel);
        }
        public ActionResult StringView(string group_filter = "All Hosts")
        {
            MonitoringViewModel viewModel;
            if (group_filter != "All Hosts")
            {viewModel = new MonitoringViewModel(group_filter);}
            else
            {viewModel = new MonitoringViewModel();}

            return View("StringView", viewModel);
        }



        public ActionResult Details(int id)
        {
            MonitoringDetailsViewModel viewModel = new MonitoringDetailsViewModel(id);

            Host myhost = viewModel.Hosts.FirstOrDefault(x => x.ID == id);
            if (myhost != null)
                return PartialView(myhost);
            return HttpNotFound();
        }

        public ActionResult HostLog(int id)
        {
            MonitoringHostLogViewModel viewModel = new MonitoringHostLogViewModel(id);
            KeyValuePair<int, string> hostlog = new KeyValuePair<int, string>();
            if (viewModel.HostsLogs.Count() > 0)
            {
                hostlog = viewModel.HostsLogs.FirstOrDefault(x => x.Key == id);
                return View(hostlog);
            }
            else {return View(hostlog);}
            
        }


        [HttpPost]
        public ActionResult Index(UpdateHostObj model)
        {
            if (model != null)
            {
                try
                {
                    string curr_hostid = model.hostid;
                    int curr_hostid_int = 0;
                    int.TryParse(curr_hostid, out curr_hostid_int);


                    MonitoringViewModel viewModel = new MonitoringViewModel(curr_hostid_int);


                    viewModel.SendHostUpdateAPI(model);


                    return Json(new { status = 1, message = "Update Host Data Success" });
                }
                catch(Exception ex) { return Json(new { status = 0, message = "Failed Update Host Data. Exception: " + ex.Message }); }
         
            }
            return Json(new { status = 0, message = "Failed Update Host Data. Post Object is null" });
        }

        [HttpPost]
        public ActionResult AddHost(string selectedgroupname, string hostname, string description, string hosttype)
        {
            string group_filter = selectedgroupname;
            try
            {                
                MonitoringViewModel viewModel = new MonitoringViewModel(group_filter);
                viewModel.AddHostAPI(hostname, description, selectedgroupname, hosttype);
                System.Threading.Thread.Sleep(2200);
            }
            catch {}
            

            //return RedirectToRoute(new { controller = "Monitoring", action = "Index" });
            //return RedirectToAction("Index", "Monitoring", new { group_filter = group_filter });
            // return RedirectToAction("Index",viewModel);
            // return View("Index", viewModel);

         return RedirectToAction("Index", "Monitoring", new { group_filter = group_filter }); 
        }


        [HttpPost]
        public ActionResult RemoveHost(string selectedgroupname, string hostid)
        {
            string group_filter = selectedgroupname;
            try
            {
                MonitoringViewModel viewModel = new MonitoringViewModel(group_filter);

               // if (group_filter != "All Hosts")
               // {
               //     viewModel = new MonitoringViewModel(group_filter);
               // }
               // else
               // {
                //    viewModel = new MonitoringViewModel();
                //}


                int hostid_int = 0;
                int.TryParse(hostid, out hostid_int);

                viewModel.RemoveHostAPI(hostid_int);
                System.Threading.Thread.Sleep(2200);
            }
            catch { }

            //viewModel.Hosts.Remove(viewModel.Hosts.Where(x => x.ID == hostid_int).FirstOrDefault());           
        

          return RedirectToAction("Index", "Monitoring", new { group_filter = group_filter }); 

        }
    }
}