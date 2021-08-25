using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Infrastructure.Data.Config
{
    public class OutboxMessage
    {
        /// <summary>
        /// Id of message.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Occurred on.
        /// </summary>
        public DateTime OccurredOn { get; private set; }

        /// <summary>
        /// Full name of message type.
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Message data - serialzed to JSON.
        /// </summary>
        public string Data { get; private set; }

        private OutboxMessage()
        {

        }

        internal OutboxMessage(DateTime occurredOn, string type, string data)
        {
            this.Id = Guid.NewGuid();
            this.OccurredOn = occurredOn;
            this.Type = type;
            this.Data = data;
        }
    }
    internal  class OutBoxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages");

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();
        }
    }

}
