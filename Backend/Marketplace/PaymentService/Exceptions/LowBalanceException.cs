namespace PaymentService.Exceptions
{
    public class LowBalanceException : Exception
    {
        public LowBalanceException() { }

        public LowBalanceException(string message) : base(message) { }

        public LowBalanceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
