namespace HomeHub.Service.Web.Models.Manufacturer
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// A lifx account link
    /// </summary>
    public class LifxAccount
    {
        /// <summary>
        /// The application key
        /// </summary>
        [Required]
        [MinLength(10)]
        // ReSharper disable once StyleCop.SA1623
        public string AppKey { get; set; }
    }
}