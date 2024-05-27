using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;




    internal class Account
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        // Create a new BsonElement below:
        [BsonElement("account_id")]
        public string AccountId { get; set; } = String.Empty;
        // Create a new BsonElement below:
        [BsonElement("account_holder")]
        public string AccountHolder { get; set; } = String.Empty;
        // Create a new BsonElement below:
        [BsonElement("account_type")]
        public string AccountType { get; set; } = String.Empty;
        [BsonRepresentation(BsonType.Decimal128)]
        // Create a new BsonElement below:
        [BsonElement("balance")]
        public decimal Balance { get; set; }
        // Create a new BsonElement below:
        [BsonElement("transfers_complete")]
        public string[] TransfersComplete { get; set; } = Array.Empty<string>();
    }
