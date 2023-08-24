# OpenApi.ErrorCodes.Library

This is a library to show examples of error codes from a constants file to a defined structure


## How to use
* Create an attribute extending ´ResponseCodeFromConstantsDataAttribute´ to save your data.
Example
```csharp
[AttributeUsage(AttributeTargets.Field)]
public class CodeSubCodeDescriptionLinkAttribute : ResponseCodeFromConstantsDataAttribute
{
    public int Code;
    public int? SubCode;
    public string Description;
    public string Link;
    public CodeSubCodeDescriptionLinkAttribute(int code, string description, string link)
    {
        Code = code;
        SubCode = null;
        Description = description;
        Link = link;
    }
}
```

* Create a Response class
Example

```csharp
public class CodeSubCodeResponse
{
    public int Code { get; set; }
    public int SubCode { get; set; }
    public string? Description { get; set; }
    public string? Link { get; set; }
}
```

* Create a mapping function
Example
```csharp
var MapAttributeToResponse = (CodeSubCodeDescriptionLinkAttribute data, int constant) =>
{
    return new CodeSubCodeResponse()
    {
        Code = data.Code,
        SubCode = constant,
        Description = data.Description,
        Link = data.Link
    };
};
```

* Add the OpenApi filter in your startup project
```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = builder.Environment.ApplicationName,
        Version = "v1",
    });
    /// add next line
    c.OperationFilter<ResponseCodeFromConstantsOperationFilter<CodeSubCodeResponse, CodeSubCodeDescriptionLinkAttribute>>(MapAttributeToResponse);
});
```

* Add attributo in your endpoint function. 
It needs a section name and the class where the constant codes are defined
Example
```csharp
var HomeFn = 
    // add next line
    [ResponseCodeFromConstants("List of error codes", typeof(ResponseSubCodes))]
([FromBody] InputRequest input) => {

    return "Hello word";
};
```

## Example of a OpenApi json file

```json
{
  "openapi": "3.0.1",
  "info": {
    "title": "OpenApi.ErrorCodes.Library.Console",
    "version": "v1"
  },
  "paths": {
    "/": {
      "post": {
        "tags": [
          "OpenApi.ErrorCodes.Library.Console"
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/InputRequest"
              }
            }
          },
          "required": true
        },
        "responses": {
          "200": {
            "description": "Success",
            "content": {
              "text/plain": {
                "schema": {
                  "type": "string"
                }
              }
            }
          },
          "List of error codes": {
            "description": null,
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/CodeSubCodeResponse"
                },
                "examples": {
                  "1031": {
                    "value": "{\"Code\":200,\"SubCode\":1031,\"Description\":\"Item created!\",\"Link\":\"http://tbd\"}"
                  },
                  "2531": {
                    "value": "{\"Code\":200,\"SubCode\":2531,\"Description\":\"Item updated!\",\"Link\":\"http://tbd\"}"
                  },
                  "9054": {
                    "value": "{\"Code\":300,\"SubCode\":9054,\"Description\":\"A field is missing\",\"Link\":\"http://tbd\"}"
                  }
                }
              }
            }
          }
        }
      }
    }
  },
  "components": {
    "schemas": {
      "CodeSubCodeResponse": {
        "type": "object",
        "properties": {
          "code": {
            "type": "integer",
            "format": "int32"
          },
          "subCode": {
            "type": "integer",
            "format": "int32"
          },
          "description": {
            "type": "string",
            "nullable": true
          },
          "link": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      },
      "InputRequest": {
        "type": "object",
        "properties": {
          "name": {
            "type": "string",
            "nullable": true
          },
          "count": {
            "type": "integer",
            "format": "int32",
            "nullable": true
          }
        },
        "additionalProperties": false
      }
    }
  }
}
```
