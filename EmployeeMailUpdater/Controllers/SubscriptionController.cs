using System.Linq;
using System.Web.Http;
using EmployeeMailUpdater.Sdk;

namespace EmployeeMailUpdater.Controllers
{
	public class SubscriptionController : ApiController
	{
		[HttpGet, Credentials]
		public IHttpActionResult Get()
		{
			using (var schedulingClient = new TeleoptiSchedulingServiceClient())
			{
				schedulingClient.Open();

				var settings = schedulingClient.GetScheduleChangeSubscriptionsByQuery(new GetScheduleChangesSubscriptionSettingsQueryDto());
				return
					Ok(
						settings.SelectMany(
								e =>
									e.Listeners.Select(
										l => new {l.Name, l.Url, l.DaysStartFromCurrentDate, l.DaysEndFromCurrentDate, e.Modulus, e.Exponent}))
							.ToArray());
			}
		}

		[HttpPost, Credentials]
		public IHttpActionResult Post([FromBody]AddScheduleChangesListenerInput model)
		{
			using (var internalClient = new TeleoptiCccSdkInternalClient())
			{
				internalClient.Open();

				internalClient.ExecuteCommand(new AddScheduleChangesListenerCommandDto
				{
					Listener =
						new ScheduleChangesListenerDto
						{
							Name = model.Name,
							Url = model.Url,
							DaysStartFromCurrentDate = model.DaysStartFromCurrentDate,
							DaysEndFromCurrentDate = model.DaysEndFromCurrentDate
						}
				});
			}
			return Ok();
		}

		[HttpDelete, Credentials]
		public IHttpActionResult Delete([FromUri]string name)
		{
			using (var internalClient = new TeleoptiCccSdkInternalClient())
			{
				internalClient.Open();

				internalClient.ExecuteCommand(new RevokeScheduleChangesListenerCommandDto { ListenerName = name});
			}
			return Ok();
		}
	}
}