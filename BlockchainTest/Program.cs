using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

using System;
using System.Numerics;

namespace BlockchainTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var account = new Account("31ca3c048a95704988ea15914a6649cd85a2b1a99f79203ce9a42dec4394d403", 1337);
            var web3 = new Web3(account, "http://btest.ddns.net:7545");
            var gasPrice = Web3.Convert.ToWei(1.5, UnitConversion.EthUnit.Gwei);

            var balance = web3.Eth.GetBalance.SendRequestAsync(account.Address).Result;
            Console.WriteLine($"BALANCE: {Web3.Convert.FromWei(balance)}");
            
            var amount = new HexBigInteger(50000000000000000);
            var toETH = Web3.Convert.FromWei(amount);

            var transactionInput = new TransactionInput
            {
                From = account.Address,
                GasPrice = new HexBigInteger(gasPrice),
                To = "0xf160c948ec41c4c4d88770c6aCD804e31034e306",
                Value = amount,
            };

            var transactionHash = web3.Eth.TransactionManager
                .SendTransactionAndWaitForReceiptAsync(transactionInput, null)
                .Result;

            if(transactionHash.Status.Value == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Transaction ID: {transactionHash.TransactionHash} | Amount: {toETH}ETH");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Transaction Failed!");
                Console.ResetColor();
            }

            //Console.WriteLine(response);
        }
    }
}
