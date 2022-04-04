using cnab_entities.dto;
using cnab_entities.map;
using cnab_entities.models;
using cnab_helpers.environment;
using cnab_services.arquivo;
using cnab_services.database;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace cnab_unit_tests.back_end
{
    public class ArquivoServiceTest
    {
        public ArquivoServiceTest()
        {
            AjustarVariaveisAmbiente();
        }

        [Fact]
        public async Task ExtensaoArquivoInvalida()
        {
            CompleteArquivoUpload<CNAB> completeArquivoUpload = await TestarLinha("CNAB0001.csv", "3201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       ");

            Assert.True((!completeArquivoUpload.Valido && completeArquivoUpload.Erros.Any(e => e.Contains("Extensão .csv não permitida!"))));
        }

        [Fact]
        public async Task LinhaValida()
        {
            CompleteArquivoUpload<CNAB> completeArquivoUpload = await TestarLinha("CNAB0002.txt", "3201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       ");

            Assert.True(completeArquivoUpload.Valido);
        }

        [Fact]
        public async Task TipoTransacaoInvalido()
        {
            CompleteArquivoUpload<CNAB> completeArquivoUpload = await TestarLinha("CNAB0003.txt", "0201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       ");

            Assert.True((!completeArquivoUpload.Valido && completeArquivoUpload.Erros.Any(e => e.Contains("O campo TIPO é inválido"))));
        }

        [Fact]
        public async Task DataInvalida()
        {
            CompleteArquivoUpload<CNAB> completeArquivoUpload = await TestarLinha("CNAB0004.txt", "3201933450000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       ");

            Assert.True((!completeArquivoUpload.Valido && completeArquivoUpload.Erros.Any(e => e.Contains("O campo DATA é inválido"))));
        }

        [Fact]
        public async Task ValorInvalido()
        {
            CompleteArquivoUpload<CNAB> completeArquivoUpload = await TestarLinha("CNAB0005.txt", "320190301a000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       ");

            Assert.True((!completeArquivoUpload.Valido && completeArquivoUpload.Erros.Any(e => e.Contains("O campo VALOR é inválido"))));
        }

        [Fact]
        public async Task ArquivoVazio()
        {
            CompleteArquivoUpload<CNAB> completeArquivoUpload = await TestarLinha("CNAB0006.txt", "");

            Assert.True((!completeArquivoUpload.Valido && completeArquivoUpload.Erros.Any(e => e.Contains("Arquivo vazio!"))));
        }

        [Fact]
        public async Task HoraInvalida()
        {
            CompleteArquivoUpload<CNAB> completeArquivoUpload = await TestarLinha("CNAB0007.txt", "3201903010000014200096206760174753****31531534aaJOÃO MACEDO   BAR DO JOÃO       ");

            Assert.True((!completeArquivoUpload.Valido && completeArquivoUpload.Erros.Any(e => e.Contains("O campo HORA é inválido"))));
        }

        [Fact]
        public async Task ValorDivididoPorCem()
        {
            CompleteArquivoUpload<CNAB> completeArquivoUpload = await TestarLinha("CNAB0008.txt", "3201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       ");

            Assert.True(completeArquivoUpload.Valido && completeArquivoUpload.Resultado.FirstOrDefault().Valor == 142);
        }

        [Fact]
        public async Task SuperaTamanhoMaximo()
        {
            string linha = "3201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       ";

            StringBuilder sbLinhas = new();

            for (int i = 0; i < 30000; i++)
                sbLinhas.AppendLine(linha);

            CompleteArquivoUpload<CNAB> completeArquivoUpload = await TestarLinha("CNAB0009.txt", sbLinhas.ToString());

            Assert.True((!completeArquivoUpload.Valido && completeArquivoUpload.Erros.Any(e => e.Contains("Arquivo supera 2 mega!"))));
        }

        [Fact]
        public async Task LinhaEmBranco()
        {
            string linha = "3201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       ";

            StringBuilder sbLinhas = new();

            sbLinhas.AppendLine(linha);
            sbLinhas.AppendLine("");
            sbLinhas.AppendLine(linha);

            CompleteArquivoUpload<CNAB> completeArquivoUpload = await TestarLinha("CNAB0010.txt", sbLinhas.ToString());

            Assert.True((!completeArquivoUpload.Valido && completeArquivoUpload.Erros.Any(e => e.Contains("A LINHA está em branco"))));
        }

        private static void AjustarVariaveisAmbiente()
        {
            Environment.SetEnvironmentVariable("FILE_UPLOAD_MAX_SIZE", "2097152");
        }

        private static async Task<CompleteArquivoUpload<CNAB>> TestarLinha(string nomeArquivo, string conteudo)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);

            await streamWriter.WriteAsync(conteudo);
            await streamWriter.FlushAsync();

            memoryStream.Position = 0;

            Mock<IFormFile> mockIFormFile = new();

            mockIFormFile.Setup(_ => _.OpenReadStream()).Returns(memoryStream);
            mockIFormFile.Setup(_ => _.FileName).Returns(nomeArquivo);
            mockIFormFile.Setup(_ => _.Length).Returns(memoryStream.Length);

            IFormFile file = mockIFormFile.Object;

            List<IFormFile> formFiles = new() { file };

            ITipoTransacaoDbService mockTipoTransacaoDbService = new TipoTransacaoDbServiceMock();
            Mock<IEnvironmentHelper> mockEnvironmentHelper = new();
            IArquivoMapPosicao mapCNABTXT = new MapCNABTXT();
            ArquivoService arquivoService;
            arquivoService = new(mockEnvironmentHelper.Object, mapCNABTXT, mockTipoTransacaoDbService);

            return (await arquivoService.Upload(formFiles)).CompleteArquivosUpload.FirstOrDefault();
        }
    }
}
