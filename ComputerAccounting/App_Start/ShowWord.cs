using Microsoft.Office.Interop.Word;

namespace ComputerAccounting.App_Start
{
    public class ShowWord
    {
        public static void ReplaceWordStub(string stubToReplace, string text, Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }

    }
}
