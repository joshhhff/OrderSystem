// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Used for converting new item line keys to meaningful key names (creating new orders)
const keyMapping = {
    column0: 'item',
    column1: 'description',
    column2: 'rate',
    column3: 'taxRate',
    column4: 'quantity',
    column5: 'total',
    column6: 'taxTotal',
    column7: 'grossAmount'
};

async function logOutUser() {
    const response = await fetch('/api/Authentication/logout');

    if (response.ok) {
        window.location.href = '/';
    }
}

document.addEventListener('DOMContentLoaded', checkAndHideElements);
window.addEventListener('popstate', checkAndHideElements);

async function checkAndHideElements() {
    const response = await fetch(`${window.location.origin}/api/Authentication/permissions`)
    const role = await response.json();
    console.log('role id', role);

    const hideOnLoginScreenElements = document.querySelectorAll('.hide-on-login-screen');
    const isIndexPage = window.location.pathname === '/' || window.location.pathname === '/Index';

    hideOnLoginScreenElements.forEach(element => {
        if (isIndexPage) {
            element.style.display = 'none';
        } else {
            if (!role.loggedIn) {
                element.style.display = 'none';
            } else if (role.loggedIn) {
                if (element.classList.contains('admin-only') && role.roleID !== '2') {
                    console.log('LOGGED IN AS USER')
                    // Current user is logged in as a user and not admin
                    element.style.display = 'none';
                } else if (element.classList.contains('admin-only') && role.roleID === '2') {
                    // CUrrent user logged in as an admin
                    console.log('LOGGED IN AS ADMIN')
                    element.style.display = '';
                }
            }
        }
    });
}

function processOrder() {
    const table = document.getElementById('itemTable');
    console.log(table)
    const tbody = table.querySelector('tbody');
    const rows = tbody.querySelectorAll('tr');

    const json = { 
        total: 0, 
        taxTotal: 0, 
        grossAmount: 0, 
        lines: []
    };

    rows.forEach((row, rowIndex) => {
        row = document.getElementById(row.id);

        const columns = row.querySelectorAll('td');
        const rowData = {};

        columns.forEach((column, colIndex) => {
            column = document.getElementById(column.id)

            const inputElement = column.querySelector('input, select');
            if (inputElement) {
                let value;

                // Check for different input types
                if (inputElement.tagName === 'SELECT') {
                    value = inputElement.options[inputElement.selectedIndex].text;
                } else {
                    value = inputElement.value;
                }

                // Store value in the rowData object with a unique key
                rowData[`column${colIndex}`] = value;
            } else {
                console.log(`  No input/select found in column ${colIndex}`); // Debug log for missing input
            }
        });
        
        const renamedKeys = renameKeys(rowData, keyMapping);
        json.total += Number(renamedKeys.total);
        json.taxTotal += Number(renamedKeys.taxTotal);
        json.grossAmount += Number(renamedKeys.grossAmount);

        json.lines.push(renameKeys(rowData, keyMapping));
    });
    
    json.total = String(json.total);
    json.taxTotal = String(json.taxTotal);
    json.grossAmount = String(json.grossAmount);

    document.getElementById('hidden-items-json').value = JSON.stringify(json);

    try {

        const form = document.getElementById('newOrderForm')

        // Check if the form is valid
        if (form.checkValidity()) {
            // Form is valid, submit it programmatically
            form.submit();
        } else {
            // If the form is not valid, trigger the browser's built-in validation UI
            form.reportValidity();
        }

    } catch(error) {
        console.error('Failed submit', error)
    }
}

// Function to convert key names to meaningful names (for line objects)
function renameKeys(obj, keyMap) {
    const newObj = {};
    Object.keys(obj).forEach(key => {
        const newKey = keyMap[key] || key; // Use new key if exists, otherwise keep the original
        newObj[newKey] = obj[key];
    });
    return newObj;
}

// Function to populate description and rate based on selected item
function updateProductInfo(selectElement) {
    const productId = selectElement.value;

    const row = selectElement.closest("tr");
    
    // For re-calculating line value when changing items and quantity is populated
    const quantityField = row.querySelector(".line-quantity");

    // Check if a valid product was selected
    if (productId) {
        // Fetch the product details from the backend (AJAX request)
        fetch(`/api/products/${productId}`)
            .then(response => response.json())
            .then(data => {
                // Populate the corresponding fields with product data
                row.querySelector(".description").value = data.description;
                row.querySelector(".rate").value = data.rate;
                row.querySelector(".line-tax-rate").value = data.taxRateString;
                
                calculateGrossAmount(quantityField);
                
            })
            .catch(error => console.error("Error fetching product data:", error));
    } else {
        // Clear the fields if no product is selected
        row.querySelector(".description").value = '';
        row.querySelector(".rate").value = '';
        row.querySelector(".line-tax-rate").value = '';
        row.querySelector(".line-quantity").value = '';
        row.querySelector(".line-total").value = '';
        row.querySelector(".line-tax-total").value = '';
        row.querySelector(".line-gross-amount").value = '';
        
        calculateTransactionTotals();   // re-calculate transaction total if clearing a line
    }
}

