using System;

namespace Lost.Runtime.Footstone.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DefaultEntityComponentProcessorAttribute : DynamicTypeAttributeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEntityComponentProcessorAttribute"/> class.
        /// </summary>
        /// <param name="type">The type must derived from <see cref="EntityProcessor"/>.</param>
        public DefaultEntityComponentProcessorAttribute(Type type) : base(type)
        {
            
        }
        public ExecutionMode ExecutionMode { get; set; } = ExecutionMode.Runtime;
    }
} 

