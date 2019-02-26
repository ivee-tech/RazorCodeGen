using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Data.SqlClient;
using System.ComponentModel;
using Iv.Text;

namespace Iv.Data
{

    public static class SqlDataReaderExtensions
    {

        public static T GetValue<T>(this SqlDataReader reader, string name)
        {
            Type type = typeof(T);
            type = Nullable.GetUnderlyingType(type) ?? type;
            /*
            if(type == typeof(Iv.GeoCoding.Location))
            {
                if(reader[name] == DBNull.Value)
                    return (T)(object)Iv.GeoCoding.Location.Empty;
                string sLocation = reader[name].ToString();
                return (T)(object)Iv.GeoCoding.Location.Parse(sLocation);
            }
            */
            return (T)Convert.ChangeType(reader[name], type);
            //Return DirectCast(TypeDescriptor.GetConverter(type).ConvertFromInvariantString(reader(name).ToString()), T)
        }

        public static T GetValue<T>(this SqlDataReader reader, Int32 i)
        {
            Type type = typeof(T);
            type = Nullable.GetUnderlyingType(type) ?? type;
            /*
            if(type == typeof(Iv.GeoCoding.Location))
            {
                if(reader[i] == DBNull.Value)
                    return (T)(object)Iv.GeoCoding.Location.Empty;
                string sLocation = reader[i].ToString();
                return (T)(object)Iv.GeoCoding.Location.Parse(sLocation);
            }
            */
            return (T)Convert.ChangeType(reader[i], type);
            //Return DirectCast(TypeDescriptor.GetConverter(type).ConvertFromInvariantString(reader(i).ToString()), T)
        }

        public static bool TryGetValue<T>(this SqlDataReader reader, string name, ref T output)
        {
            try
            {
                Type type = typeof(T);
                type = Nullable.GetUnderlyingType(type) ?? type;
                output = reader.GetValue<T>(name);
                return true;
            }
            catch (Exception ex)
            {
                Type ext = ex.GetType();
                if (object.ReferenceEquals(ext, typeof(InvalidCastException)) || object.ReferenceEquals(ext, typeof(FormatException)) || object.ReferenceEquals(ext, typeof(OverflowException)))
                {
                    output = default(T);
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public static bool TryGetValue<T>(this SqlDataReader reader, Int32 i, ref T output)
        {
            try
            {
                output = reader.GetValue<T>(i);
                return true;
            }
            catch (Exception ex)
            {
                Type ext = ex.GetType();
                if (object.ReferenceEquals(ext, typeof(InvalidCastException)) || object.ReferenceEquals(ext, typeof(FormatException)) || object.ReferenceEquals(ext, typeof(OverflowException)))
                {
                    output = default(T);
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public static bool TryGetValue(this SqlDataReader reader, string typeName, string name, ref object output)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                output = null;
                return false;
            }
            bool bRet = false;
            switch (typeName.ToLower())
            {
                case "system.string":
                    string sOutput = default(string);
                    bRet = TryGetValue<System.String>(reader, name, ref sOutput);
                    output = sOutput;
                    break;
                case "system.boolean":
                    bool boolOutput = default(bool);
                    bRet = TryGetValue<System.Boolean>(reader, name, ref boolOutput);
                    output = boolOutput;
                    break;
                case "system.boolean?":
                    bool? boolOutputN = default(bool?);
                    bRet = TryGetValue<System.Boolean?>(reader, name, ref boolOutputN);
                    output = boolOutputN;
                    break;
                case "system.byte":
                    byte byteOutput = default(byte);
                    bRet = TryGetValue<System.Byte>(reader, name, ref byteOutput);
                    output = byteOutput;
                    break;
                case "system.byte?":
                    byte? byteOutputN = default(byte?);
                    bRet = TryGetValue<System.Byte?>(reader, name, ref byteOutputN);
                    output = byteOutputN;
                    break;
                case "system.int16":
                    Int16 int16Output = default(Int16);
                    bRet = TryGetValue<System.Int16>(reader, name, ref int16Output);
                    output = int16Output;
                    break;
                case "system.int16?":
                    Int16? int16OutputN = default(Int16?);
                    bRet = TryGetValue<System.Int16?>(reader, name, ref int16OutputN);
                    output = int16OutputN;
                    break;
                case "system.int32":
                    Int32 int32Output = default(Int32);
                    bRet = TryGetValue<System.Int32>(reader, name, ref int32Output);
                    output = int32Output;
                    break;
                case "system.int32?":
                    Int32? int32OutputN = default(Int32?);
                    bRet = TryGetValue<System.Int32?>(reader, name, ref int32OutputN);
                    output = int32OutputN;
                    break;
               case "system.int64":
                    Int64 int64Output = default(Int64);
                    bRet = TryGetValue<System.Int64>(reader, name, ref int64Output);
                    output = int64Output;
                    break;
               case "system.int64?":
                    Int64? int64OutputN = default(Int64?);
                    bRet = TryGetValue<System.Int64?>(reader, name, ref int64OutputN);
                    output = int64OutputN;
                    break;
                case "system.decimal":
                    decimal decOutput = default(decimal);
                    bRet = TryGetValue<System.Decimal>(reader, name, ref decOutput);
                    output = decOutput;
                    break;
                case "system.decimal?":
                    decimal? decOutputN = default(decimal?);
                    bRet = TryGetValue<System.Decimal?>(reader, name, ref decOutputN);
                    output = decOutputN;
                    break;
                case "system.single":
                    System.Single singleOutput = default(System.Single);
                    bRet = TryGetValue<System.Single>(reader, name, ref singleOutput);
                    output = singleOutput;
                    break;
                case "system.single?":
                    System.Single? singleOutputN = default(System.Single?);
                    bRet = TryGetValue<System.Single?>(reader, name, ref singleOutputN);
                    output = singleOutputN;
                    break;
                case "system.double":
                    System.Double doubleOutput = default(System.Double);
                    bRet = TryGetValue<System.Double>(reader, name, ref doubleOutput);
                    output = doubleOutput;
                    break;
                case "system.double?":
                    System.Double? doubleOutputN = default(System.Double?);
                    bRet = TryGetValue<System.Double?>(reader, name, ref doubleOutputN);
                    output = doubleOutputN;
                    break;
                case "system.datetime":
                    DateTime dOutput = default(DateTime);
                    bRet = TryGetValue<System.DateTime>(reader, name, ref dOutput);
                    output = dOutput;
                    break;
                case "system.datetime?":
                    DateTime? dOutputN = default(DateTime?);
                    bRet = TryGetValue<System.DateTime?>(reader, name, ref dOutputN);
                    output = dOutputN;
                    break;
                    /*
                case "location":
                    Ins.GeoCoding.Location loc = default(Ins.GeoCoding.Location);
                    bRet = TryGetValue<Ins.GeoCoding.Location>(reader, name, ref loc);
                    output = loc;
                    break;
                    */
                default:
                    var type = Type.GetType(typeName, false);
                    if (type != null && type.IsEnum) // one last attempt to get the value, in case it's an enum
                    {
                        int32Output = default(Int32);
                        bRet = TryGetValue<System.Int32>(reader, name, ref int32Output);
                        output = int32Output;
                        break;
                    }
                    else
                    {
                        bRet = false;
                        output = null;
                    }
                    break;
            }
            return bRet;
        }

        public static bool ContainsColumn(this SqlDataReader rdr, string columnName)
        {
            bool hasColumnName = rdr.GetSchemaTable().AsEnumerable().Any(c => columnName.EqualsNoCase(c["ColumnName"].ToString()));
            return hasColumnName;
        }

    }
}
