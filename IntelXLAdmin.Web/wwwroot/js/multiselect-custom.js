$(document).ready(function () {
    multiselect()   
})
var selectedyears = [];
var expectedyears = [];
if (previousYears)
    selectedyears = previousYears.split(",");
if (probableYears)
    expectedyears = probableYears.split(",");
function multiselect() {
    let options = [];
    var currentYear = new Date().getFullYear();
    var startYear = currentYear - 15; 
    for (var year = startYear; year < currentYear; year++) {
        var isSelectedYear = selectedyears.includes(year.toString());
        var py = { label: year, title: year, value: year, selected: isSelectedYear }
        options.push(py);
    }
    let probableYearOptions = []; 
    var endYear = currentYear + 5;
    for (var year = currentYear; year < endYear; year++) {
        var isExcepted = expectedyears.includes(year.toString());
        var ey = { label: year, title: year, value: year, selected: isExcepted }
        probableYearOptions.push(ey);
    }
    $('#previous_year').multiselect('dataprovider', options);
    $('#probable_year').multiselect('dataprovider', probableYearOptions);
}