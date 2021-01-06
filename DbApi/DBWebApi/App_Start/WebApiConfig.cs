﻿using DBWebApi.Controllers;
using DBWebApi.Models;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace DBWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();
            builder.TrustServerCertificate = true;
            builder.SslMode = SslMode.Require;
            builder.Host = "ec2-34-248-165-3.eu-west-1.compute.amazonaws.com";
            builder.Database = "danhsa7rd0h0u5";
            builder.Port = 5432;
            builder.Username = "veycugvkaidffx";
            builder.Password = "a9fc1b7d0b47c4dffae1862492b38216ccb686d47324c5bf55e0de09a2a6c590";
            string s = builder.ToString();
            // Конфигурация и службы веб-API

            // Маршруты веб-API
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ExpandedApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            //config.Formatters.JsonFormatter.SerializerSettings.Converters.Clear();
            //config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new App_Start.DictionaryWithANonPrimitiveKeyConverter<Quest, Dictionary<string, QuestTask>>());
            //config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //config.Formatters.JsonFormatter.SerializerSettings.Error = (sender, args) =>
            //{
            //    args.ErrorContext.Handled = true;
            //};
        }
    }
}
