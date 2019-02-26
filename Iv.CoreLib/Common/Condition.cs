using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Data.SqlClient;

namespace Iv.Common
{
    [Serializable()]
    [XmlRoot("C")]
    public class Condition
    {

        const string OPEN_C_TAG = "<C>";
        const string CLOSE_C_TAG = "</C>";
        const string OPEN_OP_TAG = "<Op>";
        const string CLOSE_OP_TAG = "</Op>";
        const string OPEN_TEXT_TAG = "<Text>";
        const string CLOSE_TEXT_TAG = "</Text>";
        const string OPEN_T_TAG = "<T>";
        const string CLOSE_T_TAG = "</T>";
        const string OPEN_N_TAG = "<N>";
        const string CLOSE_N_TAG = "</N>";
        const string OPEN_V1_TAG = "<V1>";
        const string CLOSE_V1_TAG = "</V1>";
        const string OPEN_V2_TAG = "<V2>";
        const string CLOSE_V2_TAG = "</V2>";
        const string OPEN_COP_TAG = "<COp>";
        const string CLOSE_COP_TAG = "</COp>";

        public Condition()
        {
            Conditions = new List<Condition>();
            Parent = null;
            this.Enabled = true;
        }

        public Condition(string propertyName, string typeName, object value1) : this()
        {
            this.PropertyName = propertyName;
            this.TypeName = typeName;
            this.Value1 = value1;
            Parent = null;
            this.Enabled = true;
        }

        [XmlElement("T")]
        public string TypeName { get; set; }
        [XmlElement("N")]
        public string PropertyName { get; set; }
        [XmlElement("V1")]
        public object Value1 { get; set; }
        [XmlElement("V2")]
        public object Value2 { get; set; }
        [XmlElement("COp")]
        public ComparisonOperators ComparisonOperator { get; set; }
        [XmlElement("Neg")]
        public bool Negate { get; set; }
        [XmlElement("Op")]
        public BinaryOperators BinaryOperator { get; set; }
        [XmlElement("C")]
        [XmlElement("E")]
        public bool Enabled { get; set; }
        public List<Condition> Conditions { get; set; }
        /*
        [XmlElement("UP")]

        public bool UsePaging { get; set; }
        [XmlElement("OBE")]

        public string OrderByExpression { get; set; }
        [XmlElement("MinRN")]

        public int MinRowNo { get; set; }
        [XmlElement("MaxRN")]

        public int MaxRowNo { get; set; }
        [XmlElement("PS")]

        public int PageSize { get; set; }
        */
        public bool IsComposite { get; set; }
        [XmlIgnore]
        public Condition Parent { get; set; }

        public void Add(Condition childCond)
        {
            Conditions.Add(childCond);
            childCond.Parent = this;
        }

        public void Remove(Condition childCond)
        {
            childCond.Parent = null;
            Conditions.Remove(childCond);
        }

        public Condition Clone()
        {
            Condition cond = new Condition();
            CopyCondition(this, cond);
            return cond;
        }

        private void CopyCondition(Condition source, Condition target)
        {
            target.BinaryOperator = source.BinaryOperator;
            target.ComparisonOperator = source.ComparisonOperator;
            target.IsComposite = source.IsComposite;
            //target.MaxRowNo = source.MaxRowNo;
            //target.MinRowNo = source.MinRowNo;
            target.Negate = source.Negate;
            //target.OrderByExpression = source.OrderByExpression;
            //target.PageSize = source.PageSize;
            target.PropertyName = source.PropertyName;
            target.TypeName = source.TypeName;
            //target.UsePaging = source.UsePaging;
            target.Value1 = source.Value1;
            target.Value2 = source.Value2;
            foreach (Condition sc in source.Conditions)
            {
                Condition tc = new Condition();
                CopyCondition(sc, tc);
                target.Conditions.Add(tc);
            }
        }

        public static void SetRecursiveTypeName(Condition c, string typeName)
        {
            c.TypeName = typeName;
            foreach (Condition childCond in c.Conditions)
            {
                SetRecursiveTypeName(childCond, typeName);
            }
        }

        public static void MapRecursiveTypeName(Condition c, string sourceTypeName, string targetTypeName)
        {
            if (c.TypeName.Equals(sourceTypeName))
                c.TypeName = targetTypeName;
            foreach (Condition childCond in c.Conditions)
            {
                MapRecursiveTypeName(childCond, sourceTypeName, targetTypeName);
            }
        }

