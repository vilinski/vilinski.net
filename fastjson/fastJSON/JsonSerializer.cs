using System;
using System.Collections;
using System.Collections.Generic;
#if SILVERLIGHT

#else
using System.Data;
#endif
using System.Globalization;
using System.IO;
using System.Text;

namespace fastJSON
{
    internal class JSONSerializer
    {
        private readonly StringBuilder _output = new StringBuilder();
        readonly bool _useMinimalDataSetSchema;
        readonly bool _fastguid = true;
        readonly bool _useExtension = true;
        readonly bool _serializeNulls = true;
        private const int MaxDepth = 10;
        readonly bool _indent;
        int _currentDepth;

        internal JSONSerializer(bool useMinimalDataSetSchema, bool useFastGuid, bool useExtensions, bool serializeNulls, bool indentOutput)
        {
            _useMinimalDataSetSchema = useMinimalDataSetSchema;
            _fastguid = useFastGuid;
            _useExtension = useExtensions;
            _indent = indentOutput;
            _serializeNulls = serializeNulls;
        }

        internal string ConvertToJSON(object obj)
        {
            WriteValue(obj);

            return _output.ToString();
        }

        private void WriteValue(object obj)
        {
            if (_serializeNulls && (obj == null || obj is DBNull))
                _output.Append("null");

            else if (obj is string || obj is char)
                WriteString((string)obj);

            else if (obj is Guid)
                WriteGuid((Guid)obj);

            else if (obj is bool)
                _output.Append(((bool)obj) ? "true" : "false"); // conform to standard

            else if (
                obj is int || obj is long || obj is double ||
                obj is decimal || obj is float ||
                obj is byte || obj is short ||
                obj is sbyte || obj is ushort ||
                obj is uint || obj is ulong
            )
                _output.Append(((IConvertible)obj).ToString(NumberFormatInfo.InvariantInfo));

            else if (obj is DateTime)
                WriteDateTime((DateTime)obj);

            else if (obj is IDictionary<string, string>)
                WriteStringDictionary((IDictionary)obj);

            else if (obj is IDictionary)
                WriteDictionary((IDictionary)obj);
#if !SILVERLIGHT
            else if (obj is DataSet)
                WriteDataset((DataSet)obj);

            else if (obj is DataTable)
                WriteDataTable((DataTable)obj);
#endif
            else if (obj is byte[])
                WriteBytes((byte[])obj);

            else if (obj is Array || obj is IList || obj is ICollection)
                WriteArray((IEnumerable)obj);

            else if (obj is Enum)
                WriteEnum((Enum)obj);

#if CUSTOMTYPE
            else if (JSON.Instance.IsTypeRegistered(obj.GetType()))
                WriteCustom(obj);
#endif
            else
                WriteObject(obj);
        }

#if CUSTOMTYPE
        private void WriteCustom(object obj)
        {
            Serialize s;
            JSON.Instance._customSerializer.TryGetValue(obj.GetType(), out s);
            WriteStringFast(s(obj));
        }
#endif

        private void WriteEnum(Enum e)
        {
            // TODO : optimize enum write
            WriteStringFast(e.ToString());
        }

        private void WriteGuid(Guid g)
        {
            if (_fastguid == false)
                WriteStringFast(g.ToString());
            else
                WriteBytes(g.ToByteArray());
        }

        private void WriteBytes(byte[] bytes)
        {
#if !SILVERLIGHT
            WriteStringFast(Convert.ToBase64String(bytes, 0, bytes.Length, Base64FormattingOptions.None));
#else
            WriteStringFast(Convert.ToBase64String(bytes, 0, bytes.Length));
#endif
        }

        private void WriteDateTime(DateTime dateTime)
        {
            // datetime format standard : yyyy-MM-dd HH:mm:ss

            _output.Append("\"");
            _output.Append(dateTime.Year.ToString("0000", NumberFormatInfo.InvariantInfo));
            _output.Append("-");
            _output.Append(dateTime.Month.ToString("00", NumberFormatInfo.InvariantInfo));
            _output.Append("-");
            _output.Append(dateTime.Day.ToString("00", NumberFormatInfo.InvariantInfo));
            _output.Append(" ");
            _output.Append(dateTime.Hour.ToString("00", NumberFormatInfo.InvariantInfo));
            _output.Append(":");
            _output.Append(dateTime.Minute.ToString("00", NumberFormatInfo.InvariantInfo));
            _output.Append(":");
            _output.Append(dateTime.Second.ToString("00", NumberFormatInfo.InvariantInfo));
            _output.Append("\"");
        }
#if !SILVERLIGHT
        private DatasetSchema GetSchema(DataSet ds)
        {
            if (ds == null) return null;

            var m = new DatasetSchema {Info = new List<string>(), Name = ds.DataSetName};

            foreach (DataTable t in ds.Tables)
            {
                foreach (DataColumn c in t.Columns)
                {
                    m.Info.Add(t.TableName);
                    m.Info.Add(c.ColumnName);
                    m.Info.Add(c.DataType.ToString());
                }
            }
            // TODO : serialize relations and constraints here

            return m;
        }

		private string GetXmlSchema( DataTable dt )
		{
			using( var writer = new StringWriter())
			{
				dt.WriteXmlSchema( writer );
 				return dt.ToString();
			}
		}

