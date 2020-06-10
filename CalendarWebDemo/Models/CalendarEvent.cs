﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalendarWebDemo.Models
{
    public partial class CalendarEvent
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Location { get; set; }
        public string Summary { get; set; }

    }
}