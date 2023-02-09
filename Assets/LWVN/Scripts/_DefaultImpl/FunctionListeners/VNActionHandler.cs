using LWVNFramework.Controllers;

namespace LWVNFramework.FunctionListeners
{
    public class VNActionHandler : IVNScriptFunctionHandler
    {
        public bool Enabled { get; } = true;

        public void Suspend()
        {
            if (VNCommandCenter.Current != null)
            {
                VNCommandCenter.Current.Suspend();
            }
        }
        public void Unsuspend()
        {
            if (VNCommandCenter.Current != null)
            {
                VNCommandCenter.Current.Unsuspend();
            }
        }
    }
}
