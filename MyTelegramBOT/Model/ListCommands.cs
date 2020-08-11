using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MyTelegramBOT.Model.Abstractions;
using MyTelegramBOT.Model.Commands;

namespace MyTelegramBOT.Model
{
    public class ListCommands
    {
        private readonly List<Command> _commands;
      
        public ListCommands()
        {
            _commands = new List<Command>()
            {
                new Info(),
                new List(),
                new News(),
                new Subscribe(),
                new Unsubscribe(),
                new UnsubscribeAll()
            };
        }
        public List<Command> Get() => _commands;
    }
}
