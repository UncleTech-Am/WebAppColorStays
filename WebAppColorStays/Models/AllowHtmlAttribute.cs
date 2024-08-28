namespace WebAppColorStays.Models
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AllowHtmlAttribute : Attribute
    {
    }
}
