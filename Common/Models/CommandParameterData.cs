using System;

namespace CustomCommandSystem.Common.Models
{
    internal class CommandParameterData
    {
        public int UserInputLength { get; set; }
        public object? DefaultValue { get; internal set; }
        public bool IsRemainingText { get; internal set; }
        public bool IsNullable { get; internal set; }
#nullable disable
        public Type Type { get; internal set; }
        public string Name { get; set; }
#nullable enable
        
    }
}
