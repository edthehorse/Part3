using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace NorthwindService.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public double Get() // Get() is the default ! method for api/values
        {
            return Math.Sqrt(5.0);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public double Get(double id) // api/values/4.5
        {
            return Math.Sqrt(id);
        }


        // --------------- These are all (simple) POSTS which do nothing! TODO Put some code in here! opportunity for a simple post demoed

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value) // ? Use Postman.  ed
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody]string value)
        {

            return "thanks";
        }

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //    // need to find the value with that id first. ed

        //    // But we dont even have a context for Values. ed 


        //    // then delete it. ed

        //}
    }
}
