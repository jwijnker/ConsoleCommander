using System.Diagnostics;
using System.Threading.Tasks;

namespace ConsoleCommander.Helpers
{
    [DebuggerStepThrough]
    public static class AsyncHelper
    {
        public static T ExecuteAsync<T>(this Task<T> task)
        {
            //return task.GetAwaiter().GetResult();
            return AsyncHelper.ExecuteAsync(task, 10);
        }

        public static void ExecuteAsync(this Task task)
        {
            task.GetAwaiter().GetResult();
        }

        public static T ExecuteAsync<T>(this Task<T> task, int timeout)
        {
            var result = task.GetAwaiter().GetResult();

            Task.Delay(timeout);

            return result;
        }
    }
}
