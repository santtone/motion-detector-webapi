using System;

namespace MotionConfigManager
{
    public class MotionConfigProperty : Attribute
    {
        public readonly string PropertyName;

        public MotionConfigProperty(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}