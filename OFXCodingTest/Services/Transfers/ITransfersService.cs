using OFXCodingTest.Models.DTO.Transfer;

namespace OFXCodingTest.Services.Transfers
{
    public interface ITransfersService
    {
        public Task<RetrieveTransferDto> CreateTransfer(CreateTransferDto createTransferDto);
        public Task<RetrieveTransferDto> GetTransfer(Guid transferId);
    }
}
