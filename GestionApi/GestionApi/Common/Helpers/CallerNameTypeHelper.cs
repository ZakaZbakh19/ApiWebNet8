using GestionApi.Exceptions.Types;

namespace GestionApi.Common.Helpers
{
    public static class CallerNameTypeHelper
    {
        public static TypeException GetExceptionType(string callerName)
        {
            return callerName switch
            {
                var name when name.Contains("Service") => TypeException.Service,
                var name when name.Contains("Repository") => TypeException.Repository,
                var name when name.Contains("Controller") => TypeException.Controller,
                var name when name.Contains("Middleware") => TypeException.Middleware,
                var name when name.Contains("Filter") => TypeException.Filter,
                _ => TypeException.Default
            };
        }
    }
}
