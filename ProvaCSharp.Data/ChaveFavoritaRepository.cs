using Bergs.ProvacSharp.BD;
using ProvaCSharp.Data.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProvaCSharp.Data
{
    public class ChaveFavoritaRepository : IRepository<ChaveFavoritaDTO>
    {
        private readonly string pathDB;
        public ChaveFavoritaRepository()
        {
            pathDB = "C:\\soft\\pxc\\data\\Pxcz02da.mdb";
        }

        public Retorno Save(List<ChaveFavoritaDTO> chaves)
        {
            using (var db = new AcessoBancoDados(pathDB))
            {
                db.Abrir();
                try
                {
                    if (!db.ExecutarDelete("DELETE FROM CHAVE"))
                    {
                        return new Retorno(false, 980, "Falha ao deletar registros.");
                    }

                    foreach (var chave in chaves)
                    {
                        if (!db.ExecutarInsert(GetInsertSql(chave)))
                        {
                            return new Retorno(false, 990, "Falha ao incluir registro.");
                        }
                    }                  

                    db.EfetivarComandos();
                }
                catch (Exception ex)
                {
                    return new Retorno(false, 990, "Erro ao persistir chaves: " + ex.Message);
                }
                finally
                {
                    db.Dispose();
                }
                return new Retorno(true, 0, "Chaves persistidas com sucesso.");
            }
        }

        private string GetInsertSql(ChaveFavoritaDTO chave)
        {
            string sql = "INSERT INTO CHAVE (TIPO_CHAVE, NOME_TITULAR, QTDE_PIX, VLR_TOTAL_PIX, CHAVE) VALUES(@TipoChave, @NomeTitular, @QtdePix, @VlrTotalPix, @Chave)";

            SqlCommand command = new SqlCommand(sql);
            command.CommandType = CommandType.Text;

            command.AdicionaParametroValor("@TipoChave", DbType.Int32, chave.TipoChave);
            command.AdicionaParametroValor("@NomeTitular", DbType.String, chave.NomeTitular);

            command.AdicionaParametroValor("@QtdePix", DbType.Int32, chave.Quantidade);
            command.AdicionaParametroValor("@VlrTotalPix", DbType.Currency, chave.ValorTotal);
            command.AdicionaParametroValor("@Chave", DbType.String, chave.Chave);
            
            return command.GetGeneratedQuery();            
        }
    }
}
