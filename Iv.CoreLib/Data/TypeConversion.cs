using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Iv.Data
{
    public class TypeConversion
    {
        public static DbType GetDotNetTypeToDbType(Type t)
        {
            return GetDbTypeFromDotNetType(t.FullName);
        }

        public static DbType GetDbTypeFromDotNetType(string typeName)
        {
            DbType dbT = DbType.String;
            switch (typeName)
            {
                case "System.String":
                    dbT = DbType.String;
                    break;
                case "System.Boolean":
                    dbT = DbType.Boolean;
                    break;
                case "System.DateTime":
                    dbT = DbType.DateTime;
                    break;
                case "System.Int16":
                    dbT = DbType.Int16;
                    break;
                case "System.Int32":
                    dbT = DbType.Int32;
                    break;
                case "System.Int64":
                    dbT = DbType.Int64;
                    break;
                case "System.Single":
                    dbT = DbType.Single;
                    break;
                case "System.Double":
                    dbT = DbType.Double;
                    break;
                case "System.Decimal":
                    dbT = DbType.Decimal;
                    break;
                case "System.Byte":
                    dbT = DbType.Byte;
                    break;
                case "System.":
                    //dbT = DbType;
                    break;
                default:
                    dbT = DbType.String;
                    break;
            }
            return dbT;
        }


        public static Type GetDotNetTypeFromSqlDbType(SqlDbType sqlDbType)
        {
            Type t = typeof(string);
            switch (sqlDbType)
            {
                case SqlDbType.BigInt:
                    return typeof(long);

                case SqlDbType.Binary:
                case SqlDbType.Image:
                case SqlDbType.Timestamp:
                case SqlDbType.VarBinary:
                    return typeof(byte[]);

                case SqlDbType.Bit:
                    return typeof(bool);

                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                    return typeof(string);

                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                    return typeof(DateTime);

                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                    return typeof(decimal);

                case SqlDbType.Float:
                    return typeof(double);

                case SqlDbType.Int:
                    return typeof(int);

                case SqlDbType.Real:
                    return typeof(float);

                case SqlDbType.UniqueIdentifier:
                    return typeof(Guid);

                case SqlDbType.SmallInt:
                    return typeof(short);

                case SqlDbType.TinyInt:
                    return typeof(byte);

                case SqlDbType.Variant:
                    return t;

                case (SqlDbType.SmallInt | SqlDbType.Int):
                case (SqlDbType.Text | SqlDbType.Int):
                case (SqlDbType.Xml | SqlDbType.Bit):
                case (SqlDbType.TinyInt | SqlDbType.Int):
                    return t;

                case SqlDbType.Xml:
                    return typeof(string);

                case SqlDbType.Udt:
                    return t;
            }
            return t;
        }

        public static SqlDbType GetSqlDbTypeFromDotNetType(System.Type type)
        {
            System.Data.SqlClient.SqlParameter p1;
            System.ComponentModel.TypeConverter tc;
            p1 = new System.Data.SqlClient.SqlParameter();
            tc = System.ComponentModel.TypeDescriptor.GetConverter(p1.DbType);
            if (tc.CanConvertFrom(type))
            {
                p1.DbType = (DbType)tc.ConvertFrom(type.Name);
            }
            else
            {
                //Try brute force
                try
                {
                    p1.DbType = (DbType)tc.ConvertFrom(type.Name);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            return p1.SqlDbType;
        }

        public static SqlDbType GetSqlDbTypeFromDotNetType(string typeName)
        {
            if(!typeName.StartsWith("System."))
            {
                typeName = "System." + typeName;
            }
            Type type = Type.GetType(typeName);
            return GetSqlDbTypeFromDotNetType(type);
        }
    }
}
