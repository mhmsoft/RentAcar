﻿using ECommerce.Areas.Management.Models.Entities;
using ECommerce.Areas.Management.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    [Authorize(Roles = "User")]
    public class AccountController : Controller
    {
        CustomerRepository customerManager = new CustomerRepository(new Areas.Management.Models.Context.ApplicationDbContext());
        public ActionResult Index()
        { 
            if (User.Identity.IsAuthenticated)
            {
                var model = customerManager.GetAll().SingleOrDefault(x=>x.email==User.Identity.Name);
                if (model!=null)
                     return View(model);
            }
            return RedirectToAction("Login","Customer");
            
        }
        public ActionResult MyProfile()
        {
            var model = customerManager.GetAll().SingleOrDefault(x => x.email == User.Identity.Name);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyProfile(string email,string firstName,string lastName,string address,string city,string phone )
        {
            var customer = customerManager.GetAll().SingleOrDefault(x => x.email == User.Identity.Name);
            string message = "";
            bool status = false;

            customer.firstName = firstName;
            customer.lastName = lastName;
            customer.address = address;
            customer.city = city;
            customer.phone = phone;
         
                customerManager.Update(customer);
                status = true;
                message = "değişiklikler kaydedildi";
          
            ViewBag.Status = status;
            ViewBag.Message = message;
            return View(customer);
           
        }
        public ActionResult changePassword()
        {
            var model = customerManager.GetAll().SingleOrDefault(x => x.email == User.Identity.Name);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult changePassword(string email,string oldPassword,string newPassword)
        {
            string message = "";
            bool status = false;
            var model = customerManager.GetAll().SingleOrDefault(x => x.email == User.Identity.Name);
            // veritabanındaki kayıtlı şifre ile girdiğin eski şifre birbirini tutyorsa
            if (string.Compare(model.password, Crypto.Crypto.Hash(oldPassword))==0)
            {
                model.password = Crypto.Crypto.Hash(newPassword);
                model.rePassword=Crypto.Crypto.Hash(newPassword);
                customerManager.Update(model);
                status = true;
                message = "Şifreniz değiştirildi";
            }
            ViewBag.Status = status;
            ViewBag.Message = message;
            return View(model);
        }
    }
}