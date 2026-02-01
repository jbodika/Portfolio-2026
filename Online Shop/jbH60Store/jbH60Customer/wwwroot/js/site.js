
//js for the products page 

let productDetails = document.querySelectorAll(".grid-item");

productDetails.forEach(item => {


    item.addEventListener('mouseover', () => {
        console.log('Mouse over:', item.textContent);

        // Show the quantity input
        item.querySelector('.item-quantity').style.display = 'block';
        item.classList.add('selected');
        // Add the "gray-out" class to all items
        productDetails.forEach(otherItem => {
            otherItem.classList.toggle('gray-out', otherItem !== item);
        });
    });

    item.addEventListener('mouseout', () => {
        console.log('Mouse out:', item.textContent);

        // Hide the quantity input
        item.querySelector('.item-quantity').style.display = 'none';
        item.classList.remove('selected');
        // Remove the "gray-out" class from all items
        productDetails.forEach(otherItem => {
            otherItem.classList.remove('gray-out');
        });
    });


});



// js for the shopping cart
let qtnBtnMin = document.querySelectorAll('.negative');
let qtnBtnPos = document.querySelectorAll('.positive');

qtnBtnMin.forEach(decrementButton => {
    decrementButton.addEventListener('click', (event) => {
        //event.preventDefault();

        // Find the associated form
        let form = decrementButton.closest('form');
        
        // Find the quantity input within the form
        let qtyInput = form.querySelector('input[name="quantity"]');

       
        if (qtyInput) {
            console.log('Current quantity:', qtyInput.value);

            // Handle decrement logic here

            // Example: Decrease the quantity by 1
            if (parseInt(qtyInput.value) > 0) {
                qtyInput.value = parseInt(qtyInput.value) - 1;
            }
        } else {
            console.error('Quantity input not found.');
        }
    });
});



qtnBtnPos.forEach(decrementButton => {
    decrementButton.addEventListener('click', (event) => {
        //event.preventDefault();

        // Find the associated form
        let form = decrementButton.closest('form');

        // Find the quantity input within the form
        let qtyInput = form.querySelector('input[name="quantity"]');
     
        if (qtyInput) {
            console.log('Current quantity:', qtyInput.value);

            // Handle decrement logic here

            // Example: Decrease the quantity by 1
            
                qtyInput.value = parseInt(qtyInput.value) + 1;
            
        } else {
            console.error('Quantity input not found.');
        }
    });
});
