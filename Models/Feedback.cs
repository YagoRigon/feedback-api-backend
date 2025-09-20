namespace FeedbackApi.Models
{
  public class Feedback
  {
    public int Id { get; set; } // Identificador único
    public string? Nome { get; set; }
    public string? Mensagem { get; set; }
    public int Estrelas { get; set; }

    public string? Email{ get; set; }
  }
}
