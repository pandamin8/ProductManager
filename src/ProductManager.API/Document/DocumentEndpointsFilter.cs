using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProductManager.API.Document;

public class DocumentEndpointsFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (swaggerDoc.Paths.ContainsKey("/identity/register"))
        {
            swaggerDoc.Paths.Remove("/identity/register");
        }
    }
}
