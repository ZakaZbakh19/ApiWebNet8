using GestionApi.Common.Helpers;
using GestionApi.Exceptions.Types;

namespace GestionApi.Exceptions
{
    public static class ExceptionsBuilder
    {
        #region CustomExceptions

        public static CustomException ServerDownException()
        {
            return new CustomException(TypeException.Server);
        }

        public static CustomException GenericException()
        {
            return new CustomException(TypeException.Server);
        }

        public static CustomException ExistEntityException()
        {
            return new CustomException(TypeException.ExistEntity);
        }

        public static CustomException NotFoundException()
        {
            return new CustomException(TypeException.NotFound);
        }

        public static CustomException NoContentException()
        {
            return new CustomException(TypeException.NoContent);
        }

        public static CustomException OKException()
        {
            return new CustomException(TypeException.OK);
        }

        #endregion
    }
}
