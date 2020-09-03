using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gui2
{
    public partial class Form1 : Form
    {
        TreeNode Syntaxtree;
        Parser p;
        public Form1()
        {
            InitializeComponent();
             
        }

        private void button1_Click(object sender, EventArgs e)
        {
           String s;
           s = richTextBox1.Text;
           p = new Parser(s);
           Syntaxtree = p.parse();

             if (Parser.iserror==true)
            {
                //for (int i = 0; i < Parser.errorlist.Count; i++)
                    treeView1.Nodes.Add("Error Unexpected Token");


            }

            else  addView(Syntaxtree, treeView1.Nodes);

             

 
        }

      
        private void addView(TreeNode tree,TreeNodeCollection target)
        {
            while (tree != null)
            {
                target.Add(TreeNode.PrintForTreeView(tree));
                int index = target.Count - 1;

                for(int i = 0; i < tree.childrens.Length;i++)
                {

                    if(tree.childrens[i] !=null)
                    {
                        addView(tree.childrens[i],target[index].Nodes);
                    }

                    else
                    {
                        continue;
                    }
                    
                }
                tree = tree.getSibling();


               
            }
        }

        


      
        
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

       
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
