using System;
using System.Runtime.Serialization;

namespace Vehicles.Domain.SeedWork
{
    [DataContract]
    public abstract class Entity
    {
        [DataMember]
        public virtual Guid Id { get; set; }

        public Entity()
        {
        }
        
        public void GenerateNewIdentity()
        {
            this.Id = IdentityGenerator.NewSequentialGuid();
        }

    }
}
