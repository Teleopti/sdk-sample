using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace EmployeeMailUpdater.Controllers
{
	public class AuthenticationInput
	{
		public string UserName;
		public string Password;
	}

	public struct AuthenticationResult
	{
		public NameIdItem[] AvailableBusinessUnits;
		public bool IsSuccessful;
		public string Message;
		public string Tenant;
	}

	public struct NameIdItem
	{
		public Guid Id;
		public string Name;
	}
	
	public class HttpUserAgentMessageInspector : IClientMessageInspector
	{
		public void AfterReceiveReply(ref Message reply, object correlationState)
		{
		}

		public object BeforeSendRequest(ref Message request, IClientChannel channel)
		{
			var credentials = CredentialsExtension.Header ?? new AuthHeader();
			var messageHeader = MessageHeader.CreateHeader("TeleoptiAuthenticationHeader", "http://schemas.ccc.teleopti.com/2011/02", credentials);
            request.Headers.Add(messageHeader);
			return null;
		}
	}

	public class HttpUserAgentEndpointBehavior : IEndpointBehavior
	{
		public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
			HttpUserAgentMessageInspector inspector = new HttpUserAgentMessageInspector();
			clientRuntime.MessageInspectors.Add(inspector);
		}

		public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
		}

		public void Validate(ServiceEndpoint endpoint)
		{
		}
	}

	public class HttpUserAgentBehaviorExtensionElement : BehaviorExtensionElement
	{
		public override Type BehaviorType
		{
			get
			{
				return typeof(HttpUserAgentEndpointBehavior);
			}
		}

		protected override object CreateBehavior()
		{
			return new HttpUserAgentEndpointBehavior();
		}
	}

	[DataContract(Namespace= "http://schemas.ccc.teleopti.com/2011/02")]
	public class AuthHeader
	{
		public AuthHeader()
		{
			UserName = string.Empty;
			Password = string.Empty;
			DataSource = string.Empty;
			BusinessUnit = Guid.Empty;
		}

		[DataMember]
		public string UserName { get; set; }
		[DataMember]
		public string Password { get; set; }
		[DataMember]
		public string DataSource { get; set; }
		[DataMember]
		public bool UseWindowsIdentity { get; set; }
		[DataMember]
		public Guid? BusinessUnit { get; set; }
	}
}