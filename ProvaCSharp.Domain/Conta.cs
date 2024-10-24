using ProvaCSharp.Data;
using ProvaCSharp.Data.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaCSharp.Domain
{
    public class Conta
    {
        public decimal Saldo { get; set; }
        public List<ChaveFavorita> ListaChaves { get; set; }

        public Conta()
        {
            Saldo = 0;
            ListaChaves = new List<ChaveFavorita>();
        }

        #region Creditar Conta
        public Retorno CreditarConta(string strValor)
        {
            decimal valor = 0;
            Retorno retValidacao = TryGetValor(strValor, out valor);
            if (!retValidacao.Sucesso)
            {
                return retValidacao;
            }
            this.Saldo = Saldo + valor;
            return new Retorno(true, 0, "Sucesso.");
        }
        #endregion

        #region Adicionar Chave
        public Retorno AdicionarChaveFavorita(String strNome, String strTipo, String strChave)
        {
            ChaveFavorita chave = new ChaveFavorita();
            try
            {
                chave.TipoChave = (TiposChave)Convert.ToInt16(strTipo);
            }
            catch (Exception e)
            {
                String msg = String.Format("30 - Falha ao converter Tipo Chave: {0}", e.Message);
                return new Retorno(false, 30, msg);
            }

            Retorno ret = validarChave(chave.TipoChave, strChave);
            if (!ret.Sucesso)
            {
                return ret;
            }

            ret = verificaChaveExistente(strChave);
            if (!ret.Sucesso)
            {
                return ret;
            }

            chave.Chave = strChave;
            chave.NomeTitular = strNome;

            ListaChaves.Add(chave);
            return new Retorno(true, 0, "Sucesso.");
        }

        private Retorno verificaChaveExistente(String strChave)
        {
            foreach (ChaveFavorita ch in ListaChaves)
            {
                if (ch.Chave.Equals(strChave))
                {
                    return new Retorno(false, 65, "65 - Chave duplicada.");
                }
            }
            return new Retorno(true, 0, "Sucesso.");
        }

        private Retorno validarChave(TiposChave tipo, String strChave)
        {
            switch (tipo)
            {
                case TiposChave.Cpf:
                    return validarCpf(strChave);
                case TiposChave.Telefone:
                    return validarTelefone(strChave);
                default:
                    return new Retorno(false, 30, "30 - Tipo Chave inválido!");
            }
        }

        private Retorno validarCpf(string chave)
        {
            String regex = "^\\d{11}$";
            if (System.Text.RegularExpressions.Regex.IsMatch(chave, regex))
            {
                return new Retorno(true, 0, "Sucesso.");
            }
            else
            {
                return new Retorno(false, 40, "40 - CPF inválido.");
            }
        }

        private Retorno validarTelefone(String chave)
        {
            String regex = "^\\+[1-9][0-9]\\d{11}$";
            if (System.Text.RegularExpressions.Regex.IsMatch(chave, regex))
            {
                return new Retorno(true, 0, "Sucesso.");
            }
            else
            {
                return new Retorno(false, 50, "50 - Telefone inválido! Formato esperado: +5551999999999."); ;
            }
        }
        #endregion

        #region Enviar PIX
        public Retorno EnviarPIX(String strChave, String strValor)
        {
            Decimal valor = 0;
            Retorno retValidaValor = TryGetValor(strValor, out valor);
            if (!retValidaValor.Sucesso)
            {
                return retValidaValor;
            }

            ChaveFavorita chave = getChave(strChave);
            if (chave == null)
            {
                return new Retorno(false, 60, "60 - Chave informada não existe.");
            }

            Retorno retSaldo = ValidarSaldo(valor);
            if (!retSaldo.Sucesso)
            {
                return retSaldo;
            }
            chave.ValorTotal = chave.ValorTotal + valor;
            chave.Quantidade = chave.Quantidade + 1;
            Saldo = Saldo - valor;
            return new Retorno(true, 0, "Sucesso.");
        }

        private ChaveFavorita getChave(String strChave)
        {
            var chave = ListaChaves.FirstOrDefault(x => x.Chave.Equals(strChave));
            if (chave != null)
            {
                return chave;
            }

            return null;
        }

        private Retorno ValidarSaldo(Decimal valor)
        {
            if (Saldo >= valor)
            {
                return new Retorno(true, 0, "Sucesso.");
            }
            else
            {
                return new Retorno(false, 70, "70 - Saldo insuficiente.");
            }
        }
        #endregion

        #region Listar Chave
        public RetornoListaChave<List<ChaveFavorita>> ListarChavesFavoritas()
        {
            var ret = new RetornoListaChave<List<ChaveFavorita>>(true, 0, "Sucesso.");
            ListaChaves.OrderBy(x=> x.ValorTotal);
            ret.Result = ListaChaves;
            return ret;
        }
        #endregion

        #region Persistir
        public Retorno Persistir()
        {
            var repo = new ChaveFavoritaRepository();

            return repo.Save(ListaChaves.Select(chave => new ChaveFavoritaDTO()
            {
                Chave = chave.Chave,
                TipoChave = (int)chave.TipoChave,
                NomeTitular = chave.NomeTitular,
                Quantidade = chave.Quantidade,
                ValorTotal = chave.ValorTotal
            }).ToList());            
        }

        #endregion

        private Retorno TryGetValor(string str, out decimal vlr)
        {
            vlr = 0;
            if (Decimal.TryParse(str, out vlr))
            {
                if (vlr <= 0)
                {
                    return new Retorno(false, 20, "20 - Valor deve ser maior ou igual a zero.");
                }
            }
            else
            {
                return new Retorno(false, 10, "Valor inválido.");
            }
            Retorno ret = new Retorno(true, 0, "");
            return ret;
        }
    }
}
