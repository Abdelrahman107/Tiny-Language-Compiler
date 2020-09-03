/*Scanner Class*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum StateType
{
    START, DONE, RESERVED,
    INNUM, INFLOAT,
    INID, INSTRING, INASSIGN, STAR2,
    SLASH, STAR1, COMMENT, L_CHECK,
    LT,
    AND, OR
};

public enum TokenType
{
    //RESERVED_WORD,
    IF, THEN, ELSE, ELSEIF, READ, WRITE, ENDL, FLOAT, INT, REPEAT, UNTIL, RETURN, END, STRING,
    /* multicharacter tokens */
    T_ID, T_NUMBER,
    /* special symbols */
    T_ASSIGN, T_ISEQ, T_LT, T_GT, T_PLUS, T_MINUS, T_TIMES, T_STRING, T_CLPAREN, T_CRPAREN, T_COMMA,
    T_OVER, T_LPAREN, T_RPAREN, T_SEMI, T_AND, T_OR, T_NOTEQ, ERROR, NULL, T_TOKEN, T_FLOAT,


};



namespace gui2
{

    class Scanner
    {
        private string CodeStr;
        private static Dictionary<string, TokenType> ReservedWords;
        private static int linepos = 0;
        private static string current_Lex = "";

        public static int getLinePos()
        {
            return linepos;
        }

        public int getStringsize()
        {
            return CodeStr.Length;
        }
        public static string getCurrent_Lex()
        {
            return current_Lex;
        }

        public Scanner(string s)
        {
            // = s.TrimEnd();
            CodeStr = s;
           CodeStr += " ";
            IntializeReservedWords();
        }
        private void IntializeReservedWords()
        {
            //ReservedWords = new string[] { "while", "if", "then", "elseif", "else", "repeat", "read", "write", "int", "float", "string", "return", "endl", "end", "until" };

            ReservedWords =
               new Dictionary<string, TokenType>{
                { "if" , TokenType.IF},
                { "then" , TokenType.THEN},
                { "else" , TokenType.ELSE},
                { "elseif" , TokenType.ELSEIF},
                { "end" , TokenType.END},
                { "repeat" , TokenType.REPEAT},
                { "until" , TokenType.UNTIL},
                { "return" , TokenType.RETURN},
                { "endl" , TokenType.ENDL},
                { "read" , TokenType.READ},
                { "write" , TokenType.WRITE},
                { "string" , TokenType.STRING},
                { "int" , TokenType.INT},
                { "float" , TokenType.FLOAT}
               };
        }







        public static TokenType ReservedLOOKUP(string s)
        {
            foreach (string word in ReservedWords.Keys)
            {
                if (s.Equals(word)) return ReservedWords[word];

            }
            return TokenType.T_ID;


        }


        public static void GetNextChar()
        { linepos++; }
        public static void UnGetChar()
        { linepos--; }

