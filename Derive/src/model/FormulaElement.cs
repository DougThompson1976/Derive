using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derive.model {
    static class FormulaElement {

        public static class Function {
            public static readonly String IF = "IF";
        }

        public static class Symbol {
            public static readonly Char FUNCTION_START_ARGS = '(';
            public static readonly Char FUNCTION_END_ARGS = ')';
            public static readonly Char FUNCTION_SEPARATE_ARGS = ',';
            public static readonly Char FORMULA_ROOT = '=';
        }

        public static class Boolean {
            public static readonly String TRUE = "TRUE";
            public static readonly String FALSE = "FALSE";
        }

    }
}
