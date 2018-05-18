using PhotoGO.BLL.Enums;
using System;

namespace PhotoGO.BLL.Exceptions
{
    public class TargetNotFoundException : Exception
    {
        public TargetNotFoundException() : base("Target not found") { }

        public TargetNotFoundException(string message) : base(message) { }

        public TargetNotFoundException(Target target) : base(target.ToString() + " not found") { }
    }
}
