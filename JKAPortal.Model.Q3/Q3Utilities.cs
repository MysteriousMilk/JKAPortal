using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JKAPortal.Model.Q3
{
    public static class Q3Utilities
    {
        public static string GetUnformattedName(string formattedName)
        {
            string name = string.Empty;

            bool skipNext = false;
            foreach (char c in formattedName)
            {
                if (skipNext)
                {
                    skipNext = false;
                    continue;
                }

                if (c == '^')
                {
                    skipNext = true;
                    continue;
                }

                name += c;
            }

            return name;
        }

        public static string ResolveName(string name)
        {
            string resolvedName = string.Empty;

            foreach (char c in name)
            {
                char newChar = c;
                int code = (int)c;

                if (code == 65533)
                    newChar = '.';

                resolvedName += newChar;
            }

            return resolvedName;
        }
    }
}
