using GestionApi.Common.Helpers;
using GestionApi.Exceptions.Types;
using System.Runtime.CompilerServices;

namespace GestionApi.Exceptions
{
    public class CustomException : Exception
    {
        public TypeException TypeException { get; set; }
        public int ErrorCode { get; set; } = 500;

        public CustomException(string message) : base(message)
        {
        }

        public CustomException(TypeException type = TypeException.Default) : base(ExceptionStatusCodeMapper.GetStatusCodeMessage(type))
        {
            TypeException = type;
            ErrorCode = ExceptionStatusCodeMapper.GetStatusCode(type);
        }
    }
}
