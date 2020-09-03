using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler
{
        class Scanner
        {
            private string CodeStr;
            private static string[] ReservedWords;

            public Scanner(string s)
            {
                CodeStr = s;
                IntializeReservedWords();
            }
            private void IntializeReservedWords()
            {
                ReservedWords = new string[] {"main", "while", "if", "then", "elseif", "else", "repeat", "read", "write", "int", "float", "string", "return", "endl", "end", "until" };

            }

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
                RESERVED_WORD, ThrowError_IdentifierMustBeginWithLetter,
                /* multicharacter tokens */
                T_ID, T_NUMBER, Undefined_Symbol_ERROR,ERROR,
                /* special symbols */
                T_ASSIGN, T_ISEQ, T_LT, T_GT, T_PLUS, T_MINUS, T_TIMES, T_STRING, T_LeftBrace, T_RightBrace, T_COMMA,
                T_OVER, T_LeftBracket, T_RightBracket, T_SemiColon, T_AND, T_OR, T_NOTEQ, NULL, T_TOKEN, T_FLOAT,
            };

           


            public static bool ReservedLOOKUP(string s)
            {
                for (int i = 0; i < ReservedWords.Length; i++) if (s == ReservedWords[i]) return true;
                return false;
            }



            public List<KeyValuePair<string, TokenType>> getToken()
            {
                StateType current_State = StateType.START;
                string current_Lex = "";
                TokenType current_Token = TokenType.NULL;
                int Lex_index = 0;
                var ListToPrint = new List<KeyValuePair<string, TokenType>>();
                bool save;

//            CodeStr = CodeStr.Replace(System.Environment.NewLine, "");
            while (Lex_index != CodeStr.Length)

                {
                    save = true;
                    switch (current_State)
                    {
                        case StateType.START:

                            {
//                            Console.WriteLine(CodeStr[Lex_index]+ " :" + CodeStr[Lex_index]);
                                if (CodeStr[Lex_index] == ' ' | CodeStr[Lex_index] == '\n' || CodeStr[Lex_index] == '\t' || CodeStr[Lex_index] == '\r')
                                { save = false; }
                                else if (CodeStr[Lex_index] == ':')
                                {
                                    current_State = StateType.INASSIGN;
                                }
                                else if (CodeStr[Lex_index] == '"')
                                {
                                    current_State = StateType.INSTRING;
                                }
                                else if (CodeStr[Lex_index] == '|')
                                {
                                    current_State = StateType.OR;
                                }

                                else if (CodeStr[Lex_index] == '&')
                                {
                                    current_State = StateType.AND;
                                }
                                else if (System.Char.IsLetter(CodeStr[Lex_index]))
                                { current_State = StateType.INID; }

                                else if (System.Char.IsNumber(CodeStr[Lex_index]))
                                {
                                    current_State = StateType.INNUM;
                                }
                                else if (CodeStr[Lex_index] == '/')
                                {
                                    current_State = StateType.SLASH;
                                    save = false;
                                }
                                else if (CodeStr[Lex_index] == '<')
                                { current_State = StateType.L_CHECK; }



                                else
                                {
                                    current_State = StateType.DONE;
                                    switch (CodeStr[Lex_index])
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
                                            current_Token = TokenType.T_LeftBracket;
                                            break;
                                        case ')':
                                            current_Token = TokenType.T_RightBracket;
                                            break;
                                        case ';':
                                            current_Token = TokenType.T_SemiColon;
                                            break;
                                        case '}':
                                            current_Token = TokenType.T_RightBrace;
                                            break;

                                        case '{':
                                            current_Token = TokenType.T_LeftBrace;
                                            break;
                                        case '>':
                                            current_Token = TokenType.T_GT;
                                            break;

                                        case ',':
                                            current_Token = TokenType.T_COMMA;
                                            break;

                                        default:
                                        current_Token = TokenType.Undefined_Symbol_ERROR;                                         break;
                                    }

                                }
                                break;
                            }
                        case StateType.INASSIGN:
                            {
                                current_State = StateType.DONE;
                                if (CodeStr[Lex_index] == '=')
                                    current_Token = TokenType.T_ASSIGN;
                                else
                                {
                                    save = false;
                                    current_Token = TokenType.ERROR;
                                    Lex_index--;
                                }
                                break;
                            }
                        case StateType.AND:
                            {
                                current_State = StateType.DONE;
                                if ((CodeStr[Lex_index] == '&'))

                                    current_Token = TokenType.T_AND;
                                else
                                {
                                    save = false;
                                    current_Token = TokenType.ERROR;
                                    Lex_index--;
                                }
                                break;

                            }

                        case StateType.OR:
                            {
                                current_State = StateType.DONE;
                                if ((CodeStr[Lex_index] == '|'))

                                    current_Token = TokenType.T_OR;
                                else
                                {
                                    save = false;
                                    current_Token = TokenType.ERROR;
                                    Lex_index--;
                                }
                                break;

                            }
                        case StateType.INSTRING:
                            {
                                if ((CodeStr[Lex_index] == '"'))
                                {
                                    current_State = StateType.DONE;
                                    current_Token = TokenType.T_STRING;
                                }


                                break;
                            }


                        case StateType.INNUM:
                            {
                                
                                 
                                if (!(System.Char.IsNumber(CodeStr[Lex_index])))

                                {

                                if (System.Char.IsLetter(CodeStr[Lex_index]))
                                {
                                    save = true;
                                    current_Token = TokenType.ThrowError_IdentifierMustBeginWithLetter;
                                    current_State = StateType.DONE;
                                    break;
                                }

                                else if (CodeStr[Lex_index] == '.')
                                    {

                                        current_State = StateType.INFLOAT;
                                    }

                                    else
                                    {
                                        save = false;
                                        current_State = StateType.DONE;
                                        current_Token = TokenType.T_NUMBER;
                                        Lex_index--;
                                    }
                                }


                                break;
                            }

                        case StateType.INFLOAT:
                            {

                                if (!(System.Char.IsNumber(CodeStr[Lex_index])))
                                {

                                    save = false;
                                    current_State = StateType.DONE;
                                    current_Token = TokenType.T_FLOAT;
                                    Lex_index--;
                                }


                                break;
                            }




                        case StateType.INID:
                            {
                                if (!((System.Char.IsLetter(CodeStr[Lex_index]) || (System.Char.IsNumber(CodeStr[Lex_index])))))
                                {
                                    save = false;
                                    current_State = StateType.DONE;
                                    current_Token = TokenType.T_ID;
                                    Lex_index--;

                                }

                                break;
                            }



                        case StateType.L_CHECK:
                            {
                                if ((CodeStr[Lex_index] == '>'))
                                {

                                    current_State = StateType.DONE;
                                    current_Token = TokenType.T_NOTEQ;

                                }


                                else
                                {

                                    save = false;
                                    current_State = StateType.DONE;
                                    current_Token = TokenType.T_LT;
                                    Lex_index--;
                                }
                                break;
                            }
                        case StateType.SLASH: 
                            {
                                if (!(CodeStr[Lex_index] == '*'))
                                {
                                    current_Lex = "/";
                                    save = false;
                                    current_State = StateType.DONE;
                                    current_Token = TokenType.T_OVER;
                                    Lex_index--;
                                }


                                else if (CodeStr[Lex_index] == '*')
                                {
                                    save = false;
                                    current_State = StateType.STAR1;

                                }
                                break;
                            }

                        case StateType.STAR1:
                            {
                                if (!(CodeStr[Lex_index] == '*'))
                                {
                                    save = false;


                                }
                                else if (CodeStr[Lex_index] == '*')
                                {
                                    save = false;
                                    current_State = StateType.STAR2;

                                }

                                break;
                            }
                        case StateType.STAR2:
                            {
                                if ((CodeStr[Lex_index] == '*'))
                                {
                                    save = false;



                                }
                                else if ((CodeStr[Lex_index] == '/'))
                                {
                                    save = false;
                                    current_State = StateType.DONE;


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

                    if (save) { current_Lex += CodeStr[Lex_index]; }

                    if (current_State == StateType.DONE)

                    {
                        if (current_Token == TokenType.T_ID)
                        {
                            if (ReservedLOOKUP(current_Lex))
                                current_Token = TokenType.RESERVED_WORD;
                        }


                        if (current_Token != TokenType.NULL) ListToPrint.Add(new KeyValuePair<string, TokenType>(current_Lex, current_Token));
                        current_Lex = "";
                        current_State = StateType.START;
                        current_Token = TokenType.NULL;


                    }

                    Lex_index++;
                }

                if (current_Token != TokenType.NULL) ListToPrint.Add(new KeyValuePair<string, TokenType>(current_Lex, current_Token));

                return ListToPrint;
            }

        }


}
