using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gui2
{
    class Parser
    {
        static private TokenType token; /* holds current token */
        static private Scanner sc;
        public static List<string> errorlist = new List<string>();
        public static bool iserror=false;
        public Parser(string s)
        {
            sc = new Scanner(s);
        }
        static void match(TokenType expected)
        {

            if (Scanner.getLinePos() != sc.getStringsize() - 1)
            {
                if (token == expected) token = sc.getToken();
                else
                {
                    iserror = true;
                    errorlist.Add("Error Unexpected Token Detected -> "+ Scanner.getCurrent_Lex()+ " " + token);

                }
            }
        }


        public TreeNode stmt_sequence()
        {
            TreeNode node = statement();
            TreeNode node2 = node;

            while ((token != TokenType.ELSE)
                && (token != TokenType.END) &&
                (token != TokenType.UNTIL) &&
                Scanner.getLinePos() != sc.getStringsize() - 1)
            {
                TreeNode node3;
                match(TokenType.T_SEMI);
                node3 = statement();

                if (node3 != null)
                {

                    if (node == null)
                        node = node2 = node3;
                    else
                    {
                        node2.setSibling(node3);
                        node2 = node3;
                    }
                }
            }
            return node;
        }

        public TreeNode statement()
        {
            TreeNode t = null;
            switch (token)
            {
                case TokenType.IF: t = if_stmt(); break;
                case TokenType.REPEAT: t = repeat_stmt(); break;
                case TokenType.T_ID: t = assign_stmt(); break;
                case TokenType.READ: t = read_stmt(); break;
                case TokenType.WRITE: t = write_stmt(); break;

                default:
                    iserror = true;
                    errorlist.Add("Error Unexpected Token Detected -> " + Scanner.getCurrent_Lex() + " " + token);
                    token = sc.getToken();
                    break;
            }
            return t;

        }
        public TreeNode if_stmt()
        {
            TreeNode t = new TreeNode(StmtKind.IfK);
            match(TokenType.IF);
            if (t != null)
                t.childrens[0] = exp();
            match(TokenType.THEN);
            if (t != null)
                t.childrens[1] = stmt_sequence();
            if (token == TokenType.ELSE)
            {
                match(TokenType.ELSE);
                if (t != null)
                    t.childrens[2] = stmt_sequence();
            }
            match(TokenType.END);
            return t;
        }
        public TreeNode repeat_stmt()
        {
            TreeNode t = new TreeNode(StmtKind.RepeatK);
            match(TokenType.REPEAT);
            if (t != null)
                t.childrens[0] = stmt_sequence();
            match(TokenType.UNTIL);
            if (t != null)
                t.childrens[1] = exp();
            return t;

        }
        public TreeNode assign_stmt()

        {
            TreeNode t = new TreeNode(StmtKind.AssignK);
            if ((t != null) && (token == TokenType.T_ID))
                t.setAttribute(new Attribute(Scanner.getCurrent_Lex()));
            match(TokenType.T_ID);
            match(TokenType.T_ASSIGN);
            if (t != null)
                t.childrens[0] = exp();
            return t;

        }
        public TreeNode read_stmt()
        {
            TreeNode t = new TreeNode(StmtKind.ReadK);
            match(TokenType.READ);
            if ((t != null) && (token == TokenType.T_ID))
                t.setAttribute(new Attribute(Scanner.getCurrent_Lex()));
            match(TokenType.T_ID);
            return t;
        }
        public TreeNode write_stmt()
        {
            TreeNode t = new TreeNode(StmtKind.WriteK);
            match(TokenType.WRITE);
            if (t != null) t.childrens[0] = exp();
            return t;
        }



        public TreeNode exp()
        {
            TreeNode t = simple_exp();
            if (token == TokenType.T_LT || token == TokenType.T_ISEQ)
            {
                TreeNode child = new TreeNode(ExpKind.OpK);
                if (child != null)
                {
                    child.childrens[0] = t;
                    child.setAttribute(new Attribute(token));
                    t = child;
                }
                match(token);
                if (t != null)
                    t.childrens[1] = simple_exp();

            }
            return t;

        }



        public TreeNode simple_exp()
        {
            TreeNode t = term();
            while (token == TokenType.T_PLUS || token == TokenType.T_MINUS)
            {
                TreeNode child = new TreeNode(ExpKind.OpK);
                if (child != null)
                {
                    child.childrens[0] = t;
                    child.setAttribute(new Attribute(token));
                    t = child;
                    match(token);
                    t.childrens[1] = term();
                }
            }
            return t;
        }



        public TreeNode term()
        {
            TreeNode t = factor();
            while (token == TokenType.T_TIMES || token == TokenType.T_OVER)
            {
                TreeNode child = new TreeNode(ExpKind.OpK);
                if (child != null)
                {
                    child.childrens[0] = t;
                    child.setAttribute(new Attribute(token));
                    t = child;
                    match(token);
                    child.childrens[1] = factor();
                }
            }
            return t;
        }



        public TreeNode factor()
        {
            TreeNode t = null;
            switch (token)
            {
                case TokenType.T_LPAREN:
                    match(TokenType.T_LPAREN);
                    t = exp();
                    match(TokenType.T_RPAREN);
                    break;

                case TokenType.T_ID:
                    t = new TreeNode(ExpKind.IdK);
                    if (t != null && (token == TokenType.T_ID))
                    {
                        t.setAttribute(new Attribute(Scanner.getCurrent_Lex()));
                    }
                    match(TokenType.T_ID);
                    break;

                case TokenType.T_NUMBER:
                    t = new TreeNode(ExpKind.ConstK);
                    if ((t != null) && (token == TokenType.T_NUMBER))
                    {
                        t.setAttribute(new Attribute(int.Parse(Scanner.getCurrent_Lex())));

                    }
                    match(TokenType.T_NUMBER);
                    break;

                default:
                    iserror = true;
                    errorlist.Add("Error Unexpected Token Detected" + Scanner.getCurrent_Lex() + " " + token);
                    token = sc.getToken();
                    break;


            }
            return t;
        }




        public TreeNode parse()
        {
            /* Function parse returns the newly 
             * constructed syntax tree*/


            TreeNode CompleteTree;

            if (Scanner.getLinePos() != sc.getStringsize() - 1) token = sc.getToken();


            CompleteTree = stmt_sequence();
            return CompleteTree;

        }

        




    }
}