        public static ComparisonOperators GetComparisonOperator(string op)
        {
            ComparisonOperators eOp = ComparisonOperators.Equal;
            switch (op)
            {
                case "=":
                case "==":
                    eOp = ComparisonOperators.Equal;
                    break;
                case "!=":
                case "<>":
                    eOp = ComparisonOperators.NotEqual;
                    break;
                case "<":
                    eOp = ComparisonOperators.LessThan;
                    break;
                case "<=":
                    eOp = ComparisonOperators.LessOrEqualThan;
                    break;
                case ">":
                    eOp = ComparisonOperators.GreaterThan;
                    break;
                case ">=":
                    eOp = ComparisonOperators.GreaterOrEqualThan;
                    break;
                case "IS NULL":
                    eOp = ComparisonOperators.IsNull;
                    break;
                case "LIKE":
                    eOp = ComparisonOperators.Contains;
                    break;
                default:
                    break;
            }
            return eOp;
        }

        public static string GetComparisonOperatorCSharp(ComparisonOperators op)
        {
            string eOp = string.Empty;
            switch (op)
            {
                case ComparisonOperators.Equal:
                    eOp = "==";
                    break;
                case ComparisonOperators.NotEqual:
                    eOp = "!=";
                    break;
                case ComparisonOperators.LessThan:
                    eOp = "<";
                    break;
                case ComparisonOperators.LessOrEqualThan:
                    eOp = "<=";
                    break;
                case ComparisonOperators.GreaterThan:
                    eOp = ">";
                    break;
                case ComparisonOperators.GreaterOrEqualThan:
                    eOp = ">=";
                    break;
                case ComparisonOperators.IsNull:
                    eOp = "== null";
                    break;
                case ComparisonOperators.IsNotNull:
                    eOp = "!= null";
                    break;
                case ComparisonOperators.Contains:
                    eOp = "LIKE";
                    break;
                default:
                    break;
            }
            return eOp;
        }

        public static string GetComparisonOperatorSql(ComparisonOperators op)
        {
            string eOp = string.Empty;
            switch (op)
            {
                case ComparisonOperators.Equal:
                    eOp = "=";
                    break;
                case ComparisonOperators.NotEqual:
                    eOp = "<>";
                    break;
                case ComparisonOperators.LessThan:
                    eOp = "<";
                    break;
                case ComparisonOperators.LessOrEqualThan:
                    eOp = "<=";
                    break;
                case ComparisonOperators.GreaterThan:
                    eOp = ">";
                    break;
                case ComparisonOperators.GreaterOrEqualThan:
                    eOp = ">=";
                    break;
                case ComparisonOperators.IsNull:
                    eOp = "IS NULL";
                    break;
                case ComparisonOperators.Contains:
                    eOp = "LIKE";
                    break;
                default:
                    break;
            }
            return eOp;
        }

        public static BinaryOperators GetBinaryOperator(string binaryOp)
        {
            BinaryOperators binOp = BinaryOperators.None;
            switch (binaryOp.ToUpper())
            {
                case "AND":
                case "&&":
                    binOp = BinaryOperators.And;
                    break;
                case "OR":
                case "||":
                    binOp = BinaryOperators.Or;
                    break;
                default:
                    break;
            }
            return binOp;
        }

        private static string ParseCondition(string condition)
        {
            string quotePattern = "(\"[^\"]+\")|('[^']+')";
            string opPattern = "(?<op>(AND)|(OR))";
            string replacement = "</C><C><Op>${op}</Op>";
            Match prevM = null;
            string condtionString = string.Empty;
            foreach (Match m in Regex.Matches(condition, quotePattern, RegexOptions.IgnoreCase))
            {
                if (prevM == null)
                {
                    condtionString += Regex.Replace(condition.Substring(0, m.Index), opPattern, replacement, RegexOptions.IgnoreCase) + m.Value;
                }
                else
                {
                    condtionString += Regex.Replace(condition.Substring(prevM.Index + prevM.Length + 1, m.Index - (prevM.Index + prevM.Length + 1)), opPattern, replacement, RegexOptions.IgnoreCase) + m.Value;
                }
                prevM = m;
            }
            if (string.IsNullOrEmpty(condtionString)) condtionString = condition;
            int index = condtionString.IndexOf(CLOSE_C_TAG);
            if (index >= 0)
                condtionString = condtionString.Remove(index, CLOSE_C_TAG.Length) + CLOSE_C_TAG;

            return condtionString;
        }

