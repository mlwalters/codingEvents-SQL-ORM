﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodingEvents.Models;
using CodingEvents.Data;
using CodingEvents.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CodingEvents.Controllers                      // CONTINUATION OF CLASS 10,11 VIEWS & MODELS
{
    public class EventsController : Controller
    {
        // TODO Modify Edit feature

        private EventDbContext context;

        public EventsController(EventDbContext dbContext)
        {
            context = dbContext;
        }

        // GET : /<controller>/
        public IActionResult Index()
        {
            // List<Event> events = new List<Event>(EventData.GetAll());
            List<Event> events = context.Events
                .Include(e => e.Category)       // lambda expression, method chaining
                .ToList();                      // refers to Data/EventDbContext (DBSet), lazy loading

            return View(events);
        }

        //[HttpGet]
        public IActionResult Add()
        {
            List<EventCategory> categories = context.Categories.ToList();
            AddEventViewModel addEventViewModel = new AddEventViewModel(categories);

            return View(addEventViewModel);
        }

        [HttpPost]  //annotations, request type attribute
        public IActionResult Add(AddEventViewModel addEventViewModel)       // ViewModel
        {
            if(ModelState.IsValid)
            {
                EventCategory theCategory = context.Categories.Find(addEventViewModel.CategoryId);
                Event newEvent = new Event
                {                                                           // directly assign properties to Event Model using ViewModel
                    Name = addEventViewModel.Name,
                    Location = addEventViewModel.Location,
                    Description = addEventViewModel.Description,
                    NoOfAttendees = addEventViewModel.NoOfAttendees,
                    ContactEmail = addEventViewModel.ContactEmail,
                    Category = theCategory
                };

                // EventData.Add(newEvent);
                context.Events.Add(newEvent);
                context.SaveChanges();

                return Redirect("/Events");
            }

            return View(addEventViewModel);
        }

        public IActionResult Delete()
        {
            // ViewBag.events = EventData.GetAll();
            ViewBag.events = context.Events.ToList();       //TODO No ViewModel yet
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int[] eventIds)
        {
            foreach(int eventId in eventIds)
            {
                // EventData.Remove(eventId);
                Event eventToBeDeleted = context.Events.Find(eventId);
                context.Events.Remove(eventToBeDeleted);
            }

            context.SaveChanges();

            return Redirect("/Events");
        }

        [HttpGet]
        [Route("/Events/Edit/{eventId}")]
        public IActionResult Edit(int eventId)
        {

            // ViewBag.Title = "Edit Event " + EventData.GetById(eventId).Name + "(" + eventId + ")";
            // ViewBag.editEvent = EventData.GetById(eventId);
            // return View();
            Event theEvent = context.Events.Find(eventId);
            ViewBag.Title = "Edit Event " + theEvent.Name + "(" + eventId + ")";
            ViewBag.editEvent = theEvent;

            context.SaveChanges();

            return View();
        }

        [HttpPost]
        [Route("/Events/Edit")]
        public IActionResult SubmitEditEventForm(int eventId, string name, string description, string location, int noOfAttendees, string contactEmail)
        {
            // Event toBeEdited = EventData.GetById(eventId);
            Event toBeEdited = context.Events.Find(eventId);
            toBeEdited.Name = name;
            toBeEdited.Description = description;
            toBeEdited.Location = location;
            toBeEdited.NoOfAttendees = noOfAttendees;
            toBeEdited.ContactEmail = contactEmail;

            context.SaveChanges();

            return Redirect("/Events");
        }
    }
}