//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MusicLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class Song
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Song()
        {
            this.PlayLists = new HashSet<PlayList>();
        }

        private string title;
        private string artistName;
        private int? albumId;
        private int? sequenceId;
        private string description;
        private int? rating;
        private DateTime year;
        private string genre;

        public Song(string title, string artist, int albumId, int sequenceId, string description, string filePath, uint year, string genre, int rating)
        {
            Title = title;
            ArtistName = artist;
            AlbumId = albumId;
            SequenceId = sequenceId;
            Description = description;
            PathToFile = filePath;
            try
            {
                Year = DateTime.ParseExact(year.ToString(), "yyyy", null);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex);
                Year = new DateTime(1900, 01, 01);
            }
            Genre = genre;
            Rating = rating;
        }


        public int Id { get; set; }
        public string Title { get; set; }
        public string ArtistName { get; set; }
        public Nullable<int> AlbumId { get; set; }
        public Nullable<int> SequenceId { get; set; }
        public string PathToFile { get; set; }
        public System.DateTime Year { get; set; }
        public string Genre { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Description { get; set; }
    
        public virtual Album Album { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlayList> PlayLists { get; set; }
    }
}
