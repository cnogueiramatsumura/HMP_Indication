using DataAccess.Interfaces;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;

namespace WebApp.Helpers
{
    public class Helper
    {
        public static string GetJWTPayloadValue(string tokenString, string payloadkey)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);
            var payloaditem = token.Payload.FirstOrDefault(x => x.Key == payloadkey);
            return payloaditem.Value.ToString();
        }
        public static string EncryptSha512(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
        public static string showtime(DateTimeOffset datalancamento)
        {
            var now = DateTimeOffset.UtcNow;
            var difftime = now - datalancamento;

            var minutes = difftime.Minutes;
            var hours = difftime.Hours;
            var dias = difftime.Days;

            if (dias >= 1)
                return dias + (dias == 1 ? " dia" : "dias");
            else if (hours >= 1)
                return hours + (hours == 1 ? " hora" : " horas");
            else if (minutes >= 1)
                return minutes + (minutes == 1 ? " minuto" : " minutos");
            return "agora";
        }
        public static string PercentualGain(decimal valorentrada, decimal valorstop)
        {
            decimal percentualLucro = 0m;
            if (valorstop == 0)
            {
                return 0f.ToString("F2");
            }
            percentualLucro = ((valorstop - valorentrada) / valorentrada) * 100;
            return percentualLucro.ToString("F2");
        }
        public static string PercentualLoss(decimal valorentrada, decimal valorstop)
        {
            decimal percentualLucro = 0m;
            if (valorstop == 0)
            {
                return 0f.ToString("F2");
            }
            percentualLucro = ((valorentrada - valorstop) / valorentrada) * 100;
            return percentualLucro.ToString("F2");
        }
        public static BigInteger GetBitpayPrivateKeyFromFile(bool isSandbox)
        {
            var path = isSandbox ? HostingEnvironment.MapPath("~/Files//bitpay_private_test.key") : HostingEnvironment.MapPath("~/Files//bitpay_private_prod.key");
            using (var fs = File.OpenRead(path))
            {
                var b = new byte[1024];
                fs.Read(b, 0, b.Length);
                DerOctetString key;
                using (var decoder = new Asn1InputStream(b))
                {
                    var seq = (DerSequence)decoder.ReadObject();
                    //Debug.Assert(seq.Count == 4, "Input does not appear to be an ASN.1 OpenSSL EC private key");
                    //Debug.Assert(((DerInteger)seq[0]).Value.Equals(BigInteger.One), "Input is of wrong version");
                    key = (DerOctetString)seq[1];
                }
                return new BigInteger(1, key.GetOctets());
            }
        }
        public static DateTimeOffset BrazilianTime(DateTimeOffset UTCDate)
        {
            var kstZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            var dateTimeBrasilia = TimeZoneInfo.ConvertTime(UTCDate, kstZone);
            return dateTimeBrasilia;
        }
        public static string BrazilianCurrency(decimal value)
        {
            var culture = CultureInfo.GetCultureInfo("pt-BR");
            return string.Format(culture, "{0:C2}", value);
        }
    }
}