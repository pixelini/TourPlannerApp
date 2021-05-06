using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlannerApp.DAL
{
    public class TourLookupPostRequestBody
    {
        public class Options
        {
            public string routeType { get; set; }
            public string locale { get; set; }
            public string unit { get; set; }
        }

        public class RequestBodyContent
        {
            public List<string> locations { get; set; }
            public Options options { get; set; }
        }

    }
}
