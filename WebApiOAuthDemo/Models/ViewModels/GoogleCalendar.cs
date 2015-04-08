using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApiOAuthDemo.Models.ViewModels
{
    public class GoogleCalendar
    {
        [Display(Name = "名稱")]
        public string Name { get; set; }

        [Display(Name = "起始日期")]
        public DateTime StartDate { get; set; }

        [Display(Name = "結束日期")]
        public DateTime EndDate { get; set; }

        public string Link { get; set; }
    }
}