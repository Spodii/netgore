using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MySql.Data.Common
{
    class SqlTokenizer
    {
        readonly string input;
        //private StringBuilder current;
        bool ansiQuotes;
        bool backslashEscapes;
        int index;
        bool inParamters;
        bool inSize;
        bool isSize;
        bool quoted;

        public bool AnsiQuotes
        {
            get { return ansiQuotes; }
            set { ansiQuotes = value; }
        }

        public bool BackslashEscapes
        {
            get { return backslashEscapes; }
            set { backslashEscapes = value; }
        }

        public int Index
        {
            get { return index; }
        }

        public bool IsSize
        {
            get { return isSize; }
        }

        public bool Quoted
        {
            get { return quoted; }
        }

        public SqlTokenizer(string input)
        {
            this.input = input;
            index = -1;
            backslashEscapes = true;
            //current = new StringBuilder();
        }

        public string NextToken()
        {
            char lastChar = Char.MinValue;
            bool escaped = false;
            char quoteChar = Char.MinValue;
            StringBuilder current = new StringBuilder();
            bool inComment = false;
            bool inLineComment = false;
            quoted = isSize = false;

            while ((index + 1) < input.Length)
            {
                char c = input[++index];

                if (escaped)
                {
                    current.Append(c);
                    escaped = false;
                }
                else if (c == quoteChar)
                {
                    quoted = true;
                    return current.ToString();
                }
                else if (quoteChar != Char.MinValue)
                    current.Append(c);
                else if ((c == '`' || c == '\'' || (c == '\"' && AnsiQuotes)) && !inSize)
                    quoteChar = c;
                else if (c == '/' && lastChar == '*' && inComment)
                    inComment = false;
                else if (c == '*' && lastChar == '/')
                {
                    inComment = true;
                    current.Remove(current.Length - 1, 1);
                }
                else if (inComment || inLineComment)
                {
                    if (inLineComment && c == '\n')
                        inLineComment = false;
                }
                else if (c == '\\' && BackslashEscapes)
                {
                    escaped = true;
                    current.Append(c);
                }
                else if (c == '#')
                    inLineComment = true;
                else if (c == '-' && lastChar == '-')
                {
                    current.Remove(current.Length - 1, 1);
                    inLineComment = true;
                }
                else if ((c == ',' || c == ')' || c == '(') && current.Length == 0)
                {
                    inParamters = true;
                    return c.ToString();
                }
                else if (Char.IsWhiteSpace(c) || c == '(' || c == ')' || (c == ',' && !inSize))
                {
                    if (c == ',' || (c == ')' && !inSize))
                        index--;
                    if (c == ')' && inSize)
                    {
                        isSize = true;
                        inSize = false;
                    }
                    if (c == '(' && inParamters)
                        inSize = true;
                    if (current.Length > 0)
                        return current.ToString();
                }
                else
                    current.Append(c);
                lastChar = c;
            }

            if (current.Length > 0)
                return current.ToString();

            return null;
        }
    }
}