using System;
using System.Collections.Generic;
using System.Text;
using System;
using Apache.NMS.ActiveMQ;
using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;

using Spring.Messaging.Nms;
using Spring.Messaging.Nms.Listener;
using Spring.Messaging.Nms.Core;

namespace Builder.Data
{
    /// <summary>
    /// 监听
    /// </summary>
   class Program

    {

        private const string URI = "tcp://localhost:61616";

        private const string DESTINATION = "test.queue";

        static void Main(string[] args)

        {

            try

            {

                ConnectionFactory connectionFactory = new ConnectionFactory(URI);

                using (SimpleMessageListenerContainer listenerContainer = new SimpleMessageListenerContainer())

                {

                    listenerContainer.ConnectionFactory = connectionFactory;

                    listenerContainer.DestinationName = DESTINATION;

                    listenerContainer.MessageListener = new Listener();

                    listenerContainer.AfterPropertiesSet();

                    Console.WriteLine("Listener started.");

                    Console.WriteLine("Press <ENTER> to exit.");

                    Console.ReadLine();

                }

            }

            catch (Exception ex)

            {

                Console.WriteLine(ex);

                Console.WriteLine("Press <ENTER> to exit.");

                Console.Read();

            }

        }

    }

    class Listener : IMessageListener

    {

        public Listener()

        {

            Console.WriteLine("Listener created.rn");

        }

        #region IMessageListener Members

        public void OnMessage(IMessage message)

        {

            ITextMessage textMessage = message as ITextMessage;

            Console.WriteLine(textMessage.Text);

        }

        #endregion

    }
    /// <summary>
    /// 发送代码
    /// </summary>
    class Program
    {

       static void Main(string[] args)
        {
            IConnectionFactory factory = new ConnectionFactory(new Uri("tcp://localhost:61616"));
            using (IConnection connection = factory.CreateConnection())
            {
                Console.WriteLine("Created a connection!");

                ISession session = connection.CreateSession();

                ActiveMQTopic destination =(ActiveMQTopic) session.GetTopic("DTS");

                IMessageProducer producer = session.CreateProducer(destination);
                producer.DeliveryMode = MsgDeliveryMode.Persistent;

                // lets send a message
                ITextMessage request = session.CreateTextMessage(("DTS_SUCCESS");               

                producer.Send(destination, request);

                // lets consume a message
                //ActiveMQTextMessage message = (ActiveMQTextMessage)consumer.Receive();
                //if (message == null)
                //{
                //    Console.WriteLine("No message received!");
                //}
                //else
                //{
                //    Console.WriteLine("Received message with ID:   " + message.NMSMessageId);
                //    Console.WriteLine("Received message with text: " + message.Text);
                //}
            }

            Console.ReadLine();

        }
    }
}
