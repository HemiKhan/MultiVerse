using Data.DataAccess;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class RemoveVersionParameterFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Get the parameters for the current operation
        var parameters = operation.Parameters;

        // Remove any parameters named "api-version" from the parameter list
        var versionParameter = parameters.FirstOrDefault(p => p.Name.ToLower() == "api-version");
        if (versionParameter != null)
        {
            parameters.Remove(versionParameter);
        }
    }
}
public class AddAuthorizationHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (StaticPublicObjects.ado.IsSwaggerCallAdmin() == false)
        {
            if (context.MethodInfo.DeclaringType.Name == "LOVController" &&
                context.MethodInfo.Name == "GetServiceLevelList")
            {
                var parameters = operation.Parameters;
                var versionParameter = parameters.FirstOrDefault(p => p.Name.ToLower() == "IsAll".ToLower());
                if (versionParameter != null)
                {
                    parameters.Remove(versionParameter);
                }
            }
        }
        if (context.MethodInfo.DeclaringType.Name == "LOVController" &&
                (context.MethodInfo.Name == "GetServiceLevelList"))
        {
            var parameters = operation.Parameters;
            var versionParameter = parameters.FirstOrDefault(p => p.Name.ToLower() == "TypeMTVId".ToLower());
            if (versionParameter != null)
            {
                if (versionParameter.In.ToString() == "Query" && versionParameter.Schema.Required != null)
                    versionParameter.Description = string.Join(", ", versionParameter.Schema.Required);
            }
        }
        else if (context.MethodInfo.DeclaringType.Name == "LOVController" &&
                (context.MethodInfo.Name == "GetClientSpecialServicesList"))
        {
            var parameters = operation.Parameters;
            var versionParameter = parameters.FirstOrDefault(p => p.Name.ToLower() == "ST_Code".ToLower());
            if (versionParameter != null)
            {
                if (versionParameter.In.ToString() == "Query")
                    versionParameter.Description = versionParameter.Schema.Description;
            }
        }
    }
}
