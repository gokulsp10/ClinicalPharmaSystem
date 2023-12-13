function printDivContent() {
    var contentToPrint = document.getElementById('contentToPrint').innerHTML;
    var notesContent = document.getElementById('notesTextArea').value;
    var printWindow = window.open('', '_blank');
    var doctorName = document.getElementById('doctorName').innerHTML;
    var doctorRegNo = document.getElementById('doctorRegNo').value;
    var currentDate = new Date();
    var formattedDate = currentDate.toLocaleString(); 
    printWindow.document.write('<html><head><title>Doctor Prescription_' + formattedDate +'</title>');
    printWindow.document.write('<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">');
    printWindow.document.write('<style>');
    printWindow.document.write('#patientTable { max-height: 300px; overflow-y: auto; }');
    printWindow.document.write('.containerPrint { display: flex; border: 1px solid #000; }');
    printWindow.document.write('.box { margin: 5px; }');
    printWindow.document.write('</style>');
    printWindow.document.write('</head><body style="color:darkblue!important">');
    printWindow.document.write(contentToPrint);
    printWindow.document.write('<div>Notes: ' + notesContent + '</div>'); 
    var tables = printWindow.document.querySelectorAll('.table-bordered');
    tables.forEach(function (table) {
        table.style.color = 'darkblue'; // You can adjust other styles as needed
    });
    printWindow.document.write('<div style="right:0;position:absolute;">' + doctorName + '</div><br>'); 
    printWindow.document.write('<div style="right:0;position:absolute;">' + doctorRegNo + '</div>'); 
    printWindow.document.write('<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>');
    printWindow.document.write('<script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>');
    printWindow.document.write('<script>$(document).ready(function(){ window.print(); });</script>');
    printWindow.document.write('<script>document.getElementById(\'headerDiv\').style.removeProperty(\'display\');</script>');
    printWindow.document.write('<script>document.getElementById(\'PrintBtn\').style.setProperty(\'display\', \'none\', \'important\');</script>');
    printWindow.document.write('<script>document.getElementById(\'medHist\').style.setProperty(\'color\', \'darkblue\', \'important\');</script>');
    printWindow.document.write('<script>document.getElementById(\'diseasediv\').style.setProperty(\'display\', \'none\', \'important\');</script>');
    printWindow.document.write('<script>document.getElementById(\'doctorNotes\').style.setProperty(\'display\', \'none\', \'important\');</script>');
    printWindow.document.write('</body></html>');

    printWindow.document.close();
}
