using Microsoft.AspNetCore.Components;

namespace BlazorChess.Pages.dialogs;

public class DialogBase : ComponentBase
{
    protected string _modalShow = string.Empty;
    protected string _modalDisplay = "none;";
    protected bool ShowBackdrop = false;

    public void Open()
    {
        _modalShow = "show";
        _modalDisplay = "block;";
        StateHasChanged();
    }

    public void Close()
    {
        _modalShow = string.Empty;
        _modalDisplay = "none";
        StateHasChanged();
    }
}
