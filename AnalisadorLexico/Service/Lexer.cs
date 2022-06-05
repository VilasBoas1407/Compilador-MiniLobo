﻿using AnalisadorLexico.Model;
using System;

namespace AnalisadorLexico.Service
{
    public static class Lexer
    {
        public const string QUEBRA_DE_LINHA = "\r\n";

        public static void IniciarLexer()
        {
            Source = System.IO.File.ReadAllText(@"D:\Projetos\Compilador - MiniLobo\AnalisadorLexico\Code\program.txt");
            CurrentLine = 1;
            CurrentIndex = 0;
            IndexOfLine = 0;
            Estado = 1;
            Lexema = "";
            FechouTexto = true;
            FechouPontoVirgula = true;
        }

        public static int CurrentIndex { get; set; }
        public static int IndexOfLine { get; set; }
        public static int CurrentLine { get; set; }
        public static string Source { get; set; }
        public static string Lexema { get; set; }
        public static int Estado { get; set; }
        public static int EstadoAnterior { get; set; }
        public static bool FechouTexto { get; set; }
        public static bool FechouPontoVirgula { get; set; }

        public static bool IsDigit(string value)
        {
            int n;
            return int.TryParse(value, out n);
        }

        public static bool IsLetter(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            for (int i = 0; i < value.Length; i++)
            {
                if (!(char.IsLetter(value[i])))
                    return false;
            }

            return true;
        }

        public static bool IsQuote(string value)
        {
            //Verifica se o caracter é uma abertura ou fechamento de aspas
            return value.ToCharArray()[0] == 8220 || value.ToCharArray()[0] == 8221;
        }

        public static TS Error(string c, string message = null)
        {
            if (message == null)
                message = $"Token inválido [{c}] na linha :{CurrentLine} e coluna: {IndexOfLine}";

            string msgError = "============================== ERROR =============================";
            msgError += "\n" + message + "\n";
            msgError += "=================================================================";

            throw new Exception(msgError);
        }

