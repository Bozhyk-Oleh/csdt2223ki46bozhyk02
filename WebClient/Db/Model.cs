using System.ComponentModel.DataAnnotations;

namespace WebClient.Db
{
    public class Model
    {
        [Key]
        public int IdData { get; set; }

        public string InData { get; set; }

        public string OutData { get; set; }

        public DateTime InDateTime { get; set; }

        public DateTime? OutDateTime { get; set; }

        public bool isSended { get; set; }
    }
}
