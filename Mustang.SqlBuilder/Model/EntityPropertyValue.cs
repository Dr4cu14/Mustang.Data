namespace Mustang.SqlBuilder
{
    public class EntityPropertyValue
    {
        public string PropertyName { get; set; }

        public object PropertyValue { get; set; }

        public EntityPropertyValue(string propertyName, object propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }
    }
}
