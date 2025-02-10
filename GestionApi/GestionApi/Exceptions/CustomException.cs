using GestionApi.Exceptions.Types;
using System.Runtime.CompilerServices;

namespace GestionApi.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException(string message) : base(message)
        {
        }

        public CustomException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CustomException(TypeException type = TypeException.Default ,[CallerMemberName] string message = "", Exception innerException = null) : base($"[{type}] {message}")
        {

        }
    }
}