        public static void Avancar()
        {
            IndexOfLine++;
            CurrentIndex++;
        }
        public static TS ProximoToken()
        {
            while (true)
            {
                //Termina a compilação
                if (CurrentIndex == Source.Length)
                    return null;

                string c = Source.Substring(CurrentIndex, 1);
  
                if (Estado == 1)
                {
                    if (c == "\r" || c == "\n") //Nova linha 
                    {
                        if (!FechouTexto)
                        {
                            return Error("", $"Quebra de linha sem fechar texto na linha : {CurrentLine} e coluna: {IndexOfLine}");
                        }
                        else if (!FechouPontoVirgula)
                        {
                            return Error("", $"Esperado ponto e vírgula na linha : {CurrentLine} e coluna : {IndexOfLine}");
                        }

                        Avancar();
                        Lexema += c;

                        if (Lexema == "\r")
                        {
                            TS token = ProximoToken();
                            if (token.Valor == "\n")
                            {
                                return new TS(token.Valor, TipoToken.LineBreak, IndexOfLine, CurrentLine, false);
                            }

                        }
                        else if (Lexema == QUEBRA_DE_LINHA)
                        {
                            CurrentLine++;
                            IndexOfLine = 0;
                            Lexema = "";
                        }
                        else
                        {
                            return Error(c);
                        }
                        return new TS(c, TipoToken.LineBreak, IndexOfLine, CurrentLine, false);
                    }

                    if (c == "")
                    {
                        Avancar();
                        return new TS("", TipoToken.Eof, IndexOfLine, CurrentLine);
                    }
                    else if (c == " ")
                    {
                        Estado = 1;
                        Avancar();
                    }
                    else if (c == "+")
                    {
                        Avancar();
                        return new TS(c, TipoToken.OP_Adicao, IndexOfLine, CurrentLine);

                    }
                    else if (c == "-")
                    {
                        Avancar();
                        return new TS(c, TipoToken.OP_Subtracao, IndexOfLine, CurrentLine);
                    }
                    else if (c == "*")
                    {
                        Avancar();
                        return new TS(c, TipoToken.OP_Multiplicacao, IndexOfLine, CurrentLine);
                    }
                    else if (c == "/")
                    {
                        Avancar();
                        return new TS(c, TipoToken.OP_Divisao, IndexOfLine, CurrentLine);
                    }
                    else if (c == ":")
                    {
                        FechouPontoVirgula = false;
                        Estado = 2;
                        Avancar();
                    }
                    else if (IsDigit(c))
                    {
                        Estado = 3;
                    }
                    else if (IsLetter(c))
                    {
                        Lexema += c;
                        Estado = 4;
                        Avancar();
                    }
                    else if (IsQuote(c))
                    {
                        Avancar();
                        Estado = 5;
                        FechouTexto = false;
                    }
                    else if (EstadoAnterior == 2 && !FechouPontoVirgula)
                    {
                        Avancar();
                        EstadoAnterior = 1;
                        FechouPontoVirgula = true;
                    }
                    else if(c ==";" && FechouTexto)
                    {
                        Avancar();
                    }
                    else
                    {
                        return Error(c);
                    }
                }
                //Verifica se é um ID
                else if (Estado == 2)
                {
                    if(c == ";")
                    {
                        string valor = Lexema;
                        Lexema = "";
                        Estado = 1;
                        FechouPontoVirgula = true;
                        Avancar();
                        return new TS(valor, TipoToken.RW_VAR, IndexOfLine, CurrentLine);
                    }
                    else if(c == " ")
                    {
                        string valor = Lexema;
                        Lexema = "";
                        EstadoAnterior = 2;
                        Estado = 1;
                        Avancar();
                        return new TS(valor, TipoToken.RW_VAR, IndexOfLine, CurrentLine);
                    }
                    else if(c == "\r")
                    {
                        return Error("", $"Esperado ; na linha : {CurrentLine} e coluna: {IndexOfLine}");
                    }
                    else 
                    {
                        Lexema += c;
                        Avancar();
                    }
                }
                //Verifica se é um número
                else if(Estado == 3)
                {
                    if (IsDigit(c))
                    {
                        //Verifica o próximo valor
                        string nextValue = Source.Substring(CurrentIndex + 1, 1);
                        if (IsDigit(nextValue))
                        {
                            Lexema += c;
                            Avancar();
                        }
                        else
                        {
                            Lexema += c;
                            Avancar();
                            Estado = 1;
                            string valor = Lexema;
                            Lexema = "";
                            return new TS(valor, TipoToken.Number, IndexOfLine, CurrentLine);
                        }
                    }
                }
                //Verifica se é uma palavra reservada
                else if (Estado == 4)
                {
                    if (c == " " || c == "\r" || c == QUEBRA_DE_LINHA)
                    {
                        TipoToken token = TabelaSimbolo.BuscaSimbolo(Lexema);

                        string valor = Lexema;
                        Lexema = "";
                        Estado = 1;
                        
                        if (token == TipoToken.Undefined)
                            return Error("", "Token não reconhecido!");
                        else
                        {
                            return new TS(valor, token, IndexOfLine, CurrentLine);
                        }
                    }
                    else
                    {
                        if(c != ";")
                            Lexema += c;

                        Avancar();

                        if (CurrentIndex == Source.Length && TabelaSimbolo.BuscaSimbolo(Lexema) == TipoToken.RW_End)
                        {
                            return new TS(Lexema, TipoToken.RW_End, IndexOfLine, CurrentLine);
                        }
                    }
                }
                //Verifica se é uma string 
                else if (Estado == 5)
                {
                    //Fechando o texto
                    if (IsQuote(c))
                    {
                        Avancar();
                        FechouTexto = true;
                        string valor = Lexema;
                        Lexema = "";
                        Estado = 1;
                        return new TS(valor, TipoToken.String, IndexOfLine - 1, CurrentLine);
                    }
                    else if (c == QUEBRA_DE_LINHA || c == "\r")
                    {
                        return Error("", $"Quebra de linha dentro de texto na linha : {CurrentLine} e coluna: {IndexOfLine}");
                    }
                    else
                    {
                        Avancar();
                        Lexema += c;
                    }
                }
            }
        }
    }
}
