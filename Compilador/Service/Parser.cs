using AnalisadorLexico.Model;
using AnalisadorLexico.Service;
using System;
using System.Collections.Generic;

namespace Compilador.Service
{
    public static class Parser
    {
        public static List<TS> Tokens = new List<TS>();
        public static TipoToken TokenAtual;
        public static int Contador = 0;

        private static void Avancar()
        {
            Contador++;
            if (Contador == Tokens.Count)
                TokenAtual = TipoToken.Eof;
            else
                TokenAtual = Tokens[Contador].Tipo;
                    
        }

        private static void Error(TipoToken tokenEsperado)
        {
            throw new Exception($"\n [ERROR SINTÁTICO] \n Token esperado : {tokenEsperado} , encontrado:{TokenAtual}");
        }

        private static void Error(string msg)
        {
            throw new Exception("[ERRO SINTÁTICO]" + msg);
        }

        public static void Validar(List<TS> TabelaDeTokens)
        {
            Tokens = TabelaDeTokens;
            TokenAtual = TabelaDeTokens[0].Tipo;
            Program();
            Console.WriteLine("                             Código Validado!");
            Console.WriteLine("\n ================ Fim da Execução do Analisador Sintático =================");
        }

        private static void Program()
        {
            if (TokenAtual == TipoToken.RW_Program)
            {
                Avancar();
                Literal();
                Decl();
                Block();

            }
            else
            {
                Error(TipoToken.RW_Program);
            }
        }

        private static void Literal()
        {
            if(TokenAtual == TipoToken.String)
            {
                Avancar();
            }
            else
            {
                Error(TipoToken.String);
            }
        }

        private static void Decl()
        {
            if(TokenAtual == TipoToken.RW_VAR)
            {
                Avancar();
            }   
            else
            {
                Error(TipoToken.RW_VAR);
            }
        }

        private static void Block()
        {
            if(TokenAtual == TipoToken.RW_Begin)
            {
                Avancar();
                StatementList();
                if(TokenAtual == TipoToken.RW_End)
                {
                    Avancar();
                    if (TokenAtual == TipoToken.Eof)
                        return;
                }
                else
                {
                    Error(TipoToken.RW_End);
                }
            }
            else
            {
                Error(TipoToken.RW_Begin);
            }
        }

        private static void StatementList()
        {
            Statement();
        }

        private static void Statement()
        {
            if (TokenAtual == TipoToken.RW_Turn)
            {
                Turn();
            }
            else if (TokenAtual == TipoToken.RW_Foward)
            {
                Forward();
            }
            else if (TokenAtual == TipoToken.RW_Repeat) {
                Repeat();
            }
            else if (TokenAtual == TipoToken.RW_Print)
            {
                Print();
            }
            else if(TokenAtual == TipoToken.RW_IF)
            {
                IF();
            }
            else if( TokenAtual == TipoToken.RW_End)
            {
                return;
            }
            else
            {
                AssignmentStatement();
            }
            StatementList_();
        }

        private static void StatementList_()
        {
            StatementList();
        }

        private static void Turn()
        {
            Avancar();
            Term();
            Degrees();
        }

        private static void Degrees()
        {
            if(TokenAtual == TipoToken.RW_Degrees)
            {
                Avancar();
            }
            else
            {
                Error(TipoToken.RW_Degrees);
            }
        }

        private static void Term()
        {
            if (TokenAtual == TipoToken.Number || TokenAtual == TipoToken.RW_VAR)
            {
                Avancar();
            }
            else
                Error($"Token inválido, era esperado um token do tipo {TipoToken.Number} ou {TipoToken.RW_VAR}");
        }
        private static void Forward()
        {
            Avancar();
            Term();
        }

        private static void Repeat()
        {
            Avancar();
            Term();
            Do();
            Block();
        }

        private static void Do()
        {
            if(TokenAtual == TipoToken.RW_DO)
            {
                Avancar();
            }
            else
            {
                Error(TipoToken.RW_DO);
            }
        }

        private static void Print()
        {
            Avancar();
            Literal();
        }

        private static void IF()
        {
            Avancar();
            Expr();
            Do();
            Block();
        }

        private static void AssignmentStatement()
        {
            if(TokenAtual == TipoToken.RW_VAR)
            {
                Avancar();
                Expr();
            }
        }

        private static void Expr()
        {
            Expr1();
            Expr_();
        }

        private static void Expr_()
        {
            if(TokenAtual == TipoToken.OP_Adicao || TokenAtual == TipoToken.OP_Subtracao)
            {
                Avancar();
                Expr1();
                Expr_();
            }

        }

        private static void Expr1()
        {
            Expr2();
            Expr1_();
        }

        public static void Expr2()
        {
            Term();
        }

        private static void Expr1_()
        {
            if (TokenAtual == TipoToken.OP_Multiplicacao || TokenAtual == TipoToken.OP_Divisao)
            {
                Avancar();
                Expr2();
                Expr1_();
            }
        }
        
    }
}
