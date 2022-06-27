using AnalisadorLexico.Model;
using AnalisadorLexico.Service;
using Compilador.Service;
using System;
using System.Collections.Generic;

namespace AnalisadorLexico
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("================================= COMPILADOR MINI LOBO ===================================== \n \n");
            Console.WriteLine("                              Iniciando leitura do arquivo \n \n");

            try
            {

                Lexer.IniciarLexer();

                Console.WriteLine("\n ============================ Iniciando analisador léxico ==================================\n");

                TS token = Lexer.ProximoToken();

                TabelaSimbolo.AdicionaSimbolo(token);

                while (token != null && token.Tipo != TipoToken.Eof)
                {
                    token = Lexer.ProximoToken();
                    TabelaSimbolo.AdicionaSimbolo(token);
                }


                TabelaSimbolo.ImprimeListaDeTokens();

                List<TS> TabelaDeSimbolos = TabelaSimbolo.RetornaListaDeTokens();

                Console.WriteLine();

                TabelaSimbolo.ImprimeTabelaDeSimbolos();

                Console.WriteLine("\n \n ================ Iniciando analisador sintático  ========================= \n");

                Parser.Validar(TabelaSimbolo.RetornaListaDeTokens());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }
}
/*
    Trabalho desenvolvido por : 
        Lucas Vilas Boas Lage - RA: 119119592
        Leandro César Lopes Cardoso - RA: 119210676
        Fabiana Quelott Lopes Cançado - RA: 119214091
 */
