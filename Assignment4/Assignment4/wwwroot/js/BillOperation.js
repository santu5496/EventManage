var OwnerDetail = {
    displayName: "",
    branchAddress: "",
    pincode: "",
}

$(document).ready(function () {
    GetOwnerDetails();
});

$(document).on("input", ".qty-input", function () {
    let row = $(this).closest("tr");
    updateTotal(row);
});

$(document).on("click", ".remove-item", function () {
    $(this).closest("tr").remove();
    updateGrandTotal();
});

function updateTotal(row) {
    let qty = parseFloat(row.find(".qty-input").val()) || 0; // Handle decimals like 0.5
    let price = parseFloat(row.find(".price").text()) || 0; // Handle valid numbers
    let unitValue = parseFloat(row.data("unit-value")) || 1;
    // Calculate total based on the unit value
    let total = (qty / unitValue) * price;

    row.find(".total").text(total.toFixed(2)); // Display total with two decimal places
    updateGrandTotal();
}

function updateGrandTotal() {
    let total = 0;
    $("#billItems tr").each(function () {
        total += parseFloat($(this).find(".total").text());
    });
    $("#totalAmount").text(total);
}


//function addItemToBill(itemId, itemName, itemPrice, itemUnit, unitForPrice) {
//    let existingRow = $("#billItems").find(`tr[data-id='${itemId}']`);

//    if (existingRow.length > 0) {
//        let qtyInput = existingRow.find(".qty-input");
//        qtyInput.val(parseInt(qtyInput.val()) + 1);
//        updateTotal(existingRow);
//    } else {
//        $("#billItems").append(`
//                        <tr data-id="${itemId}" data-unit-value="${unitForPrice}">
//                            <td>${itemName}</td>
//                            <td><input type="number" class="qty-input form-control"  value="${unitForPrice}" min="0" style="width:75px;"></td>
//                            <td class="unit">${itemUnit}</td>
//                            <td class="price">${itemPrice}</td>
//                            <td class="total">${itemPrice}</td>
//                            <td><button class="btn btn-danger btn-sm remove-item">X</button></td>
//                        </tr>
//                    `);
//    }
//    updateGrandTotal();
//}
function addItemToBill(itemId, itemName, itemPrice, itemUnit, unitForPrice, maxValue) {
    let existingRow = $("#billItems").find(`tr[data-id='${itemId}']`);

    if (existingRow.length > 0) {
        let qtyInput = existingRow.find(".qty-input");
        let currentQty = parseInt(qtyInput.val());
        let newQty = currentQty + 1;

        if (newQty > maxValue) {
            newQty = maxValue;
            msgPopup('warning',`Only ${maxValue} items available in stock.`);
        }

        qtyInput.val(newQty);
        updateTotal(existingRow);

    } else {
        let initialQty = maxValue > 0 ? unitForPrice : 0;

        $("#billItems").append(`
            <tr data-id="${itemId}" data-unit-value="${unitForPrice}">
                <td>${itemName}</td>
                <td>
                    <input type="number" 
                           class="qty-input form-control"  
                           value="${initialQty}" 
                           min="0" 
                           max="${maxValue}" 
                           style="width:75px;">
                </td>
                <td class="unit">${itemUnit}</td>
                <td class="price">${itemPrice}</td>
                <td class="total">${(initialQty * itemPrice).toFixed(2)}</td>
                <td><button class="btn btn-danger btn-sm remove-item">X</button></td>
            </tr>
        `);
    }

    updateGrandTotal();
}
$(document).on('input', '.qty-input', function () {
    let maxVal = parseInt(this.max) || 0;
    let enteredQty = parseInt($(this).val());

    if (enteredQty > maxVal) {
        msgPopup('warning', `Maximum available stock is ${maxVal}.`);
        $(this).val(maxVal);
    } else if (enteredQty < 0 || isNaN(enteredQty)) {
        $(this).val(0);
    }

    let row = $(this).closest('tr');
    updateTotal(row);
    updateGrandTotal();
});


function GetItemsDetailsById(id) {
    $.ajax({
        url: '/Billing/GetDetailsOnBarcodeScan',
        type: 'POST',
        data: {
            itemId: id
        },
        success: function (response) {
            if (response != null) {
                addItemToBill(response.itemId, response.itemName, response.pricePerUnit, response.unit, response.priceQuantity, response.maxQuantity);
            } else {
                msgPopup('error', 'Items Is Not Present in the Inventory');
            }
        },
        error: function (xhr, status, error) {
            alert('An error occurred: ' + error);
        }
    });
}

