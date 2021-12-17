
window.onload = function() {

    var formHandle = document.forms.AddTeacherForm;

    formHandle.onsubmit = function() {
        
        var FName = formHandle.TeacherFname;
        var LName = formHandle.TeacherLname;
        var ENumber = formHandle.EmployeeNumber;
        var HDate = formHandle.HireDate;
        var Income = formHandle.Salary;


        if (FName.value === "" || FName.value === null) {

            FName.style.background = "red";
            FName.focus();
            return false;

        }

        else if (LName.value === "" || LName.value === null) {

            LName.style.background = "red";
            LName.focus();
            return false;

        }

        else if (ENumber.value < 100 || ENumber.value === null) {

            ENumber.style.background = "red";
            ENumber.focus();
            return false;

        }

        else if (HDate.value === "" || HDate.value === null) {

            HDate.style.background = "red";
            HDate.focus();
            return false;

        }


        else if (Income.value <= 0 || Income.value === null) {

            Income.style.background = "red";
            Income.focus();
            return false;

        }

        
        
    }

}