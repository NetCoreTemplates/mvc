using ServiceStack;
using MyApp.ServiceModel;

namespace MyApp.ServiceInterface;

public class MyServices : Service
{
    public object Any(Hello request)
    {
        return new HelloResponse { Result = $"Hello, {request.Name}!" };
    }

    public object Any(RequiresAuth request)
    {
        return new RequiresAuthResponse { Result = $"Hello, {request.Name}!" };
    }

    public object Any(RequiresRole request)
    {
        return new RequiresRoleResponse { Result = $"Hello, {request.Name}!" };
    }

    public object Any(RequiresAdmin request)
    {
        return new RequiresAdminResponse { Result = $"Hello, {request.Name}!" };
    }
}