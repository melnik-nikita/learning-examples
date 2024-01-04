# Cloud Fundamentals: AWS Services for C# Developers

## AWS SQS

Amazon SQS provides queues for high-throughput, system-to-system messaging. You can use queues to decouple heavyweight
processes and to buffer and batch work. Amazon SQS stores messages until microservices and serverless applications
process them.
![How AWS SQS works](./img/how-aws-sqs-works.svg)
Amazon SQS allows producers to send messages to a queue. Messages are then stored in an SQS Queue. When consumers are
ready to process new messages they poll them from the queue. Applications, microservices, and multiple AWS services can
take the role of producers or consumers.
Queue types:

- Standard - support nearly unlimited transactions per second. A message is delivered at least once, but occasionally
  more that one copy of a message is delivered. Standard queues provide best-effort ordering. Occasionally, messages
  might be delivered in an order different from which they were sent.
- FIFO - support up to 3000 messages per second, each message is delivered eactly once, and message order is preserved.
  FIFO queues are designed to enhance messaging between applications when the order of operations and events is
  critical, or where duplicates can't be tolerated.

A message can have such configuration:

- Visibility timeout - sets the length of time that a message received from a queue (by one consumer) will not be
  visible to the other message consumers.
- Message retention period - the amount of time that Amazon SQS retains a message that does not get deleted.
- Delivery delay - the amount of time to delay the first delivery of each message added to the queue.
- Maximum message size - up to 256kb
- Receive message wait time - is the maximum amount of time that polling will wait for messages to become available to
  receive.

## AWS SNS (Simple Notification Service)

Amazon SNS is a highly available, durable, secure, fully managed pub/sub messaging service that enables you to decouple
microservices, distributed systems, and event-driven serverless applications. Amazon SNS provides topics for
high-throughput, push-based, many-to-many messaging.
![How AWS SNS works](./img/how-aws-sns-works.png)

SNS Topic types:

- FIFO
    - Strictly-preserved message ordering
    - Exactly-once message delivery
    - High throughput, up to 300 publishes/second
    - Subscription protocols: SQS
- Standard
    - Best-effort message ordering
    - At-least once message delivery
    - Highest throughput in publishes/second
    - Subscription protocols: SQS, Lambda, HTTP, SMS, email, mobile application endpoints