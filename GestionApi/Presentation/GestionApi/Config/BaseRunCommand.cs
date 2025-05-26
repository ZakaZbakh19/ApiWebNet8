using GestionApi.Common.Helpers;
using GestionApi.Exceptions;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace GestionApi.Config
{
    public abstract class BaseRunCommand
    {
        public readonly ILogger<BaseRunCommand> _log;
        private readonly IConfiguration _configuration;

        protected BaseRunCommand(ILogger<BaseRunCommand> log, IConfiguration configuration)
        {
            _log = log;
            _configuration = configuration;
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
                if (_configuration != null)
                {
                    if (needConnection && !StatusServicesHelper.IsServerConnected(_configuration.GetConnectionString("DefaultConnection")!))
                    {
                        _log.LogError("Connection failure in {CallerName} method. Could not establish connection to the server using connection string 'DefaultConnection'. " +
                            "Exception details: {ExceptionMessage}",callerName, ex.Message);
                        throw ExceptionsBuilder.ServerDownException();
                    }
                }

                _log.LogError("Error in {CallerName} method. Details: {Message}", callerName, ex.Message);
                throw ExceptionsBuilder.GenericException();
            }
        }
    }
}
