using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace CommonInformation
{
    public class MessageSerializer
    {
        public byte[] Serialize(Message message)
        {
            var serializer = new XmlSerializer(typeof(Message));
            var messageContainer = new MemoryStream();
            serializer.Serialize(messageContainer, message);
            return messageContainer.GetBuffer();
        }

        public Message Deserialize(byte[] data, int amount)
        {
            var serializer = new XmlSerializer(typeof(Message));
            var messageContainer = new MemoryStream();
            messageContainer.Write(data, 0, amount);
            messageContainer.Position = 0;
            Message message = (Message)serializer.Deserialize(messageContainer);
            return message;
        }
    }
}

