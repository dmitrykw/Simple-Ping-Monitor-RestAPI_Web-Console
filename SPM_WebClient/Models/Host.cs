﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPM_WebClient.Models
{
    public class UpdateHostObj 
    { 
    
        public string hostname { get; set; }
        public string description { get; set; }
        public string groupname { get; set; }
        public string selectedgroupname { get; set; }
        public string hostid { get; set; }
        public bool? isnotifyenabled { get; set; }
        public bool? isenabled { get; set; }
        public string imgpath { get; set; }


        public bool? isenabledcustomnotificationsettings { get; set; }

        public bool? customagentnotifyifcpuisoverload { get; set; }
        public int? customagentcpuoverloadpercent { get; set; }
        public bool? customagentnotifyiflowfreemem { get; set; }
        public int? customagentlowfreememmegabytes { get; set; }
        public bool? customagentnotifylowdisksfreespace { get; set; }
        public int? customagentlowdisksfreespacemegabytes { get; set; }
        public bool? customagentnotifyifdisksoverload { get; set; }
        public int? customagentdisksoverloadpercent { get; set; }
        public bool? customagentnotifyifnetadaptersoverload { get; set; }
        public int? customagentnetadaptersoverloadpercent { get; set; }

        public bool? customnotifyifagentconnectionlost { get; set; }

        public bool? isenabledcustomothernotificationsettings { get; set; }

        public bool? customnotifyifanswertimeislong { get; set; }
        public int? customlonganswertimemiliseconds { get; set; }

        public bool? customeventlognotifyifcomputerrestarted { get; set; }
        public bool? customeventlognotifyifcomputergoingreboot { get; set; }
        public bool? customeventlognotifyifcomputerrebootedbyuser { get; set; }
        public bool? customeventlogforwardcriticalevents { get; set; }
        public bool? customeventlogforwarderrorevents { get; set; }

        public bool? isenabledcustomemail { get; set; }
        public string customemail { get; set; }

    }


    public class Host
    {
        public string HostCaptionMessage
        {
            get
            {
                if (HostAgentDataUpdatedDateTime.AddMinutes(15) < DateTime.Now)
                {
                    return "Cannot connect to the Host Agent service. Please install the Agent service on this host. Or check encription key.";
                }
                return "Agent is Connected";
            }
        }

        private string agentversion;
        public string AgentVersion { 
            get
            {
                if (agentversion == "") { return ""; }
                if (agentversion == "1.0.0.1") { return "v.1.0.0.1 - Update this Agent"; }
                if (agentversion == "1.0.0.2") { return "v.1.0.0.2 - Update this Agent"; }
                return "v." + agentversion;
            }

            set
            {
                agentversion = value;
            }
        }

        public string HostCaptionMessage_CSSClass
        {
            get
            {
                if (HostType == "WebHost")
                { return "dtls_caption_green"; }

                if (HostAgentDataUpdatedDateTime.AddMinutes(15) < DateTime.Now)
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

        private string statustext;
        public string StatusText
        {
            get { return IsEnabled ? statustext : "Host disabled"; }
            set { statustext = value; }
        }

        
        public bool Status { get; set; }
        
        public string ImgPath { get; set; }        


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

        public List<KeyValuePair<string, dynamic>> HostCustomOptions { get; set; }

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
            HostCustomOptions = new List<KeyValuePair<string, dynamic>>();
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