        public static string GetFiltersXmlString(string filters)
        {
            System.Xml.XmlDocument doc = GetFiltersXmlDocument(filters);
            return doc.OuterXml;
        }

        public static string PrepareFiltersXmlString(string filters)
        {
            //string qPattern = "(?<dq>\\\"[^\"]*\\\")"; //|(?<sq>\\'[^']*\\')";
            string lopPattern = "(?<op>(\\band\\b)|(\\bor\\b)|(&&)|(\\|\\|))(?<ob>\\s*\\()";
            //string pattern2 = @"\)";
            //string pattern3 = @"\(";
            //string opPattern = @"\s*(\>=|\<=|BETWEEN|IS NULL|\!=|==|\<|\>|=)\s*";
            string ltPattern = @"\s*\<\s*";
            string gtPattern = @"\s*\>\s*";
            string s1 = Regex.Replace(filters, ltPattern, " &lt;", RegexOptions.IgnoreCase);
            s1 = Regex.Replace(s1, gtPattern, " &gt;", RegexOptions.IgnoreCase);
            s1 = Regex.Replace(s1, lopPattern, CLOSE_TEXT_TAG + OPEN_C_TAG + OPEN_OP_TAG + "${op}" + CLOSE_OP_TAG + OPEN_TEXT_TAG, RegexOptions.IgnoreCase);
            if(s1.EndsWith(CLOSE_TEXT_TAG))
                s1 = OPEN_C_TAG + OPEN_TEXT_TAG + s1 + CLOSE_C_TAG;
            else
                s1 = OPEN_C_TAG + OPEN_TEXT_TAG + s1 + CLOSE_TEXT_TAG + CLOSE_C_TAG;
            s1 = Regex.Replace(s1, "&&", " &amp;&amp;", RegexOptions.IgnoreCase);
            int ob = 0; //int cb = 0;
            bool isQ = false;
            int offset = 0;
            string s2 = s1.Clone().ToString();
            for (int i = 0; i < s1.Length; i++)
            {
                if (s1[i] == '\"')
                {
                    isQ = !isQ;
                    continue;
                }
                if (s1[i] == '(' && !isQ)
                {
                    ob++;
                    continue;
                }
                if (s1[i] == ')' && !isQ)
                {
                    if (ob - 1 < 0)
                    {
                        s2 = s2.Remove(i + offset, 1);
                        s2 = s2.Insert(i + offset, CLOSE_TEXT_TAG + CLOSE_C_TAG);
                        offset += (CLOSE_TEXT_TAG + CLOSE_C_TAG).Length - 1;
                    }
                    else
                    {
                        ob--;
                    }
                }
            }
            string cleanupPattern = @"\<\/C\>\s*\<\/Text\>\s*\<\/C\>";
            while(Regex.IsMatch(s2, cleanupPattern))
                s2 = Regex.Replace(s2, cleanupPattern, CLOSE_C_TAG + CLOSE_C_TAG);
            //MatchCollection coll = Regex.Matches(filters, qPattern + "|" + lopPattern, RegexOptions.IgnoreCase);
            //string token;
            //int index = 0;
            //foreach (Match m in coll)
            //{
            //    if (m.Groups["op"].Success)
            //    {                    
            //        token = filters.Substring(index, m.Groups["op"].Index - index);
            //        filtersXmlString += Regex.Replace(token, lopPattern, OPEN_C_TAG + OPEN_OP_TAG + "${op}" + CLOSE_OP_TAG + OPEN_TEXT_TAG, RegexOptions.IgnoreCase);
            //        filtersXmlString += m.Groups["dq"].Value;
            //        index = m.Groups["dq"].Index + m.Groups["dq"].Length;
            //    }

            //}
            //filtersXmlString = Regex.Replace(filtersXmlString, pattern2, CLOSE_TEXT_TAG + CLOSE_C_TAG);
            //filtersXmlString = Regex.Replace(filtersXmlString, pattern3, OPEN_OP_TAG + CLOSE_OP_TAG + OPEN_C_TAG + OPEN_TEXT_TAG);
            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(s2);
            return s2;
        }

