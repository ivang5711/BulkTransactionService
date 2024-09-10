using BulkTransactionServiceWebApi.Domain;
using BulkTransactionServiceWebApi.Persistence.Repositories;

namespace BulkTransactionServiceWebApi.Services;

// FIXME: is this layer needed in this kind of application? Or could have been omitted?
public sealed class PaymentService(PaymentRepository salaryRepository)
{
    private readonly PaymentRepository _salaryRepository = salaryRepository;

    public async Task<bool> PaySalaryAsync(BulkTransferRequestDomain salaryTransferRequest)
    {
        return await _salaryRepository.PerformPaymentAsync(salaryTransferRequest);
    }
}
