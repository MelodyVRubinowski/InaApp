namespace InaApp.ProyectoInaApp.Controllers
{
    [Serializable]
    internal class NotFoundDbException : Exception
    {
        public NotFoundDbException()
        {
        }

        public NotFoundDbException(string? message) : base(message)
        {
        }

        public NotFoundDbException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}