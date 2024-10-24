using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaCSharp.Data
{
    public class ChaveFavoritaDTO
    {
        public string NomeTitular { get; set; }
        public string Chave { get; set; }
        public decimal ValorTotal { get; set; }
        public int Quantidade { get; set; }
        public int TipoChave { get; set; }
    }
}
