using System.Net;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;

var s3Client = new AmazonS3Client();

// await UploadFileAsync(s3Client);

await DownloadFileAsync(s3Client);

async Task UploadFileAsync(AmazonS3Client amazonS3Client)
{
    await using var inputStream = new FileStream("./fuji.jpg", FileMode.Open, FileAccess.Read);

    var putObjectRequest = new PutObjectRequest
    {
        BucketName = "melnn1k", Key = "images/fuji.jpg", ContentType = "image/jpeg", InputStream = inputStream
    };

    await amazonS3Client.PutObjectAsync(putObjectRequest);
}

async Task DownloadFileAsync(AmazonS3Client amazonS3Client)
{
    var getObjectRequest = new GetObjectRequest { BucketName = "melnn1k", Key = "images/fuji.jpg" };

    var objectResponse = await amazonS3Client.GetObjectAsync(getObjectRequest);

    using var memoryStream = new MemoryStream();
    await objectResponse.ResponseStream.CopyToAsync(memoryStream);
    var text = Encoding.Default.GetString(memoryStream.ToArray());
    Console.WriteLine(text);
}

async Task DeleteFileAsync(AmazonS3Client amazonS3Client)
{
    var getObjectRequest = new DeleteObjectRequest() { BucketName = "melnn1k", Key = "images/fuji.jpg" };

    var objectResponse = await amazonS3Client.DeleteObjectAsync(getObjectRequest);

    Console.WriteLine(
        objectResponse.HttpStatusCode == HttpStatusCode.OK ? "Deleted" : $"Failed to delete {objectResponse.ResponseMetadata}"
    );
}