        private void WriteDataset(DataSet ds)
        {
            _output.Append('{');
            if (_useExtension)
            {
                WritePair("$schema", _useMinimalDataSetSchema ? (object)GetSchema(ds) : ds.GetXmlSchema());
                _output.Append(',');
            }
            foreach (DataTable table in ds.Tables)
            {
                WriteDataTableData(table);
            }
            // end dataset
            _output.Append('}');
        }

        private void WriteDataTableData(DataTable table)
        {
            _output.Append('\"');
            _output.Append(table.TableName);
            _output.Append("\":[");
            DataColumnCollection cols = table.Columns;
            foreach (DataRow row in table.Rows)
            {
                _output.Append('[');

                bool pendingSeperator = false;
                foreach (DataColumn column in cols)
                {
                    if (pendingSeperator) _output.Append(',');
                    WriteValue(row[column]);
                    pendingSeperator = true;
                }
                _output.Append(']');
            }

            _output.Append(']');
        }

		void WriteDataTable( DataTable dt )
		{
			_output.Append( '{' );
			if( _useExtension )
			{
				WritePair( "$schema", _useMinimalDataSetSchema ? ( object )GetSchema( dt.DataSet ) : GetXmlSchema( dt ) );
				_output.Append( ',' );
			}

            WriteDataTableData(dt);

			// end datatable
			_output.Append( '}' );
		}
#endif
        private void WriteObject(object obj)
        {
            Indent();
            _currentDepth++;
            if (_currentDepth > MaxDepth)
                throw new Exception("Serializer encountered maximum depth of " + MaxDepth);

            _output.Append('{');
            var map = new Dictionary<string, string>();
            Type t = obj.GetType();
            bool append = false;
            if (_useExtension)
            {
                WritePairFast("$type", JSON.Instance.GetTypeAssemblyName(t));
                append = true;
            }

            List<Getters> g = JSON.Instance.GetGetters(t);
            foreach (var p in g)
            {
                if (append)
                    _output.Append(',');
                object o = p.Getter(obj);
                WritePair(p.Name, o);
                if (o != null && _useExtension)
                {
                	Type tt = o.GetType();
                	if (tt == typeof(Object))
                        map.Add(p.Name, tt.ToString());
                }
                append = true;
            }
            if (map.Count > 0 && _useExtension)
            {
                _output.Append(",\"$map\":");
                WriteStringDictionary(map);
            }
            _currentDepth--;    
            Indent();
            _output.Append('}');
            _currentDepth--;

        }

        private void Indent()
        {
            Indent(false);
        }

        private void Indent(bool dec)
        {
            if (_indent)
            {
                _output.Append("\r\n");
                for (int i = 0; i < _currentDepth-(dec?1:0); i++)
                    _output.Append("\t");
            }
        }

        private void WritePairFast(string name, string value)
        {
            Indent();
            WriteStringFast(name);

            _output.Append(':');

            WriteStringFast(value);
        }

        private void WritePair(string name, object value)
        {
			Indent();
            WriteStringFast(name);

            _output.Append(':');

            WriteValue(value);
        }

        private void WriteArray(IEnumerable array)
        {
			Indent();
            _output.Append('[');

            bool pendingSeperator = false;

            foreach (object obj in array)
            {
                if (pendingSeperator) _output.Append(',');

                WriteValue(obj);

                pendingSeperator = true;
            }
            Indent();
            _output.Append(']');
        }

        private void WriteStringDictionary(IDictionary dic)
        {
            Indent();
            _output.Append('{');

            bool pendingSeparator = false;

            foreach (DictionaryEntry entry in dic)
            {
                if (pendingSeparator) _output.Append(',');

                WritePairFast((string)entry.Key, (string)entry.Value);

                pendingSeparator = true;
            }
            Indent();
            _output.Append('}');
        }

        private void WriteDictionary(IDictionary dic)
        {
            Indent();
            _output.Append('[');

            bool pendingSeparator = false;

            foreach (DictionaryEntry entry in dic)
            {
                if (pendingSeparator) _output.Append(',');
                Indent();
                _output.Append('{');
                WritePair("k", entry.Key);
                _output.Append(",");
                WritePair("v", entry.Value);
                Indent();
                _output.Append('}');

                pendingSeparator = true;
            }
            Indent();
            _output.Append(']');
        }

        private void WriteStringFast(string s)
        {
            //Indent();
            _output.Append('\"');
            _output.Append(s);
            _output.Append('\"');
        }

        private void WriteString(string s)
        {
            //Indent();
            _output.Append('\"');

            int runIndex = -1;

            for (var index = 0; index < s.Length; ++index)
            {
                var c = s[index];

                if (c >= ' ' && c < 128 && c != '\"' && c != '\\')
                {
                    if (runIndex == -1)
                    {
                        runIndex = index;
                    }

                    continue;
                }

                if (runIndex != -1)
                {
                    _output.Append(s, runIndex, index - runIndex);
                    runIndex = -1;
                }

                switch (c)
                {
                    case '\t': _output.Append("\\t"); break;
                    case '\r': _output.Append("\\r"); break;
                    case '\n': _output.Append("\\n"); break;
                    case '"':
                    case '\\': _output.Append('\\'); _output.Append(c); break;
                    default:
                        _output.Append("\\u");
                        _output.Append(((int)c).ToString("X4", NumberFormatInfo.InvariantInfo));
                        break;
                }
            }

            if (runIndex != -1)
            {
                _output.Append(s, runIndex, s.Length - runIndex);
            }

            _output.Append('\"');
        }
    }
}