// Function to calculate Transaction Total based on all lines. Gets called every time a lines Gross Total updates
function calculateTransactionTotals() {
    const totalElements = document.querySelectorAll('.line-total');
    const taxTotalElements = document.querySelectorAll('.line-tax-total');
    const grossAmountElements = document.querySelectorAll('.line-gross-amount')

    let subtotal = 0;
    let totalTax = 0;
    let total = 0;

    taxTotalElements.forEach((element) => {
        // Parse the value of each tax-total input as a float
        const taxValue = parseFloat(element.value) || 0; // Defaults to 0 if empty or NaN
        totalTax += taxValue;
    });

    totalElements.forEach((element) => {
        const totalValue = parseFloat(element.value) || 0;
        subtotal += totalValue;
    });

    grossAmountElements.forEach((element) => {
        const grossAmountValue = parseFloat(element.value) || 0;
        total += grossAmountValue;
    });

    document.getElementById('subtotal').textContent = `£${subtotal.toFixed(2)}`;
    document.getElementById('tax-total').textContent = `£${totalTax.toFixed(2)}`;
    document.getElementById('total').textContent = `£${total.toFixed(2)}`;
}

// Function to calculate gross amount based on rate and quantity
function calculateGrossAmount(quantityInput) {
    const row = quantityInput.closest("tr");
    const rate = parseFloat(row.querySelector(".rate").value) || 0;
    const taxRate = parseFloat(row.querySelector(".line-tax-rate").value) / 100;
    const quantity = parseFloat(quantityInput.value) || 0;

    const taxTotal = taxRate * (rate * quantity);
    row.querySelector(".line-total").value = (rate * quantity).toFixed(2);
    row.querySelector(".line-tax-total").value = taxTotal.toFixed(2);
    row.querySelector(".line-gross-amount").value = (taxTotal + (rate * quantity)).toFixed(2);

    calculateTransactionTotals();
}

// Function to fetch products from database
async function fetchProducts() {
    const response = await fetch('/api/products');
    if (!response.ok) {
        throw new Error('Failed to fetch products');
    }

    return await response.json();
}


// Function to add a new item row

let itemCounter = 1;    // Initialise as 1 because line 0 is set on the HTML for the first line
async function addItemRow() {
    const items = await fetchProducts();
    
    const tableBody = document.getElementById("itemTable").querySelector("tbody");
    const newRow = document.createElement("tr");

    const itemColumnId = `line-${itemCounter}-item`;
    const descriptionColumnId = `line-${itemCounter}-description`;
    const rateColumnId = `line-${itemCounter}-rate`;
    const taxRateColumnId = `line-${itemCounter}-tax-rate`;
    const quantityColumnId = `line-${itemCounter}-quantity`;
    const totalColumnId = `line-${itemCounter}-total`;
    const taxTotalColumnId = `line-${itemCounter}-tax-total`;
    const grossAmountColumnId = `line-${itemCounter}-gross-amount`;

    newRow.classList.add('new-item-row');
    newRow.id = `lineid-${itemCounter}`;    // Set ID on item line for identifier
    newRow.innerHTML = `
    <td id="${itemColumnId}">
        <select class="form-control" onchange="updateProductInfo(this)">
                <option value="">Select Item</option>
        </select>
    </td>
    <td id="${descriptionColumnId}" colspan="2"><input type="text" class="description form-control" readonly></td>
    <td id="${rateColumnId}"><input type="text" class="rate form-control" readonly></td>
    <td id="${taxRateColumnId}"><input type="text" class="line-tax-rate form-control" readonly></td>
    <td id="${quantityColumnId}"><input type="number" class="line-quantity form-control" oninput="calculateGrossAmount(this)" min="1" required></td>
    <td id="${totalColumnId}"><input type="text" class="line-total form-control" readonly></td>
    <td id="${taxTotalColumnId}"><input type="text" class="line-tax-total form-control" readonly></td>
    <td id="${grossAmountColumnId}"><input type="text" class="line-gross-amount form-control" readonly></td>
  `;

    // Find the <select> tag in the new row
    const selectElement = newRow.querySelector('select');

    // Populate the <select> with items
    items.forEach((item) => {
        console.log('adding item', item);
        const option = document.createElement('option');
        option.value = item.value;
        option.textContent = item.text;
        selectElement.appendChild(option);
    });
    tableBody.appendChild(newRow);

    itemCounter++;    // Increment the next counter

    toggleRemoveButton();
}

// Function to remove the last item row
function removeLastRow() {
    const tableBody = document.getElementById("itemTable").querySelector("tbody");
    const rows = tableBody.querySelectorAll("tr");

    if (rows.length > 1) { // Ensure at least one row remains
        tableBody.removeChild(rows[rows.length - 1]);
        itemCounter--;
    }

    // Toggle the "Remove Last Line" button visibility
    toggleRemoveButton();
    calculateTransactionTotals();    // Need to re-calculate totals after removing latest line
}

// Function to show or hide the "Remove Last Line" button
function toggleRemoveButton() {
    const removeButton = document.getElementById("removeLastRowButton");

    // Show button only if there is more than one row
    if (itemCounter > 1) {
        removeButton.style.display = "inline-block";
    } else {
        removeButton.style.display = "none";
    }
}