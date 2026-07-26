namespace InaApp.ProyectoInaApp.Controllers
{
    [Serializable]
    internal class NotNumberPositiveException : Exception
    {
        public NotNumberPositiveException()
        {
        }

        public NotNumberPositiveException(string? message) : base(message)
        {
        }

        public NotNumberPositiveException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}