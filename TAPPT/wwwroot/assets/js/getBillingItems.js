let getBillingItemsUrl = apiBaseUrl.concat(`/admin/GetBillingItems/`);
function tableRows(data) {
  var tableRows = [];
  for (var i = 0; i < data.length; i++) {
    tableRows.push(drawRow(data[i]));
  }
  return tableRows;
}
//Start by getting publication list based on Curation Stage
$.ajax({
  url: getBillingItemsUrl,
  type: "GET",
  dataType: "json",
  success: function(data, status, jqhxr) {
    //This code snipet prepares to append Json Data
    $("#billing-items").append(tableRows(data));
  }
});
//This functionpopulates the tbody inner HTML with json data on call
function drawRow(rowData) {
  var row = $("<tr />");
  row.append($("<td>" + rowData.id + "</td>"));
  row.append($("<td>" + rowData.billingItemName + "</td>"));
  row.append(
    $(
      `<td> <a href="/Admin/EditBillingItem/${rowData.id}" class="btn btn-w-m btn-info" style="background-color:#00B95F;" role="button">Edit </a>`
    )
  );
  row.append(
    $(
      `<td> <a href="/Admin/DeleteBillingItem/${rowData.id}" class="btn btn-w-m btn-info" style="background-color:#00B95F;" role="button">Delete </a>`
    )
  );
  return row[0];
}
