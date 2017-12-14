﻿using mdryden.JsonApi.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace mdryden.JsonApi.xUnit
{
    public class ApiExceptionFilterAttributeTests
    {
        
        private ApiExceptionFilterAttribute GetTarget()
        {
            return new ApiExceptionFilterAttribute();
        }

        private ExceptionContext GetContext(Exception ex)
        {
            var httpContext = new DefaultHttpContext();
            var context = new ExceptionContext(
                 new ActionContext
                 {
                     HttpContext = httpContext,
                     RouteData = new RouteData(),
                     ActionDescriptor = new ActionDescriptor(),
                 },
                 new List<IFilterMetadata>()
            )
            {
                Exception = ex
            };

            return context;
        }

        [Fact]
        public void GenericServerExceptionTest()
        {
            var exceptionHandlingAttribute = GetTarget();
            var context = GetContext(new Exception());

            exceptionHandlingAttribute.OnException(context);

            var expected = (int)HttpStatusCode.InternalServerError;
            var actual = context.HttpContext.Response.StatusCode;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HandledApiExceptionTest()
        {
            var apiException = new JsonApiException(HttpStatusCode.Forbidden);
       
            var exceptionHandlingAttribute = GetTarget();
            var context = GetContext(apiException);

            exceptionHandlingAttribute.OnException(context);

            var expected = (int)apiException.Code;
            var actual = context.HttpContext.Response.StatusCode;

            Assert.Equal(expected, actual);
        }
    }    
}
