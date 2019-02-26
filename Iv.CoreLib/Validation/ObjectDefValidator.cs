using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Iv.Validation
{

	public class ObjectDefValidator<T, C> where T : ValidationAttribute where C : class
	{

		private Type _type = typeof(C);

		private C _instance;
		public ObjectDefValidator(C instance)
		{
			if (instance == null) {
				throw new ArgumentNullException("instance", "The class instance cannot be null.");
			}
			_instance = instance;
		}

		public T GetClassAttribute()
		{
			object[] attrs = _type.GetCustomAttributes(typeof(T), false);
			if (!(attrs.Any())) {
				return null;
			}
			return (T)attrs[0];
		}

		public T GetPropertyAttribute(string property)
		{
			System.Reflection.PropertyInfo pi = _type.GetProperty(property);
			if (pi == null) {
				throw new Exception(string.Format("Class {0} does not have a property named {1}.", typeof(C).Name, property));
			}
			object[] attrs = pi.GetCustomAttributes(typeof(T), false);
			if (!(attrs.Any())) {
				return null;
			}
			return (T)attrs[0];
		}

		public bool IsValid()
		{
			T attribute = this.GetClassAttribute();
			if (attribute == null) {
				return false;
			}
			return attribute.IsValid(_instance);
		}

		public bool IsValid(string property)
		{
			T attribute = GetPropertyAttribute(property);
			if (attribute == null) {
				return false;
			}
			return attribute.IsValid(_type.GetProperty(property).GetValue(_instance, null));
		}

	}

}