        public static System.Xml.XmlDocument GetFiltersXmlDocument(string filters)
        {
            //string pattern1 = @"((?<op>(AND)|(OR)|(\|\|)|(\&\&)))\s*\(";
            //string pattern2 = @"\)";
            //string pattern3 = @"\(";
            string opPattern = @"\s*(\>=|\<=|BETWEEN|IS NULL|\!=|==|\<|\>|=)\s*";
            //string ltPattern = @"\s*\<\s*";
            //string ltEqPattern = @"\s*\<=\s*";
            //string gtPattern = @"\s*\>\s*";
            //string gtEqPattern = @"\s*\>=\s*";
            string filtersXmlString = string.Empty;
            //filtersXmlString = Regex.Replace(filters, ltPattern, " &lt;", RegexOptions.IgnoreCase);
            //filtersXmlString = Regex.Replace(filtersXmlString, gtPattern, " &gt;", RegexOptions.IgnoreCase);
            //filtersXmlString = Regex.Replace(filtersXmlString, pattern1, OPEN_C_TAG + OPEN_OP_TAG + "${op}" + CLOSE_OP_TAG + OPEN_TEXT_TAG, RegexOptions.IgnoreCase);
            //filtersXmlString = Regex.Replace(filtersXmlString, pattern2, CLOSE_TEXT_TAG + CLOSE_C_TAG);
            //filtersXmlString = Regex.Replace(filtersXmlString, pattern3, OPEN_OP_TAG + CLOSE_OP_TAG + OPEN_C_TAG + OPEN_TEXT_TAG);
            //filtersXmlString = filtersXmlString.Replace("&&", "&amp;&amp;");
            //if (filtersXmlString.Contains(OPEN_C_TAG))
            //    filtersXmlString = OPEN_C_TAG + filtersXmlString + CLOSE_C_TAG;
            //else
            //    filtersXmlString = OPEN_C_TAG + OPEN_TEXT_TAG + filtersXmlString + CLOSE_TEXT_TAG + CLOSE_C_TAG;
            filtersXmlString = PrepareFiltersXmlString(filters);
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(filtersXmlString);
            foreach (System.Xml.XmlNode node in doc.SelectNodes("//Text"))
            {
                if (node.ChildNodes.Count == 1 && node.ChildNodes[0].NodeType == System.Xml.XmlNodeType.Text)
                {
                    node.InnerXml = OPEN_C_TAG + ParseCondition(System.Uri.UnescapeDataString(node.InnerXml)) + CLOSE_C_TAG;
                }
            }
            filtersXmlString = doc.InnerXml;
            filtersXmlString = filtersXmlString.Replace("<Text>", string.Empty); ;
            filtersXmlString = filtersXmlString.Replace("</Text>", string.Empty); ;
            doc.InnerXml = filtersXmlString;
            string[] arr;
            System.Xml.XmlDocumentFragment frag;
            string xmlFrag = string.Empty;
            bool bContinue = false;
            Match m;
            System.Xml.XmlNode tn;
            string nodeText;
            foreach (System.Xml.XmlNode n in doc.SelectNodes("//C"))
            {
                tn = n.SelectSingleNode("text()");
                if (tn != null)
                {
                    nodeText = System.Uri.UnescapeDataString(tn.InnerText);
                    m = Regex.Match(nodeText, opPattern, RegexOptions.IgnoreCase);
                    if (m.Success)
                    {
                        arr = Regex.Split(nodeText, opPattern, RegexOptions.IgnoreCase);
                        bContinue = false;
                        switch (arr.Length)
                        {
                            case 0:
                                break;
                            case 2:
                                xmlFrag = OPEN_N_TAG + arr[0].Trim() + CLOSE_N_TAG + OPEN_COP_TAG + arr[1].Trim() + CLOSE_COP_TAG;
                                bContinue = true;
                                break;
                            case 3:
                                xmlFrag = OPEN_N_TAG + arr[0].Trim() + CLOSE_N_TAG + OPEN_COP_TAG + Condition.GetComparisonOperator(arr[1]).ToString() + CLOSE_COP_TAG + OPEN_V1_TAG + arr[2].Trim() + CLOSE_V1_TAG;
                                bContinue = true;
                                break;
                            default:
                                break;
                        }
                        if (bContinue && !string.IsNullOrEmpty(xmlFrag))
                        {
                            frag = doc.CreateDocumentFragment();
                            frag.InnerXml = xmlFrag;
                            n.ReplaceChild(frag, tn);
                        }
                    }
                }
            }
            foreach (System.Xml.XmlNode n in doc.SelectNodes("//Op"))
            {
                if (string.IsNullOrEmpty(n.InnerText.Trim()))
                    n.InnerText = "None";
                //n.InnerText = ((BinaryOperators)Enum.Parse(typeof(BinaryOperators), n.InnerText.Trim(), true)).ToString();
                n.InnerText = Condition.GetBinaryOperator(n.InnerText.Trim()).ToString();
            }
            return doc;
        }

