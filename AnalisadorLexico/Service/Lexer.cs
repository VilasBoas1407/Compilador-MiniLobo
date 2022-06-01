using AnalisadorLexico.Model;
using System;

namespace AnalisadorLexico.Service
{
    public static class Lexer
    {
        public static void IniciarLexer()
        {
            Source = System.IO.File.ReadAllText(@"D:\Projetos\Compilador - MiniLobo\AnalisadorLexico\Code\program.txt");
            CurrentLine = 1;
            CurrentIndex = 1;
            Index = 0;

        }

        public static int CurrentIndex { get; set; }
        public static int Index { get; set; }
        public static int CurrentLine { get; set; }
        public static string Source { get; set; }

        public static bool IsDigit(string value)
        {
            int n;
            return int.TryParse(value, out n);
        }

        public static bool IsAlphaNum(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            for (int i = 0; i < value.Length; i++)
            {
                if (!(char.IsLetter(value[i])) && (!(char.IsNumber(value[i]))))
                    return false;
            }

            return true;
        }

        public static void Error(string msg)
        {
            Console.WriteLine("[ERROR] :" + msg);
 
        }

        public static void Avancar()
        {
            Index = CurrentIndex;
            Source.Remove(Index);
            CurrentIndex++;
        }

        public static TS ProximoToken()
        {
            int estado = 1;
            string lexema = "";

            while (true)
            {
                string c = Source.Substring(Index, CurrentIndex);

                if (estado == 1)
                {
                    if (c == "")
                    {
                        CurrentIndex++;
                        return new TS("", TipoToken.Eof, CurrentIndex, CurrentLine);
                    }
                    else if (c == " ")
                    {
                        estado = 1;
                        CurrentIndex++;
                    }
                    else if (c == "+")
                    {
                        Avancar();
                        return new TS(c, TipoToken.OP_Adicao, CurrentIndex, CurrentLine);
         
                    }
                    else if (c == "-")
                    {
                        Avancar();
                        return new TS(c, TipoToken.OP_Subtracao, CurrentIndex, CurrentLine);
                    }
                    else if (c == "*")
                    {
                        Avancar();
                        return new TS(c, TipoToken.OP_Multiplicacao, CurrentIndex, CurrentLine);
                    }
                    else if (c == "/")
                    {
                        Avancar();
                        return new TS(c, TipoToken.OP_Divisao, CurrentIndex, CurrentLine);
                    }
                    else if( c== ":")
                    {
                        estado=6;
                        CurrentIndex++; 
                    }
                    else if (IsDigit(c))
                    {
                        lexema += c;
                        estado = 7;
                    }
                    else if (IsAlphaNum(c))
                    {
                        lexema += c;
                        estado = 8;
                    }
                    else
                    {
                        string msg = $"Token inválido [{c}] na linha :{CurrentLine} e coluna: {CurrentIndex}";
                        Error(msg);
                        return null;
                    }
                }
            }
        }
    }
}
