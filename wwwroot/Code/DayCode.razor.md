@code {

    string dayCode = "";

    protected override async Task OnInitializedAsync()
    {
        var pageName = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);

        await Http.GetStringAsync($"Code/{pageName}.razor.md").ContinueWith((str) =>
        {
            dayCode = str.Result;
        });
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            await JS.InvokeVoidAsync("PR.prettyPrint");
        }
    }


}
