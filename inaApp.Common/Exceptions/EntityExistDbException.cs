namespace InaApp.ProyectoInaApp.Controllers
{
    [Serializable]
    internal class EntityExistDbException : Exception
    {
        public EntityExistDbException()
        {
        }

        public EntityExistDbException(string? message) : base(message)
        {
        }

        public EntityExistDbException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}