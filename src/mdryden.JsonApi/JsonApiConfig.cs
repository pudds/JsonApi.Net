﻿using mbsoft.JsonApi.Filters;
using mbsoft.JsonApi.Formatters;
using mbsoft.JsonApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class JsonApiConfig
    {
        public static IServiceCollection AddJsonApi(this IServiceCollection services)
        {
            services.AddMvc(option =>
            {
                option.OutputFormatters.Insert(0, new JsonApiFormatter());
            });

            return services;
        }
        
        public static IServiceCollection AddJsonApi(this IServiceCollection services, IConfigurationSection apiKeysSection)
        {
            services.AddJsonApi();
            services.Configure<JsonApiKeyCollection>(apiKeysSection);
            services.AddScoped<ApiKeyFilterAttribute>();

            return services;
        }

        public static IServiceCollection AddJsonApiMeta(this IServiceCollection services, Func<Dictionary<string, object>> getMeta)
        {
            JsonApiResponseCreator.DefaultMeta = getMeta;
            return services;
        }

        
    }
}
