using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Iv.Common;
using Iv.Reflection;

namespace Iv.Data
{
    /// <summary>
    /// Generates Sql commands based on the Model configuration and property values.
    /// </summary>
    /// <remarks>The property values are passsed as a dictionary where they keys match the property names (not the entity field names).</remarks>
    public class ObjectDefValuesSqlCommandBuilder
    {

        public static void ConfigureCreateCommand(ObjectDef m, string schemaName, SqlCommand cmd, ObjectDefValues values = null)
        {
            cmd.CommandType = CommandType.Text;
            string entityName = m.EntityName;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbParams = new StringBuilder();
            sb.AppendLine(string.Format("INSERT INTO [{0}].[{1}](", schemaName, entityName));
            var q = m.GetOrderedProperties();
            for (int i = 0; i < q.Count(); i++)
            {
                ObjectDefProperty mp = q.ElementAt(i);
                var entityPropName = mp.ColumnName;
                if (i == 0)
                {
                    sb.AppendLine(string.Format("\t[{0}]", entityPropName));
                    sbParams.AppendLine(string.Format("\t@{0}", entityPropName));
                }
                else
                {
                    sb.AppendLine(string.Format("\t, [{0}]", entityPropName));
                    sbParams.AppendLine(string.Format("\t, @{0}", entityPropName));
                }
                if (values != null)
                {
                    cmd.Parameters.AddWithValue("@" + entityPropName, values.Values.ContainsKey(mp.Name) && values.Values[mp.Name] != null ? Convert.ChangeType(values.Values[mp.Name], TypeHelper.GetFullType(null, mp.TypeName)) : DBNull.Value);
                }
            }
            sb.AppendLine(")");
            sb.AppendLine("VALUES(");
            sb.AppendLine(sbParams.ToString());
            sb.AppendLine(")");
            cmd.CommandText = sb.ToString();
        }

        public static void ConfigureGetAllCommand(ObjectDef m, string schemaName, SqlCommand cmd)
        {
            cmd.CommandType = CommandType.Text;
            string entityName = m.EntityName;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            var q = m.GetOrderedProperties();
            for (int i = 0; i < q.Count(); i++)
            {
                ObjectDefProperty mp = q.ElementAt(i);
                var entityPropName = mp.ColumnName;
                if (i == 0)
                {
                    sb.AppendLine(string.Format("\t[{0}]", entityPropName));
                }
                else
                {
                    sb.AppendLine(string.Format("\t, [{0}]", entityPropName));
                }
            }
            sb.AppendLine(string.Format("FROM [{0}].[{1}]", schemaName, entityName));
            cmd.CommandText = sb.ToString();
        }

        public static void ConfigureUpdateCommand(ObjectDef m, string schemaName, SqlCommand cmd, ObjectDefValues values = null)
        {
            cmd.CommandType = CommandType.Text;
            string entityName = m.EntityName;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbFilters = new StringBuilder();
            sb.AppendLine(string.Format("UPDATE [{0}].[{1}] SET", schemaName, entityName));
            var q = m.GetOrderedProperties();
            bool bKeyAdded = false;
            bool setColumnAdded = false;
            for (int i = 0; i < q.Count(); i++)
            {
                ObjectDefProperty mp = q.ElementAt(i);
                var entityPropName = mp.ColumnName;
                if (mp.IsKey)
                {
                    if (!bKeyAdded)
                    {
                        sbFilters.Append(string.Format("WHERE [{0}] = @{0}", entityPropName));
                        bKeyAdded = true;
                    }
                    else
                    {
                        sbFilters.Append(string.Format("AND [{0}] = @{0}", entityPropName));
                    }
                }
                else
                {
                    if (!setColumnAdded)
                    {
                        sb.AppendLine(string.Format("\t[{0}] = @{0}", entityPropName));
                        setColumnAdded = true;
                    }
                    else
                    {
                        sb.AppendLine(string.Format("\t, [{0}] = @{0}", entityPropName));
                    }
                }
                if (values != null)
                {
                    cmd.Parameters.AddWithValue("@" + entityPropName, values.Values[mp.Name] != null ? Convert.ChangeType(values.Values[mp.Name], TypeHelper.GetFullType(null, mp.TypeName)) : DBNull.Value);
                }
            }
            sb.AppendLine(sbFilters.ToString());
            cmd.CommandText = sb.ToString();
        }

        public static void ConfigureFilterCommand(ObjectDef m, string schemaName, SqlCommand cmd, Condition cond)
        {
            cmd.CommandType = CommandType.Text;
            string entityName = m.EntityName;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            var q = m.GetOrderedProperties();
            for (int i = 0; i < q.Count(); i++)
            {
                ObjectDefProperty mp = q.ElementAt(i);
                var entityPropName = mp.ColumnName;
                if (i == 0)
                {
                    sb.AppendLine(string.Format("\t[{0}]", entityPropName));
                }
                else
                {
                    sb.AppendLine(string.Format("\t, [{0}]", entityPropName));
                }
            }
            sb.AppendLine(string.Format("FROM [{0}].[{1}]", schemaName, entityName));
            if (cond != null)
            {
                string condString = cond.ToSqlString(true);
                if (!string.IsNullOrEmpty(condString))
                {
                    sb.AppendLine(string.Format("WHERE {0}", condString));
                    cmd.Parameters.AddRange(cond.GetSqlParameters().ToArray());
                }
            }
            cmd.CommandText = sb.ToString();
        }

