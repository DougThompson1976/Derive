using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derive.model {
    class RuleListFormulaComposer {

        private RuleList ruleList;

        private FormatException composeException;
        public FormatException ComposeException {
            get {
                return composeException;
            }
        }

        private int failedRuleIndex;
        public int FailedRuleIndex {
            get {
                return failedRuleIndex;
            }
        }

        private String result;
        public String Result {
            get {
                if (!HasAttemptedCompose) {
                    attemptCompose();
                } if (HasComposedSuccessfully) {
                    return result;
                } else {
                    throw composeException;
                }
            }
        }

        public bool HasAttemptedCompose {
            get {
                return HasComposedSuccessfully || HasComposedErroneously;
            }
        }
        public bool HasComposedSuccessfully {
            get {
                return result != null;
            }
        }
        public bool HasComposedErroneously {
            get {
                return composeException != null;
            }
        }

        public RuleListFormulaComposer(RuleList ruleList) {
            this.ruleList = ruleList;
        }

        public bool isCorrectlyFormatted() {
            if (!HasAttemptedCompose) {
                attemptCompose();
            }
            return HasComposedSuccessfully;
        }

        private void attemptCompose() {
            if (ruleList.Rules.Count == 0) {
                this.result = "=" + ruleList.DefaultValue;
            }
            try {
                StringBuilder formulaBuilder = new StringBuilder("=");
                composeRecursively(formulaBuilder, ruleList.Rules, ruleList.DefaultValue);
                this.result = formulaBuilder.ToString();
            } catch (FormatException fe) {
                this.composeException = fe;
            } catch (Exception e) {
                this.composeException = new FormatException("Unknown exception while composing", e);
            }
        }

        private void composeRecursively(StringBuilder formulaBuilder, List<Rule> rules, String defaultValue) {
            if (rules.Count == 0) {
                formulaBuilder.Append(defaultValue);
            } else {
                Rule rule = rules[0];
                formulaBuilder.Append(FormulaElement.Function.IF);
                formulaBuilder.Append(FormulaElement.Symbol.FUNCTION_START_ARGS);
                formulaBuilder.Append(rule.Condition);
                formulaBuilder.Append(FormulaElement.Symbol.FUNCTION_SEPARATE_ARGS);
                formulaBuilder.Append(rule.Value);
                formulaBuilder.Append(FormulaElement.Symbol.FUNCTION_SEPARATE_ARGS);

                composeRecursively(formulaBuilder, rules.GetRange(1, rules.Count - 1), defaultValue);

                formulaBuilder.Append(FormulaElement.Symbol.FUNCTION_END_ARGS);
            }
        }

    }
}
