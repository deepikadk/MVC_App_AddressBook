using MVCContactBook.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MVCContactBook.Controllers
{
    public class HomeController : Controller
    {
        //Home 
        public ActionResult Index()
        {
            
            List<ContactModel> contacts = new List<ContactModel>();
            using (AddressBookEntities dc = new AddressBookEntities())
            {
                var v = (from a in dc.People
                         join b in dc.Organizations on a.OrganizationID equals b.OrganizationID
                        
                          select new ContactModel
                         {
                             PersonID = a.PersonID,
                             FirstName = a.FirstName,
                             LastName = a.LastName,
                             Address = a.Address,
                             ContactNo = a.ContactNo,
                             Email = a.Email,
                             OrganizationID = b.OrganizationID,
                             OrganizationName = b.OrganizationName
                             

                         }).ToList();
                contacts = v;
            }
            return View(contacts);
            
        }
        public ActionResult IndexPpl()
        {
            List<ContactModel> contacts = new List<ContactModel>();
            
            using (AddressBookEntities dc = new AddressBookEntities())
            {
                var o = (from a in dc.People
                         join b in dc.Organizations on a.OrganizationID equals b.OrganizationID

                         select new ContactModel
                         {
                             OrganizationID = a.OrganizationID,
                             OrganizationName = b.OrganizationName,
                             PersonID = a.PersonID,
                             FirstName = a.FirstName.Trim(),
                             LastName = a.LastName.Trim(),
                             Address = a.Address.Trim(),
                             ContactNo = a.ContactNo,
                             Email = a.Email.Trim(),

                         });
                contacts = o.ToList();
            }
            List<ContactModel> allNames = new List<ContactModel>();

            using (AddressBookEntities dc = new AddressBookEntities())
            {
                var w = (from a in dc.Organizations
                         select new ContactModel
                         {
                             OrganizationID = a.OrganizationID,
                             OrganizationName = a.OrganizationName
                         });
                allNames = w.ToList();
            }
            ViewBag.OrganizationName = new SelectList(allNames, "OrganizationID", "OrganizationName");
            return View(contacts);
        }


        [HttpPost]
        public JsonResult AddPeople(Person contact)
        {
            
            using (AddressBookEntities entities = new AddressBookEntities())
            {
                entities.People.Add(contact);
                entities.SaveChanges();
                return Json(contact);
            }


        }
        [HttpPost]
        public ActionResult saveuser(int id, string propertyName, string value)
        {
            var status = false;
            var message = "";
            using (AddressBookEntities dc = new AddressBookEntities())
            {
                if (value.GetType() == typeof(string) && propertyName == "ContactNo")
                {
                    int v = Convert.ToInt32(value);
                    //value = v;
                    var u = dc.People.Find(id);
                    if (u != null)
                    {
                        dc.Entry(u).Property(propertyName).CurrentValue = v;
                        //status = true;
                        dc.SaveChanges();
                        status = true;
                    }
                    else
                    {
                        message = "Error";
                    }
                }
                else
                {
                    var u = dc.People.Find(id);
                    if (u != null)
                    {
                        dc.Entry(u).Property(propertyName).CurrentValue = value;
                        status = true;
                        dc.SaveChanges();
                        status = true;
                    }
                    else
                    {
                        message = "Error";
                    }
                }
            }
                var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());
        }
        public ActionResult IndexOrg()
        {   
            List<ContactModel> contacts = new List<ContactModel>();
            using (AddressBookEntities dc = new AddressBookEntities())
            {
                var o = (from b in dc.Organizations 

                         select new ContactModel
                         {
                             OrganizationID = b.OrganizationID,
                             OrganizationName = b.OrganizationName.Trim(),
                             Address = b.Address.Trim(),
                             ContactNo = b.ContactNo,
                             Email = b.Email.Trim()

                         }).ToList();
                contacts = o;
            }
               return View(contacts);
        }
        [HttpPost]
        public JsonResult AddOrganization(Organization contact)
        {
            using (AddressBookEntities entities = new AddressBookEntities())
            {
                entities.Organizations.Add(contact);
                entities.SaveChanges();
                return Json(contact);            
            }
        }
        [HttpPost]
        public ActionResult saveuser1(int id, string propertyName, string value)
        {
            var status = false;
            var message = "";
            using (AddressBookEntities dc = new AddressBookEntities())
            {

                if (value.GetType() == typeof(string) && propertyName == "ContactNo")
                {
                    int v = Convert.ToInt32(value);
                    //value = v;
                    var u = dc.Organizations.Find(id);
                    if (u != null)
                    {
                        dc.Entry(u).Property(propertyName).CurrentValue = v;
                        //status = true;
                        dc.SaveChanges();
                        status = true;
                    }
                    else
                    {
                        message = "Error";
                    }
                }
                else
                {
                    var u = dc.Organizations.Find(id);
                    if (u != null)
                    {
                        dc.Entry(u).Property(propertyName).CurrentValue = value;
                        //status = true;
                        dc.SaveChanges();
                        status = true;
                    }
                    else
                    {
                        message = "Error";
                    }
                }
            }
            var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());
        }
        
        public ActionResult DeleteOrg(int id)
        {
            using (AddressBookEntities dc = new AddressBookEntities())
            {

                var contact1 = dc.People.Where(a => a.OrganizationID.Equals(id)).ToList();
                foreach (var con in contact1)
                {
                    if (con != null)
                    {
                        dc.People.Remove(con);
                        dc.SaveChanges();
                    }
                }
            }
            using (AddressBookEntities dc = new AddressBookEntities())
            {
                var contact = dc.Organizations.Where(a => a.OrganizationID.Equals(id)).FirstOrDefault();
                if (contact != null)
                {
                    dc.Organizations.Remove(contact);
                    dc.SaveChanges();
                    return RedirectToAction("IndexOrg");
                    //return View(contacts);
                }
                else
                {
                    return HttpNotFound("Contact Not Found!");
                }
            }

        }
        public ActionResult DeletePpl(int id)
        {
            using (AddressBookEntities dc = new AddressBookEntities())
            {

                var contact1 = dc.People.Where(a => a.PersonID.Equals(id)).FirstOrDefault();
                if (contact1 != null)
                {
                    dc.People.Remove(contact1);
                    dc.SaveChanges();
                    return RedirectToAction("IndexPpl");
                }
                else
                {
                    return HttpNotFound("Contact Not Found!");
                }
            }
        }
    }
}
