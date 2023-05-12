using OnlineStoreForWoman.DTOs;
using OnlineStoreForWoman.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineStoreForWoman.Data;

public interface IPayRepository
{
    Task<List<PayOptionDto>> GetPaymentOptions(int userId);
    Task<Response<TransactionDto, TransactionContext>> GetTransactions(int userId, BaseParams @params);
    Task TransferAmount(int userId, TransferDto transfer);
}