using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MotionDetectorWebApi.Config
{
    public class MotionDetectorContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var propertyName = property.UnderlyingName;
            //Converting to lowerCamelCase
            propertyName = char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
            property.PropertyName = propertyName;
            return property;
        }
    }
}