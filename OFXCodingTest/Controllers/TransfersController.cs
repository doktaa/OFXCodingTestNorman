using Microsoft.AspNetCore.Mvc;
using OFXCodingTest.Models.DTO.Quote;
using OFXCodingTest.Models.DTO.Transfer;
using OFXCodingTest.Services.Quotes;
using OFXCodingTest.Services.Transfers;

namespace OFXCodingTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransfersController : ControllerBase
    {
        private readonly IQuotesService _quotesService;
        private readonly ITransfersService _transfersService;

        public TransfersController(
            IQuotesService quoteService,
            ITransfersService transfersService)
        {
            _quotesService = quoteService;
            _transfersService = transfersService;
        }

        [HttpPost("quote")]
        public async Task<ActionResult<RetrieveQuoteDto>> CreateQuote([FromBody] CreateQuoteDto createQuote)
        {
            try
            {
                var createQuoteResult = await _quotesService.CreateQuote(createQuote);

                return Created("quote", createQuoteResult);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case InvalidDataException _:
                        return BadRequest(ex.Message);
                    case InvalidOperationException _:
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
        }

        [HttpGet("quote/{quoteId}")]
        public async Task<ActionResult<RetrieveQuoteDto>> GetQuote([FromRoute] Guid quoteId)
        {
            try
            {
                var getQuoteResult = await _quotesService.GetQuote(quoteId);

                return Ok(getQuoteResult);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case InvalidOperationException _:
                        return NotFound(ex.Message);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult<RetrieveQuoteDto>> CreateTransfer([FromBody] CreateTransferDto createTransfer)
        {
            try
            {
                var createTransferResult = await _transfersService.CreateTransfer(createTransfer);

                return Created("transfer", createTransferResult);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case InvalidDataException _:
                        return BadRequest(ex.Message);
                    case InvalidOperationException _:
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
        }

        [HttpGet("/{transferId}")]
        public async Task<ActionResult<RetrieveQuoteDto>> GetTransfer([FromRoute] Guid transferId)
        {
            try
            {
                var getTransferResult = await _transfersService.GetTransfer(transferId);

                return Ok(getTransferResult);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case InvalidOperationException _:
                        return NotFound(ex.Message);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
        }
    }
}
