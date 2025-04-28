using Microsoft.AspNetCore.Components;

namespace Developers.Stream.Components.Controls;

public partial class DynamicTags : ComponentBase
{
    [Parameter] public List<string> Tags { get; set; } = [];
    private bool InputIsVisible { get; set; }
    private string Input { get; set; } = string.Empty;

    void OnClose(string item)
    {
        Tags.Remove(item);
    }

    void HandleInputConfirm()
    {
        if (string.IsNullOrEmpty(Input))
        {
            CancelInput();

            return;
        }

        string res = Tags.Find(s => s == Input);

        if (string.IsNullOrEmpty(res))
        {
            Tags.Add(Input);
        }

        CancelInput();
    }

    void CancelInput()
    {
        Input = string.Empty;
        InputIsVisible = false;
    }
}