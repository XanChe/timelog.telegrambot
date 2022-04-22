using System;


namespace Timelog.Core.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; set; }        
        public Guid UserUniqId { get; set; }
        public Entity()
        {
            //Id = Guid.NewGuid();
        }      
    }
}

