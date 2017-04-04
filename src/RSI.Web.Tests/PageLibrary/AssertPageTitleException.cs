namespace RSI.Web.Tests.PageLibrary
{
    public class AssertPageUrlException : AssertionExceptionBase
    {
        string _expected, _actual;
        const string BASIC_MESSAGE = "The page url did not match the expected value.";

        public AssertPageUrlException(string expected, string actual)
            : base(BASIC_MESSAGE)
        {
            _expected = expected;
            _actual = actual;
        }

        protected override string InternalMessage => BASIC_MESSAGE + " Expected: [" + _expected + "], Actual: [" + _actual + "]";
    }
}
