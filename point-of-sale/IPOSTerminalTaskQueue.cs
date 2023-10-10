
using csca5028.final.project.components;

namespace point_of_sale
{
    public interface IPOSTerminalTaskQueue
    {
        void EnqueueTask(POSTerminal terminal, string serviceBusConnection);
        Task<Task> DequeueTask();
    }
}
