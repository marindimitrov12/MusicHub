namespace MusicHub
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            // Console.WriteLine(ExportAlbumsInfo(context, 9)); 
            Console.WriteLine( ExportSongsAboveDuration(context,4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var str = new StringBuilder();
            var result = context.Albums.ToArray()
                .Where(x => x.ProducerId == producerId)
                .OrderByDescending(x=>x.Price)
                .Select(x => new
                {
                    Name = x.Name,
                    ReleaeDate = x.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = x.Producer.Name,
                    Songs = x.Songs.ToArray().Select(x => new
                    {
                        SongName = x.Name,
                        Price = x.Price.ToString("f2"),
                        SongWriter = x.Writer.Name
                    }).OrderByDescending(x => x.SongName)
                    .ThenBy(x => x.SongWriter)
                    .ToArray(),
                    TotalPrice =x.Price.ToString("f2")

                }).ToList();
            foreach ( var item in result )
            {
                str.AppendLine($"-AlbumeName: {item.Name}");
                str.AppendLine($"-ReleaseDate:{ item.ReleaeDate}");
                str.AppendLine($"-ProducerName:{item.ProducerName}");
                str.AppendLine("-Songs:");
                int i = 0;
                foreach (var item2 in item.Songs)
                {
                    str.AppendLine($"---#{i++}")
                        .AppendLine($"---SongName:{item2.SongName}")
                        .AppendLine($"---Price:{item2.Price}")
                        .AppendLine($"---Writer:{item2.SongWriter} ");
                }
                str.AppendLine($"---TotalPrice:{item.TotalPrice}");
            }
            return str.ToString();

        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var str =new StringBuilder();
            var result=context.Songs.ToArray()
                .Where(x=>x.Duration.TotalSeconds>duration)
                .Select(x => new
                {
                    Name=x.Name,
                    Writer=x.Writer.Name,
                    Performer=x.SongPerformers.ToArray()
                    .Select(x => $"{x.Performer.FirstName} {x.Performer.LastName}")
                    .FirstOrDefault(),
                    AlbumProducer=x.Album.Producer.Name,
                    Duration=x.Duration.ToString("c")
                }).OrderBy(x=>x.Name).ThenBy(x=>x.Writer).ThenBy(x=>x.Performer).ToList();

            int i = 0;
            foreach (var item in result) 
            {
                str.AppendLine($"-Song #{i++}")
                     .AppendLine($"---SongName:{item.Name}")
                     .AppendLine($"---Writer: {item.Writer}")
                     .AppendLine($"---Performer:{item.Performer}")
                     .AppendLine($"---AlbumeProducer:{item.AlbumProducer}")
                     .AppendLine($"---Duration:{item.Duration}");
            }
            return str.ToString();
        }
    }
}
