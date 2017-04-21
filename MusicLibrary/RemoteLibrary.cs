using System.ComponentModel.DataAnnotations;

namespace MusicLibrary

{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class RemoteLibrary : DbContext
    {
        // Your context has been configured to use a 'Model1' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'EFOne.Model1' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Model1' 
        // connection string in the application configuration file.
        public RemoteLibrary()
            : base("name=RemoteLibraryEntities")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Song> ListMusicLibrary { get; set; }
        public virtual DbSet<PlayList> PList { get; set; }
        public virtual DbSet<Album> Albums { get; set; }
    }

    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
