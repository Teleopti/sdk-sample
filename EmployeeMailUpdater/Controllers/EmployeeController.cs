using System;
using System.Linq;
using System.Web.Http;

namespace EmployeeMailUpdater.Controllers
{
	public class EmployeeController : ApiController
	{
		[HttpGet, Credentials]
		public IHttpActionResult Get()
		{
			using (var organizationService = new Sdk.TeleoptiOrganizationServiceClient())
			{
				organizationService.Open();
				var myTeam = organizationService.GetLoggedOnPersonTeam(DateTime.UtcNow);
				if (myTeam == null)
				{
					//Get first team from group page
					var team = organizationService.GroupPageGroupsByQuery(new Sdk.GetGroupsForGroupPageAtDateQueryDto { PageId = new Guid("6CEB0041-0722-4B36-91DD-0A3B63C545CF"), QueryDate = new Sdk.DateOnlyDto { DateTime = DateTime.Today } }).FirstOrDefault();
					myTeam = new Sdk.TeamDto { Id = team.Id.GetValueOrDefault() };
				}
				if (myTeam != null)
				{
					var employees = organizationService.GetPersonsByQuery(new Sdk.GetPeopleByGroupPageGroupQueryDto { GroupPageGroupId = myTeam.Id.GetValueOrDefault(), QueryDate = new Sdk.DateOnlyDto { DateTime = DateTime.Today } });
					return Ok(employees.Select(e => new { Name = e.Name, Id = e.Id, Email = e.Email }).ToArray());
				}
			}
			return NotFound();
		}

		[HttpPut, Credentials]
		public IHttpActionResult Put([FromBody]EmployeeEmailInput model)
		{
			using (var organizationService = new Sdk.TeleoptiOrganizationServiceClient())
			{
				organizationService.Open();

				var person = organizationService.GetPersonsByQuery(new Sdk.GetPersonByIdQueryDto { PersonId = model.Id });
				if (person.Length > 0)
				{
					person[0].Email = model.NewEmail;
					organizationService.UpdatePerson(person[0]);
				}
			}
			return Ok();
		}
	}
}