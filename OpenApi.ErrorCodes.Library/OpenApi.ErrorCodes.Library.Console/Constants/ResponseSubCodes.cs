using OpenApi.ErrorCodes.Library.Console.OpenApi;

namespace OpenApi.ErrorCodes.Library.Console.Constants
{
    public class ResponseSubCodes
    {
        [CodeSubCodeDescriptionLink(ResponseCodes.SUCCESS, "Item created!", Urls.WEB_PAGE)]
        public const int CREATED = 1031;

        [CodeSubCodeDescriptionLink(ResponseCodes.SUCCESS, "Item updated!", Urls.WEB_PAGE)]
        public const int UPDATED = 2531;

        [CodeSubCodeDescriptionLink(ResponseCodes.INVALID_REQUEST, "A field is missing", Urls.WEB_PAGE)]
        public const int FIELD_MISSING = 9054;
    }
}
