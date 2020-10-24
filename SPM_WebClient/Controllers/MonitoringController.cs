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
            if (group_filter != "All Hosts")
            {
                MonitoringViewModel viewModel = new MonitoringViewModel(group_filter);
                return View(viewModel);
            }
            else
            {
                MonitoringViewModel viewModel = new MonitoringViewModel();
                return View(viewModel);
            }
        }
        public ActionResult Details(string hostname)
        {
            MonitoringDetailsViewModel viewModel = new MonitoringDetailsViewModel(hostname);

            Host myhost = viewModel.Hosts.FirstOrDefault(x => x.Hostname == hostname);
            if (myhost != null)
                return PartialView(myhost);
            return HttpNotFound();
        }


        [HttpPost]
        public ActionResult Index(string selectedgroupname, string hostid, string hostname, string description, string groupname, bool? isnotifyenabled, bool? isenabled)
        {           
            string group_filter = selectedgroupname;

            MonitoringViewModel viewModel;

            if (group_filter != "All Hosts")
            {
                 viewModel = new MonitoringViewModel(group_filter);              
            }
            else
            {
                 viewModel = new MonitoringViewModel();              
            }


            int hostid_int = 0;
            int.TryParse(hostid, out hostid_int);

            viewModel.SendHostUpdateAPI(hostid_int, hostname, description, groupname, isnotifyenabled.HasValue ? isnotifyenabled.Value: false, isenabled.HasValue? isenabled.Value:false);

           
            foreach (Host host in viewModel.Hosts.Where(x=>x.ID == hostid_int))
            {
                host.Hostname = hostname;
                host.Description = description;
                host.GroupName = groupname;
                host.IsNotifyEnabled = isnotifyenabled.HasValue?isnotifyenabled.Value:false;
                host.IsEnabled = isenabled.HasValue?isenabled.Value:false;                
            }

            return View(viewModel);
            //return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddHost(string selectedgroupname, string hostname, string description, string hosttype)
        {
            string group_filter = selectedgroupname;

            MonitoringViewModel viewModel = new MonitoringViewModel(group_filter);

            
            viewModel.AddHostAPI(hostname, description, selectedgroupname, hosttype);


            System.Threading.Thread.Sleep(2200);

        
            //return RedirectToRoute(new { controller = "Monitoring", action = "Index" });
            return RedirectToAction("Index", "Monitoring", new { group_filter = group_filter });
            // return RedirectToAction("Index",viewModel);
            // return View("Index", viewModel);
        }


        [HttpPost]
        public ActionResult RemoveHost(string selectedgroupname, string hostid)
        {
            string group_filter = selectedgroupname;

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


            //viewModel.Hosts.Remove(viewModel.Hosts.Where(x => x.ID == hostid_int).FirstOrDefault());

            System.Threading.Thread.Sleep(2200);
            //return RedirectToAction("Index");
            //    return View("Index",viewModel);
            return RedirectToAction("Index", "Monitoring", new { group_filter = group_filter });
        }
    }
}