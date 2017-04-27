using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace EmployeeMailUpdater.Controllers
{
    public class ListenerController : ApiController
    {
		[HttpPost]
	    public async Task<IHttpActionResult> Post()
	    {
		    IEnumerable<string> signature;
		    if (!Request.Headers.TryGetValues("Signature", out signature))
		    {
			    return Unauthorized();
		    }
			var rsa = new RSACryptoServiceProvider();
			rsa.ImportParameters(new RSAParameters
			{
				Modulus = Convert.FromBase64String("... Modulus ..."),
				Exponent = Convert.FromBase64String("... Exponent ...")
			});
			
		    string content = await Request.Content.ReadAsStringAsync();
			var isValidRequest = rsa.VerifyData(Encoding.UTF8.GetBytes(content), CryptoConfig.MapNameToOID("SHA1"), Convert.FromBase64String(signature.First()));
			if (!isValidRequest)
			{
				throw new ArgumentException("The data has been tampered with!");
			}

			File.AppendAllText("c:\\temp\\schedulechanges.txt", content + Environment.NewLine);
		    return Ok();
	    }
    }
}
