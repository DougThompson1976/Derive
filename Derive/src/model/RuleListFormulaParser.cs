using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derive.model {
    class RuleListFormulaParser {

        private readonly String formula;
        
        private FormatException parseException;
        public FormatException ParseException {
            get {
                return parseException;
            }
        }

        private RuleList result;
        public RuleList Result {
            /// <exception cref="FormatException">The input was not a correctly formatted rule formula.</exception>
            get {
                if (!HasAttemptedParse) {
                    attemptParse();
                }
                if (HasParsedSuccessfully) {
                    return result;
                }
                else {
                    throw parseException;
                }
            }
        }

        public bool HasAttemptedParse {
            get {
                return HasParsedSuccessfully || HasParsedErroneously;
            }
        }
        public bool HasParsedSuccessfully {
            get {
                return result != null;
            }
        }
        public bool HasParsedErroneously {
            get {
                return parseException != null;
            }
        }

        public RuleListFormulaParser(String formula) {
            this.formula = formula;
        }

        /**
         * A correctly formatted formula is one that can be fit onto the rule format, even when some conditions
         * or values are not correctly formatted.
         */
        public bool isCorrectlyFormatted() {
            if (!HasAttemptedParse) {
                attemptParse();
            }
            return HasParsedSuccessfully;
        }

        private void attemptParse() {
            if (formula == "") {
                // an empty formula results in an empty rule list with an empty default value
                this.result = new RuleList(new List<Rule>(), "");
            } else if (formula[0] != FormulaElement.Symbol.FORMULA_ROOT) {
                // a constant results in an empty rule list with the constant as default value
                this.result = new RuleList(new List<Rule>(), formula);
            } else {
                try {
                    RuleList ruleList = new RuleList(new List<Rule>(), "");
                    parseRecursively(formula.Substring(1), ruleList);
                    this.result = ruleList;
                } catch (FormatException fe) {
                    parseException = fe;
                } catch (Exception e) {
                    parseException = new FormatException("Unknown exception while parsing", e);
                }
            }
        }

        private void parseRecursively(String formula, RuleList ruleList) {
            if (! formula.StartsWith(FormulaElement.Function.IF)) {
                // arrived at the default value, resulting in a rule with condition True
                ruleList.DefaultValue = formula;
            } else {
                // get and check the arguments, parse recursively
                List<String> ifArguments = getArguments(formula);
                if (ifArguments.Count != 3) throw new FormatException("The IF call [" + formula + "] doesn't have three arguments.");
                // TODO check first two arguments validity
                ruleList.Rules.Add(new Rule(ifArguments[0], ifArguments[1]));
                parseRecursively(ifArguments[2], ruleList);
            }
        }

        private List<String> getArguments(String functionCall) {
            if (functionCall[functionCall.Length-1] != FormulaElement.Symbol.FUNCTION_END_ARGS) {
                throw new FormatException("The function call [" + functionCall + "] doesn't end with " + FormulaElement.Symbol.FUNCTION_END_ARGS);
            }
            // get the index of the first start args symbol
            int startArgs = functionCall.IndexOf(FormulaElement.Symbol.FUNCTION_START_ARGS) + 1;
            CharEnumerator enumerator = functionCall.Substring(startArgs, functionCall.Length - startArgs - 1).GetEnumerator();
            
            List<String> arguments = new List<String>();
            int level = 0;
            int cursorPos = startArgs;
            int startCurrentArg = cursorPos;
            while (enumerator.MoveNext()) {
                if (enumerator.Current == FormulaElement.Symbol.FUNCTION_START_ARGS) {
                    // one lever deeper
                    level++;
                }
                else if (enumerator.Current == FormulaElement.Symbol.FUNCTION_END_ARGS) {
                    // one level up
                    level--;
                }
                if (enumerator.Current == FormulaElement.Symbol.FUNCTION_SEPARATE_ARGS && level == 0) {
                    // a top level separator was reached, the current argument ends at the PREVIOUS char
                    arguments.Add(functionCall.Substring(startCurrentArg, cursorPos - startCurrentArg));
                    startCurrentArg = cursorPos + 1;
                } else if (cursorPos + 2 == functionCall.Length) {
                    // the last char was reached, the current argument ends at the CURRENT char
                    arguments.Add(functionCall.Substring(startCurrentArg, cursorPos - startCurrentArg + 1));
                }
                cursorPos++;
            }
            return arguments;
        }

    }
}
