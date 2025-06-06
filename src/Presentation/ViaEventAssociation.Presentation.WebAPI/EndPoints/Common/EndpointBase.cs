using Microsoft.AspNetCore.Mvc;

namespace ViaEventAssociation.Presentation.WebAPI.EndPoints.Common;

public static class ApiEndpoint
{
    public static class WithRequest<TRequest>
    {
        public abstract class WithResponse<TResponse> : EndpointBase
        {
            public abstract Task<ActionResult<TResponse>> HandleAsync(TRequest request);
        }
        
        public abstract class WithoutResponse : EndpointBase
        {
            public abstract Task<ActionResult> HandleAsync(TRequest request);
        }
    }
    
    public static class WithoutRequest
    {
        public abstract class WithResponse<TResponse> : EndpointBase
        {
            public abstract Task<ActionResult<TResponse>> HandleAsync();
        }
        
        public abstract class WithoutResponse : EndpointBase
        {
            public abstract Task<ActionResult> HandleAsync();
        }
    }
}

[ApiController, Route("api")]
public abstract class EndpointBase : ControllerBase;