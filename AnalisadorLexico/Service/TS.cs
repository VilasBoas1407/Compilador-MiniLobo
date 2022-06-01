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

        public TS(string _valor, TipoToken _tipo, int _linha, int _coluna)
        {
            Valor = _valor;
            Tipo = _tipo;
            Linha = _linha;
            Coluna = _coluna;

        }
    }

    public static class TabelaSimbolo
    {
        public static List<TS> ListaDeToken = new List<TS>();

        public static void AdicionaSimbolo(TS Lexama)
        {
            if(Lexama != null)
                ListaDeToken.Add(Lexama);
        }

        public static void ImprimeTabela()
        {
            if(ListaDeToken.Count > 0)
            {
                foreach (TS item in ListaDeToken)
                {
                    Console.WriteLine($"<{item.Tipo},{item.Valor}> Linha: {item.Linha}, Coluna: {item.Coluna}");
                }
            }
   
        }
    }

}
