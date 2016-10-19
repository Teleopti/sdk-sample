using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EmployeeMailUpdater.Controllers
{
	public class CredentialsAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			base.OnActionExecuting(actionContext);

			var username = actionContext.Request.Headers.GetValues("X-UserName").FirstOrDefault();
			var password = actionContext.Request.Headers.GetValues("X-Password").FirstOrDefault();
			var businessUnit = actionContext.Request.Headers.GetValues("X-BusinessUnit").FirstOrDefault();
			var tenant = actionContext.Request.Headers.GetValues("X-Tenant").FirstOrDefault();

			Guid result = Guid.Empty;
			if (!string.IsNullOrEmpty(businessUnit))
				Guid.TryParse(businessUnit, out result);

			CredentialsExtension.Set(new AuthHeader { BusinessUnit = result, DataSource = tenant, UserName = username, Password = password });
		}
	}
}