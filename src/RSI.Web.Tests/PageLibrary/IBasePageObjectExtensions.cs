namespace RSI.Web.Tests.PageLibrary
{
    public static class IBasePageObjectExtensions
    {
        /// <summary>
        /// Return true if it's the good type else false
        /// </summary>
        /// <typeparam name="TPage"></typeparam>
        /// <returns></returns>
        public static bool Is<TPage>(this IBasePageObject thisPage) where TPage : IBasePageObject
        {
            return thisPage is TPage;
        }

        /// <summary>
        /// Inline cast to the given page type
        /// </summary>
        public static TPage As<TPage>(this IBasePageObject thisPage) where TPage : IBasePageObject
        {
            return (TPage)thisPage;
        }
    }
}
