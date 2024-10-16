using System;

namespace AsyncWorkloads.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class AsyncWorkloadAttribute<TValue> : Attribute
{
    
}
