﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadySetResource.Models;
using ReadySetResource.ViewModels;

namespace ReadySetResource.Controllers
{
    public class GetController : Controller
    {
        private ApplicationDbContext _context;

        public GetController()
        {
            _context = new ApplicationDbContext();

        }



        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Get/Solutions
        [HttpGet]
        public ActionResult Solutions()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BusinessInfo(int plan)
        {
            var model = new Business();
            if (plan == 1) { ViewBag.Title = "Small Business"; model.Plan = "Small"; }
            else if (plan == 2) { ViewBag.Title = "Medium Business"; model.Plan = "Medium"; }
            else if (plan == 3) { ViewBag.Title = "Large Business"; model.Plan = "Large"; }


            return View(model);
        }


        [HttpGet]
        public ActionResult ManagerDetails(int businessId)
        {
            var business = _context.Businesses.SingleOrDefault(c => c.Id == businessId);

            if (business == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                var viewModel = new ManagerDetailsViewModel
                {
                    NewBusiness = business,
                    NewManager = new SystemUser(),
                };
                return View(viewModel);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBusiness(Business business)
        {
            if (business.Plan == "1") { business.Plan = "Small"; }
            else if (business.Plan == "2") { business.Plan = "Medium"; }
            else if (business.Plan == "3") { business.Plan = "Large"; }

            business.StartDate = DateTime.Now;
            business.EndDate = DateTime.Now;

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (!ModelState.IsValid)
            {
                var viewModel = new Business();
                return View("BusinessInfo", viewModel);
            }

            if (business.Id == 0)
            {

                _context.Businesses.Add(business);
            }
            else
            {
                var businessInDb = _context.Businesses.Single(c => c.Id == business.Id);

                businessInDb.Name = business.Name;

            }

            _context.SaveChanges();
            return RedirectToAction("ManagerDetails", business.Id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddManagerOne(ManagerDetailsViewModel managerVM)
        {
            //Checking to see if the model is valid
            if (!ModelState.IsValid)
            {
                var viewModel = new ManagerDetailsViewModel();
                return View("ManagerDetails", viewModel);
            }
            else
            {
                //Setting the unset attributes - excluding some like NIN which are optional
                managerVM.NewManager.Blocked = false;
                managerVM.NewManager.TimesLoggedIn = 0;
                managerVM.NewManager.Raise = 0;
                managerVM.NewManager.Strikes = 0;

                var errors = ModelState.Values.SelectMany(v => v.Errors);

                if (managerVM.NewManager.Id == 0)
                {


                  //  _context.Businesses.Add(business);
                }
                else
                {
                   // var businessInDb = _context.Businesses.Single(c => c.Id == business.Id);

                //    businessInDb.Name = business.Name;

                }
            }








            //_context.SaveChanges();
            return RedirectToAction("ManagerDetails");
        }
    }
}