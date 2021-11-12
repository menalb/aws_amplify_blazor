using System.Collections.Generic;
using System.Threading.Tasks;
using ReceiptApp.Model;

namespace ReceiptApp.Services
{
    public class ReceiptCommand
    {
        private const string receipts_url = "receipts";
        private readonly ReceiptApi _api;
        public ReceiptCommand(ReceiptApi api)
        {
            _api = api;
        }

        public async Task Save(string receiptId, ReceiptDetails receipt)
        {
            await _api.Put<ReceiptDetails>($"{receipts_url}/receipt/{receiptId}", receipt);         
        }
    }

    public class ReceiptQuery
    {
        private const string receipts_url = "receipts";
        private readonly ReceiptApi _api;
        public ReceiptQuery(ReceiptApi api)
        {
            _api = api;
        }

        public async Task<IEnumerable<ReceiptSummary>> GetAll()
        {
            return (await _api.Get<ReceiptsResult>(receipts_url)).Receipts;
        }

        public async Task<ReceiptDetails> GetReceipt(string receiptId)
        {
            return (await _api.Get<ReceiptDetailsResult>($"{receipts_url}/receipt/{receiptId}")).Receipt;
        }
    }

     public class JobsQuery
    {
        private const string jobsUrl = "jobs";
        private readonly ReceiptApi _api;
        public JobsQuery(ReceiptApi api)
        {
            _api = api;
        }

        public async Task<IEnumerable<Model.Job>> GetAll()
        {
            return (await _api.Get<JobsResult>(jobsUrl)).Jobs;
        }
    }
}