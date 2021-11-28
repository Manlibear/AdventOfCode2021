@code {

    string dayCode = "";

    protected override async Task OnInitializedAsync()
    {
        dayCode = await Http.GetStringAsync("Code/Day1.razor.md");
        dayCode = dayCode.Replace("\n", "<br />");
        dayCode = dayCode.Replace("\t", "&#8195;&#8195;&#8195;&#8195;");

    }
}
