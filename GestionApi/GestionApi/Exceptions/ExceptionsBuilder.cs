using GestionApi.Common.Helpers;
using GestionApi.Exceptions.Types;

namespace GestionApi.Exceptions
{
    public static class ExceptionsBuilder
    {
        #region CustomExceptions

        public static CustomException ServerDownException(Exception e = null)
        {
            return new CustomException(e, TypeException.Server);
        }

        public static CustomException GenericException(Exception e = null)
        {
            return new CustomException(e, TypeException.Server);
        }

        public static CustomException ExistEntityException(Exception e = null)
        {
            return new CustomException(e, TypeException.ExistEntity);
        }

        public static CustomException NotFoundException(Exception e = null)
        {
            return new CustomException(e, TypeException.NotFound);
        }

        public static CustomException NoContentException(Exception e = null)
        {
            return new CustomException(e, TypeException.NoContent);
        }

        public static CustomException OKException(Exception e = null)
        {
            return new CustomException(e, TypeException.OK);
        }

        #endregion
    }
}
