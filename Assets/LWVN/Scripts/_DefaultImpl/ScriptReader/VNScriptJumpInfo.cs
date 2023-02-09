#nullable enable
using System.Collections.Generic;
using System;
using LWVNFramework.Infos;

namespace LWVNFramework.Controllers
{
    [Serializable]
    public class VNScriptJumpInfo : VNInfo
    {
        public string Variable { get; set; } = string.Empty;
        public Dictionary<string, string> ValueMap { get; } = new Dictionary<string, string>();
    }
}