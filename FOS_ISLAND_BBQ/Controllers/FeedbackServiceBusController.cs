using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Http.Features;
using FOS_ISLAND_BBQ.Models;
using Newtonsoft.Json;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.AspNetCore.Authorization;

namespace FOS_ISLAND_BBQ
{
    public class FeedbackServiceBusController : Controller
    {
        const string ServiceBusConnectionString = "Endpoint=sb://servicebustp047829.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=PROdTGBuTjeiRovhW+cdMnpb32guU6YXd+yYp+hxQH0=";
        const string QueueName = "ordersqueue";
        static IQueueClient queueClient;
        static List<string> items;
        static string item1;
        public async Task<ActionResult> submitefeedbackAsync()
        {
            var managementClient = new ManagementClient(ServiceBusConnectionString);
            var queue = await managementClient.GetQueueRuntimeInfoAsync(QueueName);
            ViewBag.count = queue.MessageCount;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> submitefeedbackAsync(string id, string name, string email, string feedback)
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            try
            {
                // Create a new message to send to the queue.
                string messageBody = $" { $"{name}," + $"{email}," + $"{feedback}" }";

                var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                // Write the body of the message to the console.
                Console.WriteLine($"Sending message: {messageBody}");

                // Send the message to the queue.
                await queueClient.SendAsync(message);
                ViewBag.msg = "success";

            }
            catch (Exception exception)
            {
                ViewBag.msg = exception.ToString();
            }
            Thread.Sleep(1500);
            return RedirectToAction("submitefeedbackAsync");
        }

        //Part 1: Send Message to the Service Bus
        public async Task<IActionResult> Index()
        {

            return View();
        }
        [Authorize(Roles = "Admin")]
        //Part 2: Received Message from the Service Bus - cal get data function
        public async Task<IActionResult> ProcessMsg()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName, ReceiveMode.PeekLock);
            items = new List<string>();
            await Task.Factory.StartNew(() =>
            {
                queueClient = new QueueClient(ServiceBusConnectionString, QueueName, ReceiveMode.PeekLock);
                var options = new MessageHandlerOptions(ExceptionMethod)
                {
                    MaxConcurrentCalls = 1,
                    AutoComplete = false
                };
                queueClient.RegisterMessageHandler(ExecuteMessageProcessing, options);
            });

            return RedirectToAction("ProcessMsgResult");
        }

        //Part 2: Received Message from the Service Bus - get data step
        private static async Task ExecuteMessageProcessing(Message message, CancellationToken arg2)
        {
            //var result = JsonConvert.DeserializeObject<Feedback>(Encoding.UTF8.GetString(message.Body));
            // Console.WriteLine($"Order Id is {result.name}, Order name is {result.email} and quantity is {result.feedback}");
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
            items.Add(Encoding.UTF8.GetString(message.Body));
            string messagesplit = Encoding.UTF8.GetString(message.Body);


        }

        //Part 2: Received Message from the Service Bus
        private static async Task ExceptionMethod(ExceptionReceivedEventArgs arg)
        {
            await Task.Run(() =>
           Console.WriteLine($"Error occured. Error is {arg.Exception.Message}")
           );
        }

        //Part 2: Received Message from the Service Bus - Display step
        //however, there is a bug where you have to reload the page for second time only can see the result.
        public IActionResult ProcessMsgResult()
        {
            return View(items);
        }
    }
}
