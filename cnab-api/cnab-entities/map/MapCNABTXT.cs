using cnab_entities.enums;

namespace cnab_entities.map
{
    public class MapCNABTXT : IArquivoMapPosicao
    {
        private readonly ICollection<PosicaoArquivo> _listaCampos;

        public MapCNABTXT()
        {
            _listaCampos = new List<PosicaoArquivo>
            {
                new PosicaoArquivo(NomeCamposCNAB.TIPO, 0, 1,false),
                new PosicaoArquivo(NomeCamposCNAB.DATA, 1, 9,false),
                new PosicaoArquivo(NomeCamposCNAB.VALOR, 9, 19,false),
                new PosicaoArquivo(NomeCamposCNAB.CPF, 19, 30,false),
                new PosicaoArquivo(NomeCamposCNAB.CARTAO, 30, 42,false),
                new PosicaoArquivo(NomeCamposCNAB.HORA, 42, 48,false),
                new PosicaoArquivo(NomeCamposCNAB.DONO_LOJA, 48, 62,false),
                new PosicaoArquivo(NomeCamposCNAB.NOME_LOJA, 62, 0,true)
            };
        }

        public PosicaoArquivo ObterPosicaoCampo(NomeCamposCNAB nomeCampo)
        {
            PosicaoArquivo? posicaoArquivo = _listaCampos.FirstOrDefault(l => l.NomeCampo.ToString().Equals(nomeCampo.ToString()));

            return (posicaoArquivo ?? new PosicaoArquivo(NomeCamposCNAB.ERRO, 0, 0, false));
        }
    }
}
