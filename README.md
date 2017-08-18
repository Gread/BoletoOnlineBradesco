# BoletoOnlineBradesco

Código em c# para registro de títulos de cobrança no banco Bradesco com assinatura digital utilizando o padrão PKCS#7 com o algoritmo de assinatura RSA2048 e de criptografia SHA256;

Pré-Requisitos: 

- Certificado tipo A1 e-cpf ou e-cnpj com chave privada emitido por autoridade participante do ICP-Brasil
- Biblioteca Bouncy Castle: http://www.bouncycastle.org/csharp/index.html
- JSON com aspas duplas

Erros comuns:

- Permissão do certificado: após instalado na máquina se faz necessário conceder permissão ao usuário que está executando o código. (CryptographicException 'Keyset does not exist')
- Não encontrou o certificado: o código lê certificados que se encontram na pasta Pessoal da máquina local, é possível instalar certificado em diversos grupos/pastas e em repositórios distintos como máquina local ou usuários específicos, atentar a isso.
- O manual atual v1.7, ainda consta um fluxo assincrono, onde se envia a mensagem, recebe um token, e com um token se pesquisa para saber se o registro foi processado ou não. Este fluxo não existe mais, é sincrono, e a resposta já traz os dados do boleto registrado caso sucesso.
