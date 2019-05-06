using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Poseidon.Configuration
{
    public class ODataSwaggerDocument : IDocumentFilter
    {

        private readonly Func<MethodInfo, bool> isODataQueryEnabledFunc = method => method.IsPublic && (method.GetCustomAttribute<EnableQueryAttribute>() != null || method.GetParameters().Any(p => p.ParameterType.BaseType == typeof(ODataQueryOptions)));

        private readonly List<IParameter> oDataQueryParameters = new List<IParameter>
        {
            new QueryParameter { In = "query", Name = "$expand", Description = string.Empty, Required = false, Type = "string" },
            new QueryParameter { In = "query", Name = "$filter", Description = string.Empty, Required = false, Type = "string" },
            new QueryParameter { In = "query", Name = "$select", Description = string.Empty, Required = false, Type = "string" },
            new QueryParameter { In = "query", Name = "$orderby", Description = string.Empty, Required = false, Type = "string" },
            new QueryParameter { In = "query", Name = "$top", Description = string.Empty, Required = false, Type = "string" },
            new QueryParameter { In = "query", Name = "$skip", Description = string.Empty, Required = false, Type = "string" }
        };

        private IEnumerable<Type> GetODataController()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(e => e.IsPublic && (e.BaseType == typeof(Controller) || e.BaseType == typeof(ODataController)))
                .Where(e => e.GetMethods().Any(isODataQueryEnabledFunc));
        }

        private bool IsGetMethod(MethodInfo methodInfo) => methodInfo.GetCustomAttribute<HttpGetAttribute>() != null;
        private bool IsODataQueryParameter(ParameterInfo parameterInfo) => parameterInfo.ParameterType.BaseType == typeof(ODataQueryOptions);


        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            IEnumerable<Type> oDataControllers = GetODataController();
            foreach (Type controller in oDataControllers)
            {
                string controllerName = controller.Name.Replace("Controller", string.Empty);
                string controllerPath = controller.GetCustomAttribute<RouteAttribute>().Template.Replace("[controller]", controllerName);

                IEnumerable<MethodInfo> oDataEnabledMethod = controller.GetMethods().Where(isODataQueryEnabledFunc);
                if (!oDataEnabledMethod.Any()) continue;

                foreach (MethodInfo method in oDataEnabledMethod)
                {
                    if (!IsGetMethod(method)) continue;

                    string operationPath = method.GetCustomAttribute<HttpGetAttribute>().Template;

                    Operation operation = swaggerDoc.Paths[$"/{controllerPath}/{operationPath}"].Get;
                    if (operation.Parameters == null) operation.Parameters = new List<IParameter>();

                    foreach (IParameter parameter in oDataQueryParameters)
                    {
                        operation.Parameters.Add(parameter);
                    }

                    if (method.GetParameters().Any(e => IsODataQueryParameter(e)))
                    {
                        IEnumerable<ParameterInfo> parameters = method.GetParameters().Where(e => IsODataQueryParameter(e));
                        IParameter removeParameter;
                        foreach (ParameterInfo parameter in parameters)
                        {
                            removeParameter = operation.Parameters.FirstOrDefault(e => e.Name.Equals(parameter.Name, StringComparison.InvariantCultureIgnoreCase));
                            operation.Parameters.Remove(removeParameter);
                        }
                    }
                }
            }
        }
    }

    public class QueryParameter : IParameter
    {
        public string Name { get; set; }
        public string In { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }

        public Dictionary<string, object> Extensions => null;
    }
}