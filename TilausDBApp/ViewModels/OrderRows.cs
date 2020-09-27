using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TilausDBApp.ViewModels
{
    public class OrderRows
    {
        public int TilausriviID { get; set; }
        public Nullable<int> TilausID { get; set; }
        public Nullable<int> TuoteID { get; set; }
        public Nullable<int> Maara { get; set; }
        public Nullable<decimal> Ahinta { get; set; }

        public string Nimi { get; set; }

    }
}