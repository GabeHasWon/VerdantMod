using System;

namespace Verdant.Items.Verdant
{
    /// <summary>
    /// Controls how many sacrifices are needed for a given item.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class SacrificeAttribute : Attribute
    {
        public int Count { get; set; }

        public SacrificeAttribute(int count)
        {
            Count = count;
        }
    }
}
