using System.Collections.Generic;
using System.Linq;
using RemessaCobrancaOnline.Domain.Validation;
using RemessaCobrancaOnline.Dto.Bradesco.Envio;

namespace RemessaCobrancaOnline.Service.Bradesco.Boleto
{
    public class ValidarModelo //: IValidador<EnvioRemessaCobrancaBradescoDto>
    {
        public bool EhValido(EnvioRemessaCobrancaBradescoDto entidade)
        {
            return !Criticas(entidade).Any();
        }

        public IEnumerable<string> Criticas(EnvioRemessaCobrancaBradescoDto entidade)
        {
            if (EstaVazio(entidade.IdentificadorExterno))
                yield return "IdentificadorExterno obrigatório.";

            if (EstaVazio(entidade.LoginUsuario))
                yield return "LoginUsuario obrigatório.";

            if (entidade.FormaCobrancaId != 1)
                yield return "FormaCobrancaId não suportada pelo componente de remessa de cobrança para o Bradesco.";

            if ((byte)entidade.InstituicaoFinanceira != 1)
                yield return "InstituicaoFinanceira informada precisa ser o bradesco.";

            if (entidade.RemessaCobranca == null)
                yield return "Dados da remessa não informados.";
            else
            {
                if (EstaVazio(entidade.RemessaCobranca.bairroPagador))
                    yield return "Campo bairroPagador é obrigatório";

                if (!EstaVazio(entidade.RemessaCobranca.bairroPagador) && entidade.RemessaCobranca.bairroPagador.Length > 40)
                    yield return "Campo bairroPagador não pode execeder tamanho igual a 40";

                if (EstaVazio(entidade.RemessaCobranca.bairroSacadorAvalista) && !EstaVazio(entidade.RemessaCobranca.nomeSacadorAvalista))
                    yield return "Campo bairroSacadorAvalista é obrigatório quando informado campo nomeSacadorAvalista";

                if (!EstaVazio(entidade.RemessaCobranca.bairroSacadorAvalista) && entidade.RemessaCobranca.bairroSacadorAvalista.Length > 40)
                    yield return "Campo bairroSacadorAvalista não pode execeder tamanho igual a 40";

                if (EstaVazio(entidade.RemessaCobranca.cdBanco))
                    yield return "Campo cdBanco é obrigatório";

                if (!EstaVazio(entidade.RemessaCobranca.cdBanco) && entidade.RemessaCobranca.cdBanco.Length != 3)
                    yield return "Campo cdBanco precisa ter tamanho igual a 3";

                if (EstaVazio(entidade.RemessaCobranca.cdEspecieTitulo))
                    yield return "Campo cdBanco é obrigatório";

                if (!EstaVazio(entidade.RemessaCobranca.cdEspecieTitulo) && entidade.RemessaCobranca.cdEspecieTitulo.Length != 2)
                    yield return "Campo cdEspecieTitulo precisa ter tamanho igual a 2";

                if (EstaVazio(entidade.RemessaCobranca.cdIndCpfcnpjPagador))
                    yield return "Campo cdIndCpfcnpjPagador é obrigatório";

                if (!EstaVazio(entidade.RemessaCobranca.cdIndCpfcnpjPagador) && entidade.RemessaCobranca.cdIndCpfcnpjPagador.Length != 1)
                    yield return "Campo cdIndCpfcnpjPagador precisa ter tamanho igual a 1";

                if (EstaVazio(entidade.RemessaCobranca.cdIndCpfcnpjSacadorAvalista) && !EstaVazio(entidade.RemessaCobranca.nomeSacadorAvalista))
                    yield return "Campo cdIndCpfcnpjSacadorAvalista é obrigatório";

                if (EstaVazio(entidade.RemessaCobranca.cdIndCpfcnpjSacadorAvalista) && entidade.RemessaCobranca.cdIndCpfcnpjSacadorAvalista.Length != 1)
                    yield return "Campo cdIndCpfcnpjSacadorAvalista precisa ter tamanho igual a 1";

                if (!EstaVazio(entidade.RemessaCobranca.cdPagamentoParcial) && entidade.RemessaCobranca.cdPagamentoParcial.Length != 1)
                    yield return "Campo cdPagamentoParcial precisa ter tamanho igual a 1";

                if (!EstaVazio(entidade.RemessaCobranca.cdPagamentoParcial) && entidade.RemessaCobranca.cdPagamentoParcial.ToUpper() == "S" && EstaVazio(entidade.RemessaCobranca.qtdePagamentoParcial))
                    yield return "Campo cdPagamentoParcial é obrigatório quando cdPagamentoParcial igual a 'S'";

                if (!EstaVazio(entidade.RemessaCobranca.qtdePagamentoParcial) && entidade.RemessaCobranca.qtdePagamentoParcial.Length > 3)
                    yield return "Campo cdPagamentoParcial precisa ter tamanho máximo de 3";

                if (!EstaVazio(entidade.RemessaCobranca.percentualJuros) && entidade.RemessaCobranca.percentualJuros.Length > 8)
                    yield return "Campo percentualJuros precisa ter tamanho máximo de 8";

                if (!EstaVazio(entidade.RemessaCobranca.vlJuros) && entidade.RemessaCobranca.vlJuros.Length > 17)
                    yield return "Campo vlJuros precisa ter tamanho máximo de 17";

                if (!EstaVazio(entidade.RemessaCobranca.vlJuros) && !EstaVazio(entidade.RemessaCobranca.percentualJuros))
                    yield return "Campo vlJuros não pode ser informado junto com o campo percentualJuros";

                if (!EstaVazio(entidade.RemessaCobranca.qtdeDiasJuros) && entidade.RemessaCobranca.qtdeDiasJuros.Length > 2)
                    yield return "Campo qtdeDiasJuros precisa ter tamanho máximo de 2";

                if (!EstaVazio(entidade.RemessaCobranca.percentualMulta) && entidade.RemessaCobranca.percentualMulta.Length > 8)
                    yield return "Campo percentualMulta precisa ter tamanho máximo de 8";

                if (!EstaVazio(entidade.RemessaCobranca.vlMulta) && entidade.RemessaCobranca.vlMulta.Length > 17)
                    yield return "Campo vlMulta precisa ter tamanho máximo de 17";

                if (!EstaVazio(entidade.RemessaCobranca.qtdeDiasMulta) && entidade.RemessaCobranca.qtdeDiasMulta.Length > 3)
                    yield return "Campo qtdeDiasMulta precisa ter tamanho máximo de 3";

                if (!EstaVazio(entidade.RemessaCobranca.percentualDesconto1) && entidade.RemessaCobranca.percentualDesconto1.Length > 8)
                    yield return "Campo percentualDesconto1 precisa ter tamanho máximo de 8";

                if (!EstaVazio(entidade.RemessaCobranca.vlDesconto1) && entidade.RemessaCobranca.vlDesconto1.Length > 17)
                    yield return "Campo vlDesconto1 precisa ter tamanho máximo de 17";

                if ((!EstaVazio(entidade.RemessaCobranca.vlDesconto1) || !EstaVazio(entidade.RemessaCobranca.percentualDesconto1)) && EstaVazio(entidade.RemessaCobranca.dataLimiteDesconto1))
                    yield return "Campo dataLimiteDesconto1 obrigatório quando valor ou percentual é informado";

                if (!EstaVazio(entidade.RemessaCobranca.dataLimiteDesconto1) && entidade.RemessaCobranca.dataLimiteDesconto1.Length > 10)
                    yield return "Campo dataLimiteDesconto1 precisa ter tamanho máximo de 10";

                if (!EstaVazio(entidade.RemessaCobranca.percentualDesconto2) && entidade.RemessaCobranca.percentualDesconto2.Length > 8)
                    yield return "Campo percentualDesconto2 precisa ter tamanho máximo de 8";

                if (!EstaVazio(entidade.RemessaCobranca.vlDesconto2) && entidade.RemessaCobranca.vlDesconto2.Length > 17)
                    yield return "Campo vlDesconto2 precisa ter tamanho máximo de 17";

                if ((!EstaVazio(entidade.RemessaCobranca.vlDesconto2) || !EstaVazio(entidade.RemessaCobranca.percentualDesconto2)) && EstaVazio(entidade.RemessaCobranca.dataLimiteDesconto2))
                    yield return "Campo dataLimiteDesconto2 obrigatório quando valor ou percentual é informado";

                if (!EstaVazio(entidade.RemessaCobranca.dataLimiteDesconto2) && entidade.RemessaCobranca.dataLimiteDesconto2.Length > 10)
                    yield return "Campo dataLimiteDesconto2 precisa ter tamanho máximo de 10";

                if (!EstaVazio(entidade.RemessaCobranca.percentualDesconto3) && entidade.RemessaCobranca.percentualDesconto3.Length > 8)
                    yield return "Campo percentualDesconto3 precisa ter tamanho máximo de 8";

                if (!EstaVazio(entidade.RemessaCobranca.vlDesconto3) && entidade.RemessaCobranca.vlDesconto3.Length > 17)
                    yield return "Campo vlDesconto3 precisa ter tamanho máximo de 17";

                if ((!EstaVazio(entidade.RemessaCobranca.vlDesconto3) || !EstaVazio(entidade.RemessaCobranca.percentualDesconto3)) && EstaVazio(entidade.RemessaCobranca.dataLimiteDesconto3))
                    yield return "Campo dataLimiteDesconto3 obrigatório quando valor ou percentual é informado";

                if (!EstaVazio(entidade.RemessaCobranca.dataLimiteDesconto2) && entidade.RemessaCobranca.dataLimiteDesconto2.Length > 10)
                    yield return "Campo dataLimiteDesconto3 precisa ter tamanho máximo de 10";

                if (!EstaVazio(entidade.RemessaCobranca.prazoBonificacao) && entidade.RemessaCobranca.prazoBonificacao.Length > 2)
                    yield return "Campo prazoBonificacao precisa ter tamanho máximo de 2";

                if (!EstaVazio(entidade.RemessaCobranca.percentualBonificacao) && entidade.RemessaCobranca.percentualBonificacao.Length > 8)
                    yield return "Campo percentualBonificacao precisa ter tamanho máximo de 8";

                if (!EstaVazio(entidade.RemessaCobranca.vlBonificacao) && entidade.RemessaCobranca.vlBonificacao.Length > 17)
                    yield return "Campo vlBonificacao precisa ter tamanho máximo de 17";

                if ((!EstaVazio(entidade.RemessaCobranca.percentualBonificacao) || !EstaVazio(entidade.RemessaCobranca.vlBonificacao)) && EstaVazio(entidade.RemessaCobranca.prazoBonificacao))
                    yield return "Campo prazoBonificacao é obrigatório quando vlBonificacao ou percentualBonificacao é informado";

                if (!EstaVazio(entidade.RemessaCobranca.dtLimiteBonificacao) && entidade.RemessaCobranca.dtLimiteBonificacao.Length > 10)
                    yield return "Campo dtLimiteBonificacao precisa ter tamanho máximo de 10";

                if ((!EstaVazio(entidade.RemessaCobranca.percentualBonificacao) || !EstaVazio(entidade.RemessaCobranca.vlBonificacao)) && EstaVazio(entidade.RemessaCobranca.dtLimiteBonificacao))
                    yield return "Campo percentualBonificacao é obrigatório quando vlBonificacao ou percentualBonificacao é informado";

                if (!EstaVazio(entidade.RemessaCobranca.vlAbatimento) && entidade.RemessaCobranca.vlAbatimento.Length > 17)
                    yield return "Campo vlAbatimento precisa ter tamanho máximo de 17";

                if (EstaVazio(entidade.RemessaCobranca.nomePagador) || entidade.RemessaCobranca.nomePagador.Length > 70)
                    yield return "Campo nomePagador precisa ter tamanho máximo de 70";

                if (EstaVazio(entidade.RemessaCobranca.logradouroPagador) || entidade.RemessaCobranca.logradouroPagador.Length > 40)
                    yield return "Campo logradouroPagador precisa ter tamanho máximo de 40";

                if (EstaVazioSemZero(entidade.RemessaCobranca.nuLogradouroPagador) && entidade.RemessaCobranca.nuLogradouroPagador.Length > 10)
                    yield return "Campo nuLogradouroPagador precisa ter tamanho máximo de 10";

                if (!EstaVazio(entidade.RemessaCobranca.complementoLogradouroPagador) && entidade.RemessaCobranca.nuLogradouroPagador.Length > 15)
                    yield return "Campo complementoLogradouroPagador precisa ter tamanho máximo de 15";

                if (EstaVazio(entidade.RemessaCobranca.cepPagador) || entidade.RemessaCobranca.cepPagador.Length > 5)
                    yield return "Campo cepPagador precisa ter tamanho máximo de 5";

                if (entidade.RemessaCobranca.complementoCepPagador != "000" && (EstaVazio(entidade.RemessaCobranca.complementoCepPagador) || entidade.RemessaCobranca.complementoCepPagador.Length != 3))
                    yield return "Campo complementoCepPagador precisa ter tamanho de 3";

                if (EstaVazio(entidade.RemessaCobranca.bairroPagador) || entidade.RemessaCobranca.bairroPagador.Length > 40)
                    yield return "Campo bairroPagador precisa ter tamanho máximo de 40";

                if (EstaVazio(entidade.RemessaCobranca.municipioPagador) || entidade.RemessaCobranca.municipioPagador.Length > 30)
                    yield return "Campo municipioPagador precisa ter tamanho máximo de 30";

                if (EstaVazio(entidade.RemessaCobranca.ufPagador) || entidade.RemessaCobranca.ufPagador.Length > 2)
                    yield return "Campo ufPagador precisa ter tamanho máximo de 2";

                if (EstaVazio(entidade.RemessaCobranca.cdIndCpfcnpjPagador) || entidade.RemessaCobranca.cdIndCpfcnpjPagador.Length > 1)
                    yield return "Campo cdIndCpfcnpjPagador precisa ter tamanho máximo de 1";

                if (EstaVazio(entidade.RemessaCobranca.nuCpfcnpjPagador) || entidade.RemessaCobranca.nuCpfcnpjPagador.Length > 14)
                    yield return "Campo nuCpfcnpjPagador precisa ter tamanho máximo de 14";

                if (!EstaVazio(entidade.RemessaCobranca.endEletronicoPagador) && entidade.RemessaCobranca.endEletronicoPagador.Length > 70)
                    yield return "Campo endEletronicoPagador precisa ter tamanho máximo de 14";

                if (!EstaVazio(entidade.RemessaCobranca.nomeSacadorAvalista) && entidade.RemessaCobranca.nomeSacadorAvalista.Length > 40)
                    yield return "Campo nomeSacadorAvalista precisa ter tamanho máximo de 40";

                if (!EstaVazio(entidade.RemessaCobranca.logradouroSacadorAvalista) && entidade.RemessaCobranca.logradouroSacadorAvalista.Length > 40)
                    yield return "Campo logradouroSacadorAvalista precisa ter tamanho máximo de 40";

                if (!EstaVazio(entidade.RemessaCobranca.nomeSacadorAvalista) && EstaVazio(entidade.RemessaCobranca.logradouroSacadorAvalista))
                    yield return "Campo logradouroSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado";

                if (!EstaVazio(entidade.RemessaCobranca.nuLogradouroSacadorAvalista) && entidade.RemessaCobranca.nuLogradouroSacadorAvalista.Length > 10)
                    yield return "Campo nuLogradouroSacadorAvalista precisa ter tamanho máximo de 10";

                if (!EstaVazio(entidade.RemessaCobranca.nomeSacadorAvalista) && EstaVazio(entidade.RemessaCobranca.nuLogradouroSacadorAvalista))
                    yield return "Campo nuLogradouroSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado";

                if (!EstaVazio(entidade.RemessaCobranca.complementoLogradouroSacadorAvalista) && entidade.RemessaCobranca.complementoLogradouroSacadorAvalista.Length > 15)
                    yield return "Campo complementoLogradouroSacadorAvalista precisa ter tamanho máximo de 15";

                if (!EstaVazio(entidade.RemessaCobranca.cepSacadorAvalista) && entidade.RemessaCobranca.cepSacadorAvalista.Length > 5)
                    yield return "Campo cepSacadorAvalista precisa ter tamanho máximo de 5";

                if (!EstaVazio(entidade.RemessaCobranca.nomeSacadorAvalista) && EstaVazio(entidade.RemessaCobranca.cepSacadorAvalista))
                    yield return "Campo cepSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado";

                if (!EstaVazio(entidade.RemessaCobranca.complementoCepSacadorAvalista) && entidade.RemessaCobranca.complementoCepSacadorAvalista.Length > 3)
                    yield return "Campo complementoCepSacadorAvalista precisa ter tamanho máximo de 3";

                if (!EstaVazio(entidade.RemessaCobranca.nomeSacadorAvalista) && EstaVazio(entidade.RemessaCobranca.complementoCepSacadorAvalista))
                    yield return "Campo complementoCepSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado";

                if (!EstaVazio(entidade.RemessaCobranca.bairroSacadorAvalista) && entidade.RemessaCobranca.bairroSacadorAvalista.Length > 40)
                    yield return "Campo bairroSacadorAvalista precisa ter tamanho máximo de 40";

                if (!EstaVazio(entidade.RemessaCobranca.nomeSacadorAvalista) && EstaVazio(entidade.RemessaCobranca.bairroSacadorAvalista))
                    yield return "Campo bairroSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado";

                if (!EstaVazio(entidade.RemessaCobranca.municipioSacadorAvalista) && entidade.RemessaCobranca.municipioSacadorAvalista.Length > 40)
                    yield return "Campo municipioSacadorAvalista precisa ter tamanho máximo de 40";

                if (!EstaVazio(entidade.RemessaCobranca.nomeSacadorAvalista) && EstaVazio(entidade.RemessaCobranca.municipioSacadorAvalista))
                    yield return "Campo municipioSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado";

                if (!EstaVazio(entidade.RemessaCobranca.ufSacadorAvalista) && entidade.RemessaCobranca.ufSacadorAvalista.Length > 2)
                    yield return "Campo ufSacadorAvalista precisa ter tamanho máximo de 2";

                if (!EstaVazio(entidade.RemessaCobranca.nomeSacadorAvalista) && EstaVazio(entidade.RemessaCobranca.ufSacadorAvalista))
                    yield return "Campo ufSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado";

                if (!EstaVazio(entidade.RemessaCobranca.cdIndCpfcnpjSacadorAvalista) && entidade.RemessaCobranca.cdIndCpfcnpjSacadorAvalista.Length > 1)
                    yield return "Campo cdIndCpfcnpjSacadorAvalista precisa ter tamanho máximo de 1";

                if (!EstaVazio(entidade.RemessaCobranca.nomeSacadorAvalista) && EstaVazio(entidade.RemessaCobranca.cdIndCpfcnpjSacadorAvalista))
                    yield return "Campo cdIndCpfcnpjSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado";

                if (!EstaVazio(entidade.RemessaCobranca.nuCpfcnpjSacadorAvalista) && entidade.RemessaCobranca.nuCpfcnpjSacadorAvalista.Length > 14)
                    yield return "Campo nuCpfcnpjSacadorAvalista precisa ter tamanho máximo de 14";

                if (!EstaVazio(entidade.RemessaCobranca.nomeSacadorAvalista) && EstaVazio(entidade.RemessaCobranca.nuCpfcnpjSacadorAvalista))
                    yield return "Campo nuCpfcnpjSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado";

                if (!EstaVazio(entidade.RemessaCobranca.endEletronicoSacadorAvalista) && entidade.RemessaCobranca.endEletronicoSacadorAvalista.Length > 70)
                    yield return "Campo endEletronicoSacadorAvalista precisa ter tamanho máximo de 70";
            }
        }

        private bool EstaVazio(string param)
        {
            if (string.IsNullOrWhiteSpace(param))
                return true;
            long resultado;
            var converteu = long.TryParse(param, out resultado);
            return converteu && resultado == 0;
        }

        private bool EstaVazioSemZero(string param)
        {
            if (string.IsNullOrWhiteSpace(param))
                return true;
            long resultado;
            var converteu = long.TryParse(param, out resultado);
            return converteu && resultado > 0;
        }
    }
}
