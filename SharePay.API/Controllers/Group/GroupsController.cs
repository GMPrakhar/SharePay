using Microsoft.AspNetCore.Mvc;
using SharePay.Business.Interfaces;
using SharePay.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SharePay.API.Controllers.Group
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        public readonly IUserGroupBusiness userGroupBusiness;

        public GroupsController(IUserGroupBusiness userGroupBusiness)
        {
            this.userGroupBusiness = userGroupBusiness;
        }

        // GET: api/<GroupsController>
        [HttpGet]
        public async Task<IEnumerable<GroupModel>> Get([FromHeader] string ApiKey)
        {
            return await this.userGroupBusiness.GetGroupsForUserWithApiKey(ApiKey);
        }

        // GET api/<GroupsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GroupsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GroupsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GroupsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
