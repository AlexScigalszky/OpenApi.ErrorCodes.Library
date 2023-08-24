using OpenApi.ErrorCodes.Library.Attributes;

namespace OpenApi.ErrorCodes.Library.Console.OpenApi
{
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
}
