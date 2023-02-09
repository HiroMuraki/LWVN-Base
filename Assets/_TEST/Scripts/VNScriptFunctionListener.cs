using System;
using LWVNFramework.Controllers;
using System.Collections.Generic;

namespace LWVNFramework.Test
{
    public class VNScriptFunctionHandler : IVNScriptFunctionHandler
    {
        public bool Enabled { get; private set; } = true;

        public void ActionWithStringArg(string arg)
        {
            UnityEngine.Debug.Log(nameof(ActionWithStringArg) + " called with " + arg);
        }
        public void ActionWithoutArg()
        {
            UnityEngine.Debug.Log(nameof(ActionWithoutArg) + " called with ");
        }
    }
}