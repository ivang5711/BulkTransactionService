using BulkTransactionServiceWebApi.Domain;
using BulkTransactionServiceWebApi.Persistence.Repositories;

namespace BulkTransactionServiceWebApi.Services;

public sealed class PaymentService(PaymentRepository salaryRepository)
{
    private readonly PaymentRepository _salaryRepository = salaryRepository;

    public async Task<bool> PaySalaryAsync(BulkTransferRequestDomain salaryTransferRequest)
    {
        return await _salaryRepository.PerformPaymentAsync(salaryTransferRequest);
    }
}
