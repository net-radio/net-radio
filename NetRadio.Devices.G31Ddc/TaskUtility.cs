using System;
using System.Threading.Tasks;

namespace NetRadio.Devices.G3XDdc
{
    /// <summary>
    /// Basic async utilties.
    /// </summary>
    public static class TaskUtility
    {
        /// <summary>
        /// Starts an operation asynchronously.
        /// </summary>
        /// <param name="action">Asynchronous action.</param>
        /// <returns>Returns generated task.</returns>
        public static Task Run(Action action)
        {
            var task = new Task(action);
            task.Start();
            return task;
        }
    }
}
