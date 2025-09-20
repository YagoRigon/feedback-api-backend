using FeedbackApi.Data; 
using FeedbackApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace FeedbackApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class FeedbacksController : ControllerBase
  {
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly EmailService _emailService;

    public FeedbacksController(ApplicationDbContext context, IConfiguration configuration, EmailService emailService)
    {
      _context = context;
      _configuration = configuration;
      _emailService = emailService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Feedback>> Get()
    {
      return Ok(_context.Feedbacks.ToList());
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] Feedback novoFeedback)
    {
      _context.Feedbacks.Add(novoFeedback);
      await _context.SaveChangesAsync();

      await _emailService.EnviarEmailDeFeedback(novoFeedback);

      return CreatedAtAction(nameof(Get), new { id = novoFeedback.Id }, novoFeedback);
    }
  }
}
