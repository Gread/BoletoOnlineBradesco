using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509.Store;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace BradescoOnline
{
    public class RemessaCobrancaOnlineService
    {        
        private void AssinarCriptografar(EnvioRemessaCobrancaBradescoJson model)
        {
            try
            {
                var data = ConverterParaJsonAspasSimples(model);
                var encoding = new UTF8Encoding();
                var messageBytes = encoding.GetBytes(data);                

                var impressaDigitalCertificado = ConfigurationManager.AppSettings["ImpressaoDigitalCertificado"];
                
                // certificado precisa ser instalado na máquina local e na pasta pessoal, diferente disso alterar linha abaixo
                var store = new X509Store(StoreLocation.LocalMachine);

                store.Open(OpenFlags.ReadOnly);
                var privateCert = store.Certificates.Cast<X509Certificate2>().FirstOrDefault(cert => cert.Thumbprint == impressaDigitalCertificado && cert.HasPrivateKey);

                if (privateCert == null)                                    
                    throw new Exception("Certificado não localizado.");                
                if (privateCert.PrivateKey == null)                
                    throw new Exception("chave privada não localizada no certificado.");
                
                //convertendo certificado para objeto que o bouncycastle conhece
                var bouncyCastleKey = DotNetUtilities.GetKeyPair(privateCert.PrivateKey).Private;
                var x5091 = new X509Certificate(privateCert.RawData);
                var x509CertBouncyCastle = DotNetUtilities.FromX509Certificate(x5091);

                var generator = new CmsSignedDataGenerator();
                var signerInfoGeneratorBuilder = new SignerInfoGeneratorBuilder();
                generator.AddSignerInfoGenerator(
                    signerInfoGeneratorBuilder.Build(new Asn1SignatureFactory("SHA256WithRSA", bouncyCastleKey),
                        x509CertBouncyCastle));
                
                //criando certstore que o bouncycastle conhece
                IList certList = new ArrayList();
                certList.Add(x509CertBouncyCastle);
                var store509BouncyCastle = X509StoreFactory.Create("Certificate/Collection", new X509CollectionStoreParameters(certList));
                generator.AddCertificates(store509BouncyCastle);

                var cmsdata = new CmsProcessableByteArray(messageBytes);
                //assina
                var signeddata = generator.Generate(cmsdata, true);
                var mensagemFinal = signeddata.GetEncoded();                
                //converte para base64 que eh o formato que o serviço espera
                var mensagemConvertidaparaBase64 = Convert.ToBase64String(mensagemFinal);                
                
                //chama serviço convertendo a string na base64 em bytes
                CriarServicoWebEEnviar("url_servico_bradesco_consta_manual", encoding.GetBytes(mensagemConvertidaparaBase64));
            }
            catch (Exception ex)
            {                
                throw;
            }
        }
        
        private void CriarServicoWebEEnviar(string uri, byte[] sig)
        {
            //TLS 1.2
            ServicePointManager.SecurityProtocol = (SecurityProtocolType) 3072;

            var request = WebRequest.Create(uri);

            request.Method = "POST";
            request.ContentType = "application/pkcs7-signature";
            request.ContentLength = sig.Length;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(sig, 0, sig.Length);
            }
            var response = request.GetResponse();
            
            using (var stream = response.GetResponseStream())
            {
                if (stream == null)
                    throw new ApplicationException("erro ao obter resposta");

                var reader = new StreamReader(stream);
                
                // resultado FINAL aqui
                var retorno = reader.ReadToEnd();
                
                var respostaAqui = JsonConvert.DeserializeObject<RetornoRemessaCobrancaBradescoJson>(XDocument.Parse(retorno).Root.Value);
            }
        }
        
         public string ConverterParaJsonAspasSimples(EnvioRemessaCobrancaBradescoJson data)
         {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                writer.QuoteChar = '\"';

                var ser = new JsonSerializer();
                ser.Serialize(writer, data);
            }
            return sb.ToString();
        }
    }
}
