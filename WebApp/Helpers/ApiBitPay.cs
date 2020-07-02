using DataAccess.Entidades;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebApp.Helpers
{
    public class ApiBitPay
    {

        private readonly string _baseUrL;
        private readonly string token;
        private readonly string redirectUrl;
        private readonly ECDomainParameters EcParams;
        private readonly BigInteger _privatekey;
        private readonly char[] HexArray = "0123456789abcdef".ToCharArray();
        private readonly string _identity; //public key

        public ApiBitPay(string Apitoken, string identity, bool useSandbox = true)
        {
            token = useSandbox ? "85FaHXnP1PxjyBLX1bHjx4qBrWEGEmr4LuJHMZZ8gepW" : Apitoken;
            _baseUrL = useSandbox ? "https://test.bitpay.com" : "https://bitpay.com";
            redirectUrl = useSandbox ? "localhost/usuario/pagamentos/check" : "criptostorm.doingnow.com.br/usuario/pagamentos/check";
            _identity = useSandbox ? "045f191592c25654130c9a14e6a88e5e9a0055093700e9c04a03b5825dc727fdcebd47948220732b64effbca9d920e368ab6f48cb75d0c093140eb4de361406bd3" : identity;
            var ecParameters = SecNamedCurves.GetByName("secp256k1");
            EcParams = new ECDomainParameters(ecParameters.Curve, ecParameters.G, ecParameters.N, ecParameters.H);
            _privatekey = Helper.GetBitpayPrivateKeyFromFile(useSandbox);
        }

        public HttpResponseMessage GeneratePayment(Usuario user, int pagamentoLicencaId, decimal precoLicenca, string tokenPagamento)
        {
            var invoice = new BitpayInvoice
            {
                orderId = pagamentoLicencaId,
                guid = Guid.NewGuid().ToString(),
                price = precoLicenca,
                token = token,
                redirectUrl = redirectUrl,
                notificationURL = "https://test.doingnow.com.br/usuario/pagamentos/BitpaySuccesso",
                posData = tokenPagamento,
                notify = "cnogueiramatsumura@yahoo.com.br"
            };
            var objinvoice = JsonConvert.SerializeObject(invoice);
            var response = CreateOrder(objinvoice);
            return response;
        }

        private HttpResponseMessage CreateOrder(string body)
        {
            using (var _httpClient = new HttpClient())
            {
                var bodyContent = new StringContent(body);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-accept-version", "2.0.0");
                bodyContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var signature = GetSignature(_baseUrL + "/invoices" + body);
                _httpClient.DefaultRequestHeaders.Add("x-signature", signature);
                _httpClient.DefaultRequestHeaders.Add("x-identity", _identity);
                var result = _httpClient.PostAsync(_baseUrL + "/invoices", bodyContent).Result;
                return result;
            }
        }
        #region Signature
        //todos os metodos abaixo sao para pegar a assinatura
        private string GetSignature(string input)
        {
            // return ecKey.Sign(input);
            var hash = Sha256Hash(input);
            var hashBytes = HexToBytes(hash);
            var signature = ecKeySign(hashBytes);
            var bytesHex = BytesToHex(signature);
            return bytesHex;
        }
        private static string Sha256Hash(string value)
        {
            var sb = new StringBuilder();
            using (var hash = SHA256.Create())
            {
                var enc = Encoding.UTF8;
                var result = hash.ComputeHash(enc.GetBytes(value));
                foreach (var b in result)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        private byte[] HexToBytes(string hex)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            if (hex.Length % 2 == 1)
                throw new FormatException("The binary key cannot have an odd number of digits");
            if (hex == string.Empty)
                return new byte[0];
            var arr = new byte[hex.Length >> 1];
            for (var i = 0; i < hex.Length >> 1; ++i)
            {
                var highNibble = hex[i << 1];
                var lowNibble = hex[(i << 1) + 1];
                if (!IsValidHexDigit(highNibble) || !IsValidHexDigit(lowNibble))
                    throw new FormatException("The binary key contains invalid chars.");
                arr[i] = (byte)((GetHexVal(highNibble) << 4) + GetHexVal(lowNibble));
            }
            return arr;
        }
        private bool IsValidHexDigit(char chr)
        {
            return '0' <= chr && chr <= '9' || 'a' <= chr && chr <= 'f' || 'A' <= chr && chr <= 'F';
        }
        private int GetHexVal(char hex)
        {
            var val = (int)hex;
            return val - (val < 58 ? 48 : val < 97 ? 55 : 87);
        }
        private byte[] ecKeySign(byte[] input)
        {
            var ecDsaSigner = new ECDsaSigner();
            var privateKeyParameters = new ECPrivateKeyParameters(_privatekey, EcParams);
            ecDsaSigner.Init(true, privateKeyParameters);
            var signature = ecDsaSigner.GenerateSignature(input);
            using (var memoryStream = new MemoryStream())
            {
                var sequenceGenerator = new DerSequenceGenerator(memoryStream);
                sequenceGenerator.AddObject(new DerInteger(signature[0]));
                sequenceGenerator.AddObject(new DerInteger(signature[1]));
                sequenceGenerator.Close();
                return memoryStream.ToArray();
            }
        }
        private string BytesToHex(byte[] bytes)
        {
            var hexChars = new char[bytes.Length * 2];
            for (var j = 0; j < bytes.Length; j++)
            {
                var v = bytes[j] & 0xFF;
                hexChars[j * 2] = HexArray[(int)((uint)v >> 4)];
                hexChars[j * 2 + 1] = HexArray[v & 0x0F];
            }
            return new string(hexChars);
        }
        #endregion
    }
}