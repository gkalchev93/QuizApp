using QuizApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizApp.Controllers
{
    public class QuizzController : Controller
    {
        public QuizzDbEntities dbContext = new QuizzDbEntities();

        // GET: Quizz
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetUser(UserVM user)
        {
            UserVM userConnected = dbContext.Users.Where(u => u.FullName == user.FullName)
                                         .Select(u => new UserVM
                                         {
                                             UserID = u.UserID,
                                             FullName = u.FullName
                                         }).FirstOrDefault();

            if (userConnected != null)
            {
                Session["UserConnected"] = userConnected;
                return RedirectToAction("QuizzSettings");
            }
            else
            {
                ViewBag.Msg = "Sorry : user is not found !!";
                return View();
            }

        }

        [HttpGet]
        public ActionResult QuizzSettings()
        {
            QuizzModeVM modeList = new QuizzModeVM();
            modeList.Modes = new List<SelectListItem>()
            {
                new SelectListItem(){ Text="Yes or No", Value= "0"},
                new SelectListItem(){ Text="4 Choices", Value= "1"},
                new SelectListItem(){ Text="Free Text", Value= "2"}
            };

            return View(modeList);
        }

        [HttpPost]
        public ActionResult QuizzSettings(QuizzModeVM quizMode)
        {
             Session["SelectedMode"] = quizMode.Mode;

             return RedirectToAction("QuizzView");
        }

        [HttpGet]
        public ActionResult QuizzView()
        {
            int selectedMode = (int)Session["SelectedMode"];

            Random rand = new Random();
            int toSkip = rand.Next(0, dbContext.Questions.Count());
            var tmpQ = dbContext.Questions.OrderBy(x => x.QuestionID).Skip(toSkip).Take(1).First();
            QuestionVM question = new QuestionVM()
            {
                QuestionID = tmpQ.QuestionID,
                QuestionText = tmpQ.QuestionText,
                Anwser = tmpQ.Answer,
                Choices = new List<ChoiceVM>()
            };

            foreach(var c in tmpQ.Choices)
            {
                question.Choices.Add(new ChoiceVM() { ChoiceID = c.ChoiceID, ChoiceText = c.ChoiceText });
            }

            return View(question);
        }
    }
}