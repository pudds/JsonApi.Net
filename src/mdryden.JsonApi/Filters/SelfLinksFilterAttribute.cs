﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace mdryden.JsonApi.Filters
{
	public class SelfLinksFilterAttribute : ActionFilterAttribute
	{
		ILogger logger;

		public SelfLinksFilterAttribute(ILogger<SelfLinksFilterAttribute> logger)
		{
			this.logger = logger;
		}

		public override void OnResultExecuting(ResultExecutingContext context)
		{
			var objectResult = context.Result as ObjectResult;
			var response = objectResult?.Value as IApiResponse;

			if (response != null && response.IsResource() && !response.HasErrors())
			{
				var href = context.HttpContext.Request.Path;
				response.AddLink(JsonApiConstants.SelfLinkKey, href);
			}
		}
	}
}
