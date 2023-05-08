using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Event
{
    public class UpdateModel
    {
        public bool isOpen { get; set; }
    }

    public class UpdateLoadingEvent: PubSubEvent<UpdateModel>
    { }
}
