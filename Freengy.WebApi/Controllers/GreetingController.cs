// Created by Laxale 13.11.2016
//
//


namespace Freengy.WebApi.Controllers 
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Freengy.WebApi.Models;


    public class GreetingController : ApiController
    {
        private readonly List<Greeting> greetings = new List<Greeting>();

        public string GetGreeting() 
        {
            return "Hello Freengy!";
        }

        public HttpResponseMessage PostGreeting(Greeting greeting)
        {
            this.greetings.Add(greeting);

            var greetingLocation = new Uri(this.Request.RequestUri, "greeting/" + greeting.Name);
            var response = this.Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = greetingLocation;
            return response;
        }

        public string GetGreeting(string id) 
        {
            var greeting = this.greetings.FirstOrDefault(g => g.Name == id);

            if (greeting == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            return greeting.Message;
        }
    }
}