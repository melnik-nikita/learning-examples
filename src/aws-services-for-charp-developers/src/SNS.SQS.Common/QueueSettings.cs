namespace SNS.SQS.Common;

public class QueueSettings
{
    public const string Key = "Queue";
    public string Name { get; set; } = null!;
}

public class TopicSettings
{
    public const string Key = "Topic";
    public string Name { get; set; } = null!;
}
