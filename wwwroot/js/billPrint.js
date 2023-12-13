function printDivContent() {
    var contentToPrint = document.getElementById('contentToPrint').innerHTML;
    var printWindow = window.open('', '_blank');
    printWindow.document.write('<html><head><title>Printed Content</title>');
    printWindow.document.write('<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">');
    printWindow.document.write('<style>');
    //printWindow.document.write('#patientTable { max-height: 300px; overflow-y: auto; }');
    printWindow.document.write('.containerPrint { display: flex; border: 1px solid #000; }');
    /*printWindow.document.write('..highlighted-border  { border: 2px solid #3498db; padding: 10px; }');*/
    printWindow.document.write('</style>');
    printWindow.document.write('</head><body>');
    printWindow.document.write(contentToPrint); 
    //var tables = printWindow.document.querySelectorAll('.table-bordered');
    //tables.forEach(function (table) {
    //    table.style.color = 'darkblue'; // You can adjust other styles as needed
    //});
    printWindow.document.write('<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>');
    printWindow.document.write('<script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>');
    printWindow.document.write('<script>$(document).ready(function(){ window.print(); });</script>');
    printWindow.document.write('<script>document.getElementById(\'patientinfoPrint\').style.removeProperty(\'display\');</script>');
    printWindow.document.write('<script>document.getElementById(\'searchDiv\').style.setProperty(\'display\', \'none\', \'important\');</script>');
    printWindow.document.write('<script>document.getElementById(\'saveBtn\').style.setProperty(\'display\', \'none\', \'important\');</script>');
    printWindow.document.write('<script>document.getElementById(\'printbtn\').style.setProperty(\'display\', \'none\', \'important\');</script>');
    printWindow.document.write('<script>document.getElementById(\'newBillbtn\').style.setProperty(\'display\', \'none\', \'important\');</script>');
    printWindow.document.write('<script>document.getElementById(\'PatientInfoNoPrint\').style.setProperty(\'display\', \'none\', \'important\');</script>');
    printWindow.document.write('<script>document.getElementById(\'paymentOption\').style.setProperty(\'display\', \'none\', \'important\');</script>');
    printWindow.document.write('</body></html>');

    printWindow.document.close();
}
