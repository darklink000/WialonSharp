using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WialonSharp
{
   public class Models
    {
        public class Wi_Report
        {
            public Wi_reportResult reportResult { get; set; }
        }

        public class Wi_reportResult
        {
            public List<Wi_Tables> tables { get; set; }
        }

        public class Wi_Tables
        {
            public string name { get; set; }
        }
        public class Wi_vehicles
        {
            public int i { get; set; }
            public Wi_vehicles_details d { get; set; }
            public int f { get; set; }
        }
        public class Wi_vehicles_details
        {
            public string nm { get; set; }
            public int cls { get; set; }
            public int id { get; set; }
            public int mu { get; set; }
            public long uacl { get; set; }
        }
        public class Wi_Login
        {
            public string host { get; set; }
            public string eid { get; set; }
            public string gis_sid { get; set; }
            public string au { get; set; }
            public string tm { get; set; }
            public string base_url { get; set; }
            public Wi_user user { get; set; }
        }
        public class Wi_user
        {
            public int id { get; set; }
            public string nm { get; set; }
            public string cls { get; set; }
        }
    }
}
