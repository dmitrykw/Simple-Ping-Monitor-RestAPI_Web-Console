using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Net;
using System.Text;
using System.IO;

namespace SPM_WebClient.Models
{
    public class MonitoringViewModel
    {
        public List<Host> Hosts = new List<Host>();
        public List<Group> Groups = new List<Group>();
        string cfgpath;
        string url;
        string apikey;
        bool isreadonly;

        public bool IsReadOnly { get { return isreadonly; } }

        public string GroupsHeader = "Groups:";

        private void GetConfig()
        {
            cfgpath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Config/config.cfg");
            INIManager manager = new INIManager(cfgpath);
            //Получить значение по ключу name из секции APICONFIG
            url = manager.GetPrivateString("APICONFIG", "api_hostname");
            apikey = manager.GetPrivateString("APICONFIG", "api_key");
            isreadonly = (manager.GetPrivateString("APICONFIG", "readonly").ToLower() == "true")? true : false;            
        }

        private void FillHosts()
        {                       
            using (WebClient wc = new WebClient())
            {
                string JSON = "";
                try
                {
                    wc.Encoding = Encoding.UTF8;
                    JSON = wc.DownloadString(url + "/api/Hosts?api_token=" + apikey);

                    Hosts = JsonConvert.DeserializeObject<List<Host>>(JSON);               

                }
                catch { GroupsHeader = "You have no hosts or have API connection error. Check API configuration file (Config\\config.cfg)."; }
            }

        }

        private void FillHosts(string group_filter)
        {
            using (WebClient wc = new WebClient())
            {
                string JSON = "";
                try
                {

                    wc.Encoding = Encoding.UTF8;
                    
                    JSON = wc.DownloadString(url + "/api/Groups?name=" + group_filter + "&api_token=" + apikey);

                    Hosts = JsonConvert.DeserializeObject<List<Host>>(JSON);
                }
                catch { GroupsHeader = "You have no hosts or have API connection error. Check API configuration file (Config\\config.cfg)."; }
            }

        }

        private void FillHosts(string search_filter, bool is_search)
        {
            if (is_search)
            {
                using (WebClient wc = new WebClient())
                {
                    string JSON = "";
                    try
                    {

                        wc.Encoding = Encoding.UTF8;

                        JSON = wc.DownloadString(url + "/api/Hosts?name=" + search_filter + "&is_part_of_name=true&api_token=" + apikey);

                        Hosts = JsonConvert.DeserializeObject<List<Host>>(JSON);
                    }
                    catch { GroupsHeader = "You have no hosts or have API connection error. Check API configuration file (Config\\config.cfg)."; }
                }
            }

        }

        private void FillGroups()
        {
            using (WebClient wc = new WebClient())
            {
                string JSON = "";
                try
                {
                    wc.Encoding = Encoding.UTF8;
                   
                    JSON = wc.DownloadString(url + "/api/Groups?api_token=" + apikey);

                    Groups = JsonConvert.DeserializeObject<List<Group>>(JSON);
                }
                catch { }
            }
        }

        public MonitoringViewModel()
        {
            GetConfig();

            FillHosts();
            FillGroups();

            Groups.Where(x => x.Name == "All Hosts").ToList().ForEach(c => c.IsActivated = true);

          
        }

        public MonitoringViewModel(string group_filter)
        {

            GetConfig();

            if (group_filter == null || group_filter == "") { FillGroups(); return; }
            

            FillHosts(group_filter);
            FillGroups();

            Groups.Where(x => x.Name == group_filter).ToList().ForEach(c => c.IsActivated = true);

           
        }
        public MonitoringViewModel(string search_filter, bool is_search)
        {
            GetConfig();

            if (search_filter == null || search_filter == "") { FillGroups(); return; }

            FillHosts(search_filter, is_search);
            FillGroups();

            SearchFieldText = search_filter;
            //Groups.Where(x => x.Name == group_filter).ToList().ForEach(c => c.IsActivated = true);


        }



