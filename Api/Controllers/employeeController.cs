using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableRateLimiting("fixed")]
    [ServiceFilter(typeof(ThrottleAttribute))]
    public class employeeController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet("getAllEmployees")] 
        public IEnumerable<Employee> GetAllEmployees()
        {
            return this.GetEmployeesDeatils();
        }
        private List<Employee> GetEmployeesDeatils()
        {
            return new List<Employee>()
        {
            new Employee()
            {
                Id = 1,
                FirstName= "Test",
                LastName = "Name",
                EmailId ="Test.Name@gmail.com"
            },
            new Employee()
            {
                Id = 2,
                FirstName= "Test",
                LastName = "Name1",
                EmailId ="Test.Name1@gmail.com"
            }
        };
        }
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
