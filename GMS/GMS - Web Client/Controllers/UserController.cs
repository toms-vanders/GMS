using GMS___Model;
using GMS___Web_Client.Models;
using System;
using System.Collections;
using System.Web.Mvc;

namespace GMS___Web_Client.Controllers
{
    public class UserController : AuthController
    {
        // GET: User

        public ActionResult UserPage()
        {
            if (InSession())
            {
                this.Session["Guild"] = "";
                ArrayList characterList = new ArrayList();
                ArrayList characterNameList = GetJson<ArrayList>("gw2api/characters");
                foreach (string name in characterNameList)
                {
                    string urlSuffix = "gw2api/characters/" + name + "/core";
                    characterList.Add(GetJson<Character>(urlSuffix));
                }
                ViewBag.Characters = characterList;
                ViewBag.Message = "Your user page.";
                return View();
            }
            ViewBag.Error = "You aren't authorized to access this page.";
            return RedirectToAction("Index", "Home");
        }
        public ActionResult CharacterPage(string name)
        {
            if (InSession())
            {
                string urlSuffix = "gw2api/characters/" + name;
                ViewBag.Character = GetJson<Character>(urlSuffix + "/core");
                ViewBag.Message = "Your character page.";
                ViewBag.Equipment = InitializeEquipment(GetJson<Equipments>(urlSuffix + "/equipment"));
                this.Session["Guild"] = ViewBag.Character.Guild;
                return View();
            }
            ViewBag.Error = "You aren't authorized to access this page.";
            return RedirectToAction("Index","Home");
        }
        public ActionResult ApiForm()
        {
            if (InSession())
            {
                ViewBag.Message = "Your API form.";
                return View();
            }
            ViewBag.Error = "You aren't authorized to access this page.";
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApiForm(ApiModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.EmailAddress = this.Session["EmailAddress"].ToString();
                user.ApiKey = model.ApiKey;
                if (PostJson("api/user/insertapi", user) != null)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.Error = "Invalid information was given.";
            return View();
        }

        public ArrayList InitializeEquipment(Equipments jsonList)
        {
            ArrayList equipment = new ArrayList();
            foreach (EquipmentSlot item in jsonList.Equipment)
            {
                try
                {
                    equipment.Add(GetJson<Item>("gw2api/items/" + item.Id));
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return equipment;
        }
    }
}