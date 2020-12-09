using GMS___Model;
using GMS___Web_Client.Models;
using System;
using System.Collections;
using System.Net;
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
                try
                {
                    Session["Guild"] = "";
                    ViewBag.ApiToken = Session["ApiToken"];
                    ViewBag.UserToken = Session["UserToken"];
                    ViewBag.Message = "Your user page.";
                    return View();
                } catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.Timeout)
                    {
                        ViewBag.Error = "The login action timed out. Try again later.";
                        return RedirectToAction("LogIn", "Auth");
                    }
                    return RedirectToAction("ApiForm", "User");
                } catch (Exception)
                {
                    return RedirectToAction("ApiForm", "User");
                }
            }
            ViewBag.Error = "You aren't authorized to access this page.";
            return RedirectToAction("Index", "Home");
        }
        public ActionResult CharacterPage(string name)
        {
            if (InSession())
            {
                //string urlSuffix = "gw2api/characters/" + name;
                //ViewBag.Character = GetJson<Character>(urlSuffix + "/core");
                //ViewBag.Equipment = InitializeEquipment(GetJson<Equipments>(urlSuffix + "/equipment"));
                Character character = GetJson<Character>("gw2api/characters/" + name + "/core");
                this.Session["Guild"] = character.Guild;
                ViewBag.Guild = character.Guild;
                ViewBag.Character = name;
                ViewBag.ApiToken = Session["ApiToken"];
                ViewBag.Message = "Your character page.";
                return View();
            }
            ViewBag.Error = "You aren't authorized to access this page.";
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ApiForm()
        {
            if (InSession())
            {
                ViewBag.Message = "Your API form.";
                return View();
            }
            ViewBag.Error = "You aren't authorized to access this page.";
            return RedirectToAction("Index", "Home");
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

        public ActionResult Account()
        {
            if (InSession())
            {
                var model = new UserModel();
                ViewBag.AccountCreated = Session["AccountCreated"];
                model.UserName = Session["Username"].ToString();
                model.EmailAddress = Session["EmailAddress"].ToString();
                ViewBag.Message = "Your account details.";
                return View(model);
            } else
            {
                ViewBag.Error = "You aren't authorized to access this page.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}