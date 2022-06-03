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
            Index = 0;
            Estado = 1;
            Lexema = "";
            FechouTexto = true;
            FechouPontoVirgula = true;
        }

        public static int CurrentIndex { get; set; }
        public static int Index { get; set; }
        public static int CurrentLine { get; set; }
        public static string Source { get; set; }
        public static string Lexema { get; set; }
        public static int Estado { get; set; }
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
            if(message == null)
                message = $"Token inválido [{c}] na linha :{CurrentLine} e coluna: {CurrentIndex}";

            Console.WriteLine("[ERROR] :" + message);
            return null;
        }

        public static TS ProximoToken()
        {
            //Para fazer a contagem de linhas deve-se criar um outro index e se basear nele, enquanto CurrentIndex vai estar lendo o arquivo
            while (true)
            {
                string c = Source.Substring(CurrentIndex, 1);

                if (c == "\r" || c == "\n") //Nova linha 
                {
                    if (!FechouTexto)
                    {
                        return Error("",$"Quebra de linha sem fechar texto na linha : {CurrentLine} e coluna: {CurrentIndex}");
                    }
                    else if (!FechouPontoVirgula)
                    {
                        return Error("", $"Esperado ponto e vírgula na linha : {CurrentLine} e coluna : {CurrentIndex}");
                    }

                    CurrentIndex++;
                    Lexema += c;

                    if (Lexema == "\r")
                    {
                        TS token = ProximoToken();
                        if (token.Valor == "\n")
                        {
                            return new TS(token.Valor, TipoToken.LineBreak, CurrentIndex, CurrentLine, false);
                        }

                    }
                    else if (Lexema == QUEBRA_DE_LINHA)
                    {
                        CurrentLine++;
                        Lexema = "";
                    }
                    else
                    {
                        return Error(c);
                    }
                    return new TS(c, TipoToken.LineBreak, CurrentIndex, CurrentLine, false);
                }
  
                if (Estado == 1)
                {
                    if (c == "")
                    {
                        CurrentIndex++;
                        return new TS("", TipoToken.Eof, CurrentIndex, CurrentLine);
                    }
                    else if (c == " ")
                    {
                        Estado = 1;
                        CurrentIndex++;
                    }
                    else if (c == "+")
                    {
                        CurrentIndex++;
                        return new TS(c, TipoToken.OP_Adicao, CurrentIndex, CurrentLine);

                    }
                    else if (c == "-")
                    {
                        CurrentIndex++;
                        return new TS(c, TipoToken.OP_Subtracao, CurrentIndex, CurrentLine);
                    }
                    else if (c == "*")
                    {
                        CurrentIndex++;
                        return new TS(c, TipoToken.OP_Multiplicacao, CurrentIndex, CurrentLine);
                    }
                    else if (c == "/")
                    {
                        CurrentIndex++;
                        return new TS(c, TipoToken.OP_Divisao, CurrentIndex, CurrentLine);
                    }
                    else if (c == ":")
                    {
                        FechouPontoVirgula = false;
                        Estado = 2;
                        Lexema += c;
                        CurrentIndex++;
                    }
                    else if (IsDigit(c))
                    {
                        Lexema += c;
                        Estado = 3;
                    }
                    else if (IsLetter(c))
                    {
                        Lexema += c;
                        Estado = 4;
                        CurrentIndex++;
                    }
                    else if (IsQuote(c))
                    {
                        CurrentIndex++;
                        Estado = 5;
                        Lexema += c;
                        FechouTexto = false;
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
                        Lexema += c;
                        string valor = Lexema;
                        Lexema = "";
                        Estado = 1;
                        FechouPontoVirgula = true;
                        CurrentIndex++;
                        return new TS(valor, TipoToken.RW_VAR, CurrentIndex, CurrentLine);
                    }
                    else 
                    {
                        Lexema += c;
                        CurrentIndex++;
                    }
                }
                //Verifica se é uma palavra reservada
                else if (Estado == 4)
                {
                    if (c == " ")
                    {
                        TipoToken token = TabelaSimbolo.BuscaSimbolo(Lexema);
                        string valor = Lexema;
                        Lexema = "";
                        Estado = 1;
                        return new TS(valor, token, CurrentIndex, CurrentLine);
                    }
                    else
                    {
                        Lexema += c;
                        CurrentIndex++;
                    }
                }
                //Verifica se é uma string 
                else if (Estado == 5)
                {
                    //Fechando o texto
                    if (IsQuote(c))
                    {
                        CurrentIndex++;
                        FechouTexto = true;
                        Lexema += c;
                        string valor = Lexema;
                        Lexema = "";
                        Estado = 1;
                        return new TS(valor, TipoToken.String, CurrentIndex - 1, CurrentLine);
                    }
                    else if (c == QUEBRA_DE_LINHA)
                    {
                        return Error(c);
                    }
                    else
                    {
                        CurrentIndex++;
                        Lexema += c;
                    }
                }
            }
        }
    }
}
