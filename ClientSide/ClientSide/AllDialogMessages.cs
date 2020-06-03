using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSide
{
    class AllDialogsMessages
    {
        public List<string> messages;
        public string dialogName;

        public AllDialogsMessages(string name)
        {
            dialogName = name;
            messages = new List<string>();
        }
        public void AddMessage(string messageContent)
        {
            messages.Add(messageContent);
        }
    }
}

