using AwesomeDevEvents.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEvents.API.Persistence
{
    public class DevEventsDbContext : DbContext
    {
        public DevEventsDbContext(DbContextOptions<DevEventsDbContext> options) : base(options) 
        {
            //DevEvents = new List<DevEvent>(); Criando BD em Memoria
        }
        //public List<DevEvent> DevEvents { get; set; }
        public DbSet<DevEvent> DevEvents { get; set; }
        public DbSet<DevEventSpeaker> DevEventSpeakers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Dizendo que meus fields ID's de ambas tabelas serão PK(Primary Key)
            modelBuilder.Entity<DevEvent>( e => 
            {
                e.HasKey(e => e.Id);

                e.Property(de => de.Title).IsRequired(false);

                e.Property( de => de.Description )
                                .HasMaxLength(200)
                                .HasColumnType("varchar(200)");

                e.Property(de => de.StartDate)
                        .HasColumnName("Start_Date");

                e.Property(de => de.EndDate)
                        .HasColumnName("End_Date");

                e.HasMany(de => de.Speakers)
                        //.WithOne(s => s.Event)
                        .WithOne()
                        .HasForeignKey(s => s.DevEventId);


            });

            modelBuilder.Entity<DevEventSpeaker>( e => 
            {
                e.HasKey(de => de.Id);
            });


        }
    }
}
