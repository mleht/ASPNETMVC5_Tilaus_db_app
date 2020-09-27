using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TilausDBApp.ViewModels
{
    public class TilauksetViikonpaivaClass
    {
        public string weekday { get; set; }
        public Nullable<int> order_times { get; set; }
    }
}