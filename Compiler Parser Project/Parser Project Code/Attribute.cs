using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gui2
{
    class Attribute
    {
        private string name = "";
        private int ConstVal = 0;
        private TokenType Op = TokenType.NULL;


        public Attribute(TokenType op) { this.Op = op; }
        public Attribute(int ConstVal) { this.ConstVal = ConstVal; }
        public Attribute(string name) { this.name = name; }

        public TokenType GetTokenType { get { return Op; } set { Op = value; } }

        public int getconstVal()
        {
            return this.ConstVal;
        }
        public string Name { get { return name; } set { name = value; } }



    }
}
