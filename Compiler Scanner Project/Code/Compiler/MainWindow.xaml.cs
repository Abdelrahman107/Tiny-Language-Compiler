using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Compiler.Scanner;

namespace Compiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<KeyValuePair<string, String>> arr = new List<KeyValuePair<string, String>>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Compile(object sender, RoutedEventArgs e)
        {
            Text.Text += " ";
            Scanner sc = new Scanner(Text.Text);
            var list = sc.getToken();
            bool toerror = false; 


            foreach (var element in list)
            {
                if (element.Value == TokenType.ThrowError_IdentifierMustBeginWithLetter)
                {
                    toerror = true;
                   

                }
            }

            if (toerror) { 
                arr.Add(new KeyValuePair<string, String>("Error", TokenType.ThrowError_IdentifierMustBeginWithLetter.ToString()));
                Table.ItemsSource = arr;
                Table.Columns[0].Header = "Error";
                Table.Columns[1].Header = "Error Type";
            }
            else
            {
                foreach (var element in list)
                {
                    if (element.Value == TokenType.T_ID)
                        arr.Add(new KeyValuePair<string, String>(element.Key, element.Value.ToString() + "_" + element.Key));
                    else if (element.Value == TokenType.RESERVED_WORD)
                        arr.Add(new KeyValuePair<string, String>(element.Key, element.Value.ToString() + "_" + element.Key.ToUpper()));
                    else
                        arr.Add(new KeyValuePair<string, String>(element.Key, element.Value.ToString()));
                }


                Table.ItemsSource = arr;
                Table.Columns[0].Header = "Lexeme";
                Table.Columns[1].Header = "Token Value";
            }
             
        }


        private void Clear(object sender, RoutedEventArgs e)
        {
            Text.Text = "";
            Table.ItemsSource = null;
            this.arr.Clear() ;

        }

        private void Table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
              
        }

        private void Table_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Table_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Table_SelectionChanged_3(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
