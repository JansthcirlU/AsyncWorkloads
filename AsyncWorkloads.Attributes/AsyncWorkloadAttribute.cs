using System;

namespace AsyncWorkloads.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class AsyncWorkloadAttribute<TValue> : Attribute
{
    public string Name { get; }

    public AsyncWorkloadAttribute(string name)
    {
        Name = name;
    }
}
