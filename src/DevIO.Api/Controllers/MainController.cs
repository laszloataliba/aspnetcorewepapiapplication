﻿using DevIO.Business.Interfaces;
using DevIO.Business.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

namespace DevIO.Api.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotifier _notifier;
        protected readonly IUser _appUser;

        protected Guid UserId { get; set; }
        protected bool UserAuthenticated { get; set; }

        public MainController(INotifier notifier, IUser appUser)
        {
            _notifier = notifier;
            _appUser = appUser;

            if (_appUser.IsAuthenticated())
            {
                UserId = _appUser.GetUserId();
                UserAuthenticated = true;
            }
        }

        protected bool IsValidOperation()
        {
            return !_notifier.HasNotification();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (IsValidOperation())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notifier.GetNotifications().Select(error => error.Message)
            });
        }  

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotifyInvalidErrorModel(modelState);

            return CustomResponse();
        }

        protected void NotifyInvalidErrorModel(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(error => error.Errors);

            foreach (var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(errorMsg);
            }
        }

        protected void NotifyError(string message)
        {
            _notifier.Handle(new Notification(message));
        }
    }
}
