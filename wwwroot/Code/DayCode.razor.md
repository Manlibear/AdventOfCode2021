@code {

    string dayCode = "";

    protected override async Task OnInitializedAsync()
    {
        dayCode = await Http.GetStringAsync("Code/Day1.razor.md");
        //TODO: Hacky crap to get the code files to display properly, but who cares
        dayCode = dayCode.Replace("\n", "<br />");
        dayCode = dayCode.Replace("\t", "&#8195;&#8195;&#8195;&#8195;");
        dayCode = dayCode.Replace("    ", "&#8195;&#8195;&#8195;&#8195;");

    }
}
