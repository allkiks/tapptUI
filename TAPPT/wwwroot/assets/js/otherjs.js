
//$(document).ready(function () {
//    $('#tryy').DataTable({
//        responsive: true,
//        dom: 'Bfrtip',
//        buttons: [

//            {
//                extend: 'print',

//                exportOptions: {
//                    columns: ':visible'
//                }
//            },
//            'colvis'

//        ],
//        order: [0, 'desc'],
//        columnDefs: [{
//            targets: 0,
//            visible: true
//        }]
//    });

//});


$("#homestuff").hover(
    function () {
        $(this).append($("<h6>Home</h6>"));
    }, function () {
        $(this).find("h6").last().remove();
    }
);

$("h6.fade").hover(function () {
    $(this).fadeOut(100);
    $(this).fadeIn(500);
});

$(document).ready(function () {
	document.title = 'TAPPT Water Platform';
	// DataTable initialisation
	$('#tryy').DataTable(
		{
			"dom": '<"dt-buttons"Bf><"clear">lirtp',
			"paging": true,
			"ordering": false,
			"autoWidth": true,
			"buttons": [
				'colvis',
				'copyHtml5',
				'csvHtml5',
				'excelHtml5',
				'pdfHtml5',
				'print'
			]
		}
	);
	
});

$("#success-alert").fadeTo(2000, 500).slideUp(500, function () {
	$("#success-alert").slideUp(3000);
});
$(document).ready(function () {
	
	$("#myWish").click(function showAlert() {
		$("#success-alert").fadeTo(2000, 500).slideUp(500, function () {
			$("#success-alert").slideUp(500);
		});
	});
});





//function printDiv(printfunction) {
//	var printContents = document.getElementById(printfunction).innerHTML;
//	var originalContents = document.body.innerHTML;

//	document.body.innerHTML = printContents;

//	window.print();

//	document.body.innerHTML = originalContents;
//}

$(document).ready(function () {
	$('[data-toggle="tooltip"]').tooltip();
});