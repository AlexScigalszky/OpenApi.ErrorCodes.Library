using System;

namespace OpenApi.ErrorCodes.Library.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ResponseCodeFromConstantsAttribute : Attribute
    {
        public Type ConstantType { get; set; }
        public string SectionName { get; set; }

        public ResponseCodeFromConstantsAttribute(string sectionName, Type constantType)
        {
            ConstantType = constantType;
            SectionName = sectionName;
        }
    }
}
