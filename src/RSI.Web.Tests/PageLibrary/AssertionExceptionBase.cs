namespace RSI.Web.Tests.PageLibrary
{
    // TODO modify assertion exception logic to new model - originally this an inheritance model but nunit appears to do direct 
    // type matching
    public class AssertionExceptionBase : System.Exception
    {

        public AssertionExceptionBase(string basicMessage) { }


        public override string ToString()
        {
            return InternalMessage;
        }

        protected virtual string InternalMessage { get; set; }
    }
}
