using System.Linq;
using System.Web.Http;

namespace EmployeeMailUpdater.Controllers
{
	public class AuthenticateController : ApiController
    {
		[HttpPost]
		public IHttpActionResult Post([FromBody]AuthenticationInput model)
		{
			using (var logonService = new Sdk.TeleoptiCccLogOnServiceClient())
			{
				logonService.Open();
				var result = logonService.LogOnAsApplicationUser(model.UserName, model.Password);
				
				if (!result.Successful)
				{
					return BadRequest(result.Message);
				}

				return Ok(new AuthenticationResult { IsSuccessful = result.Successful, AvailableBusinessUnits = result.BusinessUnitCollection.Select(b => new NameIdItem { Id = b.Id.GetValueOrDefault(), Name = b.Name }).ToArray(), Message = result.Message, Tenant = result.Tenant });
			}
		}
    }
}
