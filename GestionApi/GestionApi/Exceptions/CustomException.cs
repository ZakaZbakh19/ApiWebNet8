using GestionApi.Exceptions.Types;
using System.Runtime.CompilerServices;

namespace GestionApi.Exceptions
{
    public class CustomException : Exception
    {
        public TypeException TypeException { get; set; }
        public string CallerName { get; set; }
        public int ErrorCode { get; set; } = 500;

        public CustomException(string message) : base(message)
        {
        }

        public CustomException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CustomException(Exception innerException, TypeException type = TypeException.Default, string message = "",[CallerMemberName] string callerName = "", int error = 500) : base(message, innerException)
        {
            TypeException = type;
            CallerName = callerName;
        }
    }
}
