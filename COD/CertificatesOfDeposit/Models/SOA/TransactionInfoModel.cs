using Newtonsoft.Json;
using System;

public class TransactionInfoModel
{
    [JsonProperty("clientCode")]
    public string ClientCode { get; set; }
    [JsonProperty("cRefNum")]
    public string CRefNum { get; set; }

    [JsonProperty("transactionStartTime")]
    public string TransactionStartTime { get; set; }

    [JsonProperty("transactionCompletedTime")]
    public string TransactionCompletedTime { get; set; }

    [JsonProperty("transactionReturn")]
    public long TransactionReturn { get; set; }

    [JsonProperty("transactionReturnMsg")]
    public string TransactionReturnMsg { get; set; }

    [JsonProperty("branchInfo")]
    public BranchInfoModel BranchInfo { get; set; }

    [JsonProperty("transactionValue")]
    public string TransactionValue { get; set; }

    [JsonProperty("transactionBatchID")]
    public string TransactionBatchId { get; set; }

    [JsonProperty("transactionErrorCode")]
    public string TransactionErrorCode { get; set; }
    [JsonProperty("transactionErrorMsg")]
    public string TransactionErrorMsg { get; set; }
}

public class BranchInfoModel
{
    [JsonProperty("branchCode")]
    public string BranchCode { get; set; }
}