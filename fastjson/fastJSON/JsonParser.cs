using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace fastJSON
{
    /// <summary>
    /// This class encodes and decodes JSON strings.
    /// Spec. details, see http://www.json.org/
    /// 
    /// JSON uses Arrays and Objects. These correspond here to the datatypes ArrayList and Hashtable.
    /// All numbers are parsed to doubles.
    /// </summary>
    internal class JsonParser
    {
        enum Token
        {
            None = -1,           // Used to denote no Lookahead available
            CurlyOpen,
            CurlyClose,
            SquaredOpen,
            SquaredClose,
            Colon,
            Comma,
            String,
            Number,
            True,
            False,
            Null
        }

        readonly char[] _json;
        readonly StringBuilder _s = new StringBuilder();
        Token _lookAheadToken = Token.None;
        int _index;

        internal JsonParser(string json)
        {
            _json = json.ToCharArray();
        }

        public object Decode()
        {
            return ParseValue();
        }

        private Dictionary<string, object> ParseObject()
        {
            var table = new Dictionary<string, object>();

            ConsumeToken(); // {

            while (true)
            {
                switch (LookAhead())
                {

                    case Token.Comma:
                        ConsumeToken();
                        break;

                    case Token.CurlyClose:
                        ConsumeToken();
                        return table;

                    default:
                        {

                            // name
                            string name = ParseString();

                            // :
                            if (NextToken() != Token.Colon)
                            {
                                throw new Exception("Expected colon at index " + _index);
                            }

                            // value
                            object value = ParseValue();

                            table[name] = value;
                        }
                        break;
                }
            }
        }

#if SILVERLIGHT
        private List<object> ParseArray()
        {
            var array = new List<object>();
#else
        private ArrayList ParseArray()
        {
            var array = new ArrayList();
#endif
            ConsumeToken(); // [

            while (true)
            {
                switch (LookAhead())
                {

                    case Token.Comma:
                        ConsumeToken();
                        break;

                    case Token.SquaredClose:
                        ConsumeToken();
                        return array;

                    default:
                        {
                            array.Add(ParseValue());
                        }
                        break;
                }
            }
        }

        private object ParseValue()
        {
            switch (LookAhead())
            {
                case Token.Number:
                    return ParseNumber();

                case Token.String:
                    return ParseString();

                case Token.CurlyOpen:
                    return ParseObject();

                case Token.SquaredOpen:
                    return ParseArray();

                case Token.True:
                    ConsumeToken();
                    return true;

                case Token.False:
                    ConsumeToken();
                    return false;

                case Token.Null:
                    ConsumeToken();
                    return null;
            }

            throw new Exception("Unrecognized token at index" + _index);
        }

        private string ParseString()
        {
            ConsumeToken(); // "

            _s.Length = 0;

            int runIndex = -1;

            while (_index < _json.Length)
            {
                var c = _json[_index++];

                if (c == '"')
                {
                    if (runIndex != -1)
                    {
                        if (_s.Length == 0)
                            return new string(_json, runIndex, _index - runIndex - 1);

                        _s.Append(_json, runIndex, _index - runIndex - 1);
                    }
                    return _s.ToString();
                }

                if (c != '\\')
                {
                    if (runIndex == -1)
                        runIndex = _index - 1;

                    continue;
                }

                if (_index == _json.Length) break;

                if (runIndex != -1)
                {
                    _s.Append(_json, runIndex, _index - runIndex - 1);
                    runIndex = -1;
                }

                switch (_json[_index++])
                {
                    case '"':
                        _s.Append('"');
                        break;

                    case '\\':
                        _s.Append('\\');
                        break;

                    case '/':
                        _s.Append('/');
                        break;

                    case 'b':
                        _s.Append('\b');
                        break;

                    case 'f':
                        _s.Append('\f');
                        break;

                    case 'n':
                        _s.Append('\n');
                        break;

                    case 'r':
                        _s.Append('\r');
                        break;

                    case 't':
                        _s.Append('\t');
                        break;

                    case 'u':
                        {
                            int remainingLength = _json.Length - _index;
                            if (remainingLength < 4) break;

                            // parse the 32 bit hex into an integer codepoint
                            uint codePoint = ParseUnicode(_json[_index], _json[_index + 1], _json[_index + 2], _json[_index + 3]);
                            _s.Append((char)codePoint);

                            // skip 4 chars
                            _index += 4;
                        }
                        break;
                }
            }

            throw new Exception("Unexpectedly reached end of string");
        }

        private uint ParseSingleChar(char c1, uint multipliyer)
        {
            uint p1 = 0;
            if (c1 >= '0' && c1 <= '9')
                p1 = (uint)(c1 - '0') * multipliyer;
            else if (c1 >= 'A' && c1 <= 'F')
                p1 = (uint)((c1 - 'A') + 10) * multipliyer;
            else if (c1 >= 'a' && c1 <= 'f')
                p1 = (uint)((c1 - 'a') + 10) * multipliyer;
            return p1;
        }

        private uint ParseUnicode(char c1, char c2, char c3, char c4)
        {
            uint p1 = ParseSingleChar(c1, 0x1000);
            uint p2 = ParseSingleChar(c2, 0x100);
            uint p3 = ParseSingleChar(c3, 0x10);
            uint p4 = ParseSingleChar(c4, 1);

            return p1 + p2 + p3 + p4;
        }

        private string ParseNumber()
        {
            ConsumeToken();

            // Need to start back one place because the first digit is also a token and would have been consumed
            var startIndex = _index - 1;

            do
            {
                var c = _json[_index];

                if ((c >= '0' && c <= '9') || c == '.' || c == '-' || c == '+' || c == 'e' || c == 'E')
                {
                    if (++_index == _json.Length) throw new Exception("Unexpected end of string whilst parsing number");
                    continue;
                }

                break;
            } while (true);

            return new string(_json, startIndex, _index - startIndex);
        }

        private Token LookAhead()
        {
            if (_lookAheadToken != Token.None) return _lookAheadToken;

            return _lookAheadToken = NextTokenCore();
        }

        private void ConsumeToken()
        {
            _lookAheadToken = Token.None;
        }

        private Token NextToken()
        {
            var result = _lookAheadToken != Token.None ? _lookAheadToken : NextTokenCore();

            _lookAheadToken = Token.None;

            return result;
        }

        private Token NextTokenCore()
        {
            char c;

            // Skip past whitespace
            do
            {
                c = _json[_index];

                if (c > ' ') break;
                if (c != ' ' && c != '\t' && c != '\n' && c != '\r') break;

            } while (++_index < _json.Length);

            if (_index == _json.Length)
            {
                throw new Exception("Reached end of string unexpectedly");
            }

            c = _json[_index];

            _index++;

            //if (c >= '0' && c <= '9')
            //    return Token.Number;

            switch (c)
            {
                case '{':
                    return Token.CurlyOpen;

                case '}':
                    return Token.CurlyClose;

                case '[':
                    return Token.SquaredOpen;

                case ']':
                    return Token.SquaredClose;

                case ',':
                    return Token.Comma;

                case '"':
                    return Token.String;

				case '0': case '1': case '2': case '3': case '4':
				case '5': case '6': case '7': case '8': case '9':
                case '-': case '+': case '.':
                    return Token.Number;

                case ':':
                    return Token.Colon;

                case 'f':
                    if (_json.Length - _index >= 4 &&
                        _json[_index + 0] == 'a' &&
                        _json[_index + 1] == 'l' &&
                        _json[_index + 2] == 's' &&
                        _json[_index + 3] == 'e')
                    {
                        _index += 4;
                        return Token.False;
                    }
                    break;

                case 't':
                    if (_json.Length - _index >= 3 &&
                        _json[_index + 0] == 'r' &&
                        _json[_index + 1] == 'u' &&
                        _json[_index + 2] == 'e')
                    {
                        _index += 3;
                        return Token.True;
                    }
                    break;

                case 'n':
                    if (_json.Length - _index >= 3 &&
                        _json[_index + 0] == 'u' &&
                        _json[_index + 1] == 'l' &&
                        _json[_index + 2] == 'l')
                    {
                        _index += 3;
                        return Token.Null;
                    }
                    break;

            }

            throw new Exception("Could not find token at index " + --_index);
        }
    }
}
