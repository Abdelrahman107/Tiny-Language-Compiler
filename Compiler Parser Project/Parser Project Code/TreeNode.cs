using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

enum NodeKind { NULL, StmtK, ExpK }
enum StmtKind { NULL, IfK, RepeatK, AssignK, ReadK, WriteK, UntilK }
enum ExpKind { NULL, OpK, ConstK, IdK }

namespace gui2
{

    class TreeNode
    {
        public TreeNode[] childrens; // array of 3 max
        private TreeNode sibling = null;
        private NodeKind nodekind;
        private StmtKind stmtkind;
        private ExpKind expkind;
        private Attribute attr; // IT'S A CLASS FOR SIMPLICITY
        private static int indentNo = 0;
        









        public TreeNode(StmtKind stmtkind) // creates a TreeNode of Stmd kind
        {
            childrens = new TreeNode[3];
            for (int i = 0; i < 3; i++) childrens[i] = null;
            nodekind = NodeKind.StmtK;
            this.stmtkind = stmtkind;




        }


        public TreeNode(ExpKind exp) // creates a TreeNode of Attribute
        {
            childrens = new TreeNode[3];
            for (int i = 0; i < 3; i++) childrens[i] = null;

            nodekind = NodeKind.ExpK;
            expkind = exp;


        }

        public NodeKind getNodeKind()
        {
            return this.nodekind;
        }

        public StmtKind getStmtKind()
        {
            return this.stmtkind;
        }

        public ExpKind getExpKind()
        {
            return this.expkind;
        }

        public Attribute getattr()
        {
            return this.attr;
        }


        //public void addchild(TreeNode x)
        //{
        //    childrens.Add(x);
        //}

        public void setSibling(TreeNode sibling)
        {
            this.sibling = sibling;
        }

        public TreeNode[] getChildrens()
        {
            return childrens;
        }

        public TreeNode getSibling()
        {
            return sibling;
        }

        static void printSpaces(int indentno)
        {
            int i;
            for (i = 0; i < indentno; i++)
                Console.Write(" ");
        }

        public void PrintTree(TreeNode tree)
        {

            indentNo += 2;

            while (tree != null)
            {
                printSpaces(indentNo);
                if (tree.nodekind == NodeKind.StmtK)
                {
                    switch (tree.stmtkind)
                    {
                        case StmtKind.IfK:
                            Console.WriteLine("If");
                            break;
                        case StmtKind.RepeatK:
                            Console.WriteLine("Repeat");
                            break;
                        case StmtKind.AssignK:
                            Console.WriteLine("Assign(" + tree.attr.Name + ")");
                            break;
                        case StmtKind.ReadK:
                            Console.WriteLine("Read(" + tree.attr.Name + ")");

                            break;
                        case StmtKind.WriteK:
                            Console.WriteLine("Write");
                            break;
                        case StmtKind.UntilK:
                            Console.WriteLine("Until");
                            break;
                        //case StmtKind.NULL:
                        // break;
                        default:
                            Console.WriteLine("Unknown ExpNode kind\n");
                            break;
                    }


                }
                else if (tree.nodekind == NodeKind.ExpK)
                {
                    switch (tree.expkind)
                    {
                        case ExpKind.OpK:
                            Console.Write("Op(");
                            PrintToken(tree.attr.GetTokenType);
                            Console.WriteLine(")");

                            break;
                        case ExpKind.ConstK:
                            Console.WriteLine("Const(" + tree.attr.getconstVal() + ")");
                            break;
                        case ExpKind.IdK:

                            Console.WriteLine("Id(" + tree.attr.Name + ")");
                            break;
                        default:
                            Console.WriteLine("Unknown ExpNode kind\n");
                            break;
                    }
                }
                //PrintTree(tree.childrens[0]); // for only testing write fact; 
                //tree = tree.sibling;
                int size = tree.childrens.Length;
                for (int i = 0; i <size  ; i++)
                {
                     
                    PrintTree(tree.childrens[i]);



                }
                tree = tree.sibling;
            }

            indentNo -= 2;

        }

        public void setAttribute(Attribute att)
        {
            this.attr = att;
        }

        public string PrintToken(TokenType token)
        {
            switch (token)
            {
                case TokenType.T_MINUS:
                    {
                        return ("-");
                          
                    }
                case TokenType.T_PLUS:
                    {
                        return ("+");
                       
                    }
                case TokenType.T_OVER:
                    {
                        return ("/");
                        
                    }
                case TokenType.T_TIMES:
                    {
                        return ("*");
                        
                    }
                case TokenType.T_LT:
                    {
                        return ("<");
                        
                    }
                case TokenType.T_GT:
                    {
                        return (">");
                        
                    }
                case TokenType.T_ISEQ:
                    {
                        return ("=");
                        
                    }

                default:
                    return "";
            }
        }

        public static string PrintForTreeView(TreeNode tree)
        {
            string str = "";
            if (tree.nodekind == NodeKind.StmtK)
            {
                switch (tree.getStmtKind())
                {
                    case (StmtKind.IfK):
                        { 
                            str = "If";
                            break;
                        }

                    case (StmtKind.RepeatK):
                        {
                           
                            str = "Repeat";
                            break;
                        }

                    case (StmtKind.AssignK):
                        {
 
                            str = "Assign ( " + tree.attr.Name + " )";
                            break;
                        }

                    case (StmtKind.ReadK):
                        {
                                                     
                            str = "Read ( " + tree.attr.Name + " )";
                            break;
                        }

                    case (StmtKind.WriteK):
                        {
                           
                            str = "Write";
                            break;
                        }
                }
            }

            else if (tree.nodekind == NodeKind.ExpK)
            {
                switch (tree.getExpKind())
                {
                    case (ExpKind.IdK):
                        {
                                            
                            str = "Id ( " + (tree.attr.Name) + " )";
                            break;
                        }

                    case (ExpKind.OpK):
                        {
                             
                            str = "Op ( " + tree.PrintToken(tree.attr.GetTokenType) + " )";
                            break;
                        }

                    case (ExpKind.ConstK):
                        {
                            
                            str = "Const ( " + tree.attr.getconstVal() + " )";
                            break;
                        }
                }
            }
            return str;
        }




    }
}
