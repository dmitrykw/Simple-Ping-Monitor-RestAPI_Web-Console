using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPM_WebClient.Models
{
    public class Host
    {
        public string HostCaptionMessage
        {
            get
            {
                if (HostAgentDataUpdatedDateTime == DateTime.MinValue)
                {
                    return "Cannot connect to the Host Agent service. Please install the Agent service on this host. Or check encription key.";
                }
                return "Agent is Connected";
            }
        }
        public string HostCaptionMessage_CSSClass
        {
            get
            {
                if (HostAgentDataUpdatedDateTime == DateTime.MinValue)
                {
                    return "dtls_caption_red";
                }
                return "dtls_caption_green";
            }
        }

        public int ID { get; set; }
        public string Hostname { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsNotifyEnabled { get; set; }
        public string HostType { get; set; }
        public string HostVisualType { get; set; }

        private string statustext { get; set; }
        public string StatusText
        {
            get { return IsEnabled ? statustext : "Host disabled"; }
            set { statustext = value; }
        }

        
        public bool Status { get; set; }
        private string imgpath;
        public string ImgPath
        {
            get
            {               
                return @"url(\\Content\\Images\\" + imgpath + ")";
            }
            set { imgpath = value; }
        }


        public DateTime HostAgentDataUpdatedDateTime { get; set; }
        public double CpuLoad { get; set; }
        public string CpuLoadString { get { return CpuLoad.ToString().Replace(',', '.'); } }
        public double TotalMemory { get; set; }

        public string TotalMemoryString
        {
            get
            {
                return TotalMemory.ToFormatedString();
            }
        }
        public double UsedMemory
        {
            get
            {
                return TotalMemory - AvailableMemory;
            }
        }
        public double AvailableMemory { get; set; }
        public string AvailableMemoryString
        {
            get
            {
                return AvailableMemory.ToFormatedString();
            }
        }


        public List<LogicalDisc> LogicalDisks { get; set; }
        public List<NetworkAdapter> NetworkAdapters { get; set; }

        public string CpuLoad_progress_classname
        {
            get
            {
                if (CpuLoad > 85)
                { return "progress-bar-danger"; }
                else if (CpuLoad > 60)
                { return "progress-bar-warning"; }
                else
                { return "progress-bar-success"; }
            }
        }


        public double UsedMemory_Percent
        {
            get
            {
                return (UsedMemory / TotalMemory) * 100;
            }
        }
        public string UsedMemory_PercentString { get { return UsedMemory_Percent.ToString().Replace(',', '.'); } }

        public string UsedMemory_progress_classname
        {
            get
            {
                if (UsedMemory_Percent > 85)
                { return "progress-bar-danger"; }
                else if (UsedMemory_Percent > 60)
                { return "progress-bar-warning"; }
                else
                { return "progress-bar-success"; }
            }
        }

        public Host()
        {
            HostAgentDataUpdatedDateTime = DateTime.MinValue;
            LogicalDisks = new List<LogicalDisc>();
            NetworkAdapters = new List<NetworkAdapter>();
        }


        public string StatusText_0string
        {
            get
            {
                try
                {
                    return (StatusText.Split('|').Count() > 0) ? StatusText.Split('|')[0] : "";
                }
                catch { return ""; }
            }
        }
        public string StatusText_1string
        {
            get
            {
                try
                {
                    return (StatusText.Split('|').Count() > 1) ? StatusText.Split('|')[1] : "";
                }
                catch { return ""; }
            }
        }
        public string StatusText_2string
        {
            get
            {
                try { 
                return (StatusText.Split('|').Count() > 2) ? StatusText.Split('|')[2] : "";
                }
                catch { return ""; }
            }
        }
        public string StatusText_3string
        {
            get
            {
                try { 
                return (StatusText.Split('|').Count() > 3) ? StatusText.Split('|')[3] : "";
                }
                catch { return ""; }
            }
        }
        public string StatusText_4string
        {
            get
            {
                try { 
                return (StatusText.Split('|').Count() > 4) ? StatusText.Split('|')[4] : "";
                }
                catch { return ""; }
            }
        }
        public string StatusText_5string
        {
            get
            {
                try { 
                return (StatusText.Split('|').Count() > 5) ? StatusText.Split('|')[5] : "";
                }
                catch { return ""; }
            }
        }
        public string StatusText_6string
        {
            get
            {
                try { 
                return (StatusText.Split('|').Count() > 6) ? StatusText.Split('|')[6] : "";
                }
                catch { return ""; }
            }
        }
        public string StatusText_7string
        {
            get
            {
                try
                {
                    return (StatusText.Split('|').Count() > 7) ? StatusText.Split('|')[7] : "";
                }
                catch { return ""; }
        }
        }
    }


}