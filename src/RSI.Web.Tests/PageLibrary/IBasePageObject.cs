using System;

namespace RSI.Web.Tests.PageLibrary
{
    public interface IBasePageObject
    {
        string BaseUrl { get; }
        string DefaultTitle { get; }
        string Url { get; }
        string QueryString { get; }
        Func<bool> CustomPageValidation { get; set; }

        void AssertIs<TPage>() where TPage : BasePageObject, new();
    }
}