using System.Data;
using BulkTransactionServiceWebApi.Domain;
using BulkTransactionServiceWebApi.Persistence.Database;
using Dapper;

namespace BulkTransactionServiceWebApi.Persistence.Repositories;

public sealed class PaymentRepository(IDbConnectionFactory dbConnectionFactory)
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<bool> PerformPaymentAsync(BulkTransferRequestDomain salaryPaymentRequest)
    {
        using (IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync())
        {
            var accountIdQuery =
                @"SELECT
                      id
                  FROM
                      bank_accounts
                  WHERE
                      iban = @AccountIban;";
            var organizationAccountId = await connection.QueryAsync<int>(
                accountIdQuery,
                new { AccountIban = salaryPaymentRequest.OrganizationIban }
            );
            var accountId = organizationAccountId.First();
            AddAccountIdToTransaction(salaryPaymentRequest, accountId);
            var totalRequestedAmount = GetPaymentRequestTotalAmount(salaryPaymentRequest);
            using (var tran = connection.BeginTransaction())
            {
                var checkQuery =
                    @"CALL 
                          CheckAvailableFundsForTheTransaction(
                              @ExpenseAmount, 
                              @AccountIban);";
                IEnumerable<int> resultCheck = [];
                try
                {
                    resultCheck = await connection.QueryAsync<int>(
                        checkQuery,
                        new
                        {
                            ExpenseAmount = totalRequestedAmount,
                            AccountIban = salaryPaymentRequest.OrganizationIban,
                        }
                    );
                }
                catch
                {
                    connection.Close();
                    return false;
                }

                var insertTransactionQuery =
                    @"INSERT INTO
                          transfers (
                              counterparty_name,
                              counterparty_iban,
                              counterparty_bic,
                              amount_cents,
                              bank_account_id,
                              description
                          )
                      VALUES
                          (
                              @CounterpartyName,
                              @CounterpartyIban,
                              @CounterpartyBic,
                              @AmountDigit,
                              @BankAccountId,
                              @Description
                          );";
                var transfers = salaryPaymentRequest.CreditTransfers;
                connection.Execute(insertTransactionQuery, transfers);
                tran.Commit();
            }

            return true;
        }
    }

    private static void AddAccountIdToTransaction(
        BulkTransferRequestDomain salaryPaymentRequest,
        int accountId
    )
    {
        foreach (var item in salaryPaymentRequest.CreditTransfers!)
        {
            item.BankAccountId = accountId;
        }
    }

    private static int GetPaymentRequestTotalAmount(BulkTransferRequestDomain salaryPaymentRequest)
    {
        var totalAmount = 0;
        foreach (var item in salaryPaymentRequest.CreditTransfers!)
        {
            item.AmountDigit = (int)(double.Parse(item.Amount!.Trim()) * 100);
            totalAmount += item.AmountDigit;
        }

        return totalAmount;
    }
}
