using System;

namespace LogBookTask.Abstracts
{
    public abstract class Id
    {
        public Guid Guid { get; set; }

        protected Id()
        {
            Guid = Guid.NewGuid();
        }
    }
}