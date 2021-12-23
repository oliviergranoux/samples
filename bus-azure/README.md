[Home page of sample projects](https://github.com/oliviergranoux/samples)

# Running solution

1. create the queue/topic on azure.
2. set configuration (connectionString/endpoint, name of queue/topic, and subscription name on topic)
3. run both projects. Receiver will stay alive while you do not stop it; Sender will run only the time to send an batch of messages

```csharp
dotnet run --project Sender
dotnet run --project Receiver
```

# Useful links

* https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues