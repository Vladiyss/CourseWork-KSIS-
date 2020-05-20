using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSide
{
    class AllDialogsMessages
    {
        public List<string> Messages;
        public string Name;

        public AllDialogsMessages(string name)
        {
            Name = name;
            Messages = new List<string>();
        }
        public void AddMessage(string messageContent)
        {
            Messages.Add(messageContent);
        }
    }
}

