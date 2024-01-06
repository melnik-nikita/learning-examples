using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynamoDb.Transactions;

var movieByYear = new MovieByYear()
{
    Id = Guid.NewGuid(),
    Title = "21 Jump Street",
    AgeRestriction = 18,
    ReleaseYear = 2012,
    RottenTomatoesPercentage = 85
};

var movieByTitle = new MovieByTitle()
{
    Id = Guid.NewGuid(),
    Title = "21 Jump Street",
    AgeRestriction = 18,
    ReleaseYear = 2012,
    RottenTomatoesPercentage = 85
};

var asJson = JsonSerializer.Serialize(movieByYear);
var attributeMap = Document.FromJson(asJson).ToAttributeMap();

var asJson1 = JsonSerializer.Serialize(movieByTitle);
var attributeMap1 = Document.FromJson(asJson1).ToAttributeMap();

var transactionRequest = new TransactWriteItemsRequest()
{
    TransactItems = new List<TransactWriteItem>()
    {
        new () { Put = new Put() { TableName = "movies-year-title", Item = attributeMap } },
        new () { Put = new Put() { TableName = "movies-title-rotten", Item = attributeMap1 } }
    }
};

var dynamoDbClient = new AmazonDynamoDBClient();

var response = await dynamoDbClient.TransactWriteItemsAsync(transactionRequest);
