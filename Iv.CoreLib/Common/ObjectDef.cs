using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

using Iv.Text;
using Iv.Data;
using Iv.Reflection;
using Iv.Validation;

namespace Iv.Common
{
    public class ObjectDef : ObjectDefBase<string>
    {

        public ObjectDef()
        {
            Properties = new List<ObjectDefProperty>();
            DbObjects = new List<ObjectDefDbObject>();
            Validations = new List<ObjectDefPropertyValidation>();
            Views = new List<ObjectDefView>();
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string ListName { get; set; }
        public string EntityName { get; set; }
        public string ListDisplayName { get; set; }

        [IgnoreColumn(DataOperation.All)]
        public List<ObjectDefProperty> Properties { get; set; }
        [IgnoreColumn(DataOperation.All)]
        public List<ObjectDefDbObject> DbObjects { get; private set; }
        [IgnoreColumn(DataOperation.All)]
        public List<ObjectDefPropertyValidation> Validations { get; private set; }
        [IgnoreColumn(DataOperation.All)]
        public List<ObjectDefView> Views { get; set; }

        public ObjectDefProperty GetProperty(string propName)
        {
            ObjectDefProperty prop = null;
            prop = (from p in Properties where p.Name.Equals(propName, StringComparison.InvariantCultureIgnoreCase) select p).SingleOrDefault();
            return prop;
        }

        public ObjectDefProperty CreateProperty(string name, string displayName, string description,
            string columnName, string typeName, bool isKey, int propertyOrder, int length
        )
        {
            var p = new ObjectDefProperty()
            {
                ColumnName = columnName,
                DisplayName = displayName,
                Description = description,
                PropertyOrder = propertyOrder,
                ObjectDefName = this.Name,
                Name = name,
                IsKey = isKey,
                TypeName = typeName,
                Length = length, 
                ListDisplay = true,
                EditDisplay = true,
                DataSource = "",
                DefaultExpression = "",
                IsUserKey = false,
                SortOrder = 0,
                SortType = SortType.None
                
            };
            p.SetNew();
            Properties.Add(p);
            return p;
        }

        public ObjectDefProperty CreateDataSourceProperty(string name, string displayName, string description,
            string columnName, string typeName, bool isKey, int propertyOrder, int length,
            string dataSource
        )
        {
            var p = CreateProperty(name, displayName, description, columnName, typeName, isKey, propertyOrder, length);
            p.DataSource = dataSource;
            return p;
        }

        /// <summary>
        /// Creates a [name]Text property used for displaying the text rather thank the value for data source properties.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="columnName"></param>
        /// <param name="typeName"></param>
        /// <param name="isKey"></param>
        /// <param name="propertyOrder"></param>
        /// <param name="length"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public ObjectDefProperty CreateDataSourceTextProperty(string name, string displayName, string description,
            string columnName, string typeName, bool isKey, int propertyOrder, int length,
            string dataSource
        )
        {
            ObjectDefProperty p = GetProperty(name);
            if (p == null)
            {
                p = CreateProperty(name, displayName, description, columnName, typeName, isKey, propertyOrder, length);
            }
            p.DataSource = dataSource;
            p.ListDisplay = false;
            var textp = CreateProperty(name + "Text", displayName, description, columnName, typeName, isKey, p.PropertyOrder + 1, length);
            textp.DataSource = dataSource;
            textp.EditDisplay = false;
            textp.IgnoreColumnDataOperation = DataOperation.All;
            return textp;
        }

        public void DeleteProperty(string propName)
        {
            var prop = GetProperty(propName);
            if (prop != null)
            {
                prop.SetDeleted();
            }
        }

        public IEnumerable<ObjectDefProperty> GetOrderedProperties()
        {
            return Properties.OrderBy(p => p.PropertyOrder);
        }

        public IEnumerable<ObjectDefProperty> GetKeys()
        {
            return Properties.Where(p => p.IsKey).OrderBy(p => p.PropertyOrder);
        }

        public ObjectDefProperty GetKey()
        {
            return GetKeys().FirstOrDefault();
        }

        public ObjectDefDbObject GetDbObject(ObjectDefDbObjectType type)
        {
            return DbObjects.Where(p => p.Type == type).FirstOrDefault();
        }

        public static ObjectDef CreateObjectDef<T>()
        {
            Type type = typeof(T);
            string name = type.Name;
            string listName = name.Pluralize();
            ObjectDef objDef = new ObjectDef()
            {
                Name = type.Name,
                Description = "Object definition for {Name}.".Interpolate(new { Name = name }),
                DisplayName = name.SplitCamelCase(),
                EntityName = listName,
                ListName = listName
            };
            int index = 1;
            foreach (var p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (p.PropertyType.IsSimpleType())
                {
                    objDef.Properties.Add(new ObjectDefProperty()
                    {
                        Name = p.Name,
                        ColumnName = p.Name,
                        Description = "Property definition for {Name}.".Interpolate(new { Name = p.Name }),
                        DisplayName = p.Name.SplitCamelCase(),
                        Length = "System.String".Equals(p.PropertyType.FullName) ? 100 : 0,
                        ObjectDefName = name,
                        PropertyOrder = index * 10,
                        TypeName = p.PropertyType.Name,
                        IsKey = index == 1,
                        IsNew = true,
                        ListDisplay = true,
                        EditDisplay = true                        
                    });
                }
                index++;
            }
            IDictionary<string, ObjectDefDbObjectType> dict = new Dictionary<string, ObjectDefDbObjectType>();
            string part = "{Name}".Interpolate(new { Name = name });
            string parts = "{Name}".Interpolate(new { Name = name.Pluralize() });
            dict.Add("uspCreate" + part, ObjectDefDbObjectType.Create);
            dict.Add("uspDelete" + part, ObjectDefDbObjectType.Delete);
            dict.Add("uspGet" + part, ObjectDefDbObjectType.Get);
            dict.Add("uspGet" + parts, ObjectDefDbObjectType.GetAll);
            dict.Add("uspUpdate" + part, ObjectDefDbObjectType.Update);
            foreach (var k in dict.Keys)
            {
                objDef.DbObjects.Add(new ObjectDefDbObject()
                {
                    Id = (new Random()).Next(),
                    Name = k,
                    ObjectDefName = name,
                    Type = dict[k]
                });
                Thread.Sleep(150);
            }
            return objDef;
        }

        public ObjectDef Copy()
        {
            return (ObjectDef)this.MemberwiseClone();
        }

        public bool ContainsProperty(string propName)
        {
            if(string.IsNullOrEmpty(propName))
            {
                return false;
            }
            return this.Properties.Where(p => propName.Equals(p.Name, StringComparison.InvariantCultureIgnoreCase)).Any();
        }

        public IEnumerable<ObjectDefProperty> GetFilterProperties()
        {
            return this.Properties.Where(p => p.CanFilter);
        }

        public IDictionary<string, object> GetFilterValues<T>(T o)
        {
            IDictionary <string, object> dict = new Dictionary<string, object>();
            foreach (var p in GetFilterProperties())
            {
                var pi = typeof(T).GetProperty(p.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if(pi != null)
                {
                    var value = pi.GetValue(o);
                    dict.Add(p.Name, value == null ? DBNull.Value : value);
                }
            }
            return dict;
        }

        public Condition ToCondition()
        {
            Condition cond = null;
            foreach (var prop in this.Properties.OrderBy(p => p.PropertyOrder))
            {
                if (cond == null)
                {
                    cond = prop.ToCondition(prop.FilterOperator, BinaryOperators.None);
                }
                else
                {
                    var childCond = prop.ToCondition(prop.FilterOperator, BinaryOperators.And);
                    cond.Conditions.Add(childCond);
                }
            }
            return cond;
        }

        public ObjectDefInstance GetInstance()
        {
            var instance = new ObjectDefInstance() { Name = this.Name };
            foreach(var p in this.Properties)
            {
                // instance.Properties.Add(p.Name, new ObjectDefInstanceProperty(instance) { Name = p.Name, Value = p.Value });
                instance.Properties.Add(p.Name, p.Value);
            }
            return instance;
        }

        public ObjectDefPropertyValidation GetValidation(string propName, ValidationType type)
        {
            var q = (from v in this.Validations where v.PropertyName.EqualsNoCase(propName) && v.ValidationType == type select v);
            if (!q.Any()) return null;
            return q.FirstOrDefault();
        }

        public IEnumerable<ObjectDefPropertyValidation> GetValidations(string propName)
        {
            var q = (from v in this.Validations where v.PropertyName.EqualsNoCase(propName) select v);
            return q;
        }

    }
}