        public static void ConfigureFilterCommand(ObjectDef m, string schemaName, SqlCommand cmd, Condition cond, string orderByExpression, int pageNumber, int pageSize)
        {
            cmd.CommandType = CommandType.Text;
            string entityName = m.EntityName;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbColumns = new StringBuilder();
            var q = m.GetOrderedProperties();
            for (int i = 0; i < q.Count(); i++)
            {
                ObjectDefProperty mp = q.ElementAt(i);
                var entityPropName = mp.ColumnName;
                if (i == 0)
                {
                    sbColumns.AppendLine(string.Format("\t[{0}]", entityPropName));
                }
                else
                {
                    sbColumns.AppendLine(string.Format("\t, [{0}]", entityPropName));
                }
            }
            string columns = sbColumns.ToString();
            string filters = string.Empty;
            if (cond != null)
            {
                string condString = cond.ToSqlString(true);
                if (!string.IsNullOrEmpty(condString))
                {
                    filters = string.Format("WHERE {0}", condString);
                    cmd.Parameters.AddRange(cond.GetSqlParameters().ToArray());
                }
            }
            sb.AppendLine(string.Format(";WITH {0}CTE AS", entityName));
            sb.AppendLine("(");
            sb.AppendLine("\tSELECT");
            sb.AppendLine(columns);
            sb.AppendLine(string.Format("\t, ROW_NUMBER() OVER (ORDER BY {0}) AS RowNumber", orderByExpression));
            sb.AppendLine(string.Format("FROM [{0}].[{1}]", schemaName, entityName));
            sb.AppendLine(filters);
            sb.AppendLine(")");
            sb.AppendLine("SELECT TOP(@PageSize)");
            sb.AppendLine(columns);
            sb.AppendLine(string.Format("FROM {0}CTE", entityName));
            sb.AppendLine("WHERE RowNumber > (@PageNumber - 1) * @PageSize");
            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.CommandText = sb.ToString();
        }

        public static void ConfigureCountCommand(ObjectDef m, string schemaName, SqlCommand cmd, Condition cond)
        {
            cmd.CommandType = CommandType.Text;
            string entityName = m.EntityName;
            StringBuilder sb = new StringBuilder();
            var strKeys = m.GetKeys().Select(p => p.ColumnName).Aggregate((r, i) => r + ", " + i);
            sb.AppendLine(string.Format("SELECT COUNT({0})", strKeys));
            sb.AppendLine(string.Format("FROM [{0}].[{1}]", schemaName, entityName));
            if (cond != null)
            {
                string condString = cond.ToSqlString(true);
                if(!string.IsNullOrEmpty(condString)) 
                {
                    sb.AppendLine(string.Format("WHERE {0}", condString));
                    cmd.Parameters.AddRange(cond.GetSqlParameters().ToArray());
                }
            }
            cmd.CommandText = sb.ToString();
        }

        public static void ConfigureFindCommand(ObjectDef m, string schemaName, SqlCommand cmd, params object[] keys)
        {
            cmd.CommandType = CommandType.Text;
            string entityName = m.EntityName;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            var q = m.GetOrderedProperties();
            for (int i = 0; i < q.Count(); i++)
            {
                ObjectDefProperty mp = q.ElementAt(i);
                var entityPropName = mp.ColumnName;
                if (i == 0)
                {
                    sb.AppendLine(string.Format("\t[{0}]", entityPropName));
                }
                else
                {
                    sb.AppendLine(string.Format("\t, [{0}]", entityPropName));
                }
            }
            sb.AppendLine(string.Format("FROM [{0}].[{1}]", schemaName, entityName));
            var qKeys = m.GetKeys().Select(p => p.ColumnName);
            for (int i = 0; i < qKeys.Count(); i++)
            {
                if (keys[i] != null)
                {
                    sb.AppendLine(string.Format("WHERE [{0}] = @{0}", qKeys.ElementAt(i)));
                    cmd.Parameters.AddWithValue("@" + qKeys.ElementAt(i), keys[i]);
                }
            }
            cmd.CommandText = sb.ToString();
        }

        public static void ConfigureDeleteCommand(ObjectDef m, string schemaName, SqlCommand cmd, ObjectDefValues values = null)
        {
            cmd.CommandType = CommandType.Text;
            string entityName = m.EntityName;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DELETE ");
            sb.AppendLine(string.Format("FROM [{0}].[{1}]", schemaName, entityName));
            var qProps = m.GetOrderedProperties();
            for (int i = 0; i < qProps.Count(); i++)
            {
                var mp = qProps.ElementAt(i);
                if (qProps.ElementAt(i).IsKey)
                {
                    sb.AppendLine(string.Format("WHERE [{0}] = @{0}", mp.ColumnName));
                    if (values != null)
                    {
                        cmd.Parameters.AddWithValue("@" + mp.ColumnName, values.Values[mp.Name] != null ? values.Values[mp.Name] : DBNull.Value);
                    }
                }
            }
            cmd.CommandText = sb.ToString();
        }

    }
}
