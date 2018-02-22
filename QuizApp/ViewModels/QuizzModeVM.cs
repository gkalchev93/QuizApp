using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizApp.ViewModels
{
    public class QuizzModeVM
    {
        public int Mode { get; set; }
        public IEnumerable<SelectListItem> Modes { get; set; }
    }
}