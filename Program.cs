using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Principal;



string pass = "atlasPass";
string connectionUri = $"mongodb+srv://myAtlasDBUser:{pass}@myatlasclusteredu.luqajyq.mongodb.net/?retryWrites=true&w=majority&appName=myAtlasClusterEDU";


var client = new MongoClient(connectionUri);

var dataBase = client.GetDatabase("Bank");
var accountsCollection = dataBase.GetCollection<Account>("account");

using (var session = await client.StartSessionAsync())
{
    var result = await session.WithTransactionAsync(async (s, ct) =>
    {
        try
        {
            var fromid = "abc";
            var toid = "abc1";
            var amount = 100;

            var fromAccount = await accountsCollection.Find(s, Builders<Account>.Filter.Eq(a => a.AccountId, fromid)).FirstOrDefaultAsync(ct);
            var toAccount = await accountsCollection.Find(s, Builders<Account>.Filter.Eq(a => a.AccountId, toid)).FirstOrDefaultAsync(ct);

            if (fromAccount == null || toAccount == null)
            {
                Console.WriteLine("One or both accounts not found");
                return false;
            }

            if (fromAccount.Balance < amount)
            {
                Console.WriteLine("Insufficient balance in the source account");
                return false;
            }

            var fromAccountFilter = Builders<Account>.Filter.Eq(a => a.AccountId, fromid);
            var toAccountFilter = Builders<Account>.Filter.Eq(a => a.AccountId, toid);

            var fromAccountUpdateBalance = Builders<Account>.Update.Inc(a => a.Balance, -amount);
            var toAccountUpdateBalance = Builders<Account>.Update.Inc(a => a.Balance, amount);

            await accountsCollection.UpdateOneAsync(s, fromAccountFilter, fromAccountUpdateBalance, cancellationToken: ct);
            await accountsCollection.UpdateOneAsync(s, toAccountFilter, toAccountUpdateBalance, cancellationToken: ct);

            Console.WriteLine("Transferring Money");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    });

    if (result)
    {
        Console.WriteLine("Transaction committed");
    }
    else
    {
        Console.WriteLine("Transaction aborted");
    }
}

