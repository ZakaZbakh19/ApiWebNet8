using GestionApi.Exceptions.Types;

namespace GestionApi.Common.Helpers
{
    public static class ExceptionStatusCodeMapper
    {
        public static int GetStatusCode(TypeException exceptionType)
        {
            return exceptionType switch
            {
                TypeException.Default => 500,
                TypeException.Server => 500,
                TypeException.ExistEntity => 409,
                TypeException.NotFound => 404,
                TypeException.NoContent => 200,
                TypeException.OK => 200,
                _ => 500 // Default para cualquier otro valor no especificado
            };
        }

        public static string GetStatusCodeMessage(TypeException exceptionType)
        {
            return exceptionType switch
            {
                TypeException.Default => "An unexpected error occurred. Please try again later.",
                TypeException.Server => "A critical server error occurred. Please contact support if the issue persists.",
                TypeException.ExistEntity => "The requested entity already exists. Please use a different identifier.",
                TypeException.NotFound => "The requested resource could not be found. Please verify the provided information.",
                TypeException.NoContent => "The request was successful, but there is no content to return.",
                TypeException.OK => "The operation completed successfully.",
                _ => "An unknown error occurred. Please try again."
            };
        }
    }
}
