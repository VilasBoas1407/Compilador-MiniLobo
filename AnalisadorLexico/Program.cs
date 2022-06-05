using AnalisadorLexico.Model;
using AnalisadorLexico.Service;
using System;

namespace AnalisadorLexico
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("=========== COMPILADOR MINI LOBO ============== \n \n");
            Console.WriteLine("DESENVOLVIDO POR: LUCAS VILAS BOAS LAGE \n \n");
            Console.WriteLine("Iniciando leitura do arquivo \n \n");

            Lexer.IniciarLexer();
            
            TS token = Lexer.ProximoToken();
            TabelaSimbolo.AdicionaSimbolo(token);

            try
            {
                while (token != null && token.Tipo != TipoToken.Eof)
                {
                    token = Lexer.ProximoToken();
                    TabelaSimbolo.AdicionaSimbolo(token);
                }


                TabelaSimbolo.ImprimeListaDeTokens();

                Console.WriteLine();

                TabelaSimbolo.ImprimeTabelaDeSimbolos();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }
}
