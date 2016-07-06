using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicles.Domain.SeedWork
{
    public abstract class Entity
    {
        public virtual Guid Id { get; set; }

        public Entity()
        {
            this.GenerateNewIdentity();
        }
        
        public void GenerateNewIdentity()
        {
                this.Id = IdentityGenerator.NewSequentialGuid();
        }

    }
}
