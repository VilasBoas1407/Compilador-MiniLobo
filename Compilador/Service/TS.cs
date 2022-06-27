using AnalisadorLexico.Model;
using System;
using System.Collections.Generic;

namespace AnalisadorLexico.Service
{
    public class TS
    {
        public string Valor { get; set; }
        public TipoToken Tipo { get; set; }
        public int Linha { get; set; }
        public int Coluna { get; set; }
        public bool Valido { get; set; }

        public TS(string _valor, TipoToken _tipo, int _coluna, int _linha, bool _valido = true)
        {
            Valor = _valor;
            Tipo = _tipo;
            Linha = _linha;
            Coluna = _coluna;
            Valido = _valido;
        }
    }

    public static class TabelaSimbolo
    {
        private static List<TS> TabelaDeSimbolos = new List<TS>();

        public static Dictionary<string, TipoToken> DicionarioDeSimbolos = new Dictionary<string, TipoToken>()
        {
            {"program", TipoToken.RW_Program},
            {"print",TipoToken.RW_Print },
            {"begin",TipoToken.RW_Begin },
            {"end",TipoToken.RW_End },
            {"forward",TipoToken.RW_Foward},
            {"repeat",TipoToken.RW_Repeat},
            {"turn",TipoToken.RW_Turn },
            {"if", TipoToken.RW_IF },
            {"do",TipoToken.RW_DO },
            {"degrees", TipoToken.RW_Degrees },
            {"var",TipoToken.RW_VAR }
        };

        public static void AdicionaSimbolo(TS Lexama)
        {
            if (Lexama != null && Lexama.Valido)
                TabelaDeSimbolos.Add(Lexama);
        }

        public static TipoToken BuscaSimbolo(string valor)
        {
            
            foreach (var item in DicionarioDeSimbolos)
            {
                if (item.Key == valor)
                    return item.Value;
            }

            return TipoToken.Undefined;
        }

        public static void ImprimeListaDeTokens()
        {
            if (TabelaDeSimbolos.Count > 0)
            {
                foreach (TS item in TabelaDeSimbolos)
                {
                    Console.WriteLine($"<{item.Tipo},\"{item.Valor}\"> Linha: {item.Linha}, Coluna: {item.Coluna}");
                }
            }
        }

        public static void ImprimeTabelaDeSimbolos()
        {
            Console.WriteLine("================ Tabela de Símbolos ================ \n");
            
            foreach (var item in DicionarioDeSimbolos)
            {
                string line = $"{item.Key} : <{item.Value}, \"{item.Key}\">";
                Console.WriteLine(line);
            }
        }

        public static List<TS> RetornaListaDeTokens()
        {
            return TabelaDeSimbolos;
        }
    }

}

/*
    Trabalho desenvolvido por : 
        Lucas Vilas Boas Lage - RA: 119119592
        Leandro César Lopes Cardoso - RA: 119210676
        Fabiana Quelott Lopes Cançado - RA: 119214091
 */
