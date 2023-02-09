using System.Collections.Generic;
using UnityEngine.Windows.Speech;

namespace LWVNFramework.FunctionListeners
{
    public class VariablesManagementHandler : IVNScriptFunctionHandler
    {
        public bool Enabled { get; } = true;

        public void SetVariable(string variable, string value)
        {
            LWVN.ScriptReader.Variables.Set(variable, value);
        }
        public void UnsetVariable(string variable)
        {
            LWVN.ScriptReader.Variables.Unset(variable);
        }
    }
}
