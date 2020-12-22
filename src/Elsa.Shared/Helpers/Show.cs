using System;
using System.Threading.Tasks;

namespace Elsa.Shared
{
    public class Show
    {

        public Show(Func<Task> task, string description)
        {
            Task = task;
            Description = description;
        }

        public string Description { get; }

        public Func<Task> Task { get; }
    }

}