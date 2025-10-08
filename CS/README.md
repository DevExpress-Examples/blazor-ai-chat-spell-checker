<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/987706933/24.2.6%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1292610)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
# DevExpress Blazor Grid - ExpandoObject Collection Support

The [DevExpress Blazor Grid's](https://docs.devexpress.com/Blazor/403143/components/grid) ability to create, modify, and delete rows extends to dynamic data sources. When bound to [ExpandoObject](https://learn.microsoft.com/en-us/dotnet/api/system.dynamic.expandoobject) collections, our Blazor grid can use user-defined schemas from sources such as JSON files or NoSQL databases. This, in turn, allows you to introduce CRUD (Create, Read, Update, Delete) operations if your data structure is not defined at compile time.

This example illustrates how you can add a fully editable DevExpress Blazor Grid (`DxGrid`) bound to a dynamic `ExpandoObject` list in your next great Blazor app.

![Edit ExpandoObject Data in DxGrid](images/grid-edit-expandoobject.gif)

## Implementation Details

Create a `ExpandoObject` collection to store dynamic data. Populate it with initial entries.

```cs
private List<ExpandoObject>? forecasts;

protected override async Task OnInitializedAsync() {
    forecasts = await ForecastService.GetForecastAsyncExpando(DateTime.Now);
}
```

Implement a custom edit model in the [CustomizeEditModel](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGrid.CustomizeEditModel) event handler:

```cs
private void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e) {
    if (e.IsNew) {
        dynamic forecast = new ExpandoObject();
        forecast.Id = Guid.NewGuid();
        forecast.Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        forecast.TemperatureC = 0;
        forecast.Summary = "";
        e.EditModel = forecast;
    }
}
```

Use the [EditModelSaving](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGrid.EditModelSaving) event handler to retrieve data from your custom edit model and update the corresponding `ExpandoObject` instance.

```cs
private async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e) {
    if (e.IsNew) {
        forecasts.Add((ExpandoObject)e.EditModel);
    }
    else {
        dynamic editableForecast = (ExpandoObject)e.EditModel;
        dynamic originalForecast = forecasts
            .Cast<dynamic>()
            .First(s => (Guid)s.Id == (Guid)editableForecast.Id);
        originalForecast.Date = editableForecast.Date;
        originalForecast.TemperatureC = editableForecast.TemperatureC;
        originalForecast.Summary = editableForecast.Summary;
    }
}
```

Use the [DataItemDeleting](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGrid.DataItemDeleting) event handler to remove an item from the `ExpandoObject` collection.

```cs
private async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e) {
    forecasts.Remove((ExpandoObject)e.DataItem);
}
```

Add our Blazor Grid ([DxGrid](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGrid)) component to the [page](CS/Expando/Components/Pages/Index.razor) and bind it to the `ExpandoObject` list. Attach event handlers to corresponding grid properties.

Add a custom [CellEditTemplate](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGridCommandColumn.CellEditTemplate) to each column to bind an inline editor to the `ExpandoObject` through `IDictionary<string, object>`.

```razor
<DxGrid Data="@forecasts"
        EditMode="GridEditMode.EditRow"
        CustomizeEditModel="Grid_CustomizeEditModel"
        EditModelSaving="Grid_EditModelSaving"
        DataItemDeleting="Grid_DataItemDeleting">
    <Columns>
        <DxGridCommandColumn />
        <DxGridDataColumn Caption="Date" FieldName="Date">
            <CellEditTemplate>
                @{
                    var editItem = (IDictionary<string, object>)context.EditModel;
                    var date = (DateOnly)editItem["Date"];
                }
                <DxDateEdit Date="@(date)"
                            DateChanged="@((DateOnly newVal) => editItem["Date"] = newVal)"
                            DateExpression="@(() => date)">
                </DxDateEdit>
            </CellEditTemplate>
        </DxGridDataColumn>
        ...
    </Columns>
</DxGrid>
```

## Files to Review

- [Index.razor](CS/Expando/Components/Pages/Index.razor)
- [WeatherForecastService.cs](CS/Expando/Services/WeatherForecastService.cs)
- [Program.cs](CS/Expando/Program.cs)

## Documentation

- [DxGrid Class](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGrid)
- [Bind Blazor Grid to Data](https://docs.devexpress.com/Blazor/403737/components/grid/bind-to-data)
- [Editing and Validation in Blazor Grid](https://docs.devexpress.com/Blazor/403454/components/grid/editing-and-validation)

<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=blazor-editable-grid-with-expandoobject&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=blazor-editable-grid-with-expandoobject&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
