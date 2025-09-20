// Certifique-se de ter os usings necessários no topo do arquivo
using FeedbackApi.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

public class EmailService
{
  private readonly EmailSettings _emailSettings;

  public EmailService(IOptions<EmailSettings> emailSettings)
  {
    _emailSettings = emailSettings.Value;
  }

  public async Task EnviarEmailDeFeedback(Feedback feedback)
  {
    var mensagemEmail = new MimeMessage();
    mensagemEmail.From.Add(new MailboxAddress("Cestas da Manhã", _emailSettings.Email));
    mensagemEmail.To.Add(new MailboxAddress("Dona da Empresa", _emailSettings.Email_Destination));
    mensagemEmail.Subject = "Novo Feedback Recebido!";

    var corpoEmail = new BodyBuilder
    {
      HtmlBody = $@"
                <h3>Olá!</h3>
                <p>Você recebeu um novo Feedback de um cliente.</p>
                <ul>
                    <li><b>Cliente:</b> {feedback.Nome}</li>
                    <li><b>Mensagem:</b> {feedback.Mensagem}</li>
                    <li><b>Avaliação:</b> {feedback.Estrelas}</li>
                </ul>
                <p>Atenciosamente,<br/>Sua Aplicação</p>
            "
    };
    mensagemEmail.Body = corpoEmail.ToMessageBody();

    using (var clienteSmtp = new SmtpClient())
    {
      try
      {
        await clienteSmtp.ConnectAsync(_emailSettings.Host, 587, SecureSocketOptions.StartTls);
        await clienteSmtp.AuthenticateAsync(_emailSettings.Email, _emailSettings.SenhaDeApp);

        await clienteSmtp.SendAsync(mensagemEmail);
        Console.WriteLine("E-mail de notificação de feedback enviado com sucesso!");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Erro ao enviar o e-mail: {ex.Message}");
      }
      finally
      {
        await clienteSmtp.DisconnectAsync(true);
      }
    }
  }
}
