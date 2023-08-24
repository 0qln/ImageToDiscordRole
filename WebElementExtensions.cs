
using OpenQA.Selenium;

public static class WebElementExtensions
{
    public static readonly string CustomAttribute = "data-element-exists-check";


    public static void MarkForIDExistenceCheck(this IWebElement element)
    {
        element?.SetAttribute("id", CustomAttribute);
    }

    public static void MarkForExistenceCheck(this IWebElement element)
    {
        element?.SetAttribute(CustomAttribute, "true");
    }

    public static bool DoesElementStillExist(this IWebElement element)
    {
        if (element == null)
            return false;

        string attributeValue = element.GetAttribute(CustomAttribute);
        return attributeValue == "true";
    }
    
    public static void SetAttribute(this IWebElement element, string name, string value)
    {
        var js = (IJavaScriptExecutor)element;
        js.ExecuteScript($"arguments[0].setAttribute('{name}', '{value}');", element);
    }
}
