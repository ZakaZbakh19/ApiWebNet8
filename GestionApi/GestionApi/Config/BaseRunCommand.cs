using GestionApi.Common.Helpers;
using GestionApi.Exceptions;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace GestionApi.Config
{
    public abstract class BaseRunCommand
    {
        public readonly ILogger<BaseRunCommand> _log;
        protected BaseRunCommand(ILogger<BaseRunCommand> log)
        {
            _log = log;
        }

        public async Task<T> RunCommand<T>(
            Func<Task<T>> asyncFunc = null,
            [CallerMemberName] string callerName = "",
            bool needConnection = true)
        {
            try
            {
                if (asyncFunc != null)
                {
                    return await asyncFunc();
                }

                return default(T);
            }
            catch (Exception ex)
            {
                var configuration = ServiceHelper.Resolve<IConfiguration>();

                if (configuration != null)
                {
                    if (needConnection && !StatusServicesHelper.IsServerConnected(configuration.GetConnectionString("DefaultConnection")))
                    {
                        _log.LogError("Connection failure in {CallerName} method. Could not establish connection to the server using connection string 'DefaultConnection'. " +
                            "Exception details: {ExceptionMessage}",callerName, ex.Message);
                        throw ExceptionsBuilder.ServerDownException(ex);
                    }
                }

                _log.LogError("Error in {CallerName} method. Details: {Message}", callerName, ex.Message);
                throw ExceptionsBuilder.GenericException(ex);
            }
        }
    }
}
