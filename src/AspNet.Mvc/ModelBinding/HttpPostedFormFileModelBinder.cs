// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See https://github.com/aspnet/AspNetWebStack/blob/a68a57535d0adeda2b1846764e5eafd02fdeb12d/LICENSE.txt for license information.
// https://github.com/aspnet/AspNetWebStack/blob/a68a57535d0adeda2b1846764e5eafd02fdeb12d/src/System.Web.Mvc/HttpPostedFileBaseModelBinder.cs

using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;

namespace Unravel.AspNet.Mvc.ModelBinding
{
    public class HttpPostedFormFileModelBinder : IModelBinder
    {
        public object? BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException(nameof(controllerContext));
            }
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var name = bindingContext.ModelName;
            var theFile = controllerContext.HttpContext.Request.Files[name];
            return ChooseFileOrNull(name, theFile);
        }

        // helper that returns the original file if there was content uploaded, null if empty
        internal static IFormFile? ChooseFileOrNull(string name, HttpPostedFileBase rawFile)
        {
            // case 1: there was no <input type="file" ... /> element in the post
            if (rawFile == null)
            {
                return null;
            }

            // case 2: there was an <input type="file" ... /> element in the post, but it was left blank
            if (rawFile.ContentLength == 0 && string.IsNullOrEmpty(rawFile.FileName))
            {
                return null;
            }

            // case 3: the file was posted
            return new Internal.HttpPostedFormFile(name, rawFile);
        }
    }
}
