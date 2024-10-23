
    $(document).ready(function () {
        $("form").validate({
            rules: {
                EventName: {
                    required: true,
                    minlength: 2
                },
                DateOfEvent: {
                    required: true,
                    date: true
                },
                Description: {
                    required: true,
                    minlength: 10
                },
                Subevent1: {
                    required: true,
                    minlength: 2
                },
                Subdesc1: {
                    required: true,
                    minlength: 10
                },
                Subevent2: {
                    minlength: 2
                },
                Subdesc2: {
                    minlength: 10
                },
                Subevent3: {
                    minlength: 2
                },
                Subdesc3: {
                    minlength: 10
                },
                Subevent4: {
                    minlength: 2
                },
                Subdesc4: {
                    minlength: 10
                },
                Subevent5: {
                    minlength: 2
                },
                Subdesc5: {
                    minlength: 10
                }
            },
            messages: {
                EventName: {
                    required: "Event name is required.",
                    minlength: "Event name must be at least 2 characters long."
                },
                DateOfEvent: {
                    required: "Date of event is required.",
                    date: "Please enter a valid date."
                },
                Description: {
                    required: "Description is required.",
                    minlength: "Description must be at least 10 characters long."
                },
                Subevent1: {
                    required: "Sub event 1 is required.",
                    minlength: "Sub event 1 must be at least 2 characters long."
                },
                Subdesc1: {
                    required: "Sub description 1 is required.",
                    minlength: "Sub description 1 must be at least 10 characters long."
                }
                // Repeat for other sub-events and descriptions as needed
            },
            errorClass: "text-danger",
            validClass: "text-success",
            errorElement: "span"
        });
    });

