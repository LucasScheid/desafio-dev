using cnab_entities.enums;

namespace cnab_entities.map
{
    public interface IArquivoMapPosicao
    {
        public PosicaoArquivo ObterPosicaoCampo(NomeCamposCNAB nomeCampo);
    }
}
