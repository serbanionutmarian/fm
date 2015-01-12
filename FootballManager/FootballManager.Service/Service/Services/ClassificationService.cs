using DtoModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Services
{
    public class ClassificationService
    {
        public List<Classification> GetAll<T>()
        {
            var enumType = typeof(T);
            var result = new List<Classification>();
            string[] enumNames = Enum.GetNames(enumType);
            foreach (var enumName in enumNames)
            {
                FieldInfo field = enumType.GetField(enumName);
                DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attr == null)
                {
                    throw new NotSupportedException();
                }

                result.Add(new Classification()
                {
                    Name = attr.Description,
                    Id = Convert.ToInt32(Enum.Parse(enumType, enumName))
                });
            }
            return result;
        }
    }
}
