using System;
using System.Collections.Generic;

namespace Lightstone.Common.CustomExceptions
{
    public class LightstoneValidationException : Exception
    {
        public string FieldName { get; set; }
        public List<string> Messages { get; set; }

        public LightstoneValidationException()
            : base()
        {
            Messages = new List<string>();
        }

        public LightstoneValidationException(string strBindError)
            : base(strBindError)
        {
            Messages = new List<string>();
            Messages.Add(strBindError);
        }

        public LightstoneValidationException(string message, string fieldName)
            : base(message)
        {
            FieldName = fieldName;
        }

        public LightstoneValidationException(List<string> messages)
            : base()
        {
            Messages = messages;
        }
    }
}
