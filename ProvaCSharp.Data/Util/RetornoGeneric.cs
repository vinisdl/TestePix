using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaCSharp.Data.Util
{
    public class RetornoListaChave<T> : Retorno
    {
        public RetornoListaChave(bool sucesso, int codRetorno, string mensagem) : base(sucesso, codRetorno, mensagem)
        {            
        }

        public T Result { get; set; }        
    }
}
