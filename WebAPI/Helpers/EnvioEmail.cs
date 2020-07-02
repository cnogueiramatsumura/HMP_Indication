using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace WebAPI.Helpers
{
    public class EnvioEmail
    {
        private ServerConfig ServerConfig { get; set; }
        //private static string EmailServer = "ccidhighwind@yahoo.com.br";
        public EnvioEmail(IServerConfigRepository _ServerConfigRepo)
        {       
            ServerConfig = _ServerConfigRepo.GetAllConfig();
        }

        public string EmailCadastro(string domain, string Token, string Email)
        {
            string CorpoMsg = "<tr>" +
                                "<td align='center' style='font-weight:bold; padding:20px;'>" +
                                   "<h1 style='text-aligtn:center;color:white;'>Para confirmar o Cadastro Clique no botao abaixo</h1>" +
                                "</td>" +
                              "</tr>" +
                              "<tr>" +
                                "<td align='center' style='font-weight:bold;padding:20px;'>" +
                                   "<a style='background-color:#6168c1; color:white; padding:10px 25px; border-radius:25px; font-weight:bold; text-decoration:none; cursor: pointer; font-size: 17px;' href='" + domain + "/api/Login/ConfirmaEmail?token=" + Token + "' target='_blank'>CLIQUE AQUI</a>" +
                                "</td>" +
                              "</tr>";

            using (MailMessage mailMessage = new MailMessage(ServerConfig.SmtpUsername, Email, "Confirmação Cadastro", corpoEmail(CorpoMsg)))
            {
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                mailMessage.From = new MailAddress(ServerConfig.SmtpUsername, "Support@DoingNow");
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.High;
                return sendmail(mailMessage);
            }
        }

        public string EmailRecuperarSenha(string domain, string Token, string Email)
        {
            string CorpoMsg = "<tr>" +
                                "<td align='center' style='font-weight:bold; padding:20px;'>" +
                                   "<h1 style='text-aligtn:center;color:white;'>Recuperação de Senha</h1>" +
                                "</td>" +
                              "</tr>" +
                              "<tr>" +
                                "<td align='center' style='font-weight:bold; padding:20px;'>" +
                                   "<p style='text-aligtn:center;color:white;'>Uma troca de Senha foi solicitada para este email</p>" +
                                    "<p style='text-aligtn:center;color:white;'>se voce nao solicitou esta troca por favor desconsidere este email</p>" +
                                    "<p style='text-aligtn:center;color:white;'>agora caso tenha solicitado esta troca de email clique no botão abaixo</p>" +
                                    "<p style='text-aligtn:center;color:white;'>e aguarde um novo email com a sua nova senha.</p>" +
                                "</td>" +
                              "</tr>" +
                              "<tr>" +
                                "<td align='center' style='font-weight:bold;padding:20px;'>" +
                                   "<a style='background-color:#6168c1; color:white; padding:10px 25px; border-radius:25px; font-weight:bold; text-decoration:none; cursor: pointer; font-size: 17px;' href='" + domain + "/api/usuario/GerarNovaSenha?token=" + Token + "' target='_blank'>CLIQUE AQUI</a>" +
                                "</td>" +
                              "</tr>";

            using (MailMessage mailMessage = new MailMessage(ServerConfig.SmtpUsername, Email, "Recupera Senha", corpoEmail(CorpoMsg)))
            {
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                mailMessage.From = new MailAddress(ServerConfig.SmtpUsername, "Support@DoingNow");
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.High;
                return sendmail(mailMessage);
            }
        }

        public string EmailEnviarNovaSenha(string novasenha, string Email)
        {
            string CorpoMsg = "<tr>" +
                                "<td align='center' style='font-weight:bold; padding:20px;'>" +
                                   "<h1 style='text-aligtn:center;color:white;'>Recuperação de Senha</h1>" +
                                "</td>" +
                              "</tr>" +
                              "<tr>" +
                                "<td align='center' style='font-weight:bold; padding:20px;'>" +
                                   "<p style='text-aligtn:center;color:white;'>Muito bom, Confirmamos sua indentidade e seu pedido para uma nova senha</p>" +
                                    "<p style='text-aligtn:center;color:white;'>sua senha agora é " + novasenha + " </p>" +
                                    "<p style='text-aligtn:center;color:white;'>Tenha mais cuidado, e não a deixe exposta ou anotada em papel</p>" +
                                    "<p style='text-aligtn:center;color:white;'>Lembre-se que sua senha é muito importante</p>" +
                                "</td>" +
                              "</tr>";

            using (MailMessage mailMessage = new MailMessage(ServerConfig.SmtpUsername, Email, "Nova Senha", corpoEmail(CorpoMsg)))
            {
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                mailMessage.From = new MailAddress(ServerConfig.SmtpUsername, "Support@DoingNow");
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.High;
                return sendmail(mailMessage);
            }
        }

        public void EmailTrocaSMTP(string domain, string smptAdress, int smptPort, string SmtpUsername, string smtpPassword)
        {
            string CorpoMsg = "<tr>" +
                                "<td align='center' style='font-weight:bold; padding:20px;'>" +
                                   "<h1 style='text-aligtn:center;color:white;'>Troca Servidor SMTP</h1>" +
                                "</td>" +
                              "</tr>" +
                              "<tr>" +
                                "<td align='center' style='font-weight:bold;padding:20px;'>" +
                                "<p style='text-aligtn:center;color:white;'>Agora que verificamos seu acesso e configuraçoes do servidor de email confirme a troca de servidor clicando no botao abaixo <p>" +
                                  "<p><a style='background-color:#6168c1; color:white; padding:10px 25px; border-radius:25px; font-weight:bold; text-decoration:none; cursor: pointer; font-size: 17px;' href='" + domain + "/Analista/configuracoes/ConfirmaTroca'>CLIQUE AQUI</a></p>" +
                                "</td>" +
                              "</tr>";

            using (MailMessage mailMessage = new MailMessage(SmtpUsername, SmtpUsername, "Troca Servidor Email", corpoEmail(CorpoMsg)))
            {
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                mailMessage.From = new MailAddress(SmtpUsername, "Support@DoingNow");
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.High;
                using (SmtpClient smtpClient = new SmtpClient(smptAdress, smptPort))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Credentials = new NetworkCredential(SmtpUsername, smtpPassword);
                    smtpClient.Send(mailMessage);
                }
            }
        }


        private string corpoEmail(string Msg)
        {
            return "<table cellpadding='0' cellspacing='0' width='100%' style='margin: 0; padding: 0; font-family:Open Sans,sans-serif;'><tr><td>" +
                    "<table align='center' cellpadding = '0' cellspacing='0' width='600'>" +
                              "<tr>" +
                                  "<td align='center' bgcolor='#6168c1' style='padding:10px 0 10px 0;'>" +
                                  "</td>" +
                               "</tr>" +
                               "<tr>" +
                                   "<td bgcolor='#37454e' style='padding: 40px 30px 40px 30px;'>" +
                                        "<table cellpadding='0' cellspacing='0' width='100%'>" +
                                           Msg +
                                        "</table>" +
                                   "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td bgcolor = '#6168c1' style = 'padding: 30px 30px 30px 30px;'>" +
                                         "<table cellpadding='0' cellspacing='0' width='100%'>" +
                                                  "<tr>" +
                                                      "<td width = '75%' style = 'color:white;font-size: 17px;'>" +
                                                            "CriptoStorm" +
                                                       "</td>" +
                                                   "</tr>" +
                                          "</table>" +
                                    "</td>" +
                                 "</tr>" +
                      "</table></td></tr>" +
                     "</table>";
        }

        private string sendmail(MailMessage _mailMessage)
        {
            using (SmtpClient smtpClient = new SmtpClient(ServerConfig.SmtpAdress, (int)ServerConfig.SmtpPort))
            {
                var cryptograph = new AESEncription();

                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential(ServerConfig.SmtpUsername, cryptograph.DecryptMensage(ServerConfig.SmtpPassword));
                try
                {
                    smtpClient.Send(_mailMessage);
                    return "Email Entregue com Sucesso";
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    //var retmsg = "";
                    //for (int i = 0; i < ex.InnerExceptions.Length; i++)
                    //{
                    //    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    //    return string.Format("Failed to deliver message to {0}", ex.InnerExceptions[i].FailedRecipient);
                    //}
                    return ex.Message;
                }
                catch (Exception ex)
                {
                    return string.Format("Exception caught in RetryIfBusy(): {0}", ex.Message.ToString());
                }
            }
        }
    }
}