using ProvaCSharp.Data.Util;
using ProvaCSharp.Domain;
using System;

namespace ProvaCSharp
{
    class Program
    {
        private static Conta conta = new Conta();

        static void Main(string[] args)
        {
            montarMenu();
        }

        static void montarMenu()
        {
            Int16 opt = 0;
            while (opt != 6)
            {
                Console.Clear();
                Console.WriteLine($"== Saldo atual: {String.Format("{0:C}", conta.Saldo)} ==");
                Console.WriteLine("");
                Console.WriteLine("1.Efetuar crédito em conta");
                Console.WriteLine("2.Adicionar chave favorita");
                Console.WriteLine("3.Listar chaves favoritas");
                Console.WriteLine("4.Enviar PIX");
                Console.WriteLine("5.Persistir chaves");
                Console.WriteLine("6.Sair");
                Console.WriteLine("");
                Console.Write("Informe a opção desejada: ");
                String key = Console.ReadLine().Trim();
                Console.WriteLine("");

                try
                {
                    opt = Convert.ToInt16(key);
                }
                catch (Exception e)
                {
                    opt = 0;
                }

                if (opt >= 1 && opt <= 5)
                {
                    executarOpcao(opt);
                }
                else if (opt != 6)
                {
                    Console.WriteLine("Opção iválida!");
                    fimAcao();
                }
            }
        }

        static void executarOpcao(Int16 opt)
        {
            switch (opt)
            {
                case 1:
                    creditar();
                    break;
                case 2:
                    adicionarChave();
                    break;
                case 3:
                    listarChaves();
                    break;
                case 4:
                    enviarPix();
                    break;
                case 5:
                    persistirLista();
                    break;
            }
        }

        private static void creditar()
        {
            Console.WriteLine("");
            Console.Write("Informe o valor a ser creditado: ");
            string str = Console.ReadLine().Trim();

            Retorno ret = conta.CreditarConta(str);
            if (ret.Sucesso)
            {
                Console.WriteLine("Sucesso!");
            }
            else
            {
                Console.WriteLine(ret.Mensagem);
            }
            fimAcao();
        }

        private static void adicionarChave()
        {
            Console.WriteLine("");

            Console.Write("Informe o nome do titular: ");
            string nomeTitular = Console.ReadLine().Trim();

            Console.WriteLine("Informe o tipo de chave conforme opções[1-Telefone, 2-CPF]:");
            string tipo = Console.ReadLine().Trim();

            Console.Write("Informe a chave: ");
            string chave = Console.ReadLine().Trim();

            Retorno ret = conta.AdicionarChaveFavorita(nomeTitular, tipo, chave);
            Console.WriteLine(ret.Mensagem);
            fimAcao();
        }

        private static void listarChaves()
        {
            var ret = conta.ListarChavesFavoritas();
            if (ret.Sucesso)
            {
                Console.WriteLine("<< LISTA DE CAHVES PREFERENCIAIS >>");
                foreach (var ch in ret.Result)
                {
                    Console.WriteLine($"{ch.Chave} - {ch.NomeTitular} - {ch.Quantidade} - {string.Format("{0:C}", ch.ValorTotal)}");
                }
            }
            else
            {
                Console.WriteLine(ret.Mensagem);
            }
            fimAcao();
        }

        private static void enviarPix()
        {
            Console.WriteLine("");

            Console.Write("Informe a chave destino: ");
            string chave = Console.ReadLine().Trim();

            Console.Write("Informe o valor: ");
            string valor = Console.ReadLine().Trim();

            Retorno ret = conta.EnviarPIX(chave, valor);
            if (ret.Sucesso)
            {
                Console.WriteLine("Sucesso!");
            }
            else
            {
                Console.WriteLine(ret.Mensagem);
            }
            fimAcao();
        }

        private static void persistirLista()
        {
            Retorno ret = conta.Persistir();
            if (ret.Sucesso)
            {
                Console.WriteLine("Sucesso!");
            }
            else
            {
                Console.WriteLine(ret.Mensagem);
            }
            fimAcao();
        }

        private static void fimAcao()
        {
            Console.WriteLine("");
            Console.WriteLine("<<Pressione Enter>>");
            Console.ReadKey();
        }
    }
}