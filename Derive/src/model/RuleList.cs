using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derive.model {
    class RuleList {

        private List<Rule> rules;
        public List<Rule> Rules {
            get {
                return rules;
            }
        }

        private String defaultValue;
        public String DefaultValue {
            get {
                return defaultValue;
            }
            set {
                defaultValue = value;
            }
        }

        public RuleList(List<Rule> rules, String defaultValue) {
            this.rules = rules;
            this.defaultValue = defaultValue;
        }

        public override bool Equals(System.Object other) {
            if (other == null) {
                return false;
            }

            RuleList otherRL = other as RuleList;
            if (otherRL == null) {
                return false;
            }

            if (otherRL.Rules.Count != this.Rules.Count) {
                return false;
            }

            Rule thisRule, otherRule;
            for (int i = 0; i < rules.Count; i++) {
                thisRule = this.Rules[i];
                otherRule = otherRL.Rules[i];
                if(! thisRule.Condition.Equals(otherRule.Condition) ||
                    !thisRule.Value.Equals(otherRule.Value)) {
                        return false;
                }
            }

            if (!this.DefaultValue.Equals(otherRL.DefaultValue)) {
                return false;
            }

            return true;
        }

        public override int GetHashCode() {
            return rules.GetHashCode() ^ defaultValue.GetHashCode();
        }

    }
}
