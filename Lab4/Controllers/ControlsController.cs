using Lab4.Models.Controlls;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Lab4.Controllers
{
    public class ControlsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        #region TextBox

        [HttpGet]
        public IActionResult TextBox()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TextBox([Required] TextBoxModel model)
        {
            return View(model);
        }

        #endregion

        #region TextArea

        [HttpGet]
        public IActionResult TextArea()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TextArea(TextAreaModel model)
        {
            return View(model);
        }

        #endregion

        #region CheckBox

        [HttpGet]
        public IActionResult CheckBox()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckBox(CheckBoxModel model)
        {
            return View(model);
        }

        #endregion

        #region Radio

        [HttpGet]
        public IActionResult Radio()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Radio(RadioModel model)
        {
            return View(model);
        }

        #endregion

        #region DropDownList

        [HttpGet]
        public IActionResult DropDownList()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DropDownList(DropDownListModel model)
        {
            return View(model);
        }

        #endregion

        #region ListBox

        [HttpGet]
        public IActionResult ListBox()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ListBox(ListBoxModel model)
        {
            return View(model);
        }

        #endregion
    }
}
