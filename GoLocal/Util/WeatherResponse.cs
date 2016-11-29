using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoLocal.Util
{
    public class WeatherResponse
    {
        public CurrDetail currently { get; set; }
        public DailyDetail daily { get; set; }
    }
}