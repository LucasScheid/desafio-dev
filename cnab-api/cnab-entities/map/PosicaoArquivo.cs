using cnab_entities.enums;

namespace cnab_entities.map
{
    public class PosicaoArquivo
    {
        public PosicaoArquivo(NomeCamposCNAB nomeCampo, int inicio, int fim, bool ultimoCampo = false)
        {
            NomeCampo = nomeCampo;
            Inicio = inicio;
            Fim = fim;
            UltimoCampo = ultimoCampo;
        }
        public NomeCamposCNAB NomeCampo { get; set; }
        public int Inicio { get; set; }
        public int Fim { get; set; }

        public bool UltimoCampo { get; set; }
    }
}
