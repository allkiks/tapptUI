﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.TagHelpers
{
    [HtmlTargetElement(Attributes = "asp-active-route")]
    public class ActiveRouteTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }
        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        [HtmlAttributeName("asp-hide-if-active")]
        public bool HideIfActive { get; set; }
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            if (!CanShow())
            {
                output.SuppressOutput();
            }
            var attribute = output.Attributes.First(x => x.Name == "asp-active-route");
            output.Attributes.Remove(attribute);
        }

        private bool CanShow()
        {
            var currentController = ViewContext.RouteData.Values["Controller"].ToString();
            var currentAction = ViewContext.RouteData.Values["Action"].ToString();

            var show = false;
            if (!String.IsNullOrWhiteSpace(Controller) &&
                Controller.Equals(currentController, StringComparison.CurrentCultureIgnoreCase))
            {
                show = true;
            }
            if (show &&
                !String.IsNullOrWhiteSpace(Action) &&
                Action.Equals(currentAction, StringComparison.CurrentCultureIgnoreCase))
            {
                show = true;
            }
            else
            {
                show = false;
            }

            if (HideIfActive)
            {
                show = !show;
            }

            return show;
        }

    }
}