        public void SendHostUpdateAPI(int hostid_int, string hostname, string description, string groupname, bool? isnotifyenabled, bool? isenabled)
        {
            if (isreadonly) { return; }

            using (WebClient wc = new WebClient())
            {
                string JSON = "";
                try
                {


                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/Hosts"); //- Токен надо передать в header е

                    request.Method = "PUT";
                    request.ContentType = "application/json; charset=utf-8";
                    request.Accept = "application/json; charset=utf-8";
                    request.Headers.Add("api_token", apikey);

                   
                    
                    StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), Encoding.UTF8);

                    JSON = JsonConvert.SerializeObject(new { ID = hostid_int, Hostname = hostname, Description = description, GroupName = groupname, IsNotifyEnabled = isnotifyenabled.HasValue? isnotifyenabled.Value : false, IsEnabled = isenabled.HasValue? isenabled.Value:false });

                    requestWriter.Write(JSON);

                    requestWriter.Close();


                    try
                    {
                        WebResponse webResponse = request.GetResponse();

                   //     Stream webStream = webResponse.GetResponseStream();
                  //      StreamReader responseReader = new StreamReader(webStream);
                   //     string response = responseReader.ReadToEnd();
                   //     responseReader.Close();
                    }
                    catch { }

                }
                catch { }
            }
        }

        public void RemoveHostAPI(int hostid_int)
        {
            if (isreadonly) { return; }

            using (WebClient wc = new WebClient())
            {
                string JSON = "";
                try
                {


                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/Hosts"); //- Токен надо передать в header е

                    request.Method = "DELETE";
                    request.ContentType = "application/json; charset=utf-8";
                    request.Accept = "application/json; charset=utf-8";
                    request.Headers.Add("api_token", apikey);



                    StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), Encoding.UTF8);

                    JSON = JsonConvert.SerializeObject(new { ID = hostid_int});

                    requestWriter.Write(JSON);

                    requestWriter.Close();


                    try
                    {
                        WebResponse webResponse = request.GetResponse();

                        //     Stream webStream = webResponse.GetResponseStream();
                        //      StreamReader responseReader = new StreamReader(webStream);
                        //     string response = responseReader.ReadToEnd();
                        //     responseReader.Close();
                    }
                    catch { }

                }
                catch { }
            }
        }


        public void AddHostAPI(string hostname, string description, string groupname, string hosttype)
        {
            if (isreadonly) { return; }

            using (WebClient wc = new WebClient())
            {
                string JSON = "";
                try
                {


                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/Hosts"); //- Токен надо передать в header е

                    request.Method = "POST";
                    request.ContentType = "application/json; charset=utf-8";
                    request.Accept = "application/json; charset=utf-8";
                    request.Headers.Add("api_token", apikey);



                    StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), Encoding.UTF8);

                    JSON = JsonConvert.SerializeObject(new { Hostname = hostname, Description = description, GroupName = groupname, HostType = hosttype });

                    requestWriter.Write(JSON);

                    requestWriter.Close();


                    try
                    {
                        WebResponse webResponse = request.GetResponse();

                        //     Stream webStream = webResponse.GetResponseStream();
                        //      StreamReader responseReader = new StreamReader(webStream);
                        //     string response = responseReader.ReadToEnd();
                        //     responseReader.Close();
                    }
                    catch { }

                }
                catch { }
            }
        }

        public string SelectedGroupName {
            get
            {
                if (Groups.Where(x => x.IsActivated).Count() > 0)
                {
                    return Groups.Where(x => x.IsActivated).ToList().FirstOrDefault().Name;
                }
                else { return "nogroupselected"; }
            }
        }
        public string SearchFieldText { get; set; }    

}



    public class MonitoringDetailsViewModel
    {

        public List<Host> Hosts = new List<Host>();        
        string cfgpath;
        string url;
        string apikey;

        public MonitoringDetailsViewModel(string hostname)
        {
            GetConfig();

            FillHosts(hostname);            
                 
        }




        private void GetConfig()
        {
            cfgpath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Config/config.cfg");
            INIManager manager = new INIManager(cfgpath);
            //Получить значение по ключу name из секции APICONFIG
            url = manager.GetPrivateString("APICONFIG", "api_hostname");
            apikey = manager.GetPrivateString("APICONFIG", "api_key");
        }

        private void FillHosts(string hostname)
        {
            using (WebClient wc = new WebClient())
            {
                string JSON = "";
                try
                {
                    wc.Encoding = Encoding.UTF8;
                    JSON = wc.DownloadString(url + "/api/Hosts?name=" + hostname + "&api_token=" + apikey);

                    Hosts = JsonConvert.DeserializeObject<List<Host>>(JSON);
                }
                catch { }
            }

        }

    }
}