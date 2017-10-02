using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Andoria.Server.Controllers
{
    /// <summary>
    /// Defined the controller for Values
    /// </summary>
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        /// <summary>
        /// GET api/values
        /// </summary>
        /// <returns>Returns a dummy values</returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// GET api/values/5
        /// </summary>
        /// <param name="id">Id from which to retrieve values</param>
        /// <returns>Returns values</returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// post a value
        /// </summary>
        /// <param name="value">value to post</param>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// Put a value for a given if
        /// </summary>
        /// <param name="id">Id to whom value will be pushed</param>
        /// <param name="value">Value to push</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        ///  DELETE api/values/5
        /// </summary>
        /// <param name="id">Id To delete</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
