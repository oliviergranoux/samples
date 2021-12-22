﻿using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace ServiceBus
{
  class Program
  {
    // connection string to your Service Bus namespace
    static string connectionString = "ENDPOINT";

    // name of the Service Bus topic
    static string topicName = "myqueue";

    // name of the subscription to the topic
    // static string subscriptionName = "Azure pour les étudiants"; //<SERVICE BUS - TOPIC SUBSCRIPTION NAME>";

    // the client that owns the connection and can be used to create senders and receivers
    static ServiceBusClient client;

    // the processor that reads and processes messages from the subscription
    static ServiceBusProcessor processor;

    // handle received messages
    static async Task MessageHandler(ProcessMessageEventArgs args)
    {
      string body = args.Message.Body.ToString();
      // Console.WriteLine($"Received: {body} from subscription: {subscriptionName}");
      Console.WriteLine($"Received: {body}");

      // complete the message. messages is deleted from the subscription. 
      await args.CompleteMessageAsync(args.Message);
    }

    // handle any errors when receiving messages
    static Task ErrorHandler(ProcessErrorEventArgs args)
    {
      Console.WriteLine(args.Exception.ToString());
      return Task.CompletedTask;
    }

    static async Task Main()
    {
      // The Service Bus client types are safe to cache and use as a singleton for the lifetime
      // of the application, which is best practice when messages are being published or read
      // regularly.
      //
      // Create the clients that we'll use for sending and processing messages.
      client = new ServiceBusClient(connectionString);

      // create a processor that we can use to process the messages
      processor = client.CreateProcessor(topicName, new ServiceBusProcessorOptions());
      // processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());

      try
      {
        // add handler to process messages
        processor.ProcessMessageAsync += MessageHandler;

        // add handler to process any errors
        processor.ProcessErrorAsync += ErrorHandler;

        // start processing 
        await processor.StartProcessingAsync();

        Console.WriteLine("Wait for a minute and then press any key to end the processing");
        Console.ReadKey();

        // stop processing 
        Console.WriteLine("\nStopping the receiver...");
        await processor.StopProcessingAsync();
        Console.WriteLine("Stopped receiving messages");
      }
      finally
      {
        // Calling DisposeAsync on client types is required to ensure that network
        // resources and other unmanaged objects are properly cleaned up.
        await processor.DisposeAsync();
        await client.DisposeAsync();
      }
    }
  }
}