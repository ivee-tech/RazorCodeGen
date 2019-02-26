using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Iv.Data;
using Iv.Text;

namespace Iv.Common
{
    public class ObjectDefProperty : ObjectDefBase<string>
    {

        public ObjectDefProperty()
        {
            Validations = new List<ObjectDefPropertyValidation>();
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string TypeName { get; set; }
        public bool IsKey { get; set; }
        public int Length { get; set; }
        public string ObjectDefName { get; set; }
        public string ColumnName { get; set; }
        public int PropertyOrder { get; set; }
        public string DataSource { get; set; }
        public DataOperation IgnoreColumnDataOperation { get; set; }
        public SortType SortType { get; set; }
        public int SortOrder { get; set; }
        public bool CanFilter { get; set; }
        public ComparisonOperators FilterOperator { get; set; }
        public bool ListDisplay { get; set; }
        public bool EditDisplay { get; set; }
        public bool IsUserKey { get; set; }
        public string DefaultExpression { get; set; }
        public bool IsReadOnly { get; set; }
        [IgnoreColumn(DataOperation.All)]
        public object Value { get; set; }
        [IgnoreColumn(DataOperation.All)]
        public List<ObjectDefPropertyValidation> Validations { get; private set; }

        public string NetTypeName
        {
            get
            {
                if(string.IsNullOrEmpty(TypeName))
                {
                    return "System.String";
                }
                if(ObjectDefPropertyTypes.Tel.EqualsNoCase(TypeName))
                {
                    return "System.String";
                }
                if(ObjectDefPropertyTypes.Email.EqualsNoCase(TypeName))
                {
                    return "System.String";
                }
                if(ObjectDefPropertyTypes.Password.EqualsNoCase(TypeName))
                {
                    return "System.String";
                }
                return "System." + TypeName;
            }
        }

        public Type GetPropertyType()
        {
            return Type.GetType(NetTypeName);
        }

        public static object GetValue<T>(string name, T t)
        {
            if (t == null)
            {
                return null;
            }
            var prop = typeof(T).GetProperty(name);
            if (prop == null)
            {
                return null;
            }
            return prop.GetValue(t);
        }

        public bool HasIgnoreColumn(DataOperation operation)
        {
            if(IgnoreColumnDataOperation == DataOperation.None)
            {
                return false;
            }
            return (IgnoreColumnDataOperation & operation) == operation;
        }

        public Condition ToCondition(ComparisonOperators compOp, BinaryOperators binOp)
        {
            var cond = new Condition() { PropertyName = this.Name, ComparisonOperator = compOp, Value1 = this.Value, BinaryOperator = binOp };
            return cond;
        }
    }
}
