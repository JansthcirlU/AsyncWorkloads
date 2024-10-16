using System;

namespace AsyncWorkloads.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class PrerequisiteWorkloadAttribute<TPrerequisiteWorkload> : Attribute
{
    
}