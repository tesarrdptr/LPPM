<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Testing.aspx.cs" Inherits="ProposalPolmanAstra.Testing" %>

<html ng-app="myApp">

  <head>
    
    <script data-require="angular.js@*" data-semver="2.0.0" src="https://code.angularjs.org/1.4.8/angular.js
"></script>
    <script data-require="jquery@*" data-semver="2.1.4" src="https://code.jquery.com/jquery-2.1.4.js"></script>
      <style>
          /* Styles go here */
.table-header 
{
background-color: lightskyblue;  
}
      </style>
  </head>

  <body ng-controller="MyCtrl">
      <script>
          // Code goes here
          var myApp = angular.module("myApp", []);
          myApp.factory('Excel', function ($window) {
              var uri = 'data:application/vnd.ms-excel;base64,',
                  template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
                  base64 = function (s) { return $window.btoa(unescape(encodeURIComponent(s))); },
                  format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) };
              return {
                  tableToExcel: function (tableId, worksheetName) {
                      var table = $(tableId),
                          ctx = { worksheet: worksheetName, table: table.html() },
                          href = uri + base64(format(template, ctx));
                      return href;
                  }
              };
          })
              .controller('MyCtrl', function (Excel, $timeout, $scope) {
                  $scope.exportToExcel = function (tableId) { // ex: '#my-table'
                      var exportHref = Excel.tableToExcel(tableId, 'WireWorkbenchDataExport');
                      $timeout(function () { location.href = exportHref; }, 100); // trigger download
                  }
              });
      </script>
    <h1>Export to Excel</h1>
    <button class="btn btn-link" ng-click="exportToExcel('#tableToExport')">
      <span class="glyphicon glyphicon-share"></span>
 Export to Excel
    </button>
    <div id="tableToExport">
      <table border="1">
        <thead>
          <tr class="table-header">
            <th>Team</th>
            <th>Process Type</th>
            <th>Cedent</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>value 1</td>
            <td>value 2</td>
            <td>value 3</td>
          </tr>
          <tr>
            <td>value 4</td>
            <td>value 5</td>
            <td>value 6</td>
          </tr>
          <tr>
            <td>10.12.2015</td>
            <td>AXA Affin</td>
            <td>101024 - Quota Share QS</td>
          </tr>
        </tbody>
      </table>
    </div>
  </body>

</html>
