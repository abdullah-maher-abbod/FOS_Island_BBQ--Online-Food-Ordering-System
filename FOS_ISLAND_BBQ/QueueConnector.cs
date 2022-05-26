using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOS_ISLAND_BBQ
{
    public class QueueConnector
    {
        //create agent object to do communication - handle client request side.
        public static QueueClient OrderQueueAgent;
        //identify your service bus account
        public const string ServiceBusNameSpace = "servicebustp047829";
        //identify your queue Name (name decide by you)
        public const string QueueName = "OrdersQueue";

        public static NamespaceManager createNamespaceManager()
        {
            //come together with access token key
            var url = ServiceBusEnvironment.CreateServiceUri("sb", ServiceBusNameSpace, String.Empty);
            var tp = TokenProvider.CreateSharedAccessSignatureTokenProvider(
            "RootManageSharedAccessKey", "PROdTGBuTjeiRovhW+cdMnpb32guU6YXd+yYp+hxQH0=");
            return new NamespaceManager(url, tp);
        }
        public static void Initialize() // how to make a queue and linked to a queue
        {
            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Http;
            var namespaceManager = createNamespaceManager();
            if (!namespaceManager.QueueExists(QueueName)) // queue is not yet exist
            {
                namespaceManager.CreateQueue(QueueName);
            }
            var messagingfactory =
            MessagingFactory.Create(namespaceManager.Address, namespaceManager.Settings.TokenProvider);
            OrderQueueAgent = messagingfactory.CreateQueueClient(QueueName);
        }
    }
}
