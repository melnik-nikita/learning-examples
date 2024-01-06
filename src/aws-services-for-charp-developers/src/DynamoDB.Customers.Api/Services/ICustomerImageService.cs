using Amazon.S3;
using Amazon.S3.Model;

namespace DynamoDB.Customers.Api.Services;

public interface ICustomerImageService
{
    Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file);
    Task<GetObjectResponse> GetImageAsync(Guid id);
    Task<DeleteObjectResponse> DeleteImageAsync(Guid id);
}

public class CustomerImageService : ICustomerImageService
{
    private readonly IAmazonS3 _s3;
    private readonly string _bucketName = "melnn1k";

    public CustomerImageService(IAmazonS3 s3)
    {
        _s3 = s3;
    }

    /// <inheritdoc />
    public async Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file)
    {
        var putObjectRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = $"images/{id}",
            ContentType = file.ContentType,
            InputStream = file.OpenReadStream(),
            Metadata = { ["x-amz-meta-originalname"] = file.FileName, ["x-amz-meta-extension"] = Path.GetExtension(file.FileName) }
        };

        return await _s3.PutObjectAsync(putObjectRequest);
    }

    /// <inheritdoc />
    public async Task<GetObjectResponse> GetImageAsync(Guid id)
    {
        var getObjectRequest = new GetObjectRequest() { BucketName = _bucketName, Key = $"images/{id}" };
        return await _s3.GetObjectAsync(getObjectRequest);
    }

    /// <inheritdoc />
    public async Task<DeleteObjectResponse> DeleteImageAsync(Guid id)
    {
        var deleteObjectRequest = new DeleteObjectRequest { BucketName = _bucketName, Key = $"images/{id}" };

        return await _s3.DeleteObjectAsync(deleteObjectRequest);
    }
}
