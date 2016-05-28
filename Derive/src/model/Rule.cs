using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derive.model {
    class Rule {

        private String condition;
        public String Condition {
            get { return condition; }
            set { condition = value; }
        }

        private String value;
        public String Value {
            get { return value; }
            set { this.value = value; }
        }

        public Rule(String condition, String value) {
            this.condition = condition;
            this.value = value;
        }

    }
}