        public TokenType getToken()
        {
            StateType current_State = StateType.START;
            TokenType current_Token = TokenType.NULL;
            current_Lex = "";
            bool save;

            try
            {
                while (current_State != StateType.DONE)

                {//linepos != CodeStr.Length
                    save = true;
                    switch (current_State)
                    {
                        case StateType.START:

                            {
                                if (CodeStr[linepos] == ' ' | CodeStr[linepos] == '\n' || CodeStr[linepos] == '\t')
                                { save = false; }
                                else if (CodeStr[linepos] == ':')
                                {
                                    current_State = StateType.INASSIGN;
                                }
                                else if (CodeStr[linepos] == '"')
                                {
                                    current_State = StateType.INSTRING;
                                }
                                else if (CodeStr[linepos] == '|')
                                {
                                    current_State = StateType.OR;
                                }

                                else if (CodeStr[linepos] == '&')
                                {
                                    current_State = StateType.AND;
                                }
                                else if (System.Char.IsLetter(CodeStr[linepos]))
                                { current_State = StateType.INID; }

                                else if (System.Char.IsNumber(CodeStr[linepos]))
                                {
                                    current_State = StateType.INNUM;
                                }
                                else if (CodeStr[linepos] == '/')
                                {
                                    current_State = StateType.SLASH;
                                    save = false;
                                }
                                else if (CodeStr[linepos] == '<')
                                { current_State = StateType.L_CHECK; }



                                else
                                {
                                    current_State = StateType.DONE;
                                    switch (CodeStr[linepos])
                                    {


                                        case '=':
                                            current_Token = TokenType.T_ISEQ;
                                            break;

                                        case '+':
                                            current_Token = TokenType.T_PLUS; ;
                                            break;
                                        case '-':
                                            current_Token = TokenType.T_MINUS;
                                            break;

                                        case '*':
                                            current_Token = TokenType.T_TIMES;
                                            break;

                                        case '(':
                                            current_Token = TokenType.T_LPAREN;
                                            break;
                                        case ')':
                                            current_Token = TokenType.T_RPAREN;
                                            break;
                                        case ';':
                                            current_Token = TokenType.T_SEMI;
                                            break;
                                        case '}':
                                            current_Token = TokenType.T_CRPAREN;
                                            break;

                                        case '{':
                                            current_Token = TokenType.T_CLPAREN;
                                            break;
                                        case '>':
                                            current_Token = TokenType.T_GT;
                                            break;

                                        case ',':
                                            current_Token = TokenType.T_COMMA;
                                            break;

                                        default:
                                            current_Token = TokenType.ERROR;
                                            break;
                                    }

                                }
                                break;
                            }
                        case StateType.INASSIGN:
                            {
                                current_State = StateType.DONE;
                                if (CodeStr[linepos] == '=')
                                    current_Token = TokenType.T_ASSIGN;
                                else
                                {
                                    save = false;
                                    current_Token = TokenType.ERROR;
                                    UnGetChar();
                                }
                                break;
                            }
                        case StateType.AND:
                            {
                                current_State = StateType.DONE;
                                if ((CodeStr[linepos] == '&'))

                                    current_Token = TokenType.T_AND;
                                else
                                {
                                    save = false;
                                    current_Token = TokenType.ERROR;
                                    UnGetChar(); ;
                                }
                                break;

                            }
                        case StateType.OR:
                            {
                                current_State = StateType.DONE;
                                if ((CodeStr[linepos] == '|'))

                                    current_Token = TokenType.T_OR;
                                else
                                {
                                    save = false;
                                    current_Token = TokenType.ERROR;
                                    UnGetChar();
                                }
                                break;

                            }
                        case StateType.INSTRING:
                            {
                                if ((CodeStr[linepos] == '"'))
                                {
                                    current_State = StateType.DONE;
                                    current_Token = TokenType.T_STRING;
                                }


                                break;
                            }


                        case StateType.INNUM:
                            {

                                if (!(System.Char.IsNumber(CodeStr[linepos])))
                                {
                                    if (CodeStr[linepos] == '.')
                                    {

                                        current_State = StateType.INFLOAT;
                                    }
                                    else
                                    {
                                        save = false;
                                        current_State = StateType.DONE;
                                        current_Token = TokenType.T_NUMBER;
                                        UnGetChar();
                                    }
                                }


                                break;
                            }

                        case StateType.INFLOAT:
                            {

                                if (!(System.Char.IsNumber(CodeStr[linepos])))
                                {

                                    save = false;
                                    current_State = StateType.DONE;
                                    current_Token = TokenType.T_FLOAT;
                                    UnGetChar();
                                }


                                break;
                            }




                        case StateType.INID:
                            {
                                if (!((System.Char.IsLetter(CodeStr[linepos]) || (System.Char.IsNumber(CodeStr[linepos])))))
                                {
                                    save = false;
                                    current_State = StateType.DONE;
                                    current_Token = TokenType.T_ID;
                                    UnGetChar();

                                }

                                break;
                            }



                        case StateType.L_CHECK:
                            {
                                if ((CodeStr[linepos] == '>'))
                                {

                                    current_State = StateType.DONE;
                                    current_Token = TokenType.T_NOTEQ;

                                }


                                else
                                {

                                    save = false;
                                    current_State = StateType.DONE;
                                    current_Token = TokenType.T_LT;
                                    UnGetChar();
                                }
                                break;
                            }
                        case StateType.SLASH:
                            {
                                if (!(CodeStr[linepos] == '*'))
                                {
                                    current_Lex = "/";
                                    save = false;
                                    current_State = StateType.DONE;
                                    current_Token = TokenType.T_OVER;
                                    UnGetChar();
                                }


                                else if (CodeStr[linepos] == '*')
                                {
                                    save = false;
                                    current_State = StateType.STAR1;

                                }
                                break;
                            }

                        case StateType.STAR1:
                            {
                                if (!(CodeStr[linepos] == '*'))
                                {
                                    save = false;


                                }
                                else if (CodeStr[linepos] == '*')
                                {
                                    save = false;
                                    current_State = StateType.STAR2;

                                }

                                break;
                            }
                        case StateType.STAR2:
                            {
                                if ((CodeStr[linepos] == '*'))
                                {
                                    save = false;



                                }
                                else if ((CodeStr[linepos] == '/'))
                                {
                                    save = false;
                                    current_State = StateType.START;


                                }
                                else
                                {
                                    save = false;
                                    current_State = StateType.STAR1;

                                }

                                break;
                            }

                        default:
                            current_Token = TokenType.ERROR;
                            break;

                    }

                    if (save) { current_Lex += CodeStr[linepos]; }

                    if (current_State == StateType.DONE)

                    {
                        if (current_Token == TokenType.T_ID)
                        {

                            current_Token = ReservedLOOKUP(current_Lex);
                        }


                        //if (current_Token != TokenType.NULL) //TraceScan(current_Lex, current_Token);


                    }

                    GetNextChar();
                }
            }
            catch(Exception)
            {
               
            }
                return current_Token;




            
         }


        public void TraceScan(string s, TokenType t)
        {
            Console.WriteLine("   " + s + "          " + t.ToString());
            Console.WriteLine();
        }






    }
}