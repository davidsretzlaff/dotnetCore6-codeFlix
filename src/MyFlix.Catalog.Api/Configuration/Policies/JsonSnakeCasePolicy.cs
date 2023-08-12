using MyFlix.Catalog.Api.Extensions.String;
using System.Text.Json;

namespace MyFlix.Catalog.Api.Configuration.Policies
{
    public class JsonSnakeCasePolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToSnakeCase();
    }
}
