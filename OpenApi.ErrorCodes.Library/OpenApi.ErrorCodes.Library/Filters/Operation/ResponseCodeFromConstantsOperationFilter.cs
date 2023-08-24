using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using OpenApi.ErrorCodes.Library.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace OpenApi.ErrorCodes.Library.Filters.Operation
{
    public class ResponseCodeFromConstantsOperationFilter<TResponse, TDataAttribute> : IOperationFilter
        where TResponse : class
        where TDataAttribute : ResponseCodeFromConstantsDataAttribute
    {
        Func<TDataAttribute, int, TResponse> TransferFn;

        public ResponseCodeFromConstantsOperationFilter(Func<TDataAttribute, int, TResponse> transferFn)
        {
            TransferFn = transferFn;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (IsCovered(context))
            {
                return;
            }

            var parameters = GetParameters(context);
            var schema = GetTResponseSchema(context);


            foreach (var param in parameters)
            {
                var examples = new Dictionary<string, OpenApiExample>();
                var properties = param.Item1
                    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                    .ToArray();
                foreach (var prop in properties)
                {
                    var obj = CreateTResponseExample(prop);
                    if (obj != null)
                    {
                        var example = CreateExample(obj);
                        var key = Value(prop).ToString();
                        if (!examples.ContainsKey(key))
                        {
                            examples.Add(key, example);
                        }
                    }
                }
                AddResponse(operation, schema, examples, param.Item2);
            }
        }

        private TResponse? CreateTResponseExample(FieldInfo prop)
        {
            var data = prop.GetInlineAndMetadataAttributes()
                .OfType<TDataAttribute>()
                .FirstOrDefault();
            if (data == null) return null;
            return TransferFn(data, Value(prop));
        }

        public bool IsCovered(OperationFilterContext context)
        {
            return !context
                .MethodInfo
                .GetInlineAndMetadataAttributes()
                .Select(x => x.ToString())
                .Any(x => x == typeof(ResponseCodeFromConstantsAttribute).ToString());
        }

        public IEnumerable<(Type, string)> GetParameters(OperationFilterContext context)
        {
            return context
                .MethodInfo
                .GetInlineAndMetadataAttributes()
                .OfType<ResponseCodeFromConstantsAttribute>()
                .Select(x => (x.ConstantType, x.SectionName));
        }

        public string SectionName(OperationFilterContext context)
        {
            return context
                .MethodInfo
                .GetInlineAndMetadataAttributes()
                .OfType<ResponseCodeFromConstantsAttribute>()
                .Select(x => x.SectionName)
                .FirstOrDefault() ?? "ResponseCode";
        }

        public OpenApiExample CreateExample(TResponse obj)
        {
            return new OpenApiExample()
            {
                Value = new OpenApiString(JsonSerializer.Serialize(obj)),
                UnresolvedReference = true
            };
        }

        public void AddResponse(OpenApiOperation operation, OpenApiSchema schema, Dictionary<string, OpenApiExample> examples, string sectionName)
        {
            operation.Responses.Add(sectionName, new OpenApiResponse
            {
                Content = new Dictionary<string, OpenApiMediaType>() {
                {
                    "application/json", new OpenApiMediaType {
                        Schema = schema,
                        Examples = examples
                    }
                }
            }
            });
        }

        public static OpenApiSchema GetTResponseSchema(OperationFilterContext context)
        {
            context.SchemaRepository.TryLookupByType(typeof(TResponse), out var schema);

            return schema ?? context
                .SchemaGenerator
                .GenerateSchema(typeof(TResponse), context.SchemaRepository);

        }

        public int Value(FieldInfo prop)
        {
            return (int)(prop.GetRawConstantValue() ?? -1);
        }

    }
}
