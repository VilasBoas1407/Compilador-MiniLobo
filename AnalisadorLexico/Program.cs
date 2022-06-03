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

            while (token != null && token.Tipo != TipoToken.RW_End && token.Tipo != TipoToken.Eof) 
            {
                token = Lexer.ProximoToken();
                TabelaSimbolo.AdicionaSimbolo(token);
            }


            TabelaSimbolo.ImprimeTabela();
        }

    }
}
