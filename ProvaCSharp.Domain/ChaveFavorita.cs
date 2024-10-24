using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaCSharp.Domain
{
    public class ChaveFavorita
    {
        public ChaveFavorita()
        {
            ValorTotal = 0;
            Quantidade = 0;
        }

        public string NomeTitular { get; set; }
        public string Chave { get; set; }
        public decimal ValorTotal { get; set; }
        public int Quantidade { get; set; }
        public TiposChave TipoChave { get; set; }
    }
}
