using GestionApi.Exceptions;
using System.Runtime.CompilerServices;

namespace GestionApi.Common.Helpers
{
    public static class BaseRunCommand
    {
        public static async Task RunCommand(Func<Task> asyncFunc, Action action, [CallerMemberName] string callerName = "")
        {
            try
            {
                if (asyncFunc != null)
                {
                    await asyncFunc();
                }

                if (action != null)
                {
                    action();
                }
            }
            catch (Exception ex)
            {
                throw new CustomException(type: CallerNameTypeHelper.GetExceptionType(callerName), callerName: callerName, innerException: ex);
            }
        }
    }
}
