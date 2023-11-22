
        let selectedCourseId = 0;
        let selectedCourse = null;
        let userCourseId = 0;
        let coursePlanId = 0;
        let isFullPayment = true;
        let courses = null;
        let quesSec = 1;
        let studentName = null;

        $(document).ready(function () {
            var stripe = Stripe('pk_test_51O5RF5F2CKNCk6PLx3zWqOaRTyuwoeIzOM6TTjFpgxmwwxQF7e5xHbUw8QIdgEGcocHls5kBjvkLk83hL5zIX3JL00En5Rc3xj');
            var elements = stripe.elements({
                clientSecret: 'sk_test_51O5RF5F2CKNCk6PL5aSGWK8Xp76hbqjBRcG32oAzYiMRhn66YQtvqUl5rqNZROEFcPcLQTFb9EFWekfaYSFhqALD00gxBl7JmR',
            });
            getCoursesData();

            function getCoursesData() {
                $.ajax({
                    url: '/course/GetAllCourses',
                    type: 'GET',
                    success: (response) => {
                        if (response.status) {
                            courses = response.modelList;

                            let tempHtml = ``;
                            response.modelList.forEach(function (course) {
                                tempHtml += `
                                                                                    <div class="col-md-6 mt-1" onclick="selectCourse(this, ${course.Id})">
                                                                                        <div class="card custom-card" style="width: 18rem;">
                                                                                            <img class="card-img-top" src="${course.ImagePath}" alt="${course.Name}" style="width:100%;">
                                                                                            <div class="card-body">
                                                                                            <div class="d-flex">
                                                                                                <h5><span class="badge bg-primary">IT Audit</span></h5>
                                                                                                <h5 class="card-title offset-6">$${course.Price}</h5>
                                                                                            </div>
                                                                                                <h4 class="card-title">${course.Name}</h4>
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                `;
                            });

                            $('#course-row').append(tempHtml);
                        }
                        else {
                        }
                    }
                });
            }

            // agreement functions
            $('#sign-upload-form').on('submit', function (e) {
                $.ajax({
                    url: '/Signature/Upload',
                    type: "POST",
                    data: new FormData(this),
                    contentType: false,
                    cache: false,
                    processData: false,
                    success: (response) => {
                        if (response.status) {
                            window.location.href = '/account/registered'
                        }
                        else {
                            responseErrorPopUp(response.userMessage);
                        }
                    },
                    error: (error) => {
                        serverErrorPopUp();
                    }
                });
            })
            $('#sign-upload-file').on('change', function () {
                $('#sign-submit-btn').removeAttr('disabled');
            })

            // payment functions
            $('#payment-check').change(function () {
                selected_value = $("input[name='paymenttype']:checked").val();
                if (selected_value == '1') {
                    isFullPayment = false;
                    $('input[name="isFullPayment"]').val(false);
                    $('#plans').removeAttr('hidden');
                }
                else {
                    isFullPayment = true;
                    $('input[name="isFullPayment"]').val(true);
                    $('#plans').attr('hidden', 'hidden');
                }
            });
            function paymentFormValidation() {
                if ($('#cardnum').val() == null || $('#cardnum').val()?.length != 16) {
                    $('#cn-valid').text('Card number should be 16 digits!');
                    $('#cn-valid').removeAttr('hidden');
                    return false;
                }
                if ($('#mexp').val() == null || $('#mexp').val()?.length != 2) {
                    $('#em-valid').text('Exp month should be of 2 digit!');
                    $('#em-valid').removeAttr('hidden');
                    return false;
                }
                if ($('#yexp').val() == null || $('#yexp').val()?.length != 4) {
                    $('#ey-valid').text('Exp year should be of 4 digit!');
                    $('#ey-valid').removeAttr('hidden');
                    return false;
                }
                if ($('#cvv').val() == null) {
                    $('#cvc-valid').text('Invalid cvc or cvv!');
                    $('#cvc-valid').removeAttr('hidden');
                    return false;
                }
                return true;
            }
            $('#payment-btn').on('click', function (e) {
                if (paymentFormValidation()) {
                    let data = {
                        'UserCourseId': userCourseId,
                        'CoursePlanId': coursePlanId,
                        'CardNumber': $('#cardnum').val(),
                        'ExpMonth': $('#mexp').val(),
                        'ExpYear': $('#yexp').val(),
                        'Cvc': $('#cvv').val(),
                        'IsFullPayment': isFullPayment
                    };
                    $.ajax({
                        url: '/Payment/CardPayment',
                        type: 'POST',
                        dataType: 'json',
                        contentType: 'application/json',
                        data: JSON.stringify(data),
                        success: (response) => {
                            if (response.status) {
                                goToAgreementSection();
                            }
                            else {
                                responseErrorPopUp(response.errorMessage);
                            }
                        },
                        error: function (jqXhr, textStatus, errorThrown) {
                            serverErrorPopUp();
                        }
                    });
                }
            });
        });

        function selectPlan(id) {
            coursePlanId = id;
            $('input[name="coursePlanId"]').val(id);
        }
        function appendPaymentPlans() {
            $('#plans').empty();
            let htmlTemp = '';

            selectedCourse.CoursePlans.forEach(function (coursePlan) {
                htmlTemp += `
                                                        <div class="col-md-4 radio">
                                                            <div class="custom-plan-div">
                                                                <input type="radio" onchange="selectPlan(${coursePlan.Id})" id="plan${coursePlan.Id}" name="plan${selectedCourseId}" value="${coursePlan.Id}">
                                                                <label class="radio-label mt-2" for="plan${coursePlan.Id}">Pay in ${coursePlan.IntervalValue} ${coursePlan.Interval}s</label>
                                                                <p class="mx-1">Upfront Payment: $${coursePlan.UpFrontPrice}</p>
                                                                <p class="mx-1">Weekly Payment: $${coursePlan.PlanPrice}</p>
                                                            </div>
                                                        </div>
                                                    `;
            });
            $('#plans').append(htmlTemp);
        }
        function goToAgreementSection() {
            $('#payment-sec').attr('hidden', 'hidden');
            $('#agreement-sec').removeAttr('hidden');
            $('#ag-heading').removeClass('step-name');
            $('#ag-heading').addClass('active-step-name');
            $('#ag-img').attr('src', '/Assets/Website/Images/Register-Dot.png');
        }
        function goToPaymentSection() {
            $('#questions-sec').attr('hidden', 'hidden');
            $('#payment-sec').removeAttr('hidden');
            $('#pay-heading').removeClass('step-name');
            $('#pay-heading').addClass('active-step-name');
            $('#pay-img').attr('src', '/Assets/Website/Images/Register-Dot.png');
            $('#crs-img').attr('src', selectedCourse.ImagePath);
            $('#crs-name').text(selectedCourse.Name);
            $('#crs-desc').text(selectedCourse.Description.substring(0, 200));
            $('#pay-amt').html('<b>$' + selectedCourse.Price + '</b>');
            appendPaymentPlans();
        }
        function goBack() {
            if (quesSec == 3) {
                quesSec = 2;
                $('#pmng-heading').html('Personal Motivation and Goals:');
                $('#pmng-img').attr('src', '/Assets/Website/Images/InActiveRegister-Dot.png');
                $('#q3').attr('hidden', 'hidden');
                $('#q2').removeAttr('hidden');
            }
            else if (quesSec == 2) {
                quesSec = 1;
                $('#cne-heading').html('Commitment and Engagement:');
                $('#cne-img').attr('src', '/Assets/Website/Images/InActiveRegister-Dot.png');
                $('#q2').attr('hidden', 'hidden');
                $('#q1').removeAttr('hidden');
            }
        }
        function commitmentEngagementValidation() {
            debugger;
            if ($('#ans6').val() == null || $('#ans6').val() < 4) {
                $('#ans6-validation').text('participation level should be greater than 3!');
                $('#ans6-validation').removeAttr('hidden');
                return false;
            }
            if ($('#ans7').val() == null || $('#ans7').val().toLowerCase() != 'yes') {
                $('#ans7-validation').text('you should be comfortable in group work!');
                $('#ans7-validation').removeAttr('hidden');
                return false;
            }
            if ($('#ans8').val() == null || $('#ans8').val().toLowerCase() != 'yes') {
                $('#ans8-validation').text('you should be comfortable in course duration!');
                $('#ans8-validation').removeAttr('hidden');
                return false;
            }
            return true;
        }
        function proceed() {
            if (quesSec == 1) {
                quesSec = 2;
                $('#cne-heading').html('<b>Commitment and Engagement:</b>');
                $('#cne-img').attr('src', '/Assets/Website/Images/Question-Dot.png');
                $('#q1').attr('hidden', 'hidden');
                $('#q2').removeAttr('hidden');
            }
            else if (quesSec == 2) {
                if (commitmentEngagementValidation()) {
                    quesSec = 3;
                    $('#pmng-heading').html('<b>Personal Motivation and Goals:</b>');
                    $('#pmng-img').attr('src', '/Assets/Website/Images/Question-Dot.png');
                    $('#q2').attr('hidden', 'hidden');
                    $('#q3').removeAttr('hidden');
                }
            }
            else if (quesSec == 3) {
                successPopUp();
                goToPaymentSection();
            }
        }
        function goToQuestionSection() {
            $('#register-sec').attr('hidden', 'hidden');
            $('#questions-sec').removeAttr('hidden');
            $('#ques-heading').removeClass('step-name');
            $('#ques-heading').addClass('active-step-name');
            $('#ques-img').attr('src', '/Assets/Website/Images/Register-Dot.png');
        }
        function signup() {
            $('#sign-up-btn').attr('disabled', 'disabled');
            $('#ndas').text(`This Agreement is made and entered into by and between Cyfendry Consulting, LLC ("Company") and ${studentName} ("Student") for the purpose of receiving certain confidential information of Company to enable the Student to undertake the course described at the end of this Agreement ("course").`);
            if ($('#password').val() == $('#confirmPassword').val()) {
                if (selectedCourseId == 0) {
                    $('#sign-up-btn').removeAttr('disabled');
                    responseErrorPopUp('Select a course!');
                    return false;
                }
                let data = {
                    FirstName: $('#firstName').val(),
                    LastName: $('#lastName').val(),
                    Phone: $('#phone').val(),
                    Email: $('#email').val(),
                    Password: $('#password').val(),
                    CourseId: selectedCourseId
                }
                $.ajax({
                    url: '/account/register',
                    type: 'POST',
                    data: { registerRequestModel: data },
                    success: (response) => {
                        if (response.status) {
                            //$('#register-sec').attr('hidden', 'hidden');
                            //$('#questions-sec').removeAttr('hidden');
                            $('#uci').val(response.model.Id);
                            $('input[name="userCourseId"]').val(response.model.Id);
                            userCourseId = response.model.Id;
                            goToQuestionSection();
                            studentName = $('#firstName').val() + ' ' + $('#lastName').val();
                        }
                        else {
                            $('#sign-up-btn').removeAttr('disabled');
                            responseErrorPopUp(response.userMessage);
                        }
                    },
                    error: (err) => {
                        $('#sign-up-btn').removeAttr('disabled');
                        serverErrorPopUp();
                    }
                });
            }
            else {
                $('#sign-up-btn').removeAttr('disabled');
                responseErrorPopUp("Password does'nt match");
            }
        }
        function selectCourse(e, id) {
            selectedCourseId = id;
            selectedCourse = courses.find(i => i.Id === id);
            $('#duration-para').empty();
            $('#duration-para').append('<b>Course Duration:</b> The course runs for ' + selectedCourse.Duration + '. Are you confident in your ability to commit for the entire duration?');
            $('.custom-card-active').addClass("custom-card");
            $('.custom-card').removeClass("custom-card-active");
            $(e).children().removeClass("custom-card");
            $(e).children().addClass("custom-card-active");
            //console.log('selectedCourse: ', selectedCourse);
        }
        function successPopUp() {
            Swal.fire({
                //position: "top-end",
                icon: "success",
                title: "Accepted",
                //showConfirmButton: false,
                timer: 1500
            });
        }
        function responseErrorPopUp(message) {
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: message
            });
        }
        function serverErrorPopUp() {
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "Something went wrong! Please try again later .."
            });
        }