function generatePDF() {
    $.ajax({
        url: '/Billing/GetCustomerDetailsById',
        type: 'GET',
        data: {
            id: $('#ddlCustomer').val()
        },
        success: function (response) {
            const {
                jsPDF
            } = window.jspdf;
            let doc = new jsPDF();

            // Owner Details (Centered, Bold, Uppercase)
            doc.setFontSize(18);
            doc.setFont('helvetica', 'bold'); // Bold text
            let ownerTitle = (OwnerDetail?.displayName || 'N/A').toUpperCase();
            let ownerTitleWidth = doc.getTextWidth(ownerTitle);
            doc.text(ownerTitle, (doc.internal.pageSize.width - ownerTitleWidth) / 2, 15);

            doc.setFontSize(12);
            let branchAddress = (OwnerDetail?.branchAddress || 'N/A').toUpperCase();
            let branchAddressWidth = doc.getTextWidth(branchAddress);
            doc.text(branchAddress, (doc.internal.pageSize.width - branchAddressWidth) / 2, 25);

            let pincodeText = `Pincode: ${(OwnerDetail?.pincode || 'N/A').toUpperCase()}`;
            let pincodeWidth = doc.getTextWidth(pincodeText);
            doc.text(pincodeText, (doc.internal.pageSize.width - pincodeWidth) / 2, 35);

            // Horizontal Line After Owner Details
            doc.setLineWidth(0.5);
            doc.line(10, 40, 200, 40);

            // Customer Details
            doc.setFontSize(14);
            doc.setFont('helvetica', 'bold'); // Bold heading
            doc.text('CUSTOMER DETAILS', 10, 50);

            doc.setFontSize(12);
            let customerName = response?.customerName || 'N/A';
            let phoneNo = response?.phoneNo || 'N/A';
            let address = response?.address || 'N/A';

            doc.setFont('helvetica', 'bold'); // Bold labels
            doc.text('Name:', 10, 60);
            doc.setFont('helvetica', 'normal');
            doc.text(`${customerName} (${phoneNo})`, 35, 60);

            doc.setFont('helvetica', 'bold'); // Bold labels
            doc.text('Address:', 10, 70);
            doc.setFont('helvetica', 'normal');
            doc.text(address, 35, 70);

            // Horizontal Line After Customer Details
            doc.line(10, 75, 200, 75);

            // Bill Items Table
            doc.setFontSize(14);
            doc.setFont('helvetica', 'bold');
            doc.text('BILL SUMMARY', 10, 85);

            let billData = [];
            $("#billItems tr").each(function (index) {
                let itemName = $(this).find("td").eq(0).text().trim() || 'N/A';
                let qty = $(this).find(".qty-input").val() || '0';
                let unit = $(this).find(".unit").text().trim() || 'N/A';
                let price = $(this).find(".price").text().trim() || '0.00';
                let total = $(this).find(".total").text().trim() || '0.00';

                billData.push([index + 1, itemName, `${qty} ${unit}`, price, total]);
            });

            doc.autoTable({
                head: [
                    ['No', 'Item Name', 'Qty', 'Price', 'Amount']
                ],
                body: billData,
                startY: 90,
                theme: 'striped',
                headStyles: {
                    fillColor: '#f2f2f2',
                    textColor: 'black'
                },
                styles: {
                    fontSize: 10,
                    cellPadding: 2
                }
            });

            // Grand Total (Aligned under Amount Column)
            let totalAmount = $('#totalAmount').text().trim() || '0.00';
            let finalY = doc.autoTable.previous.finalY + 10;
            let tableEndX = doc.internal.pageSize.width - 85; // Align to Amount column
            doc.setFontSize(14);
            doc.setFont('helvetica', 'bold');
            doc.text(`Grand Total: ${totalAmount}`, tableEndX, finalY);

            // Horizontal Line Before Footer
            doc.line(10, finalY + 5, 200, finalY + 5);

            // Footer Message (Centered)
            doc.setFontSize(10);
            let footerMessage = 'Thank you for your business!';
            let footerContact = 'For any queries, contact us at support@example.com';

            let footerMessageWidth = doc.getTextWidth(footerMessage);
            let footerContactWidth = doc.getTextWidth(footerContact);

            doc.text(footerMessage, (doc.internal.pageSize.width - footerMessageWidth) / 2, finalY + 15);
            doc.text(footerContact, (doc.internal.pageSize.width - footerContactWidth) / 2, finalY + 20);

            // Save PDF with improved date format
            let formattedDate = new Date().toLocaleDateString('en-GB').replace(/\//g, '-');
            doc.save(`Bill_${formattedDate}.pdf`);
        },
        error: function (xhr, status, error) {
            console.error('Error fetching data:', error);
            alert('An error occurred while generating the PDF. Please try again.');
        }
    });
}
function ValidatePaymentMode(status) {
    if (status == "UnPaid") {
        $('#ddlPaymentMode').val('NA');
        $('#ddlPaymentMode').prop('disabled', true);
    } else {
        $('#ddlPaymentMode').prop('disabled', false);
    } s
}
function GetOwnerDetails() {
    $.ajax({
        url: '/Billing/GetOwnerBranchDetails',
        type: 'GET',
        success: function (response) {
            OwnerDetail.displayName = response.displayName;
            OwnerDetail.branchAddress = response.branchAddress;
            OwnerDetail.pincode = response.pincode;
        },
        error: function (xhr, status, error) {
            alert('An error occurred: ' + error);
        }
    });
}