        public static Condition FromString(string filters)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(Condition.GetFiltersXmlString(filters));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            doc.Save(ms);
            ms.Flush();
            ms.Position = 0;
            Condition c = new Condition();
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(Condition));
            System.Xml.XmlReader rdr = System.Xml.XmlReader.Create(ms);

            c = (Condition)ser.Deserialize(rdr);
            ms.Close();
            FixCondition(c);
            return c;
        }

        public static Condition FromXmlString(string condXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Condition));
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(condXml);
            Condition cond = null;
            using (MemoryStream ms = new MemoryStream())
            {
                xmlDoc.Save(ms);
                ms.Position = 0;
                XmlTextReader rdr = (XmlTextReader)XmlTextReader.Create(ms);
                cond = (Condition)serializer.Deserialize(rdr);
                rdr.Close();
            }
            return cond;
        }

        private static object FixValue(object value)
        {
            if (value == null)
                return string.Empty;
            else
                if (typeof(System.Xml.XmlNode[]).Name == value.GetType().Name && ((System.Xml.XmlNode[])value).Length > 0)
                    return ((System.Xml.XmlNode[])value)[0].InnerText;
                else
                    if (typeof(System.Xml.XmlNode).Name == value.GetType().Name)
                        return ((System.Xml.XmlNode)value).InnerText;
            return string.Empty;
        }

        private static void FixCondition(Condition c)
        {
            c.IsComposite = string.IsNullOrEmpty(c.PropertyName);
            if(c.TypeName == null)
                c.TypeName = string.Empty;
            c.Value1 = FixValue(c.Value1);
            c.Value2 = FixValue(c.Value2);
            foreach (Condition childCond in c.Conditions)
                FixCondition(childCond);
        }

        public static string QuoteValue(string value, string typeName, string quote)
        {
            string qValue = value;
            string t = typeName;
            if (t == null) t = string.Empty;
            switch(t.ToUpper())
            {
                case null:
                case "":
                    qValue = quote + qValue + quote;
                    break;
                case "SYSTEM.STRING":
                    qValue = quote + qValue + quote;
                    break;
                case "SYSTEM.DATETIME":
                    qValue = quote + qValue + quote;
                    break;
                default:
                    break;
            }
            return qValue;
        }

        private string _lambdaString;
        private void SetLambdaString(Condition c)
        {
            if (!c.Enabled) return;
            string binOp = string.Empty;
            if (c.BinaryOperator == BinaryOperators.And)
                binOp = " && ";
            else
                if (c.BinaryOperator == BinaryOperators.Or)
                    binOp = " || ";
            binOp = string.IsNullOrEmpty(binOp) ? "" : binOp + "(";
            if (!c.IsComposite)
            {
                switch (c.ComparisonOperator)
                {
                    case ComparisonOperators.IsNotNull:
                    case ComparisonOperators.IsNull:
                        _lambdaString += binOp + (c.Negate ? "!(" : "") + c.PropertyName + " " + GetComparisonOperatorCSharp(c.ComparisonOperator) + (c.Negate ? ")" : "");
                        break;
                    case ComparisonOperators.Contains:
                        if (c.Value1 != null)
                            _lambdaString += binOp + (c.Negate ? "!(" : "") + c.PropertyName + ".Contains(" + QuoteValue(c.Value1.ToString(), c.Value1.GetType().FullName, "\"") + ")" + (c.Negate ? ")" : "");
                        else
                            _lambdaString += binOp + (c.Negate ? "!(" : "") + c.PropertyName + ".Contains(\"\")" + (c.Negate ? ")" : "");
                        break;
                    case ComparisonOperators.StartsWith:
                        if (c.Value1 != null)
                            _lambdaString += binOp + (c.Negate ? "!(" : "") + c.PropertyName + ".StartsWith(" + QuoteValue(c.Value1.ToString(), c.Value1.GetType().FullName, "\"") + ")" + (c.Negate ? ")" : "");
                        else
                            _lambdaString += binOp + (c.Negate ? "!(" : "") + c.PropertyName + ".StartsWith(\"\")" + (c.Negate ? ")" : "");
                        break;
                    case ComparisonOperators.EndsWith:
                        if (c.Value1 != null)
                            _lambdaString += binOp + (c.Negate ? "!(" : "") + c.PropertyName + ".EndsWith(" + QuoteValue(c.Value1.ToString(), c.Value1.GetType().FullName, "\"") + ")" + (c.Negate ? ")" : "");
                        else
                            _lambdaString += binOp + (c.Negate ? "!(" : "") + c.PropertyName + ".EndWith(\"\")" + (c.Negate ? ")" : "");
                        break;
                    default:
                        if (c.Value1 != null)
                            _lambdaString += binOp + (c.Negate ? "!(" : "") + c.PropertyName + " " + GetComparisonOperatorCSharp(c.ComparisonOperator) + " " + QuoteValue(c.Value1.ToString(), c.Value1.GetType().FullName, "\"") + (c.Negate ? ")" : "");
                        else
                            _lambdaString += binOp + (c.Negate ? "!(" : "") + c.PropertyName + " " + GetComparisonOperatorCSharp(c.ComparisonOperator) + " \"\"" + (c.Negate ? ")" : "");
                        break;
                }
            }
            else
            {
                _lambdaString += binOp;
            }
            foreach (Condition childCond in c.Conditions)
                SetLambdaString(childCond);
            _lambdaString += string.IsNullOrEmpty(binOp) ? "" : ")";
        }

        public string ToLambdaString()
        {
            _lambdaString = string.Empty;
            SetLambdaString(this);
            return _lambdaString;
        }

        private string _sqlString;
        private void SetSqlString(Condition c, bool useParameter)
        {
            if (!c.Enabled) return;
            string binOp = string.Empty;
            if (c.BinaryOperator == BinaryOperators.And)
                binOp = " AND ";
            else
                if (c.BinaryOperator == BinaryOperators.Or)
                    binOp = " OR ";
            binOp = string.IsNullOrEmpty(binOp) ? "" : binOp + "(";
            if (!c.IsComposite)
            {
                string target = string.Empty;
                if (c.Value1 != null)
                    target = (useParameter ? "@" + c.PropertyName : QuoteValue(c.Value1.ToString(), c.Value1.GetType().FullName, "'"));
                else
                    target = "@" + c.PropertyName;

                switch (c.ComparisonOperator)
                {
                    case ComparisonOperators.Contains:
                        target = "'%' + " + target + " + '%'";
                        break;
                    //case ComparisonOperators.BeginsWith:
                    //    target = "'%' + " + target + "'%'";
                    //    break;
                    //case ComparisonOperators.EndsWith:
                    //    target = "'%' + " + target + "'%'";
                    //    break;
                    default:
                        break;
                }
                _sqlString += binOp + (c.Negate ? "NOT(" : "") + string.Format("[{0}]", c.PropertyName) + " " + GetComparisonOperatorSql(c.ComparisonOperator) + " " + target + (c.Negate ? ")" : "");
            }
            else
            {
                _sqlString += binOp;
            }
            foreach (Condition childCond in c.Conditions)
                SetSqlString(childCond, useParameter);
            _sqlString += string.IsNullOrEmpty(binOp) ? "" : ")";
        }

        public string ToSqlString(bool useParameter)
        {
            _sqlString = string.Empty;
            SetSqlString(this, useParameter);
            return _sqlString;
        }

        private SqlParameterCollection sqlParams;

        public IEnumerable<SqlParameter> GetSqlParameters()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                sqlParams = cmd.Parameters;
                AddSqlParameter(this);
                SqlParameter[] arrSqlParams = new SqlParameter[sqlParams.Count];
                sqlParams.CopyTo(arrSqlParams, 0);
                cmd.Parameters.Clear();
                return arrSqlParams;
            }
        }

        private void AddSqlParameter(Condition cond)
        {
            if (!cond.IsComposite)
            {
                sqlParams.AddWithValue("@" + cond.PropertyName, cond.Value1 != null ? cond.Value1 : DBNull.Value);
            }
            foreach (Condition childCond in cond.Conditions)
            {
                AddSqlParameter(childCond);
            }
        }
    }

    public enum ComparisonOperators
    {
        Equal = 0,
        GreaterThan = 1,
        GreaterOrEqualThan = 2,
        LessThan = 3,
        LessOrEqualThan = 4,
        Between = 5,
        IsNull = 6,
        Contains = 7,
        NotEqual = 8,
        StartsWith = 9,
        EndsWith = 10,
        IsNotNull = 12
    }

    public enum BinaryOperators
    {
        None = 0,
        And = 1,
        Or = 2
    }
}
