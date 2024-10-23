document.getElementById("state").addEventListener("change", function () {
    updateCityDropdown();
});

document.getElementById("form").addEventListener("submit", function (e) {
    if (!validateForm()) {
        e.preventDefault();
    }
});

function updateCityDropdown() {
    const stateDropdown = document.getElementById("state");
    const cityDropdown = document.getElementById("city");
    const selectedState = stateDropdown.value;

    const cities = {
        kerala: ["Thiruvananthapuram", "Kochi", "Kozhikode"],
        tamilnadu: ["Chennai", "Coimbatore", "Madurai"],
        goa: ["Panaji", "Margao", "Vasco da Gama"]
    };

    cityDropdown.innerHTML = "<option value='' selected disabled>--Select a city--</option>";

    if (selectedState in cities) {
        for (const city of cities[selectedState]) {
            const option = document.createElement("option");
            option.value = city.toLowerCase().replace(/\s+/g, "");
            option.textContent = city;
            cityDropdown.appendChild(option);
        }
    }
}

function validateForm() {
    let isValid = true;

    const firstName = document.getElementById("FirstName").value;
    const lastName = document.getElementById("LastName").value;
    const dateOfBirth = document.getElementById("DateOfBirth").value;
    const phoneNumber = document.getElementById("PhoneNumber").value;
    const emailAddress = document.getElementById("EmailAddress").value;
    const username = document.getElementById("Username").value;
    const password = document.getElementById("Password").value;
    const confirmPassword = document.getElementById("ConfirmPassword").value;

    if (!/^[A-Za-z]+$/.test(firstName)) {
        alert("First Name must contain only letters.");
        isValid = false;
    }

    if (!/^[A-Za-z]+$/.test(lastName)) {
        alert("Last Name must contain only letters.");
        isValid = false;
    }

    const today = new Date();
    const dob = new Date(dateOfBirth);
    const age = today.getFullYear() - dob.getFullYear();
    const monthDifference = today.getMonth() - dob.getMonth();
    if (monthDifference < 0 || (monthDifference === 0 && today.getDate() < dob.getDate())) {
        age--;
    }
    if (age < 18) {
        alert("You must be at least 18 years old.");
        isValid = false;
    }

    if (!/^[0-9]{10}$/.test(phoneNumber)) {
        alert("Phone Number must be a valid 10-digit number.");
        isValid = false;
    }

    const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    if (!emailPattern.test(emailAddress)) {
        alert("Please enter a valid email address.");
        isValid = false;
    }

    if (/\s/.test(username)) {
        alert("Username must not contain spaces.");
        isValid = false;
    }

    const passwordPattern = /(?=.*[0-9])(?=.*[!@#$%^&*]).{8,}/;
    if (!passwordPattern.test(password)) {
        alert("Password must be at least 8 characters long, contain at least one digit, and one special character.");
        isValid = false;
    }

    if (password !== confirmPassword) {
        alert("Passwords do not match.");
        isValid = false;
    }

    return isValid;
}
