using Asp.Versioning;
using ChallangeDotnet.ML.Sentiment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChallangeDotnet.Presentation.Controllers
{
    [Route("api/v{version:apiVersion}/ml")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize] // deixe protegido para manter os pontos de segurança da sprint
    public class MLController : ControllerBase
    {
        private readonly ISentimentService _sentiment;

        public MLController(ISentimentService sentiment)
        {
            _sentiment = sentiment;
        }

        public record SentimentRequest(string Text);

        public record SentimentResponse(bool isPositive, float score, float probability);

        /// <summary>
        /// Classifica sentimento do texto (positivo/negativo) usando ML.NET.
        /// </summary>
        [HttpPost("sentiment")]
        [ProducesResponseType(typeof(SentimentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Analyze([FromBody] SentimentRequest body)
        {
            if (body is null || string.IsNullOrWhiteSpace(body.Text))
                return BadRequest("Informe o campo 'text' com conteúdo.");

            var result = _sentiment.Predict(body.Text);

            return Ok(new SentimentResponse(
                isPositive: result.IsPositive,
                score: result.Score,
                probability: result.Probability
            ));
        }
    }
